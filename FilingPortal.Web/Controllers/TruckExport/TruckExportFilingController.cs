﻿using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller that provides actions for Truck Export Records 
    /// </summary>
    [RoutePrefix("api/export/truck/filing")]
    public class TruckExportFilingController : ApiControllerBase
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly ITruckExportFilingService _procedureService;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly ITruckExportFilingHeadersRepository _filingHeaderRepository;

        /// <summary>
        /// Truck export filing configuration factory
        /// </summary>
        private readonly IFilingConfigurationFactory<TruckExportDefValuesManualReadModel> _configurationFactory;

        private readonly ITruckExportAutoRefileService _autoRefileService;
        private readonly ITruckExportRepository _inboundRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingController"/> class
        /// </summary>
        /// <param name="procedureService">The filing procedure service</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="configurationFactory">Fields configuration builder</param>
        /// <param name="autoRefileService">Auto refile service</param>
        public TruckExportFilingController(
            ITruckExportFilingService procedureService,
            ITruckExportFilingHeadersRepository filingHeaderRepository,
            IFilingConfigurationFactory<TruckExportDefValuesManualReadModel> configurationFactory,
            ITruckExportAutoRefileService autoRefileService,
            ITruckExportRepository inboundRepository)
        {
            _procedureService = procedureService;
            _filingHeaderRepository = filingHeaderRepository;
            _configurationFactory = configurationFactory;
            _autoRefileService = autoRefileService;
            _inboundRepository = inboundRepository;
        }

        /// <summary>
        /// Starts the file procedure for inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("validate-records")]
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult ValidateRecords([FromBody] int[] ids)
        {
            using (new MonitoredScope("Truck Export Inbound record pre-file validation process"))
            {
                var validationResults = new List<PreFilingValidationResult>(ids.Length);

                var records = _inboundRepository.GetList(ids).ToList();
                _inboundRepository.Validate(records);

                var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(ids)
                    .Where(x => x.JobStatus != null && x.JobStatus != JobStatus.Open).ToList();
                validationResults.AddRange(records.Where(x => !x.ValidationPassed).Select(x =>
                    new PreFilingValidationResult { Id = x.Id, Error = "Validation failed", ErrorType = PreFilingValidationErrorType.ValidationFailed }));
                foreach (TruckExportRecord record in records.Where(x => x.ValidationPassed))
                {
                    TruckExportFilingHeader header = filingHeaders.FirstOrDefault(x => x.TruckExports.Any(y => y.Id == record.Id));

                    if (header != null)
                    {
                        if (record.IsUpdate && !header.CanBeRefiled)
                        {
                            validationResults.Add(new PreFilingValidationResult { Id = record.Id, Error = "Missing Job Number", ErrorType = PreFilingValidationErrorType.MissingJobNumber });
                            continue;
                        }

                        if (!header.CanBeEdited)
                        {
                            validationResults.Add(new PreFilingValidationResult { Id = record.Id, Error = "Invalid status", ErrorType = PreFilingValidationErrorType.InvalidStatus});
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
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult Start([FromBody] int[] ids)
        {
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
                foreach (TruckExportRecord record in records.Where(x => x.ValidationPassed))
                {
                    TruckExportFilingHeader currentHeader = filingHeaders.FirstOrDefault(x => x.TruckExports.Any(y => y.Id == record.Id));

                    if (currentHeader == null)
                    {
                        idsToProcess.Add(record.Id);
                    }
                    else if (currentHeader.CanBeEdited && !foundHeaders.Contains(currentHeader.Id))
                    {
                        if (record.IsUpdate)
                        {
                            if (currentHeader.CanBeRefiled)
                            {
                                refilingHeaders.Add(currentHeader.Id);
                                foundHeaders.Add(currentHeader.Id);
                            }
                        }
                        else
                        {
                            foundHeaders.Add(currentHeader.Id);
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
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public bool ValidateFilingHeaderId(int filingHeaderId)
        {
            TruckExportFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);

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
            IEnumerable<TruckExportFilingHeader> filingHeaders = _filingHeaderRepository.GetList(filingHeadersId);

            return filingHeaders.Any() && filingHeaders.All(x => x.CanBeEdited);
        }

        /// <summary>
        /// Cancel Filing process for records specified by filing header identifiers
        /// </summary>
        /// <param name="ids">The filing header identifier list</param>
        [HttpPost]
        [Route("cancel")]
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult CancelFilingProcess(IEnumerable<int> ids)
        {
            using (new MonitoredScope("Cancel Filing process"))
            {
                IEnumerable<TruckExportFilingHeader> filingHeaders;
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
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public IEnumerable<int> GetInboundRecordIdsByFilingHeaders(IEnumerable<int> filingHeaderIds)
        {
            return _filingHeaderRepository.GetList(filingHeaderIds).SelectMany(x => x.TruckExports.Select(t => t.Id));
        }

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="filingHeaderId">Filing Header id</param>
        [HttpGet]
        [Route("configuration/{filingHeaderId:int}")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public IHttpActionResult GetAllSections(int filingHeaderId)
        {
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
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult AddConfigurationRecord(int filingHeaderId, string sectionName, int parentRecordId)
        {
            using (new MonitoredScope("Add new record to configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Add new record' action"))
                {
                    TruckExportFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.JobStatus == JobStatus.InProgress || filingHeader.JobStatus == JobStatus.Created)
                    {
                        return BadRequest(ValidationMessages.SystemCanNotPerformOperation);
                    }
                }
                Guid operationId;
                using (new MonitoredScope("Add new section"))
                {
                    operationId = _filingHeaderRepository.AddSectionRecord(sectionName, filingHeaderId, parentRecordId, CurrentUser.Id);
                }
                using (new MonitoredScope("Prepare fields configuration for created section"))
                {
                    return Ok(_configurationFactory.CreateConfigurationForSection(filingHeaderId, sectionName, operationId));
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
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult DeleteConfigurationRecord(int filingHeaderId, string sectionName, int recordId)
        {
            using (new MonitoredScope("Delete record from configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Delete record' action"))
                {
                    TruckExportFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.JobStatus == JobStatus.InProgress || filingHeader.JobStatus == JobStatus.Created)
                    {
                        return BadRequest(ValidationMessages.SystemCanNotPerformOperation);
                    }
                }

                using (new MonitoredScope("Delete specified section"))
                {
                    _filingHeaderRepository.DeleteSectionRecord(sectionName, recordId);
                }

                return Ok(true);
            }
        }

        /// <summary>
        /// Executes auto-file manually
        /// </summary>
        [HttpPost]
        [Route("auto-refile")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public async Task<string> AutoFile()
        {
            return await _autoRefileService.Execute(CurrentUser);
        }

        /// <summary>
        /// Executes auto-file manually
        /// </summary>
        [HttpGet]
        [Route("auto-refile")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public async Task<string> GetAutoFileResults()
        {
            return await _autoRefileService.Execute(CurrentUser);
        }
    }
}