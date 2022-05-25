using System;
using System.Web.WebPages;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleVendor
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        /// <param name="unlocoRepository">The UNLOCO repository</param>
        public Validator(IClientRepository clientRepository
            , IUnlocoDictionaryRepository unlocoRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;


            RuleFor(x => x.Vendor)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Vendor"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Vendor");

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Importer");

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Exporter"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Exporter");

            RuleFor(x => x.ExportState)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));

            RuleFor(x => x.DirectShipPlace)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || unlocoRepository.GetByCode(x) != null).WithMessage("Unknown Direct Shipment Place");

            RuleFor(x => x.NoPackages)
                .Must(y => string.IsNullOrWhiteSpace(y) || y.IsInt()).WithMessage(VM.IntegerFormatMismatch);

            RuleFor(x => x.CountryOfOrigin)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));

            RuleFor(x => x.OrgState)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must((rule, orgState) =>
                    string.IsNullOrWhiteSpace(rule.CountryOfOrigin)
                    || !rule.CountryOfOrigin.Equals("US", StringComparison.InvariantCultureIgnoreCase)
                    || (rule.CountryOfOrigin.Equals("US", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(orgState)))
                .WithMessage(string.Format(VM.PropertyRequired, "Org State"));
        }
    }
}