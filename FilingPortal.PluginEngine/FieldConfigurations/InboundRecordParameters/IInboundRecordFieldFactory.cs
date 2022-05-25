using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Interface for Inbound Record field description creation
    /// </summary>
    public interface IInboundRecordFieldFactory
    {
        /// <summary>
        /// Creates the field sections from the list of predefined RailDefValue
        /// </summary>
        /// <param name="railDefValuesManual">The list of predefined RailDefValue</param>
        IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<RailDefValuesManualReadModel> railDefValuesManual);

        /// <summary>
        /// Creates fields description from def values
        /// </summary>
        /// <param name="models">Def values</param>
        IEnumerable<BaseInboundRecordField> CreateFrom(IEnumerable<RailDefValuesManualReadModel> models);
    }
}