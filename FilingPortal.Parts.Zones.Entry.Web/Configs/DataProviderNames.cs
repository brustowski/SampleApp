using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Entry.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ZonesEntryDataProviderNames")]
    internal static class DataProviderNames
    {
        private const string Prefix = "zones_entry" + "_";

        /// <summary>
        /// "TableColumns" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string TableColumns = Prefix + "TableColumns";
        /// <summary>
        /// "TableNames" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string TableNames = Prefix + "TableNames";
        /// <summary>
        /// "Form configuration" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string FormConfiguration = Prefix + "FormConfiguration";
    }
}
