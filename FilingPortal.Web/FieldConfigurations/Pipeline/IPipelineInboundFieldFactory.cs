using System.Collections.Generic;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    /// <summary>
    /// Interface for Inbound Record field description creation
    /// </summary>
    public interface IPipelineInboundFieldFactory
    {
        /// <summary>
        /// Creates Pipeline Inbound Record field description from PipelineDefValue
        /// </summary>
        /// <param name="defValuesManualReadModel">The predefined PipelineDefValue</param>
        BaseInboundRecordField CreateFrom(PipelineDefValueManualReadModel defValuesManualReadModel);

        /// <summary>
        /// Creates the field sections from the list of predefined DefValue
        /// </summary>
        /// <param name="defValuesManual">The list of predefined defValue</param>
        IEnumerable<InboundRecordFieldSection> CreateSectionsFrom(IEnumerable<PipelineDefValueManualReadModel> defValuesManual);
    }
}