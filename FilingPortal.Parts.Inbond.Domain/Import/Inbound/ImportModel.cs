using FilingPortal.Domain.Common.Parsing;
using System.Globalization;

namespace FilingPortal.Parts.Inbond.Domain.Import.Inbound
{
    /// <summary>
    /// Represents Inbond Import parsing data model
    /// </summary>
    public class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// The FIRMs code
        /// </summary>
        private string _firmsCode;
        /// <summary>
        /// Gets or sets the FIRMS code
        /// </summary>
        public string FirmsCode
        {
            get => string.IsNullOrWhiteSpace(_firmsCode) ? _firmsCode : _firmsCode.PadLeft(4, '0');
            set => _firmsCode = value;
        }
        /// <summary>
        /// Gets or sets the Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Port of Arrival
        /// </summary>
        public string PortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets Port of Destination
        /// </summary>
        public string PortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets conveyance
        /// </summary>
        public string ExportConveyance { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets value
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Gets or sets manifest quantity
        /// </summary>
        public decimal ManifestQty { get; set; }
        /// <summary>
        /// Gets or sets Manifest quantity unit
        /// </summary>
        public string ManifestQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|", new[] {
                FirmsCode,
                ImporterCode,
                PortOfArrival,
                ExportConveyance,
                ConsigneeCode,
                Carrier,
                Value.ToString(CultureInfo.InvariantCulture),
                ManifestQty.ToString(CultureInfo.InvariantCulture),
                ManifestQtyUnit,
                Weight.ToString(CultureInfo.InvariantCulture)
            });
        }
    }
}
