using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public abstract class SingleFilingGridServiceTests<TDefValuesReadModel, TDefValuesManualReadModel, TDocument, TFilingData, TInboundType, TFilingDataRepository>
        where TDefValuesReadModel: Entity
        where TDefValuesManualReadModel: BaseDefValuesManualReadModel
        where TDocument: BaseDocument
        where TFilingData : BaseFilingData, new()
        where TInboundType: IInboundType
        where TFilingDataRepository: class, IFilingDataRepository<TFilingData>
    {
        protected ISingleFilingGridService<TInboundType> Service;
        protected Mock<ISingleFilingGridWorker<TDefValuesReadModel, TDefValuesManualReadModel, TDocument>> Worker;
        protected Mock<TFilingDataRepository> UniqueDataRepository;

        [TestInitialize]
        public void Init()
        {
            Worker = new Mock<ISingleFilingGridWorker<TDefValuesReadModel, TDefValuesManualReadModel, TDocument>>();

            Worker.Setup(x => x.GetData(It.IsAny<IEnumerable<int>>())).Returns(new Dictionary<int, FPDynObject>());

            UniqueDataRepository = new Mock<TFilingDataRepository>();

            UniqueDataRepository.Setup(x => x.GetByFilingNumbers(It.IsAny<int[]>())).Returns(new List<TFilingData>());

            Service = GetService();
        }

        protected abstract ISingleFilingGridService<TInboundType> GetService();


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
        public void GetData_Calls_UniqueDataRepository()
        {
            SearchRequestModel searchRequest = GenerateSearchRequest(1, 2, 3);

            SimplePagedResult<dynamic> data = Service.GetData(searchRequest);

            UniqueDataRepository.Verify(x => x.GetByFilingNumbers(1, 2, 3), Times.AtMostOnce);
        }

        [TestMethod]
        public void GetData_Calls_Worker()
        {
            var ids = new[] { 1, 2, 3 };

            SearchRequestModel searchRequest = GenerateSearchRequest(ids);

            SimplePagedResult<dynamic> data = Service.GetData(searchRequest);

            Worker.Verify(x => x.GetData(ids), Times.AtMostOnce);
        }

        [TestMethod]
        public void GetData_Returns_Data_Ordered_By_filingHeaderId()
        {
            var ids = new[] { 1, 2, 3 };

            var workerResult = new Dictionary<int, FPDynObject>
            {
                { 3, new FPDynObject(new Dictionary<string, object>()) },
                { 2, new FPDynObject(new Dictionary<string, object>()) },
                { 1, new FPDynObject(new Dictionary<string, object>()) }
            };

            var uniqueDataResult = new List<TFilingData>()
            {
                new TFilingData{ FilingHeaderId = 3 },
                new TFilingData{ FilingHeaderId = 2 },
                new TFilingData{ FilingHeaderId = 1 }
            };

            SearchRequestModel searchRequest = GenerateSearchRequest(ids);
            Worker.Setup(x => x.GetData(ids)).Returns(workerResult);
            UniqueDataRepository.Setup(x => x.GetByFilingNumbers(ids)).Returns(uniqueDataResult);

            SimplePagedResult<dynamic> data = Service.GetData(searchRequest);

            var orderedList = data.Results.Select(x => (int)x.FilingHeaderId);

            CollectionAssert.AreEqual(orderedList.ToArray(), ids);
        }

        [TestMethod]
        public void GetTotalMatches_Calls_Worker()
        {
            var ids = new[] { 1, 2, 3 };
            SearchRequestModel searchRequest = GenerateSearchRequest(ids);
            Service.GetTotalMatches(searchRequest);

            Worker.Verify(x=>x.GetTotalMatches(ids), Times.Once);
        }
    }
}
