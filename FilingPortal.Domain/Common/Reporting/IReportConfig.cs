using System;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Describes report configuration
    /// </summary>
    public interface IReportConfig
    {
        /// <summary>
        /// Gets or sets the report name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets or sets the report file name
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// Gets or sets the Document title
        /// </summary>
        string DocumentTitle { get; }
        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        bool IsFilterSettingsVisible { get; }
        /// <summary>
        /// Gets or sets the underlying model type
        /// </summary>
        Type ModelType { get; }
        /// <summary>
        /// Gets or sets the source model type
        /// </summary>
        Type ModelDataType { get; }
    }
}
