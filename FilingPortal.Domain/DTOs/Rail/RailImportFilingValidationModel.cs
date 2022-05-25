using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.DTOs.Rail
{
    class RailImportFilingValidationModel
    {
        public string Importer { get; set; }
        public string Supplier { get; set; }
        public string PortCode { get; set; }
        public string TrainNumber { get; set; }
        public string HTS { get; set; }
        public MappingStatus MappingStatus { get; set; }
        public FilingStatus FilingStatus { get; set; }
    }
}
