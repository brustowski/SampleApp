using FilingPortal.Domain.Common.Parsing;
using System;

namespace FilingPortal.Infrastructure.Tests.Parsing
{
    internal class TestParsingModel : ParsingDataModel
    {
        public Type Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ExportDate { get; set; }
    }

    internal class TestParseModelMap : ParseModelMap<TestParsingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestParseModelMap"/> class.
        /// </summary>
        public TestParseModelMap()
        {
            Sheet("Batch Log");

            Map(p => p.Name, "Name");
            Map(p => p.Price, "Unit Price");
            Map(p => p.ExportDate, "Export Date");
        }
    }
}
