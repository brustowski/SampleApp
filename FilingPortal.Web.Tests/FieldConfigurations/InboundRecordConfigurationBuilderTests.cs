using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class InboundRecordConfigurationBuilderTests
    {
        private InboundRecordConfigurationBuilder _builder;

        private Mock<IInboundRecordFieldFactory> _factoryMock;
        private Mock<IRailDefValuesManualReadModelRepository> _repositoryMock;
        private Mock<IDocumentRepository<RailDocument>> _documentRepositoryMock;
        private Mock<IGridConfigRegistry> _configRegistryMock;
        private Mock<IGridConfiguration> _configurationMock;
        private Mock<IInboundRecordsRepository<RailBdParsed>> _inboundRecordsRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IRailDefValuesManualReadModelRepository>();
            _factoryMock = new Mock<IInboundRecordFieldFactory>();
            _documentRepositoryMock = new Mock<IDocumentRepository<RailDocument>>();
            _configRegistryMock = new Mock<IGridConfigRegistry>();

            _configurationMock = new Mock<IGridConfiguration>();

            _configRegistryMock.Setup(x => x.GetGridConfig(It.IsAny<string>())).Returns(_configurationMock.Object);
            _inboundRecordsRepositoryMock = new Mock<IInboundRecordsRepository<RailBdParsed>>();

            _builder = new InboundRecordConfigurationBuilder(_repositoryMock.Object, _factoryMock.Object, _documentRepositoryMock.Object, _configRegistryMock.Object, _inboundRecordsRepositoryMock.Object);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_RepositoryIsCalledForAdditionalParameters()
        {
            var filingHeaderId = 22;
            _repositoryMock.Setup(x => x.GetAdditionalParametersByFilingHeader(It.IsAny<int>())).Returns(new List<RailDefValuesManualReadModel>());

            _builder.Build(filingHeaderId);

            _repositoryMock.Verify(x => x.GetAdditionalParametersByFilingHeader(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_RepositoryIsCalledForCommonData()
        {
            var filingHeaderId = 22;
            _repositoryMock.Setup(x => x.GetCommonDataByFilingHeader(It.IsAny<int>())).Returns(new List<RailDefValuesManualReadModel>());

            _builder.Build(filingHeaderId);

            _repositoryMock.Verify(x => x.GetCommonDataByFilingHeader(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_RepositoryIsCalledForDocuments()
        {
            var filingHeaderId = 22;
            _documentRepositoryMock.Setup(x => x.GetListByFilingHeader(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Returns(new List<RailDocument>());

            _builder.Build(filingHeaderId);

            _documentRepositoryMock.Verify(x => x.GetListByFilingHeader(filingHeaderId, It.IsAny<IEnumerable<int>>()), Times.Once);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_FactoryIsCalledForEachRecordInAdditionalParameters()
        {
            var filingHeaderId = 22;
            var listRecords = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel {Id = 34, Label = "test", Value = "123"},
                new RailDefValuesManualReadModel {Id = 32, Label = "bb", Value = "dfg"},
                new RailDefValuesManualReadModel {Id = 87, Label = "aa", Value = "231"},
            };
            _repositoryMock.Setup(x => x.GetAdditionalParametersByFilingHeader(It.IsAny<int>())).Returns(listRecords);

            _builder.Build(filingHeaderId);

            _factoryMock.Verify(x => x.CreateFrom(It.IsAny<IEnumerable<RailDefValuesManualReadModel>>()), Times.Once);
            _factoryMock.Verify(x => x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(r => r.Count() == 3)));
            _factoryMock.Verify(x => x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(r => r.ElementAt(0).Id == 34)), Times.Once);
            _factoryMock.Verify(x => x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(r => r.ElementAt(1).Id == 32)), Times.Once);
            _factoryMock.Verify(x => x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(r => r.ElementAt(2).Id == 87)), Times.Once);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_ReturnsConfigurationWithAdditionalParameters()
        {
            var filingHeaderId = 22;
            var listRecords = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel {Id = 34, Label = "test", Value = "123"},
                new RailDefValuesManualReadModel {Id = 32, Label = "bb", Value = "dfg"},
                new RailDefValuesManualReadModel {Id = 87, Label = "aa", Value = "231"},
            };
            _repositoryMock.Setup(x => x.GetAdditionalParametersByFilingHeader(It.IsAny<int>())).Returns(listRecords);
            _factoryMock.Setup(x => x.CreateFrom(It.IsAny<IEnumerable<RailDefValuesManualReadModel>>())).Returns(
                new List<InboundRecordField>() { new InboundRecordField(), new InboundRecordField(), new InboundRecordField() });

            InboundRecordFieldConfiguration result = _builder.Build(filingHeaderId);

            Assert.AreEqual(3, result.AdditionalParameters.Count());
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_FactoryIsCalledForRecordsOfCommonData()
        {
            var filingHeaderId = 22;
            var listRecords = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel {Id = 34, Label = "test", Value = "123", SectionTitle = "test section 1"},
                new RailDefValuesManualReadModel {Id = 32, Label = "bb", Value = "dfg", SectionTitle = "test section 2"},
                new RailDefValuesManualReadModel {Id = 87, Label = "aa", Value = "231", SectionTitle = "test section 1"},
            };
            _repositoryMock.Setup(x => x.GetCommonDataByFilingHeader(It.IsAny<int>())).Returns(listRecords);

            _builder.Build(filingHeaderId);

            _factoryMock.Verify(x => x.CreateSectionsFrom(listRecords), Times.Once);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_ReturnsConfigurationWithCommonData()
        {
            var filingHeaderId = 22;
            var listRecords = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel {Id = 34, Label = "test", Value = "123", SectionTitle = "test section 1"},
                new RailDefValuesManualReadModel {Id = 32, Label = "bb", Value = "dfg", SectionTitle = "test section 2"},
                new RailDefValuesManualReadModel {Id = 87, Label = "aa", Value = "231", SectionTitle = "test section 3"},
            };
            var listSections = new List<InboundRecordFieldSection>
            {
                new InboundRecordFieldSection(),
                new InboundRecordFieldSection()
            };
            _repositoryMock.Setup(x => x.GetAdditionalParametersByFilingHeader(It.IsAny<int>())).Returns(listRecords);
            _factoryMock.Setup(x => x.CreateSectionsFrom(It.IsAny<IEnumerable<RailDefValuesManualReadModel>>())).Returns(listSections);

            InboundRecordFieldConfiguration result = _builder.Build(filingHeaderId);

            Assert.AreEqual(2, result.CommonData.Count);
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_ReturnsConfigurationWithDocuments()
        {
            var filingHeaderId = 22;
            var listDocuments = new List<RailDocument>
            {
                new RailDocument {Id = 34, DocumentType = "doc type1", Description = "description", FileName = "f name"},
                new RailDocument {Id = 32, DocumentType = "doc type2", Description = "description 4444", FileName = "file name"},
                new RailDocument {Id = 87, DocumentType = "doc type3", Description = "description ttt", FileName = "another file name"},
            };
            _documentRepositoryMock.Setup(x => x.GetListByFilingHeader(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Returns(listDocuments);

            InboundRecordFieldConfiguration result = _builder.Build(filingHeaderId);

            Assert.AreEqual(3, result.Documents.Count);
            Assert.IsTrue(result.Documents.Any(x => x.Id == 34 && x.Description == "description" && x.Status == InboundRecordDocumentStatus.None && x.Name == "f name" && x.Type == "doc type1"));
            Assert.IsTrue(result.Documents.Any(x => x.Id == 32 && x.Description == "description 4444" && x.Status == InboundRecordDocumentStatus.None && x.Name == "file name" && x.Type == "doc type2"));
            Assert.IsTrue(result.Documents.Any(x => x.Id == 87 && x.Description == "description ttt" && x.Status == InboundRecordDocumentStatus.None && x.Name == "another file name" && x.Type == "doc type3"));
        }

        [TestMethod]
        public void Build_WithFilingHeaderId_ReturnsConfigurationWithManifestFileAtLastPlace()
        {
            var filingHeaderId = 22;
            var listDocuments = new List<RailDocument>
            {
                new RailDocument {Id = 34, DocumentType = "doc type1", Description = "description", FileName = "f name"},
                new RailDocument {Id = 32, DocumentType = "doc type2", Description = "description 4444", FileName = "file name", IsManifest = 1},
                new RailDocument {Id = 87, DocumentType = "doc type3", Description = "description ttt", FileName = "another file name"},
            };
            _documentRepositoryMock.Setup(x => x.GetListByFilingHeader(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Returns(listDocuments);

            InboundRecordFieldConfiguration result = _builder.Build(filingHeaderId);

            Assert.AreEqual(3, result.Documents.Count);
            Assert.IsTrue(result.Documents.Last().Id == 32);
        }

        [TestMethod]
        public void BuildSingleFiling_Calls_Repository()
        {
            // Assign
            var ids = new[] { 1, 2, 3 };
            // Act
            _builder.BuildSingleFiling(ids);

            // Assert
            _repositoryMock.Verify(x => x.GetSingleFilingData(ids, It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [TestMethod]
        public void BuildSingleFiling_Calls_Configuration()
        {
            // Assign
            var ids = new[] { 1, 2, 3 };
            // Act
            _builder.BuildSingleFiling(ids);

            // Assert
            _configurationMock.Verify(x => x.GetColumns(), Times.Once);
        }

        [TestMethod]
        public void BuildSingleFiling_GettingData_With_Configuration_DisplayName()
        {
            // Assign
            var ids = new[] { 1, 2, 3 };
            var columnsConfig = new List<ColumnConfig>
            {
                new ColumnConfig { DisplayName = "Field1" },
                new ColumnConfig { DisplayName = "Field2" },
                new ColumnConfig { DisplayName = "Field3" }
            };

            _configurationMock.Setup(x => x.GetColumns()).Returns(columnsConfig);
            // Act
            _builder.BuildSingleFiling(ids);

            // Assert
            _repositoryMock.Verify(x => x.GetSingleFilingData(ids,
                It.Is<IEnumerable<string>>(
                    arg => new HashSet<string>(arg)
                    .SetEquals(new[] { "Field1", "Field2", "Field3" }))),
                    Times.Once);
        }

        [TestMethod]
        public void BuildSingleFiling_WithFilingHeaderIds_ReturnsConfigurationWithAdditionalParameters()
        {
            var filingHeaderIds = new[] { 22 };
            var listRecords = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel {Id = 34, Label = "test", Value = "123"},
                new RailDefValuesManualReadModel {Id = 32, Label = "bb", Value = "dfg"},
                new RailDefValuesManualReadModel {Id = 87, Label = "aa", Value = "231"},
            };
            _repositoryMock.Setup(x => x.GetSingleFilingData(It.IsAny<int[]>(), It.IsAny<IEnumerable<string>>())).Returns(listRecords);
            _factoryMock.Setup(x => x.CreateFrom(It.IsAny<IEnumerable<RailDefValuesManualReadModel>>())).Returns(new List<InboundRecordField>());

            InboundRecordFieldConfiguration result = _builder.BuildSingleFiling(filingHeaderIds);

            _factoryMock.Verify(x => x.CreateFrom(listRecords), Times.Once);
        }
    }
}
