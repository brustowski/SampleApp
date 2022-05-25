namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    using InboundRecordParameters;
    /// <summary>
    /// Interface for service that builds configuration of Pipeline Inbound Record fields
    /// </summary>
    public interface IPipelineInboundFieldsConfigurationBuilder
    {
        /// <summary>
        /// Builds configuration using the specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        InboundRecordFieldConfiguration Build(int filingHeaderId);
    }
}