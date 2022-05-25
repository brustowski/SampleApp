namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Consignee-Importer Edit Model
    /// </summary>
    public class PipelineRuleConsigneeImporterEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Importer name
        /// </summary>
        public string ImporterFromTicket { get; set; }

        /// <summary>
        /// Cargowise importer code
        /// </summary>
        public string ImporterCode { get; set; }
    }
}
