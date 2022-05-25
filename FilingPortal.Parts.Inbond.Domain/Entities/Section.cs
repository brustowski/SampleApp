using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Defines The Section Entity
    /// </summary>
    public class Section : BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual Section Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<Section> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<DefValue> Fields { get; set; }
    }
}
