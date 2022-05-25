using System;
using System.Collections.Generic;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.Inbound
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => GridNames.InboundRecords;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{GridNames.InboundRecords}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(InboundImportModel);
        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(InboundRecord);
        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.ImportInboundRecord };
    }
}
