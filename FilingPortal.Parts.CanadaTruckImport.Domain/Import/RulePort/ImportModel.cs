using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RulePort
{
    /// <summary>
    /// Provides Excel file data mapping on Port Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        private string _portOfClearance;

        /// <summary>
        /// Gets or sets Port of Clearance
        /// </summary>
        public string PortOfClearance
        {
            get => string.IsNullOrWhiteSpace(_portOfClearance) ? _portOfClearance : _portOfClearance.PadLeft(4, '0');
            set => _portOfClearance = value;
        }

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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return PortOfClearance;
        }
    }
}
