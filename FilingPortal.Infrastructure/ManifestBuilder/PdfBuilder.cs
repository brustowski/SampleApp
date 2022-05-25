using System;
using System.Collections.Generic;
using System.IO;
using FilingPortal.Domain.Services;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace FilingPortal.Infrastructure.ManifestBuilder
{
    /// <summary>
    /// Wrapper service for the PDF builder library, no usage of the PDF builder library should be made outside this service
    /// </summary>
    public class PdfBuilder : IPdfBuilder, IDisposable
    {
        /// <summary>
        /// The pdf generating configuration
        /// </summary>
        private readonly PdfGenerateConfig _genConfig;

        /// <summary>
        /// Included css data
        /// </summary>
        public CssData CssStyle = null;
        /// <summary>
        /// The current PDF document instance
        /// </summary>
        private PdfDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfBuilder"/> class
        /// </summary>
        public PdfBuilder()
        {
            _genConfig = new PdfGenerateConfig { PageSize = PageSize.A4, MarginTop = 40, MarginBottom = 40, MarginLeft = 40, MarginRight = 40};
        }

        /// <summary>
        /// Converts the specified HTML pages to the PDF byte content
        /// </summary>
        /// <param name="pages">The pages</param>
        /// <param name="css">Stylesheet</param>
        public byte[] Convert(IEnumerable<string> pages, string css = null)
        {
            if (css != null)
                CssStyle = PdfGenerator.ParseStyleSheet(css);

            var i = 0;
            foreach (var page in pages)
            {
                if (i == 0)
                {
                    Load(page);
                    i++;
                    continue;
                }
                Append(page);
                i++;
            }

            byte[] result = ToStream().ToArray();

            return result;
        }

        /// <summary>
        /// Loads the specified HTML as PDF document
        /// </summary>
        /// <param name="html">The HTML</param>
        private void Load(string html)
        {
            _document = string.IsNullOrEmpty(html)
                ? new PdfDocument()
                : PdfGenerator.GeneratePdf(html, _genConfig, CssStyle);
        }

        /// <summary>
        /// Loads the specified PDF document instance
        /// </summary>
        /// <param name="pdf">The PDF document instance</param>
        private void Load(PdfDocument pdf)
        {
            _document = pdf;
        }

        /// <summary>
        /// Converts PDF document to the MemoryStream
        /// </summary>
        private MemoryStream ToStream()
        {
            MemoryStream st = new MemoryStream(128 * 1024);
            _document.Save(st, false);
            return st;
        }

        /// <summary>
        /// Appends the specified HTML to the document
        /// </summary>
        /// <param name="html">The HTML</param>
        private void Append(string html)
        {
            if(string.IsNullOrEmpty(html)) return;
            using (PdfBuilder pb = new PdfBuilder())
            {
                if (CssStyle != null) pb.CssStyle = CssStyle;
                pb.Load(html);
                Append(pb);
            }
        }

        /// <summary>
        /// Appends the specified PDF builder to the document
        /// </summary>
        /// <param name="pb">The PDF builder</param>
        private void Append(PdfBuilder pb)
        {
            foreach (PdfPage p in AsImported(pb)._document.Pages)
                _document.AddPage(p);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public void Dispose()
        {
            _document.Dispose();
        }

        /// <summary>
        /// Creates PDF builder from existing PDF builder
        /// </summary>
        /// <param name="pb">The PDF builder</param>
        private PdfBuilder AsImported(PdfBuilder pb)
        {
            var pdfBuilder = new PdfBuilder();
            pdfBuilder.Load(pb._document.IsImported ? pb._document : PdfReader.Open(pb.ToStream(), PdfDocumentOpenMode.Import));
            return pdfBuilder;
        }
    }
}