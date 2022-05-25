using System.ComponentModel;

namespace FilingPortal.Domain.Enums
{
    /// <summary>
    /// Represent freight types
    /// </summary>
    public enum FreightType
    {
        /// <summary>
        /// Freight is count for each container
        /// </summary>
        [Description("per CNT")]
        PerContainer,
        /// <summary>
        /// Freight is count for each unit
        /// </summary>
        [Description("per UOM")]
        PerUom
    }
}
