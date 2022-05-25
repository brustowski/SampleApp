using System.Collections.Generic;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface for PDF builder service
    /// </summary>
    public interface IPdfBuilder
    {
        /// <summary>
        /// Converts the specified HTML pages to the PDF byte content
        /// </summary>
        /// <param name="pages">The pages</param>
        /// <param name="css">Stylesheet</param>
        byte[] Convert(IEnumerable<string> pages, string css = null);
    }
}