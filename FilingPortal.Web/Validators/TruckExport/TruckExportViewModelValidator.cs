using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Validators;
using FilingPortal.Web.Models.TruckExport;

namespace FilingPortal.Web.Validators.TruckExport
{
    /// <summary>
    /// Provides methods for single <see cref="TruckExportViewModel"/> record validation
    /// </summary>
    internal class TruckExportViewModelValidator : ISingleRecordValidator<TruckExportViewModel>
    {
        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<string> GetErrors(TruckExportViewModel record)
        {
            var errors = new List<string>();

            errors.Add(record.ValidationResult);

            return errors;
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(TruckExportViewModel record)
        {
            return Task.Run(() => GetErrors(record));
        }
    }
}
