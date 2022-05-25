using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Rail.Export.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "UsExpRailGridNames")]
    public class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private static string prefix = "us_exp_rail" + "_";

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string InboundRecords = prefix + "inbound";
        /// <summary>
        /// Consignee rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RuleConsignee = prefix + "rule_consignee";
        /// <summary>
        /// Exporter-Consignee rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RuleExporterConsignee = prefix + "rule_exporter_consignee";
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

        /// <summary>
        /// Containers screen grid
        /// </summary>
        [TsProperty]
        public static readonly string ContainersGrid = prefix + "containers_grid";
    }
}
