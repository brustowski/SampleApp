using System.Collections.Generic;

namespace FilingPortal.Parts.Common.Domain.Validators
{
    /// <summary>
    /// Class describing particular errors occured for single Inbound Record
    /// </summary>
    public class InboundRecordError
    {
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