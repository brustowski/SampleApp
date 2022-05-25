using FilingPortal.Parts.Common.Domain.Validators;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Describes methods that validate records specified by filter set 
    /// </summary>
    public interface IFilteredRecordsValidator
    {
        /// <summary>
        /// Determines whether records specified by filter set is valid
        /// </summary>
        /// <param name="filtersSet">List of filters</param>
        InboundRecordValidationResult Validate(FiltersSet filtersSet);

    }
}
