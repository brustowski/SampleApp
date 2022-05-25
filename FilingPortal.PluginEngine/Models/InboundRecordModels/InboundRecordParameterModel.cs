namespace FilingPortal.PluginEngine.Models.InboundRecordModels
{
    /// <summary>
    /// Class describing model of Inbound Record parameter
    /// </summary>
    public class InboundRecordParameterModel
    {
        /// <summary>
        /// Gets or sets the identifier of Inbound Record parameter
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the parameter linked record id
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets parent record id
        /// </summary>
        public int ParentRecordId { get; set; }
        /// <summary>
        /// Gets or sets the parameter value
        /// </summary>
        public string Value { get; set; }
    }
}