using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Entry.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "ZonesEntry06GridNames")]
    public class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private const string Prefix = "zones_entry_06" + "_";

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
