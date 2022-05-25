using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    /// <summary>
    /// Class for Pipeline Inbound Record field description creation
    /// </summary>
    public class PipelineInboundFieldFactory : IPipelineInboundFieldFactory
    {
        /// <summary>
        /// The value type converter
        /// </summary>
        private readonly IValueTypeConverter _valueTypeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundFieldFactory"/> class
        /// </summary>
        /// <param name="valueTypeConverter">The value type converter</param>
        public PipelineInboundFieldFactory(IValueTypeConverter valueTypeConverter)
        {
            _valueTypeConverter = valueTypeConverter;
        }
        /// <summary>
        /// Creates Pipeline Inbound Record field description from PipelineDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined PipelineDefValue</param>
        public BaseInboundRecordField CreateFrom(PipelineDefValueManualReadModel defValuesManualReadModel)
        {
            return new InboundRecordField
            {
                Id = defValuesManualReadModel.Id,
                FilingHeaderId = defValuesManualReadModel.FilingHeaderId,
                Title = defValuesManualReadModel.Label,
                DefaultValue = defValuesManualReadModel.Value,
                IsMandatory = defValuesManualReadModel.Mandatory,
                Type = _valueTypeConverter.Convert(defValuesManualReadModel.ValueType),
                MaxLength = defValuesManualReadModel.ValueMaxLength ?? 0
            };
        }

        /// <summary>
        /// Creates the field sections from the list of predefined PipelineDefValues
        /// </summary>
        /// <param name="defValuesManual">The list of predefined PipelineDefValues</param>
        public IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<PipelineDefValueManualReadModel> defValuesManual)
        {
            return defValuesManual
                .GroupBy(railDefValue => railDefValue.SectionTitle)
                .Select(rdvGroup => CreateSection(rdvGroup.Key, rdvGroup)).OrderBy(g=> g.SectionName);
        }

        /// <summary>
        /// Creates the section using the specified section name and list of predefined PipelineDefValues
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="railDefValuesManual">The list of predefined PipelineDefValues</param>
        private InboundRecordFieldSection CreateSection(string sectionName, IEnumerable<PipelineDefValueManualReadModel> railDefValuesManual)
        {
            return new InboundRecordFieldSection
            {
                SectionName = sectionName,
                Fields = railDefValuesManual.OrderBy(x => x.DisplayOnUI).Select(CreateFrom).ToList()
            };
        }
    }
}
