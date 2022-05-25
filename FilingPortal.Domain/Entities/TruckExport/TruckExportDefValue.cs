using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.TruckExport
{
    /// <summary>
    /// Defines the Truck Export Def Value Entity
    /// </summary>
    public class TruckExportDefValue : BaseDefValueWithSection<TruckExportSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual TruckExportDefValue DependsOn { get; set; }
    }
}
