using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Validators.Rail
{
    /// <summary>
    /// Describes Rail Import Records Filing validator
    /// </summary>
    public interface IRailImportRecordsFilingValidator : IListInboundValidator<RailInboundReadModel>, IFilteredRecordsValidator
    {       
    }
}
