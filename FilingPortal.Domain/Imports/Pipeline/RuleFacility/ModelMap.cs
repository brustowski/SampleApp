using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RuleFacility
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
            Sheet("Facility Rule");

            Map(p => p.Facility, "Facility");
            Map(p => p.Port, "Entry/Discharge Port");
            Map(p => p.DestinationState, "Destination State");
            Map(p => p.Origin, "Origin");
            Map(p => p.Destination, "Destination");
            Map(p => p.FIRMsCode, "FIRMs Code");
            Map(p => p.Issuer, "Issuer");
            Map(p => p.Pipeline, "Pipeline");
        }
    }
}
