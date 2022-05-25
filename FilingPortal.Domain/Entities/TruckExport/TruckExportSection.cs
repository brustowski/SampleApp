using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.TruckExport
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines The Truck Export Section Entity
    /// </summary>
    public class TruckExportSection : BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual TruckExportSection Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<TruckExportSection> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<TruckExportDefValue> Fields { get; set; }
    }
}
