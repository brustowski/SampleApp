using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Inbond.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "ZonesInBondGridNames")]
    public static class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private const string Prefix = "in_bond_";

        /// <summary>
        /// The Inbond records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string InbondRecords = Prefix + "inbound";
        /// <summary>
        /// FIRMs Code rules grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string RuleEntry = Prefix + "rule_entry";
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
    }
}
