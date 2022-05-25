namespace FilingPortal.Parts.Zones.Ftz214.Domain.Reporting.Inbound
{
    /// <summary>
    /// Represents the Zones FTZ 214 Report Model
    /// </summary>
    internal class InboundReportModel
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Applicant
        /// </summary>
        public string Applicant { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }
        /// <summary>
        /// Gets or sets Ftz Operator
        /// </summary>
        public string FtzOperator { get; set; }
        /// <summary>
        /// Gets or sets ZoneId
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Gets of sets the Admission Type
        /// </summary>
        public string AdmissionType { get; set; }
    }
}