using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.Zones.Entry.Domain.Config;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System;
using System.Collections.Generic;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.FormDataImport
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IFormImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => WorkflowNames.zonesEntry;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{WorkflowNames.zonesEntry}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(ImportModel);

        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(DefValueManual);

        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.FileInboundRecord };

        /// <summary>
        /// Gets or sets the section name
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the parent record id
        /// </summary>
        public int ParentRecordId { get; set; }

        /// <summary>
        /// Gets or sets the filing header id
        /// </summary>
        public int FilingHeaderId { get; set; }
    }
}
