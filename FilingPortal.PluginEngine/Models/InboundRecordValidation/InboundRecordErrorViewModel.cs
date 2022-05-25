using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Models.InboundRecordValidation
{
    /// <summary>
    /// Class describing model with particular errors occured for single Inbound Record
    /// </summary>
    public class InboundRecordErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordErrorViewModel"/> class
        /// </summary>
        public InboundRecordErrorViewModel()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Gets or sets the Inbound Record identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the list of errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
}