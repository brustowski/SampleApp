using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "CanadaImpTruckDataProviderNames")]
    internal static class DataProviderNames
    {
        private static readonly string prefix = "canada_imp_truck" + "_";

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
        /// "Carriers" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string Carriers = prefix + "Carriers";
        /// <summary>
        /// "ProductCodes" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string ProductCodes = prefix + "ProductCodes";
        /// <summary>
        /// "Form configuration" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string FormConfiguration = prefix + "FormConfiguration";
    }
}
