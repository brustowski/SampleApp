using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.Models.InboundRecordValidation
{
    /// <summary>
    /// Class describing validation result model of the Inbound Record set
    /// </summary>
    public class InboundRecordValidationViewModel
    { 
        /// <summary>
      /// Initializes a new instance of the <see cref="InboundRecordValidationViewModel"/> class
      /// </summary>
        public InboundRecordValidationViewModel()
        {
            RecordErrors = new List<InboundRecordErrorViewModel>();
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
        public List<InboundRecordErrorViewModel> RecordErrors { get; set; }

        /// <summary>
        /// Gets or sets the actions available after the validation
        /// </summary>
        public Actions Actions { get; set; }
    }
}