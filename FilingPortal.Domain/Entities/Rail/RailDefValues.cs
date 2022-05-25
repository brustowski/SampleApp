using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Describes Rail Default Values
    /// </summary>
    public class RailDefValues : BaseDefValueWithSection<RailSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual RailDefValues DependsOn { get; set; }
    }
}
