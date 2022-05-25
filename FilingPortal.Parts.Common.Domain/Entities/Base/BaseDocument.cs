using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Defines the file description stored in database
    /// </summary>
    public abstract class BaseDocument : Entity
    {
        /// <summary>
        /// Gets or sets the creation date of model
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets who created the model
        /// </summary>
        public string CreatedUser { get; set; }

        /// <summary>
        /// Gets or sets the Document Type
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the file description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the file extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Filing Header id
        /// </summary>
        public int? FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets the inbound Record id
        /// </summary>
        public int? InboundRecordId { get; set; }
    }
}
