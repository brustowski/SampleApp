using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Web.Models.Vessel;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Vessel
{
    /// <summary>
    /// Vessel inbound form validator
    /// </summary>
    public class VesselImportEditModelValidator : AbstractValidator<VesselImportEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportEditModelValidator"/> class
        /// </summary>
        public VesselImportEditModelValidator(
            ITariffRepository<HtsTariff> tariffRepository,
            IAppUsersRepository usersRepository,
            IFirmsCodesRepository firmsCodesRepository,
            IProductDescriptionsRepository productDescriptionsRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterId)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .Must(y => y.IsGuidFormat()).WithMessage(VM.IdFormatMismatch);

            RuleFor(x => x.SupplierId)
                .Must(y => string.IsNullOrWhiteSpace(y) || y.IsGuidFormat())
                .WithMessage(VM.IdFormatMismatch);

            RuleFor(x => x.ClassificationId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Classification"))
                .Must(y => tariffRepository.Get(y) != null).WithMessage(string.Format(VM.PropertyInvalid, "Classification"));

            RuleFor(x => x.ProductDescriptionId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Product Description"))
                .Must(y => productDescriptionsRepository.Get(y) != null).WithMessage(string.Format(VM.PropertyInvalid, "Product Description"));

            RuleFor(x => x.FirmsCodeId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "FIRMs Code"))
                .Must(y => firmsCodesRepository.Get(y) != null).WithMessage(string.Format(VM.PropertyInvalid, "FIRMs Code"));

            RuleFor(x => x.FilerId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Filer"))
                .Must(y => usersRepository.GetUserInfo(y) != null)
                .WithMessage(string.Format(VM.PropertyInvalid, "Filer"));

            RuleFor(x => x.VesselId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Vessel"));

            RuleFor(x => x.Eta)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "ETA"));

            RuleFor(x => x.Container)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Container"));

            RuleFor(x => x.EntryType)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Entry Type"));

            RuleFor(x => x.CustomsQty)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Customs Qty"));

            RuleFor(x => x.UnitPrice)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Unit Price"));

            RuleFor(x => x.OwnerRef)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Owner Ref"));

            RuleFor(x => x.CountryOfOriginId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Country of Origin"));
        }
    }
}