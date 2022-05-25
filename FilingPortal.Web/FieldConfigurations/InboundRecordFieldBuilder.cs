using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Builds inbound record fields
    /// </summary>
    public class InboundRecordFieldBuilder<TDefValuesManualReadModel> : IInboundRecordFieldBuilder<TDefValuesManualReadModel>
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Value types converter
        /// </summary>
        private readonly IValueTypeConverter _valueTypeConverter;
        /// <summary>
        /// Rules for complex fields
        /// </summary>
        private readonly IComplexFieldsRule<TDefValuesManualReadModel> _complexFieldsRule;
        /// <summary>
        /// Rules for dropdown fields
        /// </summary>
        private readonly IEnumerable<IDropdownFieldRule<TDefValuesManualReadModel>> _dropdownRules;

        /// <summary>
        /// Creates new instance of field builder
        /// </summary>
        /// <param name="valueTypeConverter">Value types converter</param>
        /// <param name="complexFieldsRule">Complex field rule</param>
        /// <param name="dropdownRules">Rules for dropdown fields</param>
        public InboundRecordFieldBuilder(IValueTypeConverter valueTypeConverter,
            IComplexFieldsRule<TDefValuesManualReadModel> complexFieldsRule,
            IEnumerable<IDropdownFieldRule<TDefValuesManualReadModel>> dropdownRules)
        {
            _valueTypeConverter = valueTypeConverter;
            _complexFieldsRule = complexFieldsRule;
            _dropdownRules = dropdownRules;
        }

        /// <summary>
        /// Creates fields description from def values
        /// </summary>
        /// <param name="models">Def values</param>
        public IEnumerable<BaseInboundRecordField> CreateFrom(IEnumerable<TDefValuesManualReadModel> models)
        {
            var modelsList = models.ToList();
            foreach (TDefValuesManualReadModel model in modelsList)
            {
                BaseInboundRecordField recordField = CreateFrom(model, modelsList);
                if (recordField != null)
                {
                    yield return recordField;
                }
            }
        }

        /// <summary>
        /// Creates Inbound Record field description from RailDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined RailDefValue</param>
        protected virtual InboundRecordField ConvertModel(TDefValuesManualReadModel defValuesManualReadModel)
        {
            return new InboundRecordField
            {
                Id = defValuesManualReadModel.Id,
                RecordId = defValuesManualReadModel.RecordId,
                ParentRecordId = defValuesManualReadModel.ParentRecordId,
                FilingHeaderId = defValuesManualReadModel.FilingHeaderId,
                Title = defValuesManualReadModel.Label,
                DefaultValue = defValuesManualReadModel.Value,
                IsMandatory = defValuesManualReadModel.Mandatory,
                IsDisabled = !defValuesManualReadModel.Editable,
                Type = _valueTypeConverter.Convert(defValuesManualReadModel.ValueType),
                MaxLength = defValuesManualReadModel.ValueMaxLength ?? 0,
                DependOn = defValuesManualReadModel.DependsOn,
                ConfirmationNeeded = defValuesManualReadModel.ConfirmationNeeded,
                MarkedForFiltering = defValuesManualReadModel.ConfirmationNeeded || defValuesManualReadModel.Mandatory
            };
        }

        private InboundRecordField CreateSimpleModel(TDefValuesManualReadModel defValuesManualReadModel)
        {
            InboundRecordField model = ConvertModel(defValuesManualReadModel);
            if (model.Type == UIValueTypes.Address)
            {
                AddressInboundRecordField addressModel = model.Map<InboundRecordField, AddressInboundRecordField>();
                model = addressModel;
            }
            else
            {
                IDropdownFieldRule<TDefValuesManualReadModel> dropdownRule = _dropdownRules.FirstOrDefault(x => x.IsDropdownField(defValuesManualReadModel));
                if (dropdownRule != null)
                {
                    DropdownInboundRecordField dropdownModel = model.Map<InboundRecordField, DropdownInboundRecordField>();
                    dropdownModel.ProviderName = defValuesManualReadModel.HandbookName;
                    dropdownModel.IsDynamicProvider = true;
                    model = dropdownModel;
                }
            }

            return model;
        }

        /// <summary>
        /// Creates Inbound Record field description from RailDefValue
        /// </summary>
        /// <param name="mainModel">The predefined RailDefValue</param>
        /// <param name="pairedModel">Paired model</param>
        private BaseInboundRecordField CreateComplexModel(TDefValuesManualReadModel mainModel, TDefValuesManualReadModel pairedModel) =>
            new ComplexInboundRecordField(CreateSimpleModel(mainModel), CreateSimpleModel(pairedModel));

        /// <summary>
        /// Creates fields description from def value
        /// </summary>
        /// <param name="model">Def value</param>
        /// <param name="allModels">Def values pool</param>
        public BaseInboundRecordField CreateFrom(TDefValuesManualReadModel model, IList<TDefValuesManualReadModel> allModels)
        {
            var ruleFound = false;
            if (_complexFieldsRule.IsPairedField(model, allModels) || _complexFieldsRule.IsComplexField(model))
            {
                if (_complexFieldsRule.IsPairedField(model, allModels) && allModels.Any(x => _complexFieldsRule.IsComplexField(x)))
                {
                    // Rule is already applied or will be applied later for this field. Skipping.
                    return null;
                }

                if (_complexFieldsRule.IsComplexField(model) && allModels.Any(x => _complexFieldsRule.IsPairedField(x, allModels)
                                                                                   && x.GetUniqueData() == new DefValuesUniqueData(model.PairedFieldTable, model.PairedFieldColumn)))
                {
                    // Rule found. Converting to complex field
                    ruleFound = true;
                }
            }

            if (!ruleFound)
            {
                return CreateSimpleModel(model);
            }

            TDefValuesManualReadModel pairedField = allModels.FirstOrDefault(x =>
                _complexFieldsRule.IsPairedField(x, allModels) && x.GetUniqueData() ==
                new DefValuesUniqueData(model.PairedFieldTable, model.PairedFieldColumn));

            return CreateComplexModel(model, pairedField);
        }
    }
}