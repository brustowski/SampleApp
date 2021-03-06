using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FilingPortal.Domain;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Framework.Infrastructure;
using Permissions = FilingPortal.Parts.Rail.Export.Domain.Enums.Permissions;

namespace FilingPortal.Parts.Rail.Export.Web.Controllers
{
    /// <summary>
    /// Controller that provides actions for Inbound Records 
    /// </summary>
    [RoutePrefix("api/us/export/rail/filing")]
    public class UsRailExportFilingProcedureController : ApiControllerBase
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly IFilingService<InboundRecord> _procedureService;

        /// <summary>
        /// The filing parameters service
        /// </summary>
        private readonly IFilingParametersService<DefValueManual, DefValueManualReadModel> _parametersService;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IFilingHeadersRepository _filingHeaderRepository;

        /// <summary>
        /// Filing configuration factory
        /// </summary>
        private readonly IFilingConfigurationFactory<DefValueManualReadModel> _configurationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsRailExportFilingProcedureController"/> class
        /// </summary>
        /// <param name="procedureService">The filing procedure service</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="configurationFactory">Fields configuration builder</param>
        /// <param name="parametersService">The filing parameters service</param>
        public UsRailExportFilingProcedureController(
            IFilingService<InboundRecord> procedureService,
            IFilingHeadersRepository filingHeaderRepository,
            IFilingConfigurationFactory<DefValueManualReadModel> configurationFactory,
            IFilingParametersService<DefValueManual, DefValueManualReadModel> parametersService)
        {
            _procedureService = procedureService;
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
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult Start([FromBody] int[] ids)
        {
            using (new MonitoredScope("Starting Filing process"))
            {
                // Let's find existing filing headers
                var foundHeaders = new List<int>();
                var idsToProcess = new List<int>();

                var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(ids).Where(x => x.CanBeEdited).ToList();
                foreach (int id in ids)
                {
                    FilingHeader currentHeader = filingHeaders.FirstOrDefault(x => x.InboundRecords.Any(y => y.Id == id));
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
        [PermissionRequired(Permissions.FileInboundRecord)]
        public bool ValidateFilingHeaderId(int filingHeaderId)
        {
            FilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);

            return filingHeader != null && filingHeader.CanBeEdited;
        }

        /// <summary>
        /// Validates the list of the filing headers
        /// </summary>
        /// <param name="filingHeadersId">List of the filing header identifiers</param>
        [HttpPost]
        [Route("validate")]
        [PermissionRequired(Permissions.FileInboundRecord)]
        public bool ValidateFilingHeaderIds(IEnumerable<int> filingHeadersId)
        {
            IEnumerable<FilingHeader> filingHeaders = _filingHeaderRepository.GetList(filingHeadersId);

            return filingHeaders.Any() && filingHeaders.All(x => x.CanBeEdited);
        }

        /// <summary>
        /// Cancel Filing process for records specified by filing header identifiers
        /// </summary>
        /// <param name="ids">The filing header identifier list</param>
        [HttpPost]
        [Route("cancel")]
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult CancelFilingProcess(IEnumerable<int> ids)
        {
            using (new MonitoredScope("Cancel Filing process"))
            {
                IEnumerable<FilingHeader> filingHeaders;
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
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public IEnumerable<int> GetInboundRecordIdsByFilingHeaders(IEnumerable<int> filingHeaderIds)
        {
            return _filingHeaderRepository.GetList(filingHeaderIds).SelectMany(x => x.InboundRecords.Select(t => t.Id));
        }

        /// <summary>
        /// Gets all available filing data grouped by sections
        /// </summary>
        /// <param name="filingHeaderId">Filing Header id</param>
        [HttpGet]
        [Route("configuration/{filingHeaderId:int}")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
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
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult ProcessChanges([FromBody] InboundRecordFileModel model)
        {
            try
            {
                InboundRecordFilingParameters parameters =
                    model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
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
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult AddConfigurationRecord(int filingHeaderId, string sectionName, int parentRecordId)
        {
            using (new MonitoredScope("Add new record to configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Add new record' action"))
                {
                    FilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
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
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult DeleteConfigurationRecord(int filingHeaderId, string sectionName, int recordId)
        {
            using (new MonitoredScope("Delete record from configuration"))
            {
                using (new MonitoredScope("Validating Filing Header for 'Delete record' action"))
                {
                    FilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
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

        /// <summary>
        /// Tries to confirm filing header status
        /// </summary>
        /// <param name="confirmationRequest">Confirmation Request</param>
        [HttpPost]
        [Route("confirm")]
        [PermissionRequired(Permissions.FileInboundRecord)]
        public IHttpActionResult Confirmation(FilingHeaderConfirmation[] confirmationRequest)
        {
            string monitoringMessage =
                $"Update confirmation status for filing header ids: {string.Join(", ", confirmationRequest.Select(x => x.FilingHeaderId))}";

            using (new MonitoredScope(monitoringMessage))
            {
                foreach (FilingHeaderConfirmation filingHeaderConfirmation in confirmationRequest)
                {
                    FilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderConfirmation.FilingHeaderId);
                    if (filingHeader != null && filingHeader.CanBeConfirmed)
                    {
                        filingHeader.Confirmed = filingHeaderConfirmation.Confirmed;
                        _filingHeaderRepository.Save();

                        filingHeaderConfirmation.Confirmed = filingHeader.Confirmed;
                    }
                    else
                    {
                        filingHeaderConfirmation.Confirmed = false;
                    }
                }
            }

            return Ok(confirmationRequest);
        }
    }
}