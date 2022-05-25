using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    public abstract class ReportModelMap<T> where T : class
    {
        private readonly Dictionary<string, IColumnMapInfo> _columnMapInfos = new Dictionary<string, IColumnMapInfo>();

        private PropertyToColumnNameConverter GetPropertyToColumnNameConverter()
        {
            return new PropertyToColumnNameConverter();
        }

        protected ReportModelMap()
        {
            var type = typeof(T);
            var converter = GetPropertyToColumnNameConverter();

            foreach (var prop in type.GetProperties())
            {
                var columnMapInfo = new ColumnMapInfo<T>(
                    converter.Convert(prop.Name),
                    FastPropertyFactory.GeneratePropertyGetter<T>(prop.Name),
                    prop.PropertyType);
                _columnMapInfos.Add(prop.Name, columnMapInfo);
            }
        }

        public IColumnMapOptions Map<TValue>(Expression<Func<T, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);

            var columnMapInfo = _columnMapInfos[propertyName];

            var columnMapOptions = new ColumnMapOptions(columnMapInfo);
            columnMapOptions.ColumnTitle(propertyName);

            return columnMapOptions;
        }


        protected void Ignore<TValue>(Expression<Func<T, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            _columnMapInfos.Remove(propertyName);
        }



        public IEnumerable<IColumnMapInfo> GetColumnInfos()
        {
            return _columnMapInfos.Values;
        }

        public Type GetModelType => typeof(T);

        public IColumnMapInfo GetConfig(string name)
        {
            if (_columnMapInfos.ContainsKey(name))
            {
                return _columnMapInfos[name];
            }

            throw new KeyNotFoundException($"The config for property {name} not found");
        }
    }
}
