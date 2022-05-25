using System.ComponentModel;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Render control type
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Numeric control
        /// </summary>
        [Description("Number")]
        Number,
        /// <summary>
        /// Text control
        /// </summary>
        [Description("Text")]
        Text,
        /// <summary>
        /// Date control
        /// </summary>
        [Description("Date")]
        Date,
        /// <summary>
        /// Lookup field control
        /// </summary>
        [Description("Lookup")]
        Lookup,
        /// <summary>
        /// Text control
        /// </summary>
        [Description("MultilineText")]
        MultilineText,
        /// <summary>
        /// Table field
        /// </summary>
        [Description("Table")]
        Table,
        /// <summary>
        /// Address field
        /// </summary>
        [Description("Address")]
        Address
    }
}