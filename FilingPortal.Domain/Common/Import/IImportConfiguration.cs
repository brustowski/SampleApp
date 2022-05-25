using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Describes the import configuration
    /// </summary>
    public interface IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the template File Name
        /// </summary>
        string FileName { get; set; }
        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        Type ModelType { get; }
        /// <summary>
        /// Gets the result entity model
        /// </summary>
        Type ResultType { get; }
        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        IEnumerable<int> Permissions { get; }
    }
}
