using System;
using System.Linq.Expressions;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Describes the field configuration builder
    /// </summary>
    public interface IConfigFieldModelBuilder<TModel> : IFieldConfigurationBuilder
        where TModel : class
    {
        /// <summary>
        /// Creates underlying Field Model
        /// </summary>
        /// <param name="getterExpression">Model property</param>
        /// <param name="value">Field value</param>
        IFieldConfigurationBuilder Create<TValue>(Expression<Func<TModel, TValue>> getterExpression, string value = "");
    }
}
