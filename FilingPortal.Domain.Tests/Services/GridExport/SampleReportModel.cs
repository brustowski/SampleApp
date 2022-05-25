using System;

namespace FilingPortal.Domain.Tests.Services.GridExport
{
    internal class SampleReportModel
    {
        public string NotMappedName { get; set; }

        public decimal NotMappedValue { get; set; }
        public decimal? NotMappedNullableValue { get; set; }

        public DateTime NotMappedDate { get; set; }
        public DateTime? NotMappedNullableDate { get; set; }

        public decimal SomeLongWord { get; set; }
        public decimal ABBREVIATION { get; set; }

        public string OverridenName { get; set; }
        public decimal OverridenValue { get; set; }

        public string Ignored { get; set; }
    }
}
