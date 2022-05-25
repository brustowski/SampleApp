
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.TruckExport;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.TruckExport;

namespace FilingPortal.Domain.Tests.Services.TruckExport
{
    [TestClass]
    public class TruckExportSingleFilingGridServiceTests
    {
        private TruckExportSingleFilingGridService service;
        private Mock<ISingleFilingGridWorker<TruckExportDefValue, TruckExportDefValuesManualReadModel, TruckExportDocument>> _workerMock;
        private Mock<ITruckExportReadModelRepository> _uniqueDataRepositoryMock;
        [TestInitialize]
        public void Init()
        {
            _workerMock = new Mock<ISingleFilingGridWorker<TruckExportDefValue, TruckExportDefValuesManualReadModel, TruckExportDocument>>();
            _uniqueDataRepositoryMock = new Mock<ITruckExportReadModelRepository>();
            service = new TruckExportSingleFilingGridService(_workerMock.Object, _uniqueDataRepositoryMock.Object);
        }

        public SearchRequestModel GenerateSearchRequest(params int[] values)
        {
            var searchRequest = new SearchRequestModel();
            searchRequest.FilterSettings.Filters.Add(new Filter()
            {
                FieldName = "FilingHeaderId",
                Operand = "equals",
                Values = values.Select(x => new LookupItem() { Value = x, DisplayValue = x.ToString() }).ToList()
            });

            return searchRequest;
        }

        [TestMethod]
        public void GetData_Calls_Worker()
        {
            var ids = new[] { 1, 2, 3 };

            SearchRequestModel searchRequest = GenerateSearchRequest(ids);

            _workerMock.Setup(x => x.GetData(ids)).Returns(new Dictionary<int, FPDynObject>());

            SimplePagedResult<dynamic> data = service.GetData(searchRequest);

            _workerMock.Verify(x => x.GetData(ids), Times.AtMostOnce);
        }
    }
}
