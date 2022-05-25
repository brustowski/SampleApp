namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Batch Code Edit Model
    /// </summary>
    public class PipelineRuleBatchCodeEditModel
    {
        /// <summary>
        /// Gets or sets the Batch Code
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public string Product { get; set; }
    }
}
