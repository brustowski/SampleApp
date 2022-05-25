using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Parts.Rail.Export.Web.Configs
{
    /// <summary>
    /// Record fields builder
    /// </summary>
    public class RecordFieldBuilder : InboundRecordFieldBuilder<DefValueManualReadModel>
    {
        private readonly Dictionary<DefValuesUniqueData, string> _prefixConfiguration =
            new Dictionary<DefValuesUniqueData, string>
            {
                { new DefValuesUniqueData("us_exp_rail.invoice_header", "origin_indicator"), "take-a-look" },
                { new DefValuesUniqueData("us_exp_rail.invoice_line", "goods_origin"), "take-a-look" },
            };

        /// <summary>
        /// Creates new instance of field builder
        /// </summary>
        /// <param name="valueTypeConverter">Value types converter</param>
        /// <param name="complexFieldsRule">Rule for complex fields</param>
        /// <param name="dropdownRules">Rules for dropdown fields</param>
        public RecordFieldBuilder(IValueTypeConverter valueTypeConverter, IComplexFieldsRule<DefValueManualReadModel> complexFieldsRule, IEnumerable<IDropdownFieldRule<DefValueManualReadModel>> dropdownRules) : base(valueTypeConverter, complexFieldsRule, dropdownRules)
        {
        }

        /// <summary>
        /// Creates Inbound Record field description from RailDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined RailDefValue</param>
        protected override InboundRecordField ConvertModel(DefValueManualReadModel defValuesManualReadModel)
        {
            var result =  base.ConvertModel(defValuesManualReadModel);
            var uniqueData = defValuesManualReadModel.GetUniqueData();
            if (_prefixConfiguration.ContainsKey(uniqueData))
            {
                result.Class = _prefixConfiguration[uniqueData];
                result.MarkedForFiltering = true;
            }

            return result;
        }
    }
}