using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Rail.RulePort
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

            Map(p => p.Port, "Port");
            Map(p => p.Origin, "Origin");
            Map(p => p.Destination, "Destination");
            Map(p => p.FIRMsCode, "FIRMs Code");
            Map(p => p.Export, "Export");
        }
    }
}
