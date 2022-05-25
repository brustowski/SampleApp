using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines The Vessel Import Section Entity
    /// </summary>
    public class VesselImportSection : BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual VesselImportSection Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<VesselImportSection> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<VesselImportDefValue> Fields { get; set; }
    }
}
