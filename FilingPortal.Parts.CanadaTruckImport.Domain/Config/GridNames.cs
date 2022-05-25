using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "CanadaImpTruckGridNames")]
    public class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private static string prefix = "canada_imp_truck" + "_";

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string InboundRecords = prefix + "inbound";
        /// <summary>
        /// Importer rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RuleVendor = prefix + "rule_vendor";
        /// <summary>
        /// Port rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RulePortRecords = prefix + "rule_port";
        /// <summary>
        /// Carrier rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RuleProduct = prefix + "rule_product";
        /// <summary>
        /// Default values grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string DefaultValues = prefix + "default_values";
        /// <summary>
        /// Filing screen grid
        /// </summary>
        [TsProperty]
        public static readonly string FilingGrid = prefix + "filing_grid";
    }
}
