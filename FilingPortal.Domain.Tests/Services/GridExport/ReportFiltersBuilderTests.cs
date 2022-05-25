using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Services.GridExport;
using Framework.Domain.Paging;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport
{
    [TestClass]
    public class ReportFiltersBuilderTests
    {
        private readonly IReportFiltersBuilder _reportFiltersBuilder = new ReportFiltersBuilder();
        private IList<Filter> _filters;


        [TestInitialize]
        public void Init()
        {
            _filters = new List<Filter>()
            {
                new Filter() { FieldName = "ImportDate", Operand = FilterOperands.Contains }
            };

            _filters[0].Values.Add(new LookupItem() { DisplayValue = "TestDisplayValue", Value = "TestValue" });
        }

        [TestMethod]
        public void GetRows_WhenFilterOperandIsDateFrom_FieldNameContainsFrom()
        {
            _filters[0].Operand = FilterOperands.DateFrom;

            var filterRows = _reportFiltersBuilder.GetRows(_filters).ToList();
            var filterFieldName = $"{_filters[0].FieldName} (From)";

            filterRows[1].Cells[0].Content.Equals(filterFieldName).ShouldBeTrue();
        }

        [TestMethod]
        public void GetRows_WhenFilterOperandIsDateTo_FieldNameContainsTo()
        {
            _filters[0].Operand = FilterOperands.DateTo;

            var filterRows = _reportFiltersBuilder.GetRows(_filters).ToList();
            var filterFieldName = $"{_filters[0].FieldName} (To)";

            filterRows[1].Cells[0].Content.Equals(filterFieldName).ShouldBeTrue();
        }

        [TestMethod]
        public void GetRows_WhenFilterOperandIsNotDateToOrDateFrom_FieldNameEqualsFilterFieldName()
        {
            _filters[0].Operand = FilterOperands.Custom;

            var filterRows = _reportFiltersBuilder.GetRows(_filters).ToList();

            filterRows[1].Cells[0].Content.Equals(_filters[0].FieldName).ShouldBeTrue();
        }
    }
}
