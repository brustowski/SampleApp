using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Interface for service that builds configuration of Inbound Record fields
    /// </summary>
    public interface IInboundRecordConfigurationBuilder
    {
        /// <summary>
        /// Builds configuration using the specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        InboundRecordFieldConfiguration Build(int filingHeaderId);

        /// <summary>
        /// Builds configuration using the specified filing header identifiers
        /// </summary>
        /// <param name="filingHeaderId"></param>
        InboundRecordFieldConfiguration BuildSingleFiling(int[] filingHeaderId);
    }
}