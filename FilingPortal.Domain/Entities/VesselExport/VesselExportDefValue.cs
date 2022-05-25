using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.VesselExport
{
    /// <summary>
    /// Defines the Vessel Export Def Value Entity
    /// </summary>
    public class VesselExportDefValue : BaseDefValueWithSection<VesselExportSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual VesselExportDefValue DependsOn { get; set; }
    }
}
