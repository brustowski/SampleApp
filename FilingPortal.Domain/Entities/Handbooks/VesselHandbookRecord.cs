using System.Collections.Generic;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Vessel handbook record
    /// </summary>
    public class VesselHandbookRecord: Entity
    {
        /// <summary>
        /// Vessel name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of associated vessel imports
        /// </summary>
        public virtual ICollection<VesselImportRecord> VesselImports { get; set; }

        /// <summary>
        /// List od corresponding vessel exports
        /// </summary>
        public virtual ICollection<VesselExportRecord> VesselExports { get; set; }
    }
}
