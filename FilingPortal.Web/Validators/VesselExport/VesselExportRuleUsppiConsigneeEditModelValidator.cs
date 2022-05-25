using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Web.Models.VesselExport;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.VesselExport
{
    /// <summary>
    /// Validator for <see cref="VesselExportRuleUsppiConsigneeEditModel"/>
    /// </summary>
    public class VesselExportRuleUsppiConsigneeEditModelValidator : AbstractValidator<VesselExportRuleUsppiConsigneeEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRuleUsppiConsigneeEditModelValidator"/> class
        /// </summary>
        public VesselExportRuleUsppiConsigneeEditModelValidator(ILookupMasterRepository<LookupMaster> repository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.UsppiId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "USPPI"));
            RuleFor(x => x.ConsigneeId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee"));
        }
    }
}