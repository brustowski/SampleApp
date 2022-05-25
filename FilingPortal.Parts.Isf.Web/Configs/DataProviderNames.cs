using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "IsfDataProviderNames")]
    internal static class DataProviderNames
    {
        private static readonly string prefix = "isf" + "_";

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
        /// "BillTypes" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string BillTypes = prefix + "BillTypes";
        /// <summary>
        /// "Form configuration" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string FormConfiguration = prefix + "FormConfiguration";
    }
}
