using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class InboundRecordFieldFactoryTests
    {
        private InboundRecordFieldFactory _factory;

        private Mock<IValueTypeConverter> _valueTypeConverterMock;
        private Mock<IInboundRecordFieldBuilder<RailDefValuesManualReadModel>> _fieldBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _valueTypeConverterMock = new Mock<IValueTypeConverter>();
            _valueTypeConverterMock.Setup(x => x.Convert(It.IsAny<string>())).Returns("AType");
            _fieldBuilder = new Mock<IInboundRecordFieldBuilder<RailDefValuesManualReadModel>>();
            _factory = new InboundRecordFieldFactory(_fieldBuilder.Object);
        }

        [TestMethod]
        public void CreateSectionFrom_RailDefValueRecords_DividesBySections()
        {
            var railDefValues = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel
                {
                    Id = 234,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 1",
                    HasDefaultValue = false,
                    SectionTitle = "section1"
                },
                new RailDefValuesManualReadModel
                {
                    Id = 897,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 2",
                    HasDefaultValue = false,
                    SectionTitle = "section2"
                },
                new RailDefValuesManualReadModel
                {
                    Id = 90,
                    FilingHeaderId = 109,
                    Label = "Label 3",
                    HasDefaultValue = false,
                    SectionTitle = "section1"
                },
            };

            var result = _factory.CreateSectionsFrom(railDefValues).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.SectionName == "section1"));
            Assert.IsTrue(result.Any(x => x.SectionName == "section2"));
        }

        [TestMethod]
        public void CreateSectionFrom_RailDefValueRecords_HasRecordsInSpecificSections()
        {
            var railDefValues = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel
                {
                    Id = 234,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 1",
                    HasDefaultValue = false,
                    SectionTitle = "section1"
                },
                new RailDefValuesManualReadModel
                {
                    Id = 897,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 2",
                    HasDefaultValue = false,
                    SectionTitle = "section2"
                },
                new RailDefValuesManualReadModel
                {
                    Id = 90,
                    FilingHeaderId = 109,
                    Label = "Label 3",
                    HasDefaultValue = false,
                    SectionTitle = "section1"
                },
            };

            var result = _factory.CreateSectionsFrom(railDefValues).ToList();

            _fieldBuilder.Verify(x=>x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(y=>y.Count()==2)), Times.Once);
            _fieldBuilder.Verify(x => x.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(y => y.Count() == 1)), Times.Once);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void CreateSectionFrom_RailDefValueRecords_HasRecordsOrderedInSection()
        {
            var railDefValues = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel
                {
                    Id = 234,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 1",
                    SectionTitle = "test section",
                    DisplayOnUI = 19
                },
                new RailDefValuesManualReadModel
                {
                    Id = 897,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 2",
                    SectionTitle = "test section",
                    DisplayOnUI = 2
                },
                new RailDefValuesManualReadModel
                {
                    Id = 90,
                    FilingHeaderId = 109,
                    Label = "Label 3",
                    HasDefaultValue = false,
                    SectionTitle = "test section",
                    DisplayOnUI = 7
                },
            };

            var result = _factory.CreateSectionsFrom(railDefValues).ToList();

            _fieldBuilder.Verify(builder=>builder.CreateFrom(It.Is<IEnumerable<RailDefValuesManualReadModel>>(
                x => x.ElementAt(0).Id == 897 && x.ElementAt(1).Id == 90 && x.ElementAt(2).Id == 234
            )));
        }

        [TestMethod]
        public void CreateSectionFrom_RailDefValueRecords_RecordsAreNotDisabledForEditing()
        {
            var railDefValues = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel
                {
                    Id = 234,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 1",
                    SectionTitle = "test section",
                    DisplayOnUI = 19
                },
                new RailDefValuesManualReadModel
                {
                    Id = 897,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 2",
                    SectionTitle = "test section",
                    DisplayOnUI = 2
                },
                new RailDefValuesManualReadModel
                {
                    Id = 90,
                    FilingHeaderId = 109,
                    Label = "Label 3",
                    HasDefaultValue = false,
                    SectionTitle = "test section",
                    DisplayOnUI = 7
                },
            };

            var result = _factory.CreateSectionsFrom(railDefValues).ToList();

            Assert.IsTrue(result.All(x => x.Fields.Cast<InboundRecordField>().All(f => !f.IsDisabled)));
        }

        [TestMethod]
        public void CreateSectionFrom_RailDefValueRecords_HasOrderedSections()
        {
            var railDefValues = new List<RailDefValuesManualReadModel>
            {
                new RailDefValuesManualReadModel
                {
                    Id = 234,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 1",
                    SectionTitle = "c section",
                    DisplayOnUI = 19
                },
                new RailDefValuesManualReadModel
                {
                    Id = 897,
                    Value = "Value1",
                    FilingHeaderId = 109,
                    Label = "Label 2",
                    SectionTitle = "b section",
                    DisplayOnUI = 2
                },
                new RailDefValuesManualReadModel
                {
                    Id = 90,
                    FilingHeaderId = 109,
                    Label = "Label 3",
                    HasDefaultValue = false,
                    SectionTitle = "a section",
                    DisplayOnUI = 7
                },
            };

            var result = _factory.CreateSectionsFrom(railDefValues).ToList();

            ICollection expected = new List<string>
            {
                "a section", "b section", "c section"
            };

            CollectionAssert.AreEqual(expected, result.Select(x => x.SectionName).ToList());
        }

    }
}