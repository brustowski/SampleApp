using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using System;
using System.Linq;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.VesselExport.RuleUsppiConsignee
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        private readonly string[] _allowedUltimateConsigneeTypes = { "D", "G", "O", "R" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        /// <param name="addressRepository">The Client address repository</param>
        public Validator(IClientRepository clientRepository, IClientAddressRepository addressRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Usppi)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "USPPI"))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsSupplierValid(x)).WithMessage(VM.InvalidUsppiCode);
            RuleFor(x => x.Consignee)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee"))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsImporterValid(x)).WithMessage(VM.InvalidConsignee);
            RuleFor(x => x.TransactionRelated)
                .Must(x => string.IsNullOrWhiteSpace(x)
                           || x.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                           || x.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                .WithMessage(string.Format(VM.IncorrectValueType, "Y, N"));
            RuleFor(x => x.UltimateConsigneeType)
                .Must(x => string.IsNullOrWhiteSpace(x)
                           || (x.Trim().Length == 1 && _allowedUltimateConsigneeTypes.Any(y => y.Equals(x, StringComparison.InvariantCultureIgnoreCase))))
                .WithMessage(string.Format(VM.IncorrectValueType, string.Join(", ", _allowedUltimateConsigneeTypes)));
            RuleFor(x => x.ConsigneeAddress)
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.Search(x, 1).Count > 0)
                .WithMessage("Invalid address");
        }
    }
}