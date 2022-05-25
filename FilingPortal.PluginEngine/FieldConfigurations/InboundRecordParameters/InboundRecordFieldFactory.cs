using FilingPortal.Domain.Entities.Rail;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Class for Inbound Record field description creation
    /// </summary>
    public class InboundRecordFieldFactory : IInboundRecordFieldFactory
    {
        /// <summary>
        /// Rail fields builder
        /// </summary>
        private readonly IInboundRecordFieldBuilder<RailDefValuesManualReadModel> _fieldBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFieldFactory"/> class
        /// </summary>
        /// <param name="fieldBuilder">Rail fields builder</param>
        public InboundRecordFieldFactory(IInboundRecordFieldBuilder<RailDefValuesManualReadModel> fieldBuilder)
        {
            _fieldBuilder = fieldBuilder;
        }
        /// <summary>
        /// Creates the field sections from the list of predefined RailDefValues
        /// </summary>
        /// <param name="railDefValuesManual">The list of predefined RailDefValues</param>
        public IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<RailDefValuesManualReadModel> railDefValuesManual)
        {
            return railDefValuesManual
                .GroupBy(railDefValue => railDefValue.SectionTitle)
                .Select(rdvGroup => CreateSection(rdvGroup.Key, rdvGroup)).OrderBy(g => g.SectionName);
        }

        /// <summary>
        /// Creates fields description from def values
        /// </summary>
        /// <param name="models">Def values</param>
        public IEnumerable<BaseInboundRecordField> CreateFrom(IEnumerable<RailDefValuesManualReadModel> models) =>
            _fieldBuilder.CreateFrom(models);

        /// <summary>
        /// Creates the section using the specified section name and list of predefined RailDefValues
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="railDefValuesManual">The list of predefined RailDefValues</param>
        private InboundRecordFieldSection CreateSection(string sectionName, IEnumerable<RailDefValuesManualReadModel> railDefValuesManual)
        {
            return new InboundRecordFieldSection
            {
                SectionName = sectionName,
                Fields = CreateFrom(railDefValuesManual.OrderBy(x => x.DisplayOnUI)).ToList()
            };
        }
    }
}
