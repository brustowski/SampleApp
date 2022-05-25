using FilingPortal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Provides base functionality for single record validation
    /// </summary>
    /// <typeparam name="TModel">The record type</typeparam>
    public abstract class BaseSingleRecordValidator<TModel> : ISingleRecordValidator<TModel> where TModel : BaseInboundReadModel
    {
        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record to check</param>
        public List<string> GetErrors(TModel record)
        {
            var errors = new List<string>();

            if (record.IsErrorStatus())
            {
                errors.Add(ErrorMessages.FilingProcessErrorMessage);
            }

            return AddCustomErrors(errors, record);
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(TModel record)
        {
            return Task.Run(() => GetErrors(record));
        }

        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected virtual List<string> AddCustomErrors(List<string> errors, TModel record) => errors;
    }
}
