namespace FilingPortal.Parts.Common.Domain.DTOs
{
    /// <summary>
    /// Class describing inbound record parameter
    /// </summary>
    public class InboundRecordParameter
    {
        /// <summary>
        /// Gets or sets the identifier of Inbound Record parameter description
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