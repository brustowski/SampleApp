using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines the Vessel Import Default Value Entity
    /// </summary>
    public class VesselImportDefValue : BaseDefValueWithSection<VesselImportSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual VesselImportDefValue DependsOn { get; set; }
    }
}
