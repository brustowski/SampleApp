using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "ZonesFtz214GridNames")]
    public class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private const string Prefix = "zones_vtz_214" + "_";

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string InboundRecords = Prefix + "inbound";
        /// <summary>
        /// Default values grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string DefaultValues = Prefix + "default_values";
        /// <summary>
        /// Filing screen grid
        /// </summary>
        [TsProperty]
        public static readonly string FilingGrid = Prefix + "filing_grid";
        /// <summary>
        /// Importer Rule Grid name
        /// </summary>
        [TsProperty]
        public static readonly string ImporterRuleGrid = Prefix + "importer_rule_grid";
    }
}
