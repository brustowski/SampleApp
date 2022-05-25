using System.Linq;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.Models.Fields;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Isf.Web.Validators
{
    /// <summary>
    /// Inbound form validator
    /// </summary>
    public class InboundRecordEditModelValidator : AbstractValidator<InboundRecordEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordEditModelValidator"/> class
        /// </summary>
        public InboundRecordEditModelValidator(IBillsRepository billsRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .Must(y => y.IsGuidFormat()).WithMessage(VM.IdFormatMismatch);
            When(x => x.Manufacturers != null, () =>
            {
                RuleFor(x => x.Manufacturers)
                    .Must(x => x.All(NotEmpty)).WithMessage("Records should not be empty");
            });

            RuleFor(x => x.Bills).NotEmpty().WithMessage("Bills are required");

            When(x => x.Bills != null, () =>
            {
                When(x => x.Bills.Any(b => b.BillType == "MB"), () =>
                {
                    RuleFor(x => x.Bills).Must(x => x.Count(b => b.BillType == "MB") == 1)
                        .WithMessage("Only one Master Bill may be added");
                });
                When(x => x.Bills.Any(b => b.BillType == "OB"), () =>
                {
                    RuleFor(x => x.Bills).Must(x => x.All(b => b.BillType == "OB")).WithMessage("Ocean Bills can not be provided with any other bill types");
                });

                RuleFor(x => x.Bills)
                    .Must(x => x.All(b => !string.IsNullOrWhiteSpace(b.BillNumber)))
                    .WithMessage("Please make sure that all bill numbers have values");
                RuleFor(x => x.Bills)
                    .Must(x => x.All(b => !string.IsNullOrWhiteSpace(b.BillType)))
                    .WithMessage("Please make sure that all bill types have values")
                    ;
                RuleFor(x => x.Bills)
                    .Must(x => x.All(b => b.BillNumber.Length <= 16))
                    .WithMessage("Please make sure that all bill numbers has length less then 16 symbols")
                    ;
                When(
                    x => x.Bills.All(b =>
                        !string.IsNullOrWhiteSpace(b.BillType) && !string.IsNullOrWhiteSpace(b.BillNumber)),
                    () =>
                    {
                        RuleForEach(x => x.Bills)
                            .Must((record,bill) => !billsRepository.Exists(GetBill(record, bill)))
                            .WithMessage((record, bill) =>
                                $"Bill Number {bill.BillNumber} already exists in database");

                        RuleForEach(x => x.Bills)
                            .Must((record, bill) => record.Bills.Count(x =>
                                                        x.BillNumber == bill.BillNumber) == 1)
                            .WithMessage((record, bill) => $"Duplicate bill: {bill.BillNumber}");
                    });
            });

            When(x => x.Containers != null, () =>
            {
                RuleFor(x => x.Containers)
                    .Must(x => x.All(b => !string.IsNullOrWhiteSpace(b.ContainerNumber)))
                    .WithMessage("Please make sure that all container numbers have values");
                RuleFor(x => x.Containers)
                    .Must(x => x.All(b => !string.IsNullOrWhiteSpace(b.ContainerType)))
                    .WithMessage("Please make sure that all container types have values")
                    ;
            });

        }

        private InboundBillRecord GetBill(InboundRecordEditModel record, BillsRecordEditModel bill)
        {
            var dbBill = bill.Map<BillsRecordEditModel, InboundBillRecord>();
            dbBill.InboundRecordId = record.Id;
            return dbBill;
        }

        private bool NotEmpty(InboundManufacturerRecordEditModel manufacturer)
        {
            return
                manufacturer != null &&
                (!string.IsNullOrWhiteSpace(manufacturer.ManufacturerId) ||
                 (manufacturer.Address != null && NotEmpty(manufacturer.Address) ||
                  !string.IsNullOrWhiteSpace(manufacturer.CountryOfOrigin) ||
                  !string.IsNullOrWhiteSpace(manufacturer.HtsNumbers)));
        }

        private bool NotEmpty(AddressFieldEditModel address)
        {
            return address != null &&
                   (!string.IsNullOrWhiteSpace(address.Addr1) ||
                    !string.IsNullOrWhiteSpace(address.Addr2) ||
                    !string.IsNullOrWhiteSpace(address.AddressId) ||
                    !string.IsNullOrWhiteSpace(address.City) ||
                    !string.IsNullOrWhiteSpace(address.CompanyName) ||
                    !string.IsNullOrWhiteSpace(address.CountryCode) ||
                    !string.IsNullOrWhiteSpace(address.PostalCode) ||
                    !string.IsNullOrWhiteSpace(address.StateCode));
        }
    }
}