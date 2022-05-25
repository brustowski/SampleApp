using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Recon.Domain.Config
{
    /// <summary>
    /// Contains report names
    /// </summary>
    [TsClass(IncludeNamespace = false, Name = "ReconReportNames")]
    public static class ReportNames
    {
        /// <summary>
        /// Reports prefix
        /// </summary>
        private static string prefix = "recon" + "_";

        /// <summary>
        /// The inbound internal report
        /// </summary>
        [TsProperty]
        public static readonly string CargoWiseInternal = prefix + "cargo_wise_internal";

        /// <summary>
        /// The inbound Client FTA report
        /// </summary>
        [TsProperty]
        public static readonly string CargoWiseClientFta = prefix + "cargo_wise_client_fta";

        /// <summary>
        /// The inbound Client Value report
        /// </summary>
        [TsProperty]
        public static readonly string CargoWiseClientValue = prefix + "cargo_wise_client_value";

        /// <summary>
        /// The Association Recon Entry report
        /// </summary>
        [TsProperty]
        public static readonly string AssociationReconEntryReport = prefix + "entry_association";

        /// <summary>
        /// The Entry Value Recon report
        /// </summary>
        [TsProperty]
        public static readonly string ValueReconEntryReport = prefix + "value_entry_report";

        /// <summary>
        /// The Entry Line Value Recon report
        /// </summary>
        [TsProperty]
        public static readonly string ValueReconEntryLineReport = prefix + "value_entry_line_report";

    }
}
