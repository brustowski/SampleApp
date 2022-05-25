using System;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes Information for Property Mapping
    /// </summary>
    public interface IPropertyMapInfo
    {
        /// <summary>
        /// Gets Getter method
        /// </summary>
        Func<object, object> Getter { get; }
        /// <summary>
        /// Gets Setter method
        /// </summary>
        Action<object, object> Setter { get; }
        /// <summary>
        /// Gets or sets Field Name
        /// </summary>
        string FieldName { get; set; }
        /// <summary>
        /// Gets Property name
        /// </summary>
        string PropertyName { get; }
        /// <summary>
        /// Get property type
        /// </summary>
        Type PropertyType { get; }
    }
}
