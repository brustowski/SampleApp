using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FilingPortal.PluginEngine.GridConfigurations.Columns
{
    /// <summary>
    /// Class for column configuration
    /// </summary>
    public class ColumnConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnConfig"/> class
        /// </summary>
        public ColumnConfig()
        {
            IsSortable = true;
            Align = ColumnAlign.Left;
            IsResizable = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnConfig"/> class with specified field name
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public ColumnConfig(string propertyName): this()
        {
            FieldName = propertyName;
            DisplayName = FieldName;
            IsViewOpen = true;
        }

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the name of the key field that should be used in edit mode
        /// </summary>
        public string KeyFieldName { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is sortable
        /// </summary>
        public bool IsSortable { get; set; }

        /// <summary>
        /// Gets or sets the minimum width
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is sorted by default
        /// </summary>
        public bool DefaultSorted { get; set; }

        /// <summary>
        /// Gets or sets the maximum width
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is view open
        /// </summary>
        public bool IsViewOpen { get; set; }

        /// <summary>
        /// Gets or sets the align
        /// </summary>
        public ColumnAlign Align { get; set; }

        /// <summary>
        /// Gets or sets the editable field type
        /// </summary>
        public string EditType { get; set; }

        /// <summary>
        /// Gets or sets the type of the data source
        /// </summary>
        [JsonIgnore]
        public Type DataSourceType { get; set; }

        /// <summary>
        /// Gets or sets the field dependency from another field by name
        /// </summary>
        public string DependOn { get; set; }
        /// <summary>
        /// Get or sets whether column is resizable
        /// </summary>
        public bool IsResizable { get; set; }
        /// <summary>
        /// Get or sets whether column is searchable
        /// </summary>
        public bool IsSearchable { get; set; }
        /// <summary>
        /// Gets or sets whether column should be marked as key field
        /// </summary>
        public bool IsKeyField { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that should provide dependent value for the lookup field
        /// </summary>
        public string DependOnProperty { get; set; }

        /// <summary>
        /// Gets or sets the list of the sub columns
        /// </summary>
        public List<ColumnConfig> Columns { get; set; }
    }
}