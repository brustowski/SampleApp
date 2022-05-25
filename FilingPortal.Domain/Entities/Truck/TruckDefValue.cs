using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{
    /// <summary>
    /// Defines the Truck Default Values
    /// </summary>
    public class TruckDefValue : BaseDefValueWithSection<TruckSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual TruckDefValue DependsOn { get; set; }
    }
}
