using System;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Information for Property Mapping
    /// </summary>
    /// <typeparam name="T">Type of the property</typeparam>
    public sealed class PropertyMapInfo<T> : IPropertyMapInfo
    {
        /// <summary>
        /// Gets Getter method
        /// </summary>
        public Func<T, object> Getter { get; set; }
        /// <summary>
        /// Gets Getter method
        /// </summary>
        Func<object, object> IPropertyMapInfo.Getter => o => Getter((T)o);
        /// <summary>
        /// Gets Setter method
        /// </summary>
        public Action<T, object> Setter { get; set; }
        /// <summary>
        /// Gets Setter method
        /// </summary>
        Action<object, object> IPropertyMapInfo.Setter => (o, p) => Setter((T)o, p);
        /// <summary>
        /// Gets or sets Field Name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets Property name
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Get property type
        /// </summary>
        public Type PropertyType { get; set; }
    }
}
