using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "IsfGridNames")]
    public class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private static string prefix = "isf" + "_";

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string InboundRecords = prefix + "inbound";
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
