using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Common.Domain.Validators
{
    /// <summary>
    /// Class describing validation result of the Inbound Record set
    /// </summary>
    public class InboundRecordValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordValidationResult"/> class
        /// </summary>
        public InboundRecordValidationResult()
        {
            RecordErrors = new List<InboundRecordError>();
        }

        /// <summary>
        /// Determines whether there are errors within the result
        /// </summary>
        public bool IsValid => string.IsNullOrEmpty(CommonError) && RecordErrors.All(r => !r.Errors.Any());

        /// <summary>
        /// Gets or sets the common error for Inbound Record set
        /// </summary>
        public string CommonError { get; set; }

        /// <summary>
        /// Gets or sets particular errors for each of the Inbound Records in set
        /// </summary>
        public List<InboundRecordError> RecordErrors { get; set; }
    }
}