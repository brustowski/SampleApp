namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Rule Importer Edit model
    /// </summary>
    public class VesselRuleImporterEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the CargoWise Importer
        /// </summary>
        public string CWImporter { get; set; }
    }
}