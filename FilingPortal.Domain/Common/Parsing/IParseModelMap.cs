using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes mapping parsed data to model
    /// </summary>
    public interface IParseModelMap
    {
        /// <summary>
        /// Gets collection of the all <see cref="IPropertyMapInfo"/>
        /// </summary>
        IEnumerable<IPropertyMapInfo> MapInfos { get; }
        /// <summary>
        /// Get <see cref="IPropertyMapInfo"/> by  model field name
        /// </summary>
        /// <param name="modelFieldName">Model field name</param>
        IPropertyMapInfo GetMapInfo(string modelFieldName);
        /// <summary>
        /// Get Column name by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        string GetColumnName(string propertyName);
        /// <summary>
        /// Get property name by column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        string GetPropertyName(string columnName);
        /// <summary>
        /// Gets Sheet name
        /// </summary>
        string SheetName { get; }
        /// <summary>
        /// Gets the model type
        /// </summary>
        Type GetModelType { get; }
    }
}
