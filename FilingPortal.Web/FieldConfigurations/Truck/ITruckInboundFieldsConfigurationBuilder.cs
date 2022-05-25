namespace FilingPortal.Web.FieldConfigurations.Truck
{
    using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
    /// <summary>
    /// Interface for service that builds configuration of Truck Inbound Record fields
    /// </summary>
    public interface ITruckInboundFieldsConfigurationBuilder
    {
        /// <summary>
        /// Builds configuration using the specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        InboundRecordFieldConfiguration Build(int filingHeaderId);
    }
}