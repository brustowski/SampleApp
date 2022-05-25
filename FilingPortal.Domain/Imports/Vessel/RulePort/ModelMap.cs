using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Vessel.RulePort
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

            Map(p => p.FirmsCode, "FIRMs Code");
            Map(p => p.EntryPort, "Entry Port");
            Map(p => p.DischargePort, "Discharge Port");
            Map(p => p.HMF, "HMF");
            Map(p => p.DestinationCode, "Destination Code");
        }
    }
}
