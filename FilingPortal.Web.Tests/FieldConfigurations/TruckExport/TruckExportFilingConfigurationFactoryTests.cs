using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.Common;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.TruckExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Web.Tests.FieldConfigurations.TruckExport
{
    [TestClass]
    public class TruckExportFilingConfigurationFactoryTests
    {
        private TruckExportFilingConfigurationFactory _factory;
        private Mock<ITruckExportDefValuesManualReadModelRepository> _fieldsRepositoryMock;
        private Mock<IDocumentRepository<TruckExportDocument>> _documentsRepository;
        private Mock<ITruckExportSectionRepository> _sectionsRepository;
        private Mock<IInboundRecordFieldBuilder<TruckExportDefValuesManualReadModel>> _fieldBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fieldsRepositoryMock = new Mock<ITruckExportDefValuesManualReadModelRepository>();
            _fieldsRepositoryMock.Setup(x => x.GetDefValuesByFilingHeader(It.IsAny<int>()))
                .Returns(new List<TruckExportDefValuesManualReadModel>());
            _documentsRepository = new Mock<IDocumentRepository<TruckExportDocument>>();
            _documentsRepository.Setup(x => x.GetListByFilingHeader(It.IsAny<int>()))
                .Returns(new List<TruckExportDocument>());
            _sectionsRepository = new Mock<ITruckExportSectionRepository>();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<TruckExportSection>());

            _fieldBuilder = new Mock<IInboundRecordFieldBuilder<TruckExportDefValuesManualReadModel>>();

            _factory = new TruckExportFilingConfigurationFactory(_fieldsRepositoryMock.Object, _documentsRepository.Object, _sectionsRepository.Object, _fieldBuilder.Object);
        }

        [TestMethod]
        public void CreateConfiguration_ReturnsConfiguration()
        {
            const int filingHeaderId = 22;

            FilingConfiguration result = _factory.CreateConfiguration(filingHeaderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(filingHeaderId, result.FilingHeaderId);
        }

        [TestMethod]
        public void CreateConfiguration_ReturnsConfiguration_WithCorrectFieldsNumber()
        {
            const int filingHeaderId = 22;

            _fieldsRepositoryMock.Setup(x => x.GetDefValuesByFilingHeader(It.IsAny<int>()))
                .Returns(new List<TruckExportDefValuesManualReadModel>
                {
                    new TruckExportDefValuesManualReadModel { Id = 1, FilingHeaderId = filingHeaderId, ValueType = "numeric"},
                    new TruckExportDefValuesManualReadModel { Id = 2, FilingHeaderId = filingHeaderId, ValueType = "varchar"},
                    new TruckExportDefValuesManualReadModel { Id = 3, FilingHeaderId = filingHeaderId, ValueType = "date"}
                });

            var resultField = new Mock<BaseInboundRecordField>();

            _fieldBuilder.Setup(x => x.CreateFrom(It.IsAny<TruckExportDefValuesManualReadModel>(),
                It.IsAny<IList<TruckExportDefValuesManualReadModel>>())).Returns(resultField.Object);

            FilingConfiguration result = _factory.CreateConfiguration(filingHeaderId);

            Assert.AreEqual(3, result.Fields.Count);
        }

        [TestMethod]
        public void CreateConfiguration_ReturnConfiguration_WithCorrectSectionsNumber()
        {
            const int filingHeaderId = 22;
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<TruckExportSection>
            {
                new TruckExportSection {Id = 1, Name = "root"},
                new TruckExportSection {Id = 2, Name = "declaration", ParentId = 1},
                new TruckExportSection {Id = 3, Name = "invoice", ParentId = 1},
                new TruckExportSection {Id = 4, Name = "invoice_line", ParentId = 3}
            });

            FilingConfiguration result = _factory.CreateConfiguration(filingHeaderId);

            Assert.AreEqual(4, result.Sections.Count);
        }
    }
}
