using System.ComponentModel;

namespace FilingPortal.Parts.Common.Domain.Enums
{
    /// <summary>
    /// Specify client types
    /// </summary>
    public enum ClientType : byte
    {
        /// <summary>
        /// Empty Client type
        /// </summary>
        [Description("None")]
        None = 0,
        /// <summary>
        /// Importer Client type
        /// </summary>
        [Description("Importer")]
        Importer = 1,
        /// <summary>
        /// Supplier Client type
        /// </summary>
        [Description("Supplier")]
        Supplier = 2,
    }
}
