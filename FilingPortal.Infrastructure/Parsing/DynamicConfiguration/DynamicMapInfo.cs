using FilingPortal.Domain.Common.Parsing;
using System;

namespace FilingPortal.Infrastructure.Parsing.DynamicConfiguration
{
    /// <summary>
    /// Represents Dynamic Property Mapping
    /// </summary>
    internal sealed class DynamicMapInfo : IPropertyMapInfo
    {
        /// <summary>
        /// Gets Getter method
        /// </summary>
        public Func<object, object> Getter { get; }

        /// <summary>
        /// Gets Setter method
        /// </summary>
        public Action<object, object> Setter { get; set; }
        /// <summary>
        /// Gets or sets Field Name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets Property name
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Get property type
        /// </summary>
        public Type PropertyType { get; }
    }
}
