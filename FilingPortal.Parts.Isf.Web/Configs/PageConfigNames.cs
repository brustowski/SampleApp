using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "IsfPageConfigNames")]
    internal class PageConfigNames
    {
        private static readonly string prefix = "isf" + "_";
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
    }
}
