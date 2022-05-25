using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Framework.Domain;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Controller that provides filing actions
    /// </summary>
    public abstract class FilingControllerBase<TEntity, TFilingHeader, TDefValueManual, TDefValuesReadModel> : ApiControllerBase
    where TEntity : Entity, IValidationRequiredEntity
    where TFilingHeader : FilingHeaderNew
    where TDefValuesReadModel : BaseDefValuesManualReadModel
    where TDefValueManual : BaseDefValuesManual
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly IFilingService<TEntity> _procedureService;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IFilingHeaderRepository<TFilingHeader> _filingHeaderRepository;

        private readonly IFilingSectionRepository _filingSectionsRepository;

        /// <summary>
        /// Truck export filing configuration factory
        /// </summary>
        private readonly IFilingConfigurationFactory<TDefValuesReadModel> _configurationFactory;

        private readonly IValidationRepository<TEntity> _inboundRepository;
        private readonly IFilingParametersService<TDefValueManual, TDefValuesReadModel> _parametersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingControllerBase{TEntity, TFilingHeader, TDefValueManual, TDefValuesReadModel}"/> class
        /// </summary>
        /// <param name="procedureService">The filing procedure service</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="filingSectionsRepository">Filing sections repository</param>
        /// <param name="configurationFactory">Fields configuration builder</param>
        /// <param name="inboundRepository">Inbound records repository</param>
        /// <param name="parametersService">Parameters service for recalculation</param>
        protected FilingControllerBase(
            IFilingService<TEntity> procedureService,
            IFilingHeaderRepository<TFilingHeader> filingHeaderRepository,
            IFilingSectionRepository filingSectionsRepository,
            IFilingConfigurationFactory<TDefValuesReadModel> configurationFactory,
            IValidationRepository<TEntity> inboundRepository,
            IFilingParametersService<TDefValueManual, TDefValuesReadModel> parametersService)
        {
            _procedureService = procedureService;
            _filingHeaderRepository = filingHeaderRepository;
            _filingSectionsRepository = filingSectionsRepository;
            _configurationFactory = configurationFactory;
            _inboundRepository = inboundRepository;
            _parametersService = parametersService;
        }

        /// <summary>
        /// Starts the file procedure for inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("validate-records")]
        [PermissionRequired]
        public IHttpActionResult ValidateRecords([FromBody] int[] ids)
        {
            CheckPermissions(FilePermission);

            using (new MonitoredScope("Pre-file validation process"))
            {
                var validationResults = new List<PreFilingValidationResult>(ids.Length);

                var records = _inboundRepository.GetList(ids).ToList();
                _inboundRepository.Validate(records);

                var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(ids)
                    .Where(x => x.JobStatus != null && x.JobStatus != JobStatus.Open).ToList();
                validationResults.AddRange(records.Where(x => !x.ValidationPassed).Select(x =>
                    new PreFilingValidationResult { Id = x.Id, Error = "Validation failed", ErrorType = PreFilingValidationErrorType.ValidationFailed }));
                foreach (TEntity record in records.Where(x => x.ValidationPassed))
                {
                    TFilingHeader header = filingHeaders.FirstOrDefault(x => x.GetInboundRecordIds().Any(y => y == record.Id));

                    if (header != null)
                    {
                        if (!header.CanBeRefiled)
                        {
                            validationResults.Add(new PreFilingValidationResult { Id = record.Id, Error = "Missing Job Number", ErrorType = PreFilingValidationErrorType.MissingJobNumber });
                            continue;
                        }

                        if (!header.CanBeEdited)
                        {
                            validationResults.Add(new PreFilingValidationResult { Id = record.Id, Error = "Invalid status", ErrorType = PreFilingValidationErrorType.InvalidStatus });
                            continue;
                        }
                    }

                    validationResults.Add(new PreFilingValidationResult { Id = record.Id });
                }

                return Ok(validationResults);
            }
        }

        /// <summary>
        /// Starts the file procedure for inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("start")]
        [PermissionRequired]
        public virtual IHttpActionResult Start([FromBody] int[] ids)
        {
            CheckPermissions(FilePermission);

            using (new MonitoredScope("Starting Filing process"))
            {
                // Let's find existing filing headers
                var foundHeaders = new List<int>();
                var idsToProcess = new List<int>();
                var refilingHeaders = new List<int>();

                var records = _inboundRepository.GetList(ids).ToList();
                _inboundRepository.Validate(records);

                var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(ids)
                    .Where(x => x.JobStatus != null && x.JobStatus != JobStatus.Open).ToList();
                foreach (TEntity record in records.Where(x => x.ValidationPassed))
                {
                    TFilingHeader currentHeader = filingHeaders.FirstOrDefault(x => x.GetInboundRecordIds().Any(y => y == record.Id));

                    if (currentHeader == null)
                    {
                        idsToProcess.Add(record.Id);
                    }
                    else if (currentHeader.CanBeEdited && !foundHeaders.Contains(currentHeader.Id))
                    {
                        foundHeaders.Add(currentHeader.Id);
                        if (currentHeader.CanBeRefiled)
                        {
                            refilingHeaders.Add(currentHeader.Id);
                        }
                    }
                }

                if (refilingHeaders.Any())
                {
                    _procedureService.Refile(refilingHeaders, CurrentUser.Id);
                }

                if (idsToProcess.Any())
                {
                    OperationResultWithValue<int[]> newFilingHeaders =
                        _procedureService.CreateSingleFilingFilingHeaders(idsToProcess, CurrentUser.Id);
                    if (newFilingHeaders.IsValid)
                    {
                        foundHeaders.AddRange(newFilingHeaders.Value);
                    }
                    else
                    {
                        return ReturnOperationResult(newFilingHeaders);
                    }
                }

                var result = new OperationResultWithValue<int[]> { Value = foundHeaders.ToArray() };
                return ReturnOperationResult(result);
            }
        }

        /// <summary>
        /// Returns IHttpActionResult result based on the provided data
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="result">Data to check</param>
        private IHttpActionResult ReturnOperationResult<T>(OperationResultWithValue<T> result)
        {
            if (!result.IsValid)
            {
                return BadRequest(string.Join(" ;", result.Errors));
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Validates the filing header by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        [HttpGet]
        [Route("validate/{filingHeaderId:int}")]
        [PermissionRequired]
        public bool ValidateFilingHeaderId(int filingHeaderId)
        {
            CheckPermissions(FilePermission);

            TFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);

            return filingHeader != null && filingHeader.CanBeEdited;
        }

        /// <summary>
        /// Validates the list of the filing headers
        /// </summary>
        /// <param name="filingHeadersId">List of the filing header identifiers</param>
        [HttpPost]
        [Route("validate")]
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public bool ValidateFilingHeaderIds(IEnumerable<int> filingHeadersId)
        {
            IEnumerable<TFilingHeader> filingHeaders = _filingHeaderRepository.GetList(filingHeadersId);

            return filingHeaders.Any() && filingHeaders.All(x => x.CanBeEdited);
        }

        /// <summary>
        /// Cancel Filing process for records specified by filing header identifiers
        /// </summary>
        /// <param name="ids">The filing header identifier list</param>
        [HttpPost]
        [Route("cancel")]
        [PermissionRequired]
        public IHttpActionResult CancelFilingProcess(IEnumerable<int> ids)
        {
            CheckPermissions(FilePermission);

            using (new MonitoredScope("Cancel Filing process"))
            {
                IEnumerable<TFilingHeader> filingHeaders;
                using (new MonitoredScope("Getting Filing Header"))
                {
                    filingHeaders = _filingHeaderRepository.GetList(ids);
                    if (!filingHeaders.Any())
                    {
                        return BadRequest(ValidationMessages.FilingHeadersDoNotExist);
                    }
                }
                using (new MonitoredScope("Validating Filing Header for Cancel action"))
                {
                    if (filingHeaders.Any(x => !x.CanBeCanceled))
                    {
                        return BadRequest(ValidationMessages.SystemCanNotPerformCancelAction);
                    }
                }
                using (new MonitoredScope("Canceling Filing process"))
                {
                    _procedureService.CancelFilingProcess(ids.ToArray());

                    return Ok();
                }
            }
        }

        /// <summary>
        /// Gets list of record identifiers for the specified list of the filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the filing header identifiers</param>
        [HttpPost]
        [Route("record-ids")]
        [PermissionRequired]
        public IEnumerable<int> GetInboundRecordIdsByFilingHeaders(IEnumerable<int> filingHeaderIds)
        {
            CheckPermissions(ViewPermission);

            return _filingHeaderRepository.GetList(filingHeaderIds).SelectMany(x => x.GetInboundRecordIds());
        }

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="filingHeaderId">Filing Header id</param>
        [HttpGet]
        [Route("configuration/{filingHeaderId:int}")]
        [PermissionRequired]
        public IHttpActionResult GetAllSections(int filingHeaderId)
        {
            CheckPermissions(ViewPermission);

            using (new MonitoredScope("Prepare configuration for Filing Header"))
            {
                return Ok(_configurationFactory.CreateConfiguration(filingHeaderId));
            }
        }

        /// <summary>
        /// Add new record to the specified section, filing header and parent section
        /// </summary>
        /// <param name="filingHeaderId">Filing Header Id</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="parentRecordId">Parent Section Id</param>
        [HttpPost]
        [Route("configuration/{filingHeaderId:int}/{sectionName}/{parentRecordId:int}")]
        [PermissionRequired]
        public IHttpActionResult AddConfigurationRecord(int filingHeaderId, string sectionName, int parentRecordId)
        {
            CheckPermissions(FilePermission);

            using (new MonitoredScope("Add new record to configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Add new record' action"))
                {
                    TFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.JobStatus == JobStatus.InProgress || filingHeader.JobStatus == JobStatus.Created)
                    {
                        return BadRequest(ValidationMessages.SystemCanNotPerformOperation);
                    }
                }
                Guid operationId;
                using (new MonitoredScope("Add new section"))
                {
                    operationId = _filingSectionsRepository.AddSectionRecord(sectionName, filingHeaderId, parentRecordId, CurrentUser.Id);
                }
                using (new MonitoredScope("Prepare fields configuration for created section"))
                {
                    return Ok(_configurationFactory.CreateConfigurationForSection(filingHeaderId, sectionName, operationId));
                }
            }
        }

        /// <summary>
        /// Add new record to the specified section, filing header and parent section
        /// </summary>
        /// <param name="filingHeaderId">Filing Header Id</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record id</param>
        [HttpGet]
        [Route("configuration/{filingHeaderId:int}/{sectionName}/{recordId:int}")]
        [PermissionRequired]
        public IHttpActionResult GetConfiguration(int filingHeaderId, string sectionName, int recordId)
        {
            using (new MonitoredScope("Get record to configuration"))
            {
                using (new MonitoredScope("Prepare fields configuration for specified section"))
                {
                    return Ok(_configurationFactory.CreateConfigurationForSection(filingHeaderId, sectionName, recordId));
                }
            }
        }

        /// <summary>
        /// Delete record with specified id from the specified section
        /// </summary>
        /// <param name="filingHeaderId">Filing Header Id</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record Id</param>
        [HttpDelete]
        [Route("configuration/{filingHeaderId:int}/{sectionName}/{recordId:int}")]
        [PermissionRequired]
        public IHttpActionResult DeleteConfigurationRecord(int filingHeaderId, string sectionName, int recordId)
        {
            CheckPermissions(FilePermission);

            using (new MonitoredScope("Delete record from configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Delete record' action"))
                {
                    TFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.JobStatus == JobStatus.InProgress || filingHeader.JobStatus == JobStatus.Created)
                    {
                        return BadRequest(ValidationMessages.SystemCanNotPerformOperation);
                    }
                }

                using (new MonitoredScope("Delete specified section"))
                {
                    _filingSectionsRepository.DeleteSectionRecord(sectionName, recordId);
                }

                return Ok(true);
            }
        }

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="model">Current values</param>
        [HttpPost]
        [Route("process_changes")]
        [PermissionRequired]
        public IHttpActionResult ProcessChanges([FromBody] InboundRecordFileModel model)
        {
            CheckPermissions(FilePermission);

            try
            {
                InboundRecordFilingParameters parameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                return Ok(_parametersService.ProcessChanges(parameters));
            }
            catch
            {
                return BadRequest("Recalculation error. Please provide valid data or contact administrator.");
            }
        }

        /// <summary>
        /// Returns permission for filing
        /// </summary>
        protected abstract int FilePermission { get; }

        /// <summary>
        /// Returns permission for view
        /// </summary>
        protected abstract int ViewPermission { get; }
    }
}
