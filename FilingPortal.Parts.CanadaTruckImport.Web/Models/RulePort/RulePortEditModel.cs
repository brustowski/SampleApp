namespace FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort
{
    /// <summary>
    /// Describes Port Rule Edit Model
    /// </summary>
    public class RulePortEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Port of Clearance
        /// </summary>
        public string PortOfClearance { get; set; }
        /// <summary>
        /// Gets or sets sub-location
        /// </summary>
        public string SubLocation { get; set; }
        /// <summary>
        /// Gets or sets First port of arrival
        /// </summary>
        public string FirstPortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets final destination
        /// </summary>
        public string FinalDestination { get; set; }
    }
}