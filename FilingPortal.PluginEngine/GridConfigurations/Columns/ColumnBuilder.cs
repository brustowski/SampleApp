using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Specifications;

namespace FilingPortal.PluginEngine.GridConfigurations.Columns
{
    /// <summary>
    /// Class for column configuration builder
    /// </summary>
    public class ColumnBuilder<TModel> : IColumnBuilder<TModel>
    {
        /// <summary>
        /// The column configuration
        /// </summary>
        private readonly ColumnConfig _columnConfig;

        public static IColumnBuilder<TModel> CreateFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            return new ColumnBuilder<TModel>(propertyName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnBuilder{TModel}"/> class
        /// </summary>
        /// <param name="columnConfig">The column configuration</param>
        public ColumnBuilder(ColumnConfig columnConfig)
        {
            _columnConfig = columnConfig;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ColumnBuilder{TModel}"/> class
        /// </summary>
        /// <param name="propertyName">The name of the column</param>
        public ColumnBuilder(string propertyName)
        {
            _columnConfig = new ColumnConfig(propertyName);
        }

        /// <summary>
        /// Sets the name of the key field that should be used in edit mode
        /// </summary>
        /// <param name="keyFieldName">the name of the key field</param>
        [Obsolete("Please use KeyField method instead", false)]
        public IColumnBuilder<TModel> KeyFieldName(string keyFieldName)
        {
            _columnConfig.KeyFieldName = keyFieldName;

            return this;
        }

        /// <summary>
        /// Sets the key field that should be used in edit mode
        /// </summary>
        /// <param name="getterExpression">Expression to get property name</param>
        /// <returns></returns>
        public IColumnBuilder<TModel> KeyField(Expression<Func<TModel, object>> getterExpression)
        {
            _columnConfig.KeyFieldName = PropertyExpressionHelper.GetPropertyName(getterExpression);

            return this;
        }

        /// <summary>
        /// Sets the user-friendly name
        /// </summary>
        /// <param name="name">The name</param>
        public IColumnBuilder<TModel> DisplayName(string name)
        {
            _columnConfig.DisplayName = name;

            return this;
        }

        /// <summary>
        /// Sets the minimum width
        /// </summary>
        /// <param name="width">The width</param>
        public IColumnBuilder<TModel> MinWidth(int width)
        {
            _columnConfig.MinWidth = width;

            return this;
        }

        /// <summary>
        /// Sets the maximum width
        /// </summary>
        /// <param name="width">The width</param>
        public IColumnBuilder<TModel> MaxWidth(int width)
        {
            _columnConfig.MaxWidth = width;
            return this;
        }

        /// <summary>
        /// Sets the fixed width
        /// </summary>
        /// <param name="width">The width</param>
        public IColumnBuilder<TModel> FixWidth(int width)
        {
            _columnConfig.MinWidth = width;
            _columnConfig.MaxWidth = width;

            return this;
        }

        /// <summary>
        /// Sets the column as not sortable
        /// </summary>
        public IColumnBuilder<TModel> NotSortable()
        {
            _columnConfig.IsSortable = false;
            return this;
        }

        /// <summary>
        /// Sets the column as sorted by default
        /// </summary>
        public IColumnBuilder<TModel> DefaultSorted()
        {
            _columnConfig.DefaultSorted = true;
            return this;
        }

        /// <summary>
        /// Sets the column not being able to open for viewing by click
        /// </summary>
        public IColumnBuilder<TModel> NotOpenForViewByClick()
        {
            _columnConfig.IsViewOpen = false;
            return this;
        }

        /// <summary>
        /// Sets the alignment of the column text to the right
        /// </summary>
        public IColumnBuilder<TModel> AlignRight()
        {
            _columnConfig.Align = ColumnAlign.Right;
            return this;
        }

        /// <summary>
        /// Sets the alignment of the column text to the left
        /// </summary>
        public IColumnBuilder<TModel> AlignLeft()
        {
            _columnConfig.Align = ColumnAlign.Left;
            return this;
        }

        /// <summary>
        /// Sets the alignment of the column text to the center
        /// </summary>
        public IColumnBuilder<TModel> Centered()
        {
            _columnConfig.Align = ColumnAlign.Center;
            return this;
        }

        /// <summary>
        /// Sets the number field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableNumber()
        {
            _columnConfig.EditType = ColumnEditTypes.Number;
            return this;
        }

        /// <summary>
        /// Sets the floating point number field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableFloatNumber()
        {
            _columnConfig.EditType = ColumnEditTypes.FloatNumber;
            return this;
        }

        /// <summary>
        /// Sets the text field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableText()
        {
            _columnConfig.EditType = ColumnEditTypes.Text;
            return this;
        }
        /// <summary>
        /// Sets the boolean field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableBoolean()
        {
            _columnConfig.EditType = ColumnEditTypes.Boolean;
            return this;
        }
        /// <summary>
        /// Sets the lookup field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableLookup()
        {
            _columnConfig.EditType = ColumnEditTypes.Lookup;
            return this;
        }
        /// <summary>
        /// Defines the source of column data provider
        /// </summary>
        public IColumnBuilder<TModel> DataSourceFrom<T>() where T : ILookupDataProvider
        {
            return DataSourceFrom(typeof(T));
        }

        /// <summary>
        /// Defines the source of column data provider
        /// </summary>
        /// <param name="dataProviderType">Type of data provider</param>
        public IColumnBuilder<TModel> DataSourceFrom(Type dataProviderType)
        {
            _columnConfig.DataSourceType = dataProviderType;
            return this;
        }

        /// <summary>
        /// Defines the dependency of the column on another column by column name
        /// </summary>
        /// <param name="getter">The field name getter</param>
        public IColumnBuilder<TModel> DependsOn<TDataSource>(Expression<Func<TDataSource, object>> getter)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getter);
            _columnConfig.DependOn = propertyName;
            return this;
        }

