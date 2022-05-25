using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.RuleConsignee
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
        public Validator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee Code"))
                .Must(clientRepository.IsExist).WithMessage("Unknown client code");

            RuleFor(x => x.Country).MaximumLength(2);
            RuleFor(x => x.UltimateConsigneeType).MaximumLength(1);
            RuleFor(x => x.Destination).MaximumLength(5);
        }

    }
}