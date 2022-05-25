using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "CanadaImpTruckPageConfigNames")]
    internal class PageConfigNames
    {
        private static readonly string prefix = "canada_imp_truck" + "_";
        /// <summary>
        /// The single inbound record actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InboundActions = prefix + "InboundActionsConfiguration";

        /// <summary>
        /// The inbound list actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InboundListActions = prefix + "InboundListActionsConfiguration";

        /// <summary>
        /// View inbound records page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InboundViewPageActions = prefix + "InboundViewPageActionsConfiguration";

        /// <summary>
        /// The rules page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string RulesPageActions = prefix + "RulesPageActionsConfiguration";

        /// <summary>
        /// Rule record actions configuration name
        /// </summary>
        [TsProperty]
        public static string RulesRecordActions = prefix + "RulesRecordActionsConfiguration";
    }
}
