using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass()]
    public class InboundRecordFieldBuilderTests<TDefValuesManualReadModel>
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        public InboundRecordFieldBuilder<TDefValuesManualReadModel> _builder;
        public Mock<IValueTypeConverter> _valueTypeConverter;
        public Mock<IComplexFieldsRule<TDefValuesManualReadModel>> _complexFieldsRule;
        public List<IDropdownFieldRule<TDefValuesManualReadModel>> _dropdownRules;


        [TestInitialize]
        public void InboundRecordFieldBuilderTest()
        {
            _valueTypeConverter = new Mock<IValueTypeConverter>();
            _complexFieldsRule = new Mock<IComplexFieldsRule<TDefValuesManualReadModel>>();
            _dropdownRules = new List<IDropdownFieldRule<TDefValuesManualReadModel>>();
            _builder = new InboundRecordFieldBuilder<TDefValuesManualReadModel>(_valueTypeConverter.Object, _complexFieldsRule.Object, _dropdownRules);
        }

        [TestMethod()]
        public void CreateFrom_Multiple_Empty_Test()
        {
            var models = new List<TDefValuesManualReadModel>();

            var result = _builder.CreateFrom(models).ToList();

            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod()]
        public void CreateFrom_Multiple_With_Items_Test()
        {
            var models = new List<TDefValuesManualReadModel>
            {
                new Mock<TDefValuesManualReadModel>().Object,
                new Mock<TDefValuesManualReadModel>().Object,
                new Mock<TDefValuesManualReadModel>().Object
            };

            var result = _builder.CreateFrom(models).ToList();

            Assert.AreEqual(result.Count, 3);
        }

        [TestMethod()]
        public void CreateFrom_Multiple_Complex_Field_Test()
        {
            var complexField = new Mock<TDefValuesManualReadModel>();
            complexField.Object.TableName = "Table1";
            complexField.Object.ColumnName = "Column1";
            complexField.Object.PairedFieldTable = "Table2";
            complexField.Object.PairedFieldColumn = "Column2";
            complexField.Setup(x => x.GetUniqueData()).Returns(new DefValuesUniqueData("Table1", "Column1"));

            var pairedField = new Mock<TDefValuesManualReadModel>();
            pairedField.Object.TableName = "Table2";
            pairedField.Object.ColumnName = "Column2";
            pairedField.Setup(x => x.GetUniqueData()).Returns(new DefValuesUniqueData("Table2", "Column2"));

            _complexFieldsRule.Setup(x => x.IsComplexField(It.Is<TDefValuesManualReadModel>(field => field == complexField.Object)))
                .Returns(true);
            _complexFieldsRule.Setup(x => x.IsPairedField(It.Is<TDefValuesManualReadModel>(field => field == complexField.Object), It.IsAny<IEnumerable<TDefValuesManualReadModel>>()))
                .Returns(false);

            _complexFieldsRule.Setup(x => x.IsComplexField(It.Is<TDefValuesManualReadModel>(field => field == pairedField.Object)))
                .Returns(false);
            _complexFieldsRule.Setup(x => x.IsPairedField(It.Is<TDefValuesManualReadModel>(field => field == pairedField.Object), It.IsAny<IEnumerable<TDefValuesManualReadModel>>()))
                .Returns(true);

            var models = new List<TDefValuesManualReadModel>
            {
                complexField.Object,
                pairedField.Object
            };

            var result = _builder.CreateFrom(models).ToList();

            Assert.AreEqual(result.Count, 1);

            var model = result.First();

            Assert.IsInstanceOfType(model, typeof(ComplexInboundRecordField));
        }

        [TestMethod()]
        public void CreateFrom_Multiple_Dropdown_Field_Test()
        {
            var uniqueData = new Mock<DefValuesUniqueData>("arg1", "arg2");

            var dropdownField = new Mock<TDefValuesManualReadModel>();
            dropdownField.Setup(x => x.GetUniqueData()).Returns(uniqueData.Object);

            var dropdownFieldRule = new Mock<IDropdownFieldRule<TDefValuesManualReadModel>>();
            dropdownFieldRule.Setup(x => x.IsDropdownField(It.Is<TDefValuesManualReadModel>(field => field == dropdownField.Object)))
                .Returns(true);

            _dropdownRules.Add(dropdownFieldRule.Object);

            var models = new List<TDefValuesManualReadModel>
            {
                dropdownField.Object,
            };

            var result = _builder.CreateFrom(models).ToList();

            Assert.AreEqual(result.Count, 1);

            var model = result.First();

            Assert.IsInstanceOfType(model, typeof(DropdownInboundRecordField));
        }

        [TestMethod()]
        public void CreateFrom_Single_Default_Test()
        {
            var model = new Mock<TDefValuesManualReadModel>().Object;

            var models = new List<TDefValuesManualReadModel>();

            var result = _builder.CreateFrom(model, models);

            Assert.IsNotNull(result);
        }
    }
}