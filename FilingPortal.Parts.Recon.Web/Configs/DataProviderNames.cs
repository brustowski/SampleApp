using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Recon.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ReconDataProviderNames")]
    internal static class DataProviderNames
    {
        private static readonly string prefix = "recon" + "_";

        /// <summary>
        /// "TableColumns" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string TableColumns = prefix + "TableColumns";
        /// <summary>
        /// "TableNames" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string TableNames = prefix + "TableNames";
        /// <summary>
        /// "Form configuration" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string FormConfiguration = prefix + "FormConfiguration";
    }
}
