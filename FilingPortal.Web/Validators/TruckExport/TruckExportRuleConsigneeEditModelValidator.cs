using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Web.Models.TruckExport;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.TruckExport
{
    /// <summary>
    /// Validator for <see cref="TruckExportRuleConsigneeEditModel"/>
    /// </summary>
    public class TruckExportRuleConsigneeEditModelValidator : AbstractValidator<TruckExportRuleConsigneeEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleConsigneeEditModelValidator"/> class
        /// </summary>
        public TruckExportRuleConsigneeEditModelValidator(ILookupMasterRepository<LookupMaster> repository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(VM.ConsigneeCodeIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Country)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.Destination)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.UltimateConsigneeType)
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1));
        }
    }
}