using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass()]
    public class RailInboundRecordFieldBuilderTests : InboundRecordFieldBuilderTests<RailDefValuesManualReadModel>
    {
        [TestMethod]
        public void CreateFrom_RailDefValueRecord_CopiesNotDisabled()
        {
            var railDefValue = new RailDefValuesManualReadModel
            {
                Id = 234,
                Value = "Value1",
                FilingHeaderId = 109,
                Label = "Label 1",
                HasDefaultValue = false,
                Editable = true
            };

            var result = _builder.CreateFrom(new List<RailDefValuesManualReadModel>() { railDefValue }).First() as InboundRecordField;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsDisabled);
        }

        [TestMethod]
        public void CreateFrom_RailDefValueRecord_CopiesWithDefValue()
        {
            var railDefValue = new RailDefValuesManualReadModel()
            {
                Id = 234,
                Value = "Value1",
                FilingHeaderId = 109,
                Label = "Label 1",
                HasDefaultValue = true,
                Mandatory = true,
                ValueType = "DbType",
                ValueMaxLength = 22
            };

            _valueTypeConverter.Setup(x => x.Convert("DbType")).Returns("UIType");

            var result = _builder.CreateFrom(new List<RailDefValuesManualReadModel>() { railDefValue }).First() as InboundRecordField;

            Assert.IsNotNull(result);
            Assert.AreEqual(railDefValue.Id, result.Id);
            Assert.AreEqual(railDefValue.Value, result.DefaultValue);
            Assert.AreEqual(railDefValue.FilingHeaderId, result.FilingHeaderId);
            Assert.AreEqual(22, result.MaxLength);
            Assert.AreEqual("UIType", result.Type);
            Assert.AreEqual(railDefValue.Label, result.Title);
            Assert.IsTrue(result.IsMandatory);
        }

        [TestMethod]
        public void CreateFrom_RailDefValueRecord_CopiesWithoutDefValue()
        {
            var railDefValue = new RailDefValuesManualReadModel
            {
                Id = 234,
                Value = "Value1",
                FilingHeaderId = 109,
                Label = "Label 1",
                HasDefaultValue = false,
                ValueMaxLength = 34
            };

            var result = _builder.CreateFrom(new List<RailDefValuesManualReadModel>() { railDefValue }).First() as InboundRecordField;

            Assert.IsNotNull(result);
            Assert.AreEqual(railDefValue.Id, result.Id);
            Assert.AreEqual(railDefValue.Value, result.DefaultValue);
            Assert.AreEqual(railDefValue.FilingHeaderId, result.FilingHeaderId);
            Assert.AreEqual(34, result.MaxLength);
            Assert.AreEqual(railDefValue.Label, result.Title);
        }
    }
}