using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.FieldConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class FieldConfigurationBuilderTests
    {
        private FieldConfigurationBuilder _builder;
        private Mock<ILookupDataProvider> _dataProviderMock;

        [TestInitialize]
        public void Init()
        {
            _dataProviderMock = new Mock<ILookupDataProvider>();
            _dataProviderMock.Setup(x => x.Name).Returns("test_provider");
            _builder = new FieldConfigurationBuilder();
        }

        [TestMethod]
        public void Create_WhenCalled_CreateFieldWithNameTitleValue()
        {
            _builder.Create("Title 77007").DefaultValue("Value 44004");
            FieldModel field = _builder.Build();

            Assert.AreEqual("Value 44004", field.Value);
            Assert.AreEqual("Title 77007", field.Title);
        }

        [TestMethod]
        public void Create_WhenCalledWithoutParams_CreateFieldWithEmptyNameTitleValue()
        {
            _builder.Create();
            FieldModel field = _builder.Build();

            Assert.IsTrue(string.IsNullOrEmpty(field.Value));
            Assert.IsTrue(string.IsNullOrEmpty(field.Name));
            Assert.IsTrue(string.IsNullOrEmpty(field.Title));
        }

        [TestMethod]
        public void Title_WhenCalled_SetFiledTitle()
        {
            _builder.Create("Title 77007").Title("Title 44004");
            FieldModel field = _builder.Build();

            Assert.AreEqual("Title 44004", field.Title);
        }

        [TestMethod]
        public void DefaultValue_WhenCalled_SetFiledValue()
        {
            _builder.Create().DefaultValue("Value 44004");
            FieldModel field = _builder.Build();

            Assert.AreEqual("Value 44004", field.Value);
        }

        [TestMethod]
        public void Create_doesnt_set_any_options()
        {
            _builder.Create();
            FieldModel field = _builder.Build();

            Assert.AreEqual(0, field.Options.Count);
        }

        [TestMethod]
        public void Long_Creates_long_option_in_field_dictionary_and_sets_to_true()
        {
            _builder.Create();
            _builder.Long();
            FieldModel field = _builder.Build();

            Assert.IsInstanceOfType(field.Options["long"], typeof(bool));
            Assert.IsTrue((bool)field.Options["long"]);
        }

        [TestMethod]
        public void Long_Creates_separator_option_in_field_dictionary_and_sets_to_true()
        {
            _builder.Create();
            _builder.Separator();
            FieldModel field = _builder.Build();

            Assert.IsInstanceOfType(field.Options["separator"], typeof(bool));
            Assert.IsTrue((bool)field.Options["separator"]);
        }

        [TestMethod]
        public void Lookup_WhenCalled_SetFiledTypeToLookup()
        {
            _builder.Create();
            _builder.Lookup(_dataProviderMock.Object);

            FieldModel field = _builder.Build();

            Assert.AreEqual("Lookup", field.Options["type"]);
        }

        [TestMethod]
        public void Lookup_WhenCalled_SetProviderOption()
        {
            _builder.Create();
            _builder.Lookup(_dataProviderMock.Object);

            FieldModel field = _builder.Build();

            Assert.AreEqual("test_provider", field.Options["provider"]);
        }

        [TestMethod]
        public void Mandatory_WhenCalled_SetIsMandatoryPropertyToTrue()
        {
            _builder.Create();
            _builder.Mandatory();

            FieldModel field = _builder.Build();

            Assert.IsTrue(field.IsMandatory);
        }

    }
}
