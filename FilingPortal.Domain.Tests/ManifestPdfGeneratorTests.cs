using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.DTOs.Rail.Manifest;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests
{
    [TestClass]
    public class ManifestPdfGeneratorTests: TestWithApplicationMapping
    {
        private ManifestPdfGenerator _generator;
        private Mock<IPdfBuilder> _pdfBuilderMock;
        private Mock<IManifestFormatter> _formatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _pdfBuilderMock = new Mock<IPdfBuilder>();
            _formatter = new Mock<IManifestFormatter>();

            _generator = new ManifestPdfGenerator(_pdfBuilderMock.Object, _formatter.Object);
        }

        [TestMethod]
        public void Generate_ReturnsBinaryFileModel_Correct()
        {
            var railBdParsed = new RailBdParsed() { RailEdiMessage = new RailEdiMessage() { EmMessageText = "AAA" } };

            var railFilingHeader = new RailFilingHeader()
            {
                RailBdParseds = new List<RailBdParsed>()
                {
                    railBdParsed
                }
            };

            _pdfBuilderMock.Setup(x => x.Convert(It.Is<IEnumerable<string>>(c => c.Contains("AAA")), null))
                .Returns(new byte[] { 1 });
            _formatter.Setup(x => x.Format(It.IsAny<Manifest>())).Returns("AAA");

            var result = _generator.Generate(railFilingHeader);

            Assert.AreEqual("Manifest.pdf", result.FileName);
            Assert.AreEqual(1, result.Content.First());

            _pdfBuilderMock.Verify(x => x.Convert(It.Is<IEnumerable<string>>(c => c.Contains("AAA")), null));
        }
    }
}