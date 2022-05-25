using System.Threading.Tasks;

namespace FilingPortal.Domain.Validators
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for single Record validation
    /// </summary>
    public interface ISingleRecordTypedValidator<T, in TModel>
    {
        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        List<T> GetErrors(TModel record);

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        Task<List<T>> GetErrorsAsync(TModel record);
    }
}