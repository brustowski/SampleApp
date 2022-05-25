using FilingPortal.Web.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.Tests.Common
{
    [TestClass]
    public class FilingResultBuilderTests
    {
        private FilingResultBuilder builder;

        [TestInitialize]
        public void TestInitialize()
        {
            builder = new FilingResultBuilder();
        }

        [TestMethod]
        public void Build_With_BadResult_Returns_Invalid()
        {
            // Assign
            var goodModel = new InboundRecordFileModel { FilingHeaderId = 1 };
            var badModel = new InboundRecordFileModel { FilingHeaderId = 2 };

            // Act
            builder.AddResult(goodModel);
            builder.AddBadResult(badModel, "Bad message");

            // Assert
            Assert.IsFalse(builder.IsValid);
        }

        [TestMethod]
        public void Build_With_GoodResults_Returns_Valid()
        {
            // Assign
            var goodModel = new InboundRecordFileModel { FilingHeaderId = 1 };

            // Act
            builder.AddResult(goodModel);

            // Assert
            Assert.IsTrue(builder.IsValid);
        }

        [TestMethod]
        public void Build_With_2_Models_Returns_2_Results()
        {
            // Assign
            var goodModel = new InboundRecordFileModel { FilingHeaderId = 1 };
            var badModel = new InboundRecordFileModel { FilingHeaderId = 2 };

            // Act
            builder.AddResult(goodModel);
            builder.AddBadResult(badModel, "Bad message");

            var resultsAmount = builder.Results.Count;

            // Assert
            Assert.AreEqual(resultsAmount, 2);
        }

        [TestMethod]
        public void Build_With_2_ErrorMessages_Returns_2_Messages()
        {
            // Assign
            var badModel1 = new InboundRecordFileModel { FilingHeaderId = 1 };
            var badModel2 = new InboundRecordFileModel { FilingHeaderId = 2 };

            // Act
            builder.AddErrorMessage("The quick brown fox");
            builder.AddErrorMessage("jumps over the lazy dog");

            // Assert
            Assert.AreEqual(2, builder.Messages.Count);
        }
    }
}
