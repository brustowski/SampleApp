using System.Collections.Generic;

namespace FilingPortal.Parts.Common.Domain.DTOs
{
    /// <summary>
    /// Class describing parameters needed for File procedure
    /// </summary>
    public class InboundRecordFilingParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFilingParameters"/> class
        /// </summary>
        public InboundRecordFilingParameters()
        {
            Parameters = new List<InboundRecordParameter>();
        }

        /// <summary>
        /// Gets or sets the filing header identifier
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Parameters
        /// </summary>
        public IEnumerable<InboundRecordParameter> Parameters { get; set; }
    }
}