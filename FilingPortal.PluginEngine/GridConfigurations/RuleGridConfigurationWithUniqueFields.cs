using System.Reflection;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.Models;
using Framework.Domain;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Class for rule grid configuration
    /// </summary>
    public abstract class RuleGridConfigurationWithUniqueFields<TModel, TEntity> : RuleGridConfiguration<TModel>
        where TModel : RuleViewModelWithActions
        where TEntity : Entity
    {
        private readonly IKeyFieldsService _keyFieldsService;

        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        protected RuleGridConfigurationWithUniqueFields(IKeyFieldsService keyFieldsService)
        {
            _keyFieldsService = keyFieldsService;
        }

        /// <summary>
        /// Adds the column with custom name
        /// </summary>
        /// <param name="propertyName">Column property name</param>
        protected override IColumnBuilder<TModel> AddColumn(string propertyName)
        {
            IColumnBuilder<TModel> builder = base.AddColumn(propertyName);
            PropertyInfo originalProperty = typeof(TEntity).GetProperty(propertyName);
            if (originalProperty != null && _keyFieldsService.IsKeyField<TEntity>(originalProperty.Name))
            {
                builder.IsKeyField();
            }
            return builder;
        }
    }
}