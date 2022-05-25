using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RulePort
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Port Rule");

            Map(p => p.PortOfClearance, "Port Of Clearance");
            Map(p => p.SubLocation, "Sub Location");
            Map(p => p.FirstPortOfArrival, "First Port Of Arrival");
            Map(p => p.FinalDestination, "Final Destination");
        }
    }
}
