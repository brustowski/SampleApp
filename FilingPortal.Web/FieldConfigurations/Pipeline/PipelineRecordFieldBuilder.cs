using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    /// <summary>
    /// Pipeline record fields builder
    /// </summary>
    public class PipelineRecordFieldBuilder : InboundRecordFieldBuilder<PipelineDefValueManualReadModel>
    {
        private readonly Dictionary<DefValuesUniqueData, string> _prefixConfiguration =
            new Dictionary<DefValuesUniqueData, string>
            {
                { new DefValuesUniqueData("imp_pipeline_invoice_line", "attribute2"), "API @ 60° F = " }
            };

        /// <summary>
        /// Creates new instance of field builder
        /// </summary>
        /// <param name="valueTypeConverter">Value types converter</param>
        /// <param name="complexFieldsRule">Rule for complex fields</param>
        /// <param name="dropdownRules">Rules for dropdown fields</param>
        public PipelineRecordFieldBuilder(IValueTypeConverter valueTypeConverter, IComplexFieldsRule<PipelineDefValueManualReadModel> complexFieldsRule, IEnumerable<IDropdownFieldRule<PipelineDefValueManualReadModel>> dropdownRules) : base(valueTypeConverter, complexFieldsRule, dropdownRules)
        {
        }

        /// <summary>
        /// Creates Inbound Record field description from RailDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined RailDefValue</param>
        protected override InboundRecordField ConvertModel(PipelineDefValueManualReadModel defValuesManualReadModel)
        {
            var result =  base.ConvertModel(defValuesManualReadModel);
            var uniqueData = defValuesManualReadModel.GetUniqueData();
            if (_prefixConfiguration.ContainsKey(uniqueData))
                result.Prefix = _prefixConfiguration[uniqueData] ?? "";

            return result;
        }
    }
}