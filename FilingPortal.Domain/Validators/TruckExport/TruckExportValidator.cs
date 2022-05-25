using System;
using FilingPortal.Domain.Entities.TruckExport;
using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure;
using Newtonsoft.Json;


namespace FilingPortal.Domain.Validators.TruckExport
{
    /// <summary>
    /// Provides methods for single <see cref="TruckExportReadModel"/> record validation
    /// </summary>
    internal class TruckExportValidator : BaseSingleRecordValidator<TruckExportReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, TruckExportReadModel record)
        {
            try
            {
                var validationResult = JsonConvert.DeserializeObject<TruckExportErrorMessage[]>(record.ValidationResult);
                errors.AddRange(validationResult.Where(x => !string.IsNullOrWhiteSpace(x.Message))
                    .Select(x => x.Message));
            }
            catch (Exception e)
            {
                errors.Add("Error parsing validation result");
                AppLogger.Error(e, $"Error parsing JSON {record.ValidationResult}");
            }
            
            return errors;
        }
    }

    internal class TruckExportErrorMessage
    {
        public string Message { get; set; }
    }
}
