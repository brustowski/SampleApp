using FilingPortal.Domain;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Truck;
using FilingPortal.Domain.Validators;
using FilingPortal.Web.Common;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.Controllers.Truck
{
    /// <summary>
    /// Controller that provides filing actions for Truck Inbound Records
    /// </summary>
    public class TruckFilingController : Controller
    {
        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly ITruckFilingService _filingService;

        /// <summary>
        /// The filing parameters service
        /// </summary>
        private readonly IFilingParametersService<TruckDefValueManual, TruckDefValueManualReadModel> _parametersService;

        /// <summary>
        /// The inbound record validator
        /// </summary>
        private readonly IListInboundValidator<TruckInboundReadModel> _listInboundValidator;

        /// <summary>
        /// The document filing procedure service
        /// </summary>
        private readonly IFilingHeaderDocumentUpdateService<TruckDocumentDto> _documentUpdateService;
        
        /// <summary>
        /// Model validator checks field values are correct
        /// </summary>
        private readonly IDefValuesManualValidator<TruckDefValueManualReadModel> _modelValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingController" /> class
        /// </summary>
        /// <param name="filingService">The filing service</param>
        /// <param name="listInboundValidator">The inbound record validator</param>
        /// <param name="documentUpdateService">The document filing procedure service</param>
        /// <param name="modelValidator">Model contents validator</param>
        /// <param name="parametersService">The filing parameters service</param>
        public TruckFilingController(
            ITruckFilingService filingService,
            IListInboundValidator<TruckInboundReadModel> listInboundValidator,
            IFilingHeaderDocumentUpdateService<TruckDocumentDto> documentUpdateService,
            IDefValuesManualValidator<TruckDefValueManualReadModel> modelValidator, 
            IFilingParametersService<TruckDefValueManual, TruckDefValueManualReadModel> parametersService)
        {
            _filingService = filingService;
            _listInboundValidator = listInboundValidator;
            _documentUpdateService = documentUpdateService;
            _modelValidator = modelValidator;
            _parametersService = parametersService;
        }

        /// <summary>
        /// Files Truck Inbound records by the specified model
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
                    continue;
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
                        model.Documents.Map<List<InboundRecordDocumentEditModel>, List<TruckDocumentDto>>());
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
        /// Files Truck Inbound records by the specified model
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
                        model.Documents.Map<List<InboundRecordDocumentEditModel>, List<TruckDocumentDto>>());
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
    }
}