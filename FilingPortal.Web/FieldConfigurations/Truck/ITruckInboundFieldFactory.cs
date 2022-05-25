using System.Collections.Generic;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Truck
{
    /// <summary>
    /// Interface for Inbound Record field description creation
    /// </summary>
    public interface ITruckInboundFieldFactory
    {
        /// <summary>
        /// Creates Truck Inbound Record field description from TruckDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined TruckDefValue</param>
        BaseInboundRecordField CreateFrom(TruckDefValueManualReadModel defValuesManualReadModel);

        /// <summary>
        /// Creates the field sections from the list of predefined DefValue
        /// </summary>
        /// <param name="defValuesManual">The list of predefined defValue</param>
        IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<TruckDefValueManualReadModel> defValuesManual);
    }
}