using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.Fields;
using Framework.Domain.Specifications;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Can create form configurations
    /// </summary>
    public interface IFormConfigFactory<TEditModel> where TEditModel : class
    {
        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        FormConfigModel CreateFormConfig();
    }
    /// <summary>
    /// Factory that provides abstract Form configuration
    /// </summary>
    public abstract class FormConfigFactory<TEditModel> : IFormConfigFactory<TEditModel>
    where TEditModel : class
    {
        readonly IList<FieldModel> _configs = new List<FieldModel>();

        /// <summary>
        /// Sets up field configurations
        /// </summary>
        protected abstract void Setup();

        /// <summary>
        /// Creates form configuration
        /// </summary>
        public FormConfigModel CreateFormConfig()
        {
            Setup();
            return new FormConfigModel { Fields = _configs };
        }

        /// <summary>
        /// Adds fields configuration
        /// </summary>
        /// <typeparam name="TValue">Parameter type</typeparam>
        /// <param name="getterExpression">Edit model</param>
        /// <param name="value">Default value</param>
        protected IFieldConfigurationBuilder AddField<TValue>(Expression<Func<TEditModel, TValue>> getterExpression, string value = "")
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var propertyType = typeof(TValue);

            var model = new FieldModel(propertyName);
            _configs.Add(model);

            var fieldBuilder = new FieldConfigurationBuilder();

            return fieldBuilder.Configure(model).DefaultValue(value).Type(propertyType);
        }
    }
}
