using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ZonesFtz214DataProviderNames")]
    internal static class DataProviderNames
    {
        private const string Prefix = "zones_ftz_214" + "_";

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
