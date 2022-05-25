using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Inbond.Web.Configs
{
    [TsClass(IncludeNamespace = false,Name = "ZonesInBondPageConfigNames")]
    internal static class PageConfigNames
    {
        private const string Prefix = "in_bond_";

        /// <summary>
        /// The single inbond record configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InbondActions = Prefix + "InboundActionsConfiguration";

        /// <summary>
        /// The inbond list record configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InbondListActions = Prefix + "InboundListActionsConfiguration";

        /// <summary>
        /// View Page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InbondViewPageActions = Prefix + "InboundViewPageActionsConfiguration";

        /// <summary>
        /// The rules page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string RulesPageActions = Prefix + "RulesPageActionsConfiguration";

        /// <summary>
        /// Rule record actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string RulesRecordActions = Prefix + "RulesRecordActionsConfiguration";
    }
}
