using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Models.TruckExport;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.TruckExport
{
    /// <summary>
    /// Validator for <see cref="TruckExportRuleExporterConsigneeEditModel"/>
    /// </summary>
    public class TruckExportRuleExporterConsigneeEditModelValidator : AbstractValidator<TruckExportRuleExporterConsigneeEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleExporterConsigneeEditModelValidator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        /// <param name="addressRepository">The Client Address repository</param>
        /// <param name="contactsRepository">The Client Contact repository</param>
        public TruckExportRuleExporterConsigneeEditModelValidator(IClientRepository clientRepository
            , IClientAddressRepository addressRepository
            , IClientContactsRepository contactsRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(VM.USPPIIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(clientRepository.IsExist).WithMessage(VM.InvalidUSPPI);
            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(VM.ConsigneeCodeIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(clientRepository.IsExist).WithMessage(VM.InvalidConsignee);
            RuleFor(x => x.Address)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.GetByCode(x) != null).WithMessage(VM.InvalidAddress);
            RuleFor(x => x.Contact)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || contactsRepository.GetByCode(x) != null).WithMessage(VM.InvalidContact);
            RuleFor(x => x.Phone)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.TranRelated)
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1)); ;
        }
    }
}