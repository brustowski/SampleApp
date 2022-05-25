using FilingPortal.Domain;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Web.Common;
using Framework.Infrastructure;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller that provides filing actions for Rail Inbound Records
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class FilingController : Controller
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly IFileProcedureService _procedureService;
        /// <summary>
        /// The document filing procedure service
        /// </summary>
        private readonly IFilingHeaderDocumentUpdateService<RailDocumentDto> _documentUpdateService;
        /// <summary>
        /// Model validator checks field values are correct
        /// </summary>
        private readonly IDefValuesManualValidator<RailDefValuesManualReadModel> _modelValidator;

        /// <summary>
        /// The inbound record validator
        /// </summary>
        private readonly IListInboundValidator<RailInboundReadModel> _selectedInboundRecordValidator;

        private readonly IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel>
            _filingParametersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingController" /> class
        /// </summary>
        /// <param name="procedureService">The procedure service</param>
        /// <param name="selectedInboundRecordValidator">The inbound record validator</param>
        /// <param name="documentUpdateService">The document filing procedure service</param>
        /// <param name="modelValidator">Model contents validator</param>
        /// <param name="filingParametersService">Filing parameters service</param>
        public FilingController(IFileProcedureService procedureService,
            IListInboundValidator<RailInboundReadModel> selectedInboundRecordValidator,
            IFilingHeaderDocumentUpdateService<RailDocumentDto> documentUpdateService,
            IDefValuesManualValidator<RailDefValuesManualReadModel> modelValidator, 
            IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel> filingParametersService)
        {
            _procedureService = procedureService;
            _selectedInboundRecordValidator = selectedInboundRecordValidator;
            _documentUpdateService = documentUpdateService;
            _modelValidator = modelValidator;
            _filingParametersService = filingParametersService;
        }

        /// <summary>
        /// Files Rail Inbound records by the specified model
        /// </summary>
        /// <param name="models">Models to file</param>
        public ActionResult File(InboundRecordFileModel[] models)
        {
            var results = new FilingResultBuilder();

            var dtos = AutoMapper.Mapper.Map<InboundRecordFilingParameters[]>(models);

            var validationResults = _modelValidator.ValidateUserModels(dtos);

            foreach (InboundRecordFileModel model in models)
            {
                using (new MonitoredScope("File Rail Import records"))
                {
                    using (new MonitoredScope("Validate Rail Import records"))
                    {
                        var errorMessage = _selectedInboundRecordValidator.ValidateRecordsForFiling(model.FilingHeaderId);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            results.AddBadResult(model, errorMessage).AddErrorMessage(errorMessage);
                            continue;
                        }

                        if (!ModelState.IsValid)
                        {
                            var message = string.Join("; ",
                        ModelState.SelectMany(c => c.Value.Errors.Select(x => x.ErrorMessage)).Distinct());
                            results.AddBadResult(model, message).AddErrorMessage(message);
                            continue;
                        }

                        var validationResult = validationResults.First(x => x.Key.FilingHeaderId == model.FilingHeaderId).Value;
                        if (!validationResult.IsValid)
                        {
                            results.AddBadResult(model, ErrorMessages.InvalidFilingParameterValue);
                            continue;
                        }
                    }
                    using (new MonitoredScope("Save and File Rail Import records"))
                    {
                        try
                        {
                            InboundRecordFilingParameters filingParameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                            _filingParametersService.UpdateFilingParameters(filingParameters);
                            _documentUpdateService.UpdateForFilingHeader(model.FilingHeaderId,
                                model.Documents.Map<List<InboundRecordDocumentEditModel>, List<RailDocumentDto>>());
                            _procedureService.File(new[] { filingParameters.FilingHeaderId });
                            results.AddResult(model);
                        }
                        catch (Exception exception)
                        {
                            AppLogger.Error(exception, ErrorMessages.ProcessingRecordErrorMessage);
                            results.AddBadResult(model, ErrorMessages.ProcessingRecordErrorMessage).AddErrorMessage(ErrorMessages.ProcessingRecordErrorMessage);
                        }
                    }
                }
            }
            
            return Json(results);
        }


        /// <summary>
        /// Save Rail Inbound records by the specified model
        /// </summary>
        /// <param name="models">The models to save</param>
        public ActionResult Save(InboundRecordFileModel[] models)
        {
            var results = new FilingResultBuilder();

            foreach (InboundRecordFileModel model in models)
            {
                using (new MonitoredScope("Save Rail Import records"))
                {
                    try
                    {
                        var error = _selectedInboundRecordValidator.ValidateBeforeSave(model.FilingHeaderId);
                        if (!string.IsNullOrEmpty(error))
                        {
                            results.AddBadResult(model, error);
                            break;
                        }

                        InboundRecordFilingParameters filingParameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                        _filingParametersService.UpdateFilingParameters(filingParameters);

                        _documentUpdateService.UpdateForFilingHeader(model.FilingHeaderId,
                            model.Documents.Map<List<InboundRecordDocumentEditModel>, List<RailDocumentDto>>());
                        _procedureService.SetInReview(new[] { model.FilingHeaderId });
                        results.AddResult(model);
                    }
                    catch (Exception exception)
                    {
                        AppLogger.Error(exception, ErrorMessages.ProcessingRecordErrorMessage);
                        results.AddBadResult(model, ErrorMessages.ProcessingRecordErrorMessage);
                    }
                }
            }

            if (!results.IsValid)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return Json(results);
        }
    }
}