using Framework.Domain;
using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Represents the raw Rail EDI Message Entity
    /// </summary>
    public class RailEdiMessage : Entity
    {
        /// <summary>
        /// Gets or sets EDI Message Text
        /// </summary>
        public string EmMessageText { get; set; }
        /// <summary>
        /// Gets or sets the CargoWise Last Modified Date
        /// </summary>
        public DateTime CwLastModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the Last Modified Date
        /// </summary>
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the Created Date
        /// </summary>
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the Created User
        /// </summary>
        public string CreatedUser { get; set; } = "suser_name()";

        /// <summary>
        /// Gets or sets the Rail Parsed Broker Download Entities
        /// </summary>
        public virtual ICollection<RailBdParsed> RailBdParseds { get; set; } = new List<RailBdParsed>();
    }

}
