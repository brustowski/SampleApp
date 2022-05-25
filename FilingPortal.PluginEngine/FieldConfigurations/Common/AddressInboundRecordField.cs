using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Implements dropdown record field
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class AddressInboundRecordField : InboundRecordField
    {
        /// <summary>
        /// Dropdown values provider name
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// Gets or sets whether provider is database-driven
        /// </summary>
        public bool IsDynamicProvider { get; set; }
    }
}