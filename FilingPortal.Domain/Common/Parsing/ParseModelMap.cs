using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Base class for Parse Model Map classes
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="IParsingDataModel"/></typeparam>
    public abstract class ParseModelMap<T> : IParseModelMap 
        where T : IParsingDataModel, new()
    {
        /// <summary>
        /// List of <see cref="IPropertyMapInfo"/> objects
        /// </summary>
        private readonly List<IPropertyMapInfo> _propertyMapInfos = new List<IPropertyMapInfo>();
        /// <summary>
        /// Gets collection of the all <see cref="IPropertyMapInfo"/>
        /// </summary>
        public IEnumerable<IPropertyMapInfo> MapInfos => _propertyMapInfos.AsReadOnly();

        /// <summary>
        /// Set property mapping for model 
        /// </summary>
        /// <typeparam name="TValue"><see cref="IParseModelMap"/> type</typeparam>
        /// <param name="setterExpression">Value setter</param>
        /// <param name="name">Field name</param>
        public IPropertyMapOptions Map<TValue>(Expression<Func<T, TValue>> setterExpression, string name = null)
        {
            Action<T, TValue> set = PropertyExpressionHelper.InitializeSetter(setterExpression);
            Func<T, TValue> get = PropertyExpressionHelper.InitializeGetter(setterExpression);
            var setter = new Action<T, object>((o, v) => set(o, (TValue)v));
            var getter = new Func<T, object>(o => get(o));
            var propertyName = PropertyExpressionHelper.GetPropertyName(setterExpression);
            var mapInfo = new PropertyMapInfo<T>
            {
                Setter = setter,
                Getter = getter,
                FieldName = name ?? propertyName,
                PropertyName = propertyName,
                PropertyType = PropertyExpressionHelper.GetPropertyType(setterExpression)
            };
            var columnMapOptions = new PropertyMapOptions<T>(mapInfo);

            _propertyMapInfos.Add(mapInfo);

            return columnMapOptions;
        }
        /// <summary>
        /// Sets the sheet name
        /// </summary>
        /// <param name="sheetName">Sheet name</param>
        protected void Sheet(string sheetName)
        {
            SheetName = sheetName;
        }
        /// <summary>
        /// Get <see cref="IPropertyMapInfo"/> by  model field name
        /// </summary>
        /// <param name="modelFieldName">Model field name</param>
        public IPropertyMapInfo GetMapInfo(string modelFieldName)
        {
            var excelColumnName = modelFieldName.Trim();
            IPropertyMapInfo columnInfo = _propertyMapInfos.FirstOrDefault(x => x.FieldName.Equals(excelColumnName, StringComparison.InvariantCultureIgnoreCase));
            return columnInfo;
        }
        /// <summary>
        /// Get Column name by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public string GetColumnName(string propertyName)
        {
            return _propertyMapInfos.First(x => x.PropertyName == propertyName).FieldName;
        }

        /// <summary>
        /// Get property name by column name
        /// </summary>
        /// <param name="columnName">column name</param>
        public string GetPropertyName(string columnName)
        {
            return _propertyMapInfos.First(x => x.FieldName == columnName).PropertyName;
        }

        /// <summary>
        /// Gets Sheet name
        /// </summary>
        public string SheetName { get; private set; }

        /// <summary>
        /// Gets the model type
        /// </summary>
        public Type GetModelType => typeof(T);
    }
}
