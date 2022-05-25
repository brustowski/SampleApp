using FilingPortal.Domain;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Models;
using Framework.Domain.Paging;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller that provides actions for Rail Inbound Records 
    /// </summary>
    [RoutePrefix("api/inbound/rail/filing")]
    public class FilingProcedureController : ApiControllerBase
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly IFileProcedureService _procedureService;

        /// <summary>
        /// The filing parameter service
        /// </summary>
        private readonly IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel> _parametersService;

        /// <summary>
        /// The inbound record validator
        /// </summary>
        private readonly IRailImportRecordsFilingValidator _selectedInboundRecordValidator;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IRailFilingHeadersRepository _filingHeaderRepository;

        /// <summary>
        /// Rail filing configuration factory
        /// </summary>
        private readonly IFilingConfigurationFactory<RailDefValuesManualReadModel> _configurationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingProcedureController"/> class
        /// </summary>
        /// <param name="selectedInboundRecordValidator">The inbound record validator</param>
        /// <param name="procedureService">The filing procedure service</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="configurationFactory">Configuration factory</param>
        /// <param name="parametersService">The filing parameters service</param>
        public FilingProcedureController(
            IRailImportRecordsFilingValidator selectedInboundRecordValidator,
            IFileProcedureService procedureService,
            IRailFilingHeadersRepository filingHeaderRepository,
            IFilingConfigurationFactory<RailDefValuesManualReadModel> configurationFactory,
            IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel> parametersService)
        {
            _procedureService = procedureService;
            _selectedInboundRecordValidator = selectedInboundRecordValidator;
            _filingHeaderRepository = filingHeaderRepository;
            _configurationFactory = configurationFactory;
            _parametersService = parametersService;
        }

        /// <summary>
        /// Starts the file procedure for inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("start")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult Start([FromBody] int[] ids)
        {
            using (new MonitoredScope("Starting Filing process"))
            {
                // Let's find existing filing headers
                var foundHeaders = new List<int>();
                var idsToProcess = new List<int>();

                var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(ids).Where(x => x.CanBeEdited).ToList();
                foreach (var id in ids)
                {
                    RailFilingHeader currentHeader = filingHeaders.FirstOrDefault(x => x.RailBdParseds.Any(y => y.Id == id));
                    if (currentHeader == null)
                    {
                        idsToProcess.Add(id);
                    }
                    else if (!foundHeaders.Contains(currentHeader.Id))
                    {
                        foundHeaders.Add(currentHeader.Id);
                    }
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
        /// Starts the Unit Train file procedure for inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("start-unit-train")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult StartUnitTrain([FromBody] int[] ids)
        {
            using (new MonitoredScope("Starting Filing process"))
            {
                using (new MonitoredScope("Creating Unit Train filing header"))
                {
                    using (new MonitoredScope("Validating selected records"))
                    {
                        InboundRecordValidationResult validationResult = _selectedInboundRecordValidator.Validate(ids);
                        if (!validationResult.IsValid)
                        {
                            return BadRequest(validationResult.CommonError);
                        }
                    }

                    OperationResultWithValue<int> result = _procedureService.CreateUnitTrainFilingHeader(ids, CurrentUser.Id);
                    return ReturnOperationResult(result);
                }
            }
        }

        private IHttpActionResult ReturnOperationResult<T>(OperationResultWithValue<T> result)
        {
            if (!result.IsValid)
            {
                return BadRequest(string.Join(" ;", result.Errors));
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Starts the file procedure for rail import records provided by filter set and excluded records id
        /// </summary>
        /// <param name="filters">List of the filters with excluded records id</param>
        [HttpPost]
        [Route("start-filtered")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult StartFiltered([FromBody]FilterSetWithExludedRecordsId filters)
        {
            if (filters.ExcludedRecordsId != null)
            {
                foreach (var id in filters.ExcludedRecordsId)
                {
                    var fb = FilterBuilder.CreateFor<RailInboundReadModel>(x => x.Id);
                    Filter filter = fb.AddValue(id, string.Empty).Operand(FilterOperands.NotEqual).Build();
                    filters.Filters.Add(filter);
                }
            }
            using (new MonitoredScope("Creating Single Filing filing headers"))
            {
                OperationResultWithValue<int[]> result = _procedureService.CreateSingleFilingFilingHeaders(filters);
                return ReturnOperationResult(result);
            }
        }

        /// <summary>
        /// Starts the unit train file procedure for rail import records provided by filter set and excluded records id
        /// </summary>
        /// <param name="filters">List of the filters with excluded records id</param>
        [HttpPost]
        [Route("start-unit-train-filtered")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult StartUnitTrainFiltered([FromBody]FilterSetWithExludedRecordsId filters)
        {
            if (filters.ExcludedRecordsId != null)
            {
                foreach (var id in filters.ExcludedRecordsId)
                {
                    var fb = FilterBuilder.CreateFor<RailInboundReadModel>(x => x.Id);
                    Filter filter = fb.AddValue(id, string.Empty).Operand(FilterOperands.NotEqual).Build();
                    filters.Filters.Add(filter);
                }
            }

            using (new MonitoredScope("Creating Unit Train filing header"))
            {
                using (new MonitoredScope("Validating selected records"))
                {
                    InboundRecordValidationResult validationResult = _selectedInboundRecordValidator.Validate(filters);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.CommonError);
                    }
                }

                OperationResultWithValue<int> result = _procedureService.CreateUnitTrainFilingHeader(filters);
                return ReturnOperationResult(result);
            }
        }

        /// <summary>
        /// Validates the filing header by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        [HttpGet]
        [Route("validate/{filingHeaderId:int}")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public bool ValidateFilingHeaderId(int filingHeaderId)
        {
            using (new MonitoredScope("Validating Filing Header"))
            {
                RailFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);

                return filingHeader != null && filingHeader.CanBeEdited;
            }
        }

        /// <summary>
        /// Validates the list of the filing headers
        /// </summary>
        /// <param name="filingHeadersId">List of the filing header identifiers</param>
        [HttpPost]
        [Route("validate")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public bool ValidateFilingHeaderIds(IEnumerable<int> filingHeadersId)
        {
            using (new MonitoredScope("Validating Filing Headers"))
            {
                IEnumerable<RailFilingHeader> filingHeaders = _filingHeaderRepository.GetList(filingHeadersId);

                return filingHeaders.Any() && filingHeaders.All(x => x.CanBeEdited);
            }
        }

        /// <summary>
        /// Cancel Filing process for records specified by filing header identifiers
        /// </summary>
        /// <param name="ids">The filing header identifier list</param>
        [HttpPost]
        [Route("cancel")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult CancelFilingProcess(IEnumerable<int> ids)
        {
            using (new MonitoredScope("Cancel Filing process"))
            {
                IEnumerable<RailFilingHeader> filingHeaders;
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
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public IEnumerable<int> GetInboundRecordIdsByFilingHeaders(IEnumerable<int> filingHeaderIds)
            => _filingHeaderRepository.GetList(filingHeaderIds).SelectMany(x => x.RailBdParseds.Select(r => r.Id));

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="filingHeaderId">Filing Header id</param>
        [HttpGet]
        [Route("configuration/{filingHeaderId:int}")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public IHttpActionResult GetAllSections(int filingHeaderId)
        {
            using (new MonitoredScope("Prepare configuration for Filing Header"))
            {
                return Ok(_configurationFactory.CreateConfiguration(filingHeaderId));
            }
        }

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="model">Current values</param>
        [HttpPost]
        [Route("process_changes")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult ProcessChanges([FromBody] InboundRecordFileModel model)
        {
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
        /// Add new record to the specified section, filing header and parent section
        /// </summary>
        /// <param name="filingHeaderId">Filing Header Id</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="parentRecordId">Parent Section Id</param>
        [HttpPost]
        [Route("configuration/{filingHeaderId:int}/{sectionName}/{parentRecordId:int}")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult AddConfigurationRecord(int filingHeaderId, string sectionName, int parentRecordId)
        {
            using (new MonitoredScope("Add new record to configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Add new record' action"))
                {
                    RailFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.MappingStatus == MappingStatus.InProgress || filingHeader.MappingStatus == MappingStatus.Mapped ||
                        filingHeader.FilingStatus == FilingStatus.InProgress || filingHeader.FilingStatus == FilingStatus.Filed)
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
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public IHttpActionResult DeleteConfigurationRecord(int filingHeaderId, string sectionName, int recordId)
        {
            using (new MonitoredScope("Delete record from configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Delete record' action"))
                {
                    RailFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
                    if (filingHeader.MappingStatus == MappingStatus.InProgress || filingHeader.MappingStatus == MappingStatus.Mapped ||
                        filingHeader.FilingStatus == FilingStatus.InProgress || filingHeader.FilingStatus == FilingStatus.Filed)
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
    }
}