using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Inbond.Web.Configs
{
    [TsClass(IncludeNamespace = false, Name = "ZonesInBondDataProviderNames")]
    internal static class DataProviderNames
    {
        private const string Prefix = "in_bond_";

        /// <summary>
        /// "TableColumns" data provider name
        /// </summary>
        [TsProperty]
        public const string TableColumns = Prefix + "TableColumns";
        /// <summary>
        /// "TableNames" data provider name
        /// </summary>
        [TsProperty]
        public const string TableNames = Prefix + "TableNames";
        /// <summary>
        /// "MarksRemarksTemplateTypes" data provider name
        /// </summary>
        [TsProperty]
        public const string MarksRemarksTemplateTypes = Prefix + "MarksRemarksTemplateTypes";
        /// <summary>
        /// "InBondCarriers" data provider name
        /// </summary>
        [TsProperty]
        public const string InBondCarriers = Prefix + "InBondCarriers";
        /// <summary>
        /// "Form configuration" data provider name
        /// </summary>
        [TsProperty]
        public static readonly string FormConfiguration = Prefix + "FormConfiguration";
    }
}
