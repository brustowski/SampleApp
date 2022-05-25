using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ZonesFtz214PageConfigNames")]
    internal class PageConfigNames
    {
        private const string Prefix = "zones_ftz_214" + "_";

        /// <summary>
        /// The single inbound record actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InboundActions = Prefix + "InboundActionsConfiguration";

        /// <summary>
        /// View inbound records page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string InboundViewPageActions = Prefix + "InboundViewPageActionsConfiguration";
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
