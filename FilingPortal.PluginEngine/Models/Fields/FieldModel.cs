using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Models.Fields
{
    /// <summary>
    /// Class represents base field model
    /// </summary>
    public class FieldModel
    {
        /// <summary>
        /// Creates a new instance of <see cref="FieldModel"/>
        /// </summary>
        public FieldModel() { }

        /// <summary>
        /// Creates a new instance of <see cref="FieldModel"/> with property name set
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public FieldModel(string propertyName): this()
        {
            Name = propertyName;
            Title = propertyName;
        }

        /// <summary>
        /// Gets or sets field name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets field title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets field value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether field is mandatory
        /// </summary>
        public bool IsMandatory { get; set; }
        /// <summary>
        /// Gets or sets included field model
        /// </summary>
        public FormConfigModel SubFormModel { get; set; }
        /// <summary>
        /// Gets field options dictionary
        /// </summary>
        public IDictionary<string, object> Options { get; } = new Dictionary<string, object>();
    }
}