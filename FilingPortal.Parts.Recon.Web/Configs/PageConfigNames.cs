using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Recon.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ReconPageConfigNames")]
    internal class PageConfigNames
    {
        private static readonly string prefix = "recon" + "_";
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
        /// The single FTA record actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string FtaActions = prefix + "FtaActionsConfiguration";
        
        /// <summary>
        /// The FTA list actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string FtaListActions = prefix + "FtaListActionsConfiguration";

        /// <summary>
        /// View FTA records page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string FtaViewPageActions = prefix + "FtaViewPageActionsConfiguration";

        /// <summary>
        /// The single Value record actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string ValueActions = prefix + "ValueActionsConfiguration";
        /// <summary>
        /// The Value list actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string ValueListActions = prefix + "ValueListActionsConfiguration";

        /// <summary>
        /// View Value records page actions configuration name
        /// </summary>
        [TsProperty]
        public static readonly string ValueViewPageActions = prefix + "ValueViewPageActionsConfiguration";
    }
}
