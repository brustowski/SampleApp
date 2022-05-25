using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.Models.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.Tests.Common;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ManifestControllerTests : ApiControllerFunctionTestsBase<ManifestController>
    {
        private Mock<IBdParsedRepository> repositoryMock;
        private Mock<IManifestFactory> factoryMock;
        protected override ManifestController TestInitialize()
        {
            repositoryMock = new Mock<IBdParsedRepository>();
            factoryMock = new Mock<IManifestFactory>();

            return new ManifestController(repositoryMock.Object,factoryMock.Object);
        }

        [TestMethod]
        public void GetRecordManifest_ReturnsManifestText_FromRepository()
        {
            var manifest = new Manifest { Importer = "Importer" };

            repositoryMock.Setup(x => x.GetManifest(2)).Returns(manifest);
            factoryMock.Setup(f => f.CreateFrom(manifest)).Returns(new ManifestModel() { Fields = new List<FieldModel> { new FieldModel { Name = "Importer", Title = "Importer", Value = "Importer" } } });
            var result = Controller.GetRecordManifest(2);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ManifestModel>));
            var data = (result as OkNegotiatedContentResult<ManifestModel>).Content;
            Assert.AreEqual(1, data.Fields.Count());
            Assert.AreEqual("Importer", data.Fields.First().Value);
        }

        [TestMethod]
        public void GetRecordManifest_ReturnsBedRequest_IfRecordDoesNotExist()
        {
            repositoryMock.Setup(x => x.GetManifest(2)).Returns<Manifest>(null);
            var result = Controller.GetRecordManifest(2);

            Assert.IsTrue(result is BadRequestErrorMessageResult);            
        }
    }
}