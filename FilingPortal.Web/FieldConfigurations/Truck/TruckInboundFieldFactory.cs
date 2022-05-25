using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Truck
{
    /// <summary>
    /// Factory for the Truck Inbound Record field description creation
    /// </summary>
    public class TruckInboundFieldFactory : ITruckInboundFieldFactory
    {
        /// <summary>
        /// The value type converter
        /// </summary>
        private readonly IValueTypeConverter _valueTypeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundFieldFactory"/> class
        /// </summary>
        /// <param name="valueTypeConverter">The value type converter</param>
        public TruckInboundFieldFactory(IValueTypeConverter valueTypeConverter)
        {
            _valueTypeConverter = valueTypeConverter;
        }
        /// <summary>
        /// Creates Truck Inbound Record field description from TruckDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined TruckDefValue</param>
        public BaseInboundRecordField CreateFrom(TruckDefValueManualReadModel defValuesManualReadModel)
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
        /// Creates the field sections from the list of predefined TruckDefValues
        /// </summary>
        /// <param name="railDefValuesManual">The list of predefined TruckDefValues</param>
        public IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<TruckDefValueManualReadModel> railDefValuesManual)
        {
            return railDefValuesManual
                .GroupBy(railDefValue => railDefValue.SectionTitle)
                .Select(rdvGroup => CreateSection(rdvGroup.Key, rdvGroup)).OrderBy(g=> g.SectionName);
        }

        /// <summary>
        /// Creates the section using the specified section name and list of predefined TruckDefValues
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="railDefValuesManual">The list of predefined TruckDefValues</param>
        private InboundRecordFieldSection CreateSection(string sectionName, IEnumerable<TruckDefValueManualReadModel> railDefValuesManual)
        {
            return new InboundRecordFieldSection
            {
                SectionName = sectionName,
                Fields = railDefValuesManual.OrderBy(x => x.DisplayOnUI).Select(CreateFrom).ToList()
            };
        }
    }
}
