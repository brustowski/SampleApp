using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.Rail.Manifest;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Base class for services that converts the entity information into document file model
    /// </summary>
    public class ManifestPdfGenerator : IFileGenerator<RailFilingHeader>
    {
        /// <summary>
        /// The manifest file name definition
        /// </summary>
        private const string ManifestFileName = "Manifest.pdf";

        /// <summary>
        /// The PDF builder instance
        /// </summary>
        private readonly IPdfBuilder _pdfBuilder;

        /// <summary>
        /// Manifest formatter
        /// </summary>
        private readonly IManifestFormatter _formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestPdfGenerator"/> class
        /// </summary>
        /// <param name="pdfBuilder">The PDF builder</param>
        /// <param name="formatter">Manifest formatter</param>
        public ManifestPdfGenerator(IPdfBuilder pdfBuilder, IManifestFormatter formatter)
        {
            _pdfBuilder = pdfBuilder;
            _formatter = formatter;
        }

        /// <summary>
        /// Generates the manifest document for specified filing header
        /// </summary>
        /// <param name="filingHeader">The filing header</param>
        public BinaryFileModel Generate(RailFilingHeader filingHeader)
        {
            IEnumerable<string> htmls = CreatePages(filingHeader);
            var css = _formatter.GetManifestStyles();

            var convertedPdf = ConvertToPdf(htmls, css);

            BinaryFileModel binaryFileModel = CreateBinaryModel(convertedPdf);

            return binaryFileModel;
        }

        /// <summary>
        /// Creates the binary model using specified data
        /// </summary>
        /// <param name="data">The data</param>
        private BinaryFileModel CreateBinaryModel(byte[] data)
        {
            return new BinaryFileModel
            {
                Content = data,
                FileName = ManifestFileName
            };
        }

        /// <summary>
        /// Converts HTML pages to PDF content
        /// </summary>
        /// <param name="htmls">The HTMLS</param>
        /// <param name="css">Stylesheet</param>
        protected virtual byte[] ConvertToPdf(IEnumerable<string> htmls, string css)
        {
            return _pdfBuilder.Convert(htmls, css);
        }

        /// <summary>
        /// Creates the pages for document by specified filing header
        /// </summary>
        /// <param name="filingHeader">The filing header</param>
        protected IEnumerable<string> CreatePages(RailFilingHeader filingHeader)
        {
            return filingHeader.RailBdParseds
                .Select(x => x.RailEdiMessageId.HasValue ? x.RailEdiMessage.Map<RailEdiMessage, Manifest>() : x.Map<RailBdParsed, Manifest>())
                .Select(x => _formatter.Format(x));
        }
    }
}