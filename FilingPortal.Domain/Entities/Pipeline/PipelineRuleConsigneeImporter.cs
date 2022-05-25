namespace FilingPortal.Domain.Entities.Pipeline
{
    using Framework.Domain;

    /// <summary>
    /// Provides the Pipeline Rule Consignee-Importer Entity
    /// </summary>
    public class PipelineRuleConsigneeImporter : AuditableEntity, IRuleEntity
    {
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
