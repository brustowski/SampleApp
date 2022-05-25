using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="InboundReadModel"/> record validation
    /// </summary>
    internal class InboundRecordValidator : BaseSingleRecordValidator<InboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, InboundReadModel record)
        {
            try
            {
                var errorType = new[] {new {Message = ""}};
                var validationResult = JsonConvert.DeserializeAnonymousType(record.ValidationResult, errorType);
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
}
