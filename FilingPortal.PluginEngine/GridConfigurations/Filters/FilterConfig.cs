using System;
using Framework.Domain.Paging;
using Newtonsoft.Json;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// Configuration of filter
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterConfig"/> class for specified property name
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public FilterConfig(string propertyName)
        {
            FieldName = propertyName;
            IsSearch = true;
            Operand = FilterOperands.Equal;
            Advanced = false;
        }

        /// <summary>
        /// Gets or sets the operand
        /// </summary>
        public string Operand { get; set; }

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter has search ability
        /// </summary>
        public bool IsSearch { get; set; }

        /// <summary>
        /// Gets or sets the maximum length
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the type of the data source
        /// </summary>
        [JsonIgnore]
        public Type DataSourceType { get; set; }

        /// <summary>
        /// Gets or sets the filter dependency from another filter by name
        /// </summary>
        public string DependOn { get; set; }

        /// <summary>
        /// Gets or sets the filter should be located in Advanced collapsed section
        /// </summary>
        public bool Advanced { get; set; }

        /// <summary>
        /// Gets or sets the value indicating if a filter is used for an update operation
        /// </summary>
        public bool IsUpdateFilter { get; set; }

        /// <summary>
        /// Gets or sets whether current filter applies multiple values
        /// </summary>
        public bool IsMultiSelect { get; set; }
    }
}