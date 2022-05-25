using System;
using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;

namespace FilingPortal.Domain.DTOs.ReviewSectionModels
{
    /// <summary>
    /// Represents the Review section Configuration
    /// </summary>
    public class ReviewSectionStaticConfiguration: IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => "form-data";

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(FPDynObject);

        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(FPDynObject);

        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions { get; }
    }
}