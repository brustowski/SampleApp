using FilingPortal.Domain.DTOs.Audit.Rail;
using FluentValidation;

namespace FilingPortal.Domain.Validators.Audit.Rail
{
    /// <summary>
    /// Provides validator for <see cref="RailAuditTrainConsistSheetImportParsingModel"/>
    /// </summary>
    public class RailAuditTrainConsistSheetImportParsingModelValidator : AbstractValidator<RailAuditTrainConsistSheetImportParsingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailAuditTrainConsistSheetImportParsingModelValidator"/> class
        /// </summary>
        public RailAuditTrainConsistSheetImportParsingModelValidator()
        {
            RuleFor(x => x.BillNumber).NotNull();
            RuleFor(x => x.EntryNumber).NotNull();

            CascadeMode = CascadeMode.StopOnFirstFailure;
        }
    }
}