        /// <summary>
        /// Defines the dependency of the column on the Property value
        /// </summary>
        /// <param name="getter">The Property name getter</param>
        public IColumnBuilder<TModel> DependsOnProperty(Expression<Func<TModel, object>> getter)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getter);
            _columnConfig.DependOnProperty = propertyName;
            return this;
        }

        /// <summary>
        /// Marks column as a key field
        /// </summary>
        public IColumnBuilder<TModel> IsKeyField()
        {
            _columnConfig.IsKeyField = true;
            return this;
        }

        /// <summary>
        /// Sets the date field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableDate()
        {
            _columnConfig.EditType = ColumnEditTypes.Date;
            return this;
        }

        //todo: move to the CompositeColumnBuilder or delete
        /// <summary>
        /// Sets the composite field type to editable column
        /// </summary>
        public IColumnBuilder<TModel> EditableComposite()
        {
            _columnConfig.EditType = ColumnEditTypes.Composite;
            return this;
        }

        /// <summary>
        /// Defines column as resizable
        /// </summary>
        public IColumnBuilder<TModel> Resizable(bool value = true)
        {
            _columnConfig.IsResizable = value;
            return this;
        }
        
        /// <summary>
        /// Defines column as searchable 
        /// </summary>
        public IColumnBuilder<TModel> Searchable()
        {
            _columnConfig.IsSearchable = true;
            return this;
        }
        
        //todo: move to the CompositeColumnBuilder or set type in it 
        /// <summary>
        /// Adds sub column to the complex column 
        /// </summary>
        /// <param name="column">Sub column</param>
        public IColumnBuilder<TModel> AddSubColumn(ColumnConfig column)
        {
            if (_columnConfig.Columns == null)
            {
                _columnConfig.Columns = new List<ColumnConfig>();
            }

            _columnConfig.Columns.Add(column);

            return this;
        }

        /// <summary>
        /// Builds the column configuration
        /// </summary>
        public ColumnConfig Build()
        {
            return _columnConfig;
        }
    }
}