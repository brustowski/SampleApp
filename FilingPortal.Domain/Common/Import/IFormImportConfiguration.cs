using System;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Describes the import configuration
    /// </summary>
    public interface IFormImportConfiguration : IImportConfiguration
    {
        /// <summary>
        /// Gets or sets the section name
        /// </summary>
        string Section { get; set; }

        /// <summary>
        /// Gets or sets the parent record id
        /// </summary>
        int ParentRecordId { get; set; }

        /// <summary>
        /// Gets or sets the filing header id
        /// </summary>
        int FilingHeaderId { get; set; }
    }
}
