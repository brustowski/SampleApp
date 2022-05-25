using System;
using System.IO;
using System.Linq;
using FilingPortal.Infrastructure.ManifestBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Infrastructure.Tests
{
    [TestClass]
    public class PdfBuilderTests
    {
        private PdfBuilder _pdfBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _pdfBuilder = new PdfBuilder();
        }

        [TestMethod]
        public void Convert_WithOneHtmlPage_ReturnsBytes()
        {
            var resultContent = _pdfBuilder.Convert(new[] {"<div>This is text in pdf</div>"});

            Assert.IsTrue(resultContent.Any());
        }

        [TestMethod]
        public void Convert_WithSeveralHtmlPages_ReturnsBytes()
        {
            var resultContent = _pdfBuilder.Convert(new[] { "<div>This is text in pdf</div>", "<div>This is second page in pdf</div>", "<div>This is third page in pdf</div>" });
            
            Assert.IsTrue(resultContent.Any());
        }

        [TestMethod]
        public void Convert_WithSeveralHtmlPages_PdfIsRendered()
        {
            var resultContent = _pdfBuilder.Convert(new[] { "<div>This is text in pdf</div>", "<div>This is second page in pdf</div>", "<div>This is third page in pdf</div>" });

            string path = Path.Combine(Path.GetTempPath(), "TestPdfRendered1.pdf");
            File.WriteAllBytes(path, resultContent);
        }


        [TestMethod]
        public void Convert_WithOneHtmlPageIsEmpty_ReturnsBytes()
        {
            var resultContent = _pdfBuilder.Convert(new[]
            {
                "<div>This is first text string in pdf. Second string is null and will not be rendered</div>",
                null,
                "<div>This is third text string in pdf</div>"
            });

            string path = Path.Combine(Path.GetTempPath(), "TestPdfRendered2.pdf");
            File.WriteAllBytes(path, resultContent);
            Assert.IsTrue(resultContent.Any()); 
        }

        [TestMethod]
        public void Convert_OneNullHtmlPage_ThrowsException()
        {
            try
            {
                _pdfBuilder.Convert(new[] {(string) null});
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }
        }
    }
}
