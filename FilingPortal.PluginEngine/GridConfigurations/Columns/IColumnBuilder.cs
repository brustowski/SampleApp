using FilingPortal.PluginEngine.Lookups;
using System;
using System.Linq.Expressions;

namespace FilingPortal.PluginEngine.GridConfigurations.Columns
{
    /// <summary>
    /// Interface for column builder
    /// </summary>
    public interface IColumnBuilder<TModel>
    {
        /// <summary>
        /// Sets the name of the key field that should be used in edit mode
        /// </summary>
        /// <param name="keyFieldName">the name of the key field</param>
        [Obsolete("Please use KeyField method instead", false)]
        IColumnBuilder<TModel> KeyFieldName(string keyFieldName);
        /// <summary>
        /// Sets the key field that should be used in edit mode
        /// </summary>
        /// <param name="getterExpression">Property selector for key field</param>
        IColumnBuilder<TModel> KeyField(Expression<Func<TModel, object>> getterExpression);
        /// <summary>
        /// Sets the user-friendly name
        /// </summary>
        /// <param name="name">The name</param>
        IColumnBuilder<TModel> DisplayName(string name);
        /// <summary>
        /// Sets the minimum width
        /// </summary>
        /// <param name="width">The width</param>
        IColumnBuilder<TModel> MinWidth(int width);
        /// <summary>
        /// Sets the fixed width
        /// </summary>
        /// <param name="width">The width</param>
        IColumnBuilder<TModel> FixWidth(int width);
        /// <summary>
        /// Sets the column resizable
        /// </summary>
        IColumnBuilder<TModel> Resizable(bool value = true);
        /// <summary>
        /// Sets the column searchable
        /// </summary>
        IColumnBuilder<TModel> Searchable();
        /// <summary>
        /// Sets the maximum width
        /// </summary>
        /// <param name="width">The width</param>
        IColumnBuilder<TModel> MaxWidth(int width);
        /// <summary>
        /// Sets the column as not sortable
        /// </summary>
        IColumnBuilder<TModel> NotSortable();
        /// <summary>
        /// Sets the column as sorted by default
        /// </summary>
        IColumnBuilder<TModel> DefaultSorted();
        /// <summary>
        /// Sets the column not being able to open for viewing by click
        /// </summary>
        IColumnBuilder<TModel> NotOpenForViewByClick();
        /// <summary>
        /// Sets the alignment of the column text to the right
        /// </summary>
        IColumnBuilder<TModel> AlignRight();
        /// <summary>
        /// Sets the alignment of the column text to the left
        /// </summary>
        IColumnBuilder<TModel> AlignLeft();
        /// <summary>
        /// Sets the alignment of the column text to the center
        /// </summary>
        IColumnBuilder<TModel> Centered();
        /// <summary>
        /// Sets the number field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableNumber();
        /// <summary>
        /// Sets the floating point number field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableFloatNumber();
        /// <summary>
        /// Sets the text field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableText();
        /// <summary>
        /// Sets the boolean field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableBoolean();
        /// <summary>
        /// Sets the lookup field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableLookup();
        /// <summary>
        /// Defines the source of column data provider
        /// </summary>
        IColumnBuilder<TModel> DataSourceFrom<T>() where T : ILookupDataProvider;
        /// <summary>
        /// Defines the source of column data provider
        /// </summary>
        IColumnBuilder<TModel> DataSourceFrom(Type dataProviderType);
        /// <summary>
        /// Defines the dependency of the column on another column by column name
        /// </summary>
        /// <param name="getter">The field name getter</param>
        IColumnBuilder<TModel> DependsOn<TDataSource>(Expression<Func<TDataSource, object>> getter);
        /// <summary>
        /// Defines the dependency of the column on the property value
        /// </summary>
        /// <param name="getter">The property name getter</param>
        IColumnBuilder<TModel> DependsOnProperty(Expression<Func<TModel, object>> getter);

        /// <summary>
        /// Marks column as a key field
        /// </summary>
        IColumnBuilder<TModel> IsKeyField();

        /// <summary>
        /// Sets the date field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableDate();

        /// <summary>
        /// Sets the composite field type to editable column
        /// </summary>
        IColumnBuilder<TModel> EditableComposite();

        /// <summary>
        /// Adds sub column to the composite column 
        /// </summary>
        /// <param name="column">Sub column</param>
        IColumnBuilder<TModel> AddSubColumn(ColumnConfig column);

        /// <summary>
        /// Builds the column configuration
        /// </summary>
        ColumnConfig Build();
    }
}