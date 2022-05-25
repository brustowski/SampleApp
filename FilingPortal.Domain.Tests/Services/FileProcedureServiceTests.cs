using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class FileProcedureServiceTests
    {
        private FileProcedureService _service;
        private Mock<IBdParsedRepository> _bdParsedRepositoryMock;
        private Mock<IFileGenerator<RailFilingHeader>> _manifestPdfGeneratorMock;
        private Mock<IRailDocumentFactory> _railDocumentFactoryMock;
        private Mock<IRailInboundReadModelRepository> _readModelRepositoryMock;
        private Mock<ISpecificationBuilder> _specificationBuilderMock;
        private Mock<IConsolidatedFilingWorkflow<RailFilingHeader, RailDefValuesManual>> _filingWorkflow;

        private const string userAccount = "testUser";

        [TestInitialize]
        public void TestInitialize()
        {
            _bdParsedRepositoryMock = new Mock<IBdParsedRepository>();
            _manifestPdfGeneratorMock = new Mock<IFileGenerator<RailFilingHeader>>();
            _railDocumentFactoryMock = new Mock<IRailDocumentFactory>();
            _readModelRepositoryMock = new Mock<IRailInboundReadModelRepository>();
            _specificationBuilderMock = new Mock<ISpecificationBuilder>();
            _filingWorkflow = new Mock<IConsolidatedFilingWorkflow<RailFilingHeader, RailDefValuesManual>>();

            var header = new RailFilingHeader();

            _filingWorkflow.Setup(x => x.CreateHeader(userAccount)).Returns(header);

            _service = new FileProcedureService(
                _bdParsedRepositoryMock.Object,
                _manifestPdfGeneratorMock.Object,
                _railDocumentFactoryMock.Object,
                _readModelRepositoryMock.Object,
                _specificationBuilderMock.Object,
                _filingWorkflow.Object
                );
        }

        [TestMethod]
        public void Start_GetsListOfBdParsedRecords()
        {
            int[] bdParsedIds = new int[] { 1 };

            _service.CreateUnitTrainFilingHeader(bdParsedIds);

            _filingWorkflow.Verify(x => x.StartUnitTradeFiling(It.IsAny<RailFilingHeader>()), Times.Once);
        }

        [TestMethod]
        public void CreateSingleFilingFilingHeaders_Calls_FilingWorkflow()
        {
            var filtersSet = new FiltersSet();

            _service.CreateSingleFilingFilingHeaders(filtersSet);

            _filingWorkflow.Verify(x => x.StartSingleFiling(It.IsAny<IEnumerable<RailFilingHeader>>()));
        }

        [TestMethod]
        public void FiltersetToIds_Calls_Repository()
        {
            var filtersSet = new FiltersSet();

            ISpecification specification = new Mock<ISpecification>().Object;

            _specificationBuilderMock.Setup(x => x.Build<RailInboundReadModel>(filtersSet)).Returns(specification);

            _service.FiltersetToIds(filtersSet);

            _readModelRepositoryMock.Verify(x => x.GetAll<EntityDto>(specification, 100000, false), Times.Once);
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

            _readModelRepositoryMock.Setup(x => x.GetAll<EntityDto>(It.IsAny<ISpecification>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(entities);

            IEnumerable<int> result = _service.FiltersetToIds(filtersSet);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(29, result.ElementAt(0));
            Assert.AreEqual(67, result.ElementAt(1));
        }

        [TestMethod]
        public void CreateUnitTradeFilingHeader_Calls_FilingWorkflow()
        {
            var filtersSet = new FiltersSet();

            _service.CreateUnitTrainFilingHeader(filtersSet);

            _filingWorkflow.Verify(x => x.StartUnitTradeFiling(It.IsAny<RailFilingHeader>()));
        }

        [TestMethod]
        public void CreateFilingHeader_Generates_Manifest()
        {
            int[] bdParsedIds = new int[] { 1 };

            var manifestFile = new BinaryFileModel();
            _manifestPdfGeneratorMock.Setup(x => x.Generate(It.IsAny<RailFilingHeader>()))
                .Returns(manifestFile);

            _railDocumentFactoryMock.Setup(x => x.CreateManifest(manifestFile, null)).Returns(new RailDocument() { Id = 5 });

            RailFilingHeader header = _service.CreateFilingHeader(bdParsedIds, userAccount);

            Assert.AreEqual(5, header.RailDocuments.First().Id);

        }

        [TestMethod]
        public void Start_CreateFilingHeader_CallsWorkflow()
        {
            int[] bdParsedIds = new int[] { 1 };

            _service.CreateUnitTrainFilingHeader(bdParsedIds);

            _filingWorkflow.Verify(x => x.StartUnitTradeFiling(It.IsAny<RailFilingHeader>()), Times.AtMostOnce);
        }

        [TestMethod]
        public void SetInReview_Calls_FilingWorkflow()
        {
            int filingHeaderId = 45;

            _service.SetInReview(filingHeaderId);

            _filingWorkflow.Verify(x => x.SetInReview(filingHeaderId), Times.Once);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(100)]
        public void CreateSingleFilingFilingHeaders_With_Elements_Calls_FilingWorkflow(int elementsInArray)
        {
            int[] bdParsedIds = Enumerable.Repeat(0, elementsInArray).ToArray();

            _service.CreateSingleFilingFilingHeaders(bdParsedIds);

            _filingWorkflow.Verify(x => x.StartSingleFiling(It.IsAny<IEnumerable<RailFilingHeader>>()), Times.Once);
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

            _bdParsedRepositoryMock.Setup(x => x.GetList(inboundIds)).Returns(new List<RailBdParsed>
            {
                new RailBdParsed {Id = 1},
                new RailBdParsed {Id = 2},
                new RailBdParsed {Id = 3}
            });

            IEnumerable<RailFilingHeader> headers = _service.CreateSingleFilingHeaders(inboundIds, userAccount);

            Assert.AreEqual(3, headers.Count());
        }

        [TestMethod]
        public void CreateSingleFilingHeaders_calls_parsed_repository()
        {
            var inboundIds = new List<int> { 1, 2, 3 };

            IEnumerable<RailFilingHeader> headers = _service.CreateSingleFilingHeaders(inboundIds, userAccount);

            _bdParsedRepositoryMock.Verify(x => x.GetList(inboundIds), Times.AtMostOnce);
        }


    }
}
