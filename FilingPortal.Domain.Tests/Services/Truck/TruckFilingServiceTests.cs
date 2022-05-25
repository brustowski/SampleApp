using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Truck;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Tests.Services.Truck
{
    [TestClass]
    public class TruckFilingServiceTests
    {
        private TruckFilingService _service;
        private Mock<ITruckInboundRepository> _inboundRepositoryMock;
        private Mock<ISpecificationBuilder> _specificationBuilderMock;
        private Mock<IFilingWorkflow<TruckFilingHeader, TruckDefValueManual>> _filingWorkflow;

        private const string userAccount = "testUser";

        [TestInitialize]
        public void TestInitialize()
        {
            _inboundRepositoryMock = new Mock<ITruckInboundRepository>();
            _specificationBuilderMock = new Mock<ISpecificationBuilder>();
            _filingWorkflow = new Mock<IFilingWorkflow<TruckFilingHeader, TruckDefValueManual>>();

            var header = new TruckFilingHeader();

            _filingWorkflow.Setup(x => x.CreateHeader(userAccount)).Returns(header);

            _service = new TruckFilingService(
                _inboundRepositoryMock.Object,
                _specificationBuilderMock.Object,
                _filingWorkflow.Object
                );
        }

        [TestMethod]
        public void CreateSingleFilingFilingHeaders_Calls_FilingWorkflow()
        {
            var filtersSet = new FiltersSet();

            _service.CreateSingleFilingFilingHeaders(filtersSet);

            _filingWorkflow.Verify(x => x.StartSingleFiling(It.IsAny<IEnumerable<TruckFilingHeader>>()));
        }

        [TestMethod]
        public void FiltersetToIds_Calls_Repository()
        {
            var filtersSet = new FiltersSet();

            ISpecification specification = new Mock<ISpecification>().Object;

            _specificationBuilderMock.Setup(x => x.Build<TruckInbound>(filtersSet)).Returns(specification);

            _service.FiltersetToIds(filtersSet);

            _inboundRepositoryMock.Verify(x => x.GetAll<EntityDto>(specification, 100000, false), Times.Once);
        }

        [TestMethod]
        public void FiltersetToIds_Returns_Ids()
        {
            var filtersSet = new FiltersSet();

            var entities = new List<EntityDto>
            {
                new EntityDto{ Id = 29 },
                new EntityDto{ Id = 67 }
            };

            _inboundRepositoryMock.Setup(x => x.GetAll<EntityDto>(It.IsAny<ISpecification>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(entities);

            IEnumerable<int> result = _service.FiltersetToIds(filtersSet);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(29, result.ElementAt(0));
            Assert.AreEqual(67, result.ElementAt(1));
        }

        [TestMethod]
        public void SetInReview_Calls_FilingWorkflow()
        {
            var filingHeaderId = 45;

            _service.SetInReview(filingHeaderId);

            _filingWorkflow.Verify(x => x.SetInReview(filingHeaderId), Times.Once);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(100)]
        public void CreateSingleFilingFilingHeaders_With_Elements_Calls_FilingWorkflow(int elementsInArray)
        {
            var bdParsedIds = Enumerable.Repeat(0, elementsInArray).ToArray();

            _service.CreateSingleFilingFilingHeaders(bdParsedIds);

            _filingWorkflow.Verify(x => x.StartSingleFiling(It.IsAny<IEnumerable<TruckFilingHeader>>()), Times.Once);
        }

        [TestMethod]
        public void File_Calls_FilingWorkflow()
        {
            _service.File(1, 2, 3);

            _filingWorkflow.Verify(x => x.File(1, 2, 3), Times.AtMostOnce);
        }

        [TestMethod]
        public void CancelFilingProcess_Calls_FilingWorkflow()
        {
            _service.CancelFilingProcess(1, 2, 3);

            _filingWorkflow.Verify(x => x.CancelFilingProcess(1, 2, 3), Times.AtMostOnce);
        }

        [TestMethod]
        public void CreateSingleFilingHeaders_creates_header_for_each_id()
        {
            var inboundIds = new List<int> { 1, 2, 3 };

            _inboundRepositoryMock.Setup(x => x.GetList(inboundIds)).Returns(new List<TruckInbound>
            {
                new TruckInbound {Id = 1},
                new TruckInbound {Id = 2},
                new TruckInbound {Id = 3}
            });

            IEnumerable<TruckFilingHeader> headers = _service.CreateSingleFilingHeaders(inboundIds, userAccount);

            Assert.AreEqual(3, headers.Count());
        }

        [TestMethod]
        public void CreateSingleFilingHeaders_calls_parsed_repository()
        {
            var inboundIds = new List<int> { 1, 2, 3 };

            IEnumerable<TruckFilingHeader> headers = _service.CreateSingleFilingHeaders(inboundIds, userAccount);

            _inboundRepositoryMock.Verify(x => x.GetList(inboundIds), Times.AtMostOnce);
        }


    }
}
