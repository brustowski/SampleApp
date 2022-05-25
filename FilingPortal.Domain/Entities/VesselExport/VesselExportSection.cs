using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.VesselExport
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines The Vessel Export Section Entity
    /// </summary>
    public class VesselExportSection : BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual VesselExportSection Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<VesselExportSection> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<VesselExportDefValue> Fields { get; set; }
    }
}
