using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Recon.Domain.Config
{
    /// <summary>
    /// Contains grid names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "ReconGridNames")]
    public static class GridNames
    {
        /// <summary>
        /// All grids prefix
        /// </summary>
        private const string Prefix = "recon" + "_";

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string CargoWiseRecords = Prefix + "cargo_wise_records";

        /// <summary>
        /// The FTA records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string FtaRecords = Prefix + "fta_records";

        /// <summary>
        /// The Value records grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string ValueRecords = Prefix + "value_records";

        /// <summary>
        /// The Ace Report grid name definition
        /// </summary>
        [TsProperty]
        public static readonly string AceReport = Prefix + "ace_report";


    }
}
