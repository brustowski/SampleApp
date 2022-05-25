using System;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Implements Report config
    /// </summary>
    public class DynamicReportConfig : IReportConfig
    {
        /// <summary>
        /// Creates a new instance of <see cref="DynamicReportConfig"/>
        /// </summary>
        /// <param name="name"></param>
        public DynamicReportConfig(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            Name = name;
        }

        /// <summary>
        /// Grid name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => true;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(FPDynObject);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => typeof(FPDynObject);
    }
}
