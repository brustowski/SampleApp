using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilingPortal.Domain;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller that provides filing actions for Pipeline Inbound Records
    /// </summary>
    public class PipelineFilingController : Controller
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly IPipelineFilingService _filingService;
        /// <summary>
        /// The filing parameters service
        /// </summary>
        private readonly IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel> _parametersService;
        /// <summary>
        /// The document filing procedure service
        /// </summary>
        private readonly IPipelineFilingHeaderDocumentUpdateService _documentUpdateService;
        /// <summary>
        /// The inbound record validator
        /// </summary>
        private readonly IListInboundValidator<PipelineInboundReadModel> _listInboundValidator;
        /// <summary>
        /// Model validator checks field values are correct
        /// </summary>
        private readonly IDefValuesManualValidator<PipelineDefValueManualReadModel> _modelValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingController" /> class
        /// </summary>
        /// <param name="filingService">The filing service</param>
        /// <param name="listInboundValidator">The inbound record validator</param>
        /// <param name="documentUpdateService">The document filing procedure service</param>
        /// <param name="modelValidator">Model contents validator</param>
        /// <param name="parametersService">The filing parameters service</param>
        public PipelineFilingController(
            IPipelineFilingService filingService,
            IListInboundValidator<PipelineInboundReadModel> listInboundValidator,
            IPipelineFilingHeaderDocumentUpdateService documentUpdateService,
            IDefValuesManualValidator<PipelineDefValueManualReadModel> modelValidator,
            IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel> parametersService)
        {
            _filingService = filingService;
            _listInboundValidator = listInboundValidator;
            _documentUpdateService = documentUpdateService;
            _modelValidator = modelValidator;
            _parametersService = parametersService;
        }

        /// <summary>
        /// Files Pipeline Inbound records by the specified model
        /// </summary>
        /// <param name="models">The model</param>
        public ActionResult File(InboundRecordFileModel[] models)
        {
            var results = new FilingResultBuilder();

            var filingParameters = AutoMapper.Mapper.Map<InboundRecordFilingParameters[]>(models);

            IDictionary<InboundRecordFilingParameters, DetailedValidationResult> validationResults = _modelValidator.ValidateUserModels(filingParameters);

            foreach (InboundRecordFileModel model in models)
            {
                var errorMessage =
                _listInboundValidator.ValidateRecordsForFiling(model.FilingHeaderId);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    results.AddBadResult(model, errorMessage).AddErrorMessage(errorMessage);
                    break;
                }

                if (!ModelState.IsValid)
                {
                    var message = string.Join("; ",
                        ModelState.SelectMany(c => c.Value.Errors.Select(x => x.ErrorMessage)).Distinct());
                    results.AddBadResult(model, message).AddErrorMessage(message);
                    continue;
                }

                DetailedValidationResult validationResult = validationResults.First(x => x.Key.FilingHeaderId == model.FilingHeaderId).Value;
                if (!validationResult.IsValid)
                {
                    results.AddBadResult(model, ErrorMessages.InvalidFilingParameterValue);
                    continue;
                }

                try
                {
                    InboundRecordFilingParameters parameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                    _parametersService.UpdateFilingParameters(parameters);
                    _documentUpdateService.UpdateForFilingHeader(model.FilingHeaderId,
                        model.Documents.Map<List<InboundRecordDocumentEditModel>, List<PipelineDocumentDto>>(), true);
                    if (parameters.Parameters.Any())
                    {
                        _documentUpdateService.UpdateApiCalculator(parameters);
                    }
                    _filingService.File(parameters.FilingHeaderId);
                    results.AddResult(model);
                }
                catch (Exception exception)
                {
                    AppLogger.Error(exception, ErrorMessages.ProcessingRecordErrorMessage);
                    results.AddBadResult(model, ErrorMessages.ProcessingRecordErrorMessage).AddErrorMessage(ErrorMessages.ProcessingRecordErrorMessage);
                }
            }

            return Json(results);
        }


        /// <summary>
        /// Files Pipeline Inbound records by the specified model
        /// </summary>
        /// <param name="models">The model</param>
        public ActionResult Save(InboundRecordFileModel[] models)
        {
            var results = new FilingResultBuilder();

            foreach (InboundRecordFileModel model in models)
            {
                try
                {
                    var error = _listInboundValidator.ValidateBeforeSave(model.FilingHeaderId);
                    if (!string.IsNullOrEmpty(error))
                    {
                        results.AddBadResult(model, error);
                        break;
                    }

                    if (!ModelState.IsValid && ModelState.SelectMany(c => c.Value.Errors.Select(x => x.ErrorMessage))
                        .Distinct().Any(c => ValidationMessages.DocumentTypeIsRequired.Equals(c)))
                    {
                        results.AddBadResult(model, ErrorMessages.ProcessingRecordErrorMessage);
                        break;
                    }

                    InboundRecordFilingParameters parameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                    _parametersService.UpdateFilingParameters(parameters);
                    _documentUpdateService.UpdateForFilingHeader(model.FilingHeaderId,
                        model.Documents.Map<List<InboundRecordDocumentEditModel>, List<PipelineDocumentDto>>());
                    if (parameters.Parameters.Any())
                    {
                        _documentUpdateService.UpdateApiCalculator(parameters);
                    }
                    _filingService.SetInReview(model.FilingHeaderId);
                    results.AddResult(model);
                }
                catch (Exception exception)
                {
                    AppLogger.Error(exception, ErrorMessages.ProcessingRecordErrorMessage);
                    results.AddBadResult(model, ErrorMessages.ProcessingRecordErrorMessage);
                }
            }

            if (!results.IsValid)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return Json(results);
        }

        /// <summary>
        /// Generates API Calculators for models
        /// </summary>
        /// <param name="model">Form model</param>
        public ActionResult GenerateApiCalculator(InboundRecordFileModel model)
        {
            try
            {
                InboundRecordFilingParameters parameters = model.Map<InboundRecordFileModel, InboundRecordFilingParameters>();
                if (parameters.Parameters.Any())
                {
                    return new FileContentResult(_documentUpdateService.GenerateApiCalculator(parameters).Content,
                        MimeMapping.GetMimeMapping(".xml"));
                }
            }
            catch (Exception exception)
            {
                AppLogger.Error(exception, ErrorMessages.ProcessingRecordErrorMessage);
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            return null;
        }

    }
}