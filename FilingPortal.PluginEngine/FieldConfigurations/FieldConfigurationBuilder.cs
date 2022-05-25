using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.Fields;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Builds FieldModel
    /// </summary>
    public class FieldConfigurationBuilder : IFieldConfigurationBuilder
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        protected FieldModel _model;

        /// <summary>
        /// Creates model builder to configure model
        /// </summary>
        /// <param name="model">Model to configure</param>
        public IFieldConfigurationBuilder Configure(FieldModel model)
        {
            _model = model;
            return this;
        }

        /// <summary>
        /// Creates underlying FieldModel
        /// </summary>
        /// <param name="name">Field title</param>
        public IFieldConfigurationBuilder Create(string name = "")
        {
            _model = new FieldModel {Name = name, Title = name };
            return this;
        }
        /// <summary>
        /// Adds option to FieldModel
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        public IFieldConfigurationBuilder AddOptions(string name, object value)
        {
            _model.Options[name] = value;
            return this;
        }
        /// <summary>
        /// Sets data provider for field value and set lookup filed type
        /// </summary>
        /// <param name="provider">Data provider</param>
        public IFieldConfigurationBuilder Lookup(ILookupDataProvider provider) => Lookup(provider.Name);

        /// <summary>
        /// Sets data provider for field value and set lookup filed type
        /// </summary>
        /// <param name="providerName">Data provider name</param>
        /// <param name="canAdd">With this flag enabled user can add own values to Data Provider</param>
        public IFieldConfigurationBuilder Lookup(string providerName, bool canAdd = false)
        {
            AddOptions("provider", providerName);
            if (canAdd)
            {
                AddOptions("providerCanAdd", true);
            }

            Type(FieldType.Lookup);
            return this;
        }

        /// <summary>
        /// Sets Address field type
        /// </summary>
        public IFieldConfigurationBuilder Address()
        {
            AddOptions("provider", DataProviderNames.ClientAddresses);
            Type(FieldType.Address);
            return this;
        }

        /// <summary>
        /// Sets field dependency and allowed values for field activation
        /// </summary>
        /// <param name="fieldName">The name of the field on which it depends</param>
        /// <param name="allowedValues">List of allowed values at which the field becomes active</param>
        public IFieldConfigurationBuilder DependsOn(string fieldName, IEnumerable<string> allowedValues = null)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return this;
            }

            AddOptions("dependsOn", fieldName);
            if (allowedValues != null && allowedValues.Any())
            {
                AddOptions("dependsOnValues", allowedValues);
            }

            return this;
        }

        /// <summary>
        /// Creates configuration for table field
        /// </summary>
        public IFieldConfigurationBuilder Table(FormConfigModel model)
        {
            _model.SubFormModel = model;
            return Type(FieldType.Table);
        }

        /// <summary>
        /// Makes field start from new line
        /// </summary>
        public IFieldConfigurationBuilder FullLineControl() => AddOptions("fullLine", true);

        /// <summary>
        /// Builds FieldModel
        /// </summary>
        public FieldModel Build() => _model;
        /// <summary>
        /// Make field long
        /// </summary>
        public IFieldConfigurationBuilder Long() => AddOptions("long", true);
        /// <summary>
        /// Adds separator after field
        /// </summary>
        public IFieldConfigurationBuilder Separator() => AddOptions("separator", true);
        /// <summary>
        /// Makes field multiline
        /// </summary>
        public IFieldConfigurationBuilder Multiline() => AddOptions("multiline", true);
        /// <summary>
        /// Makes field multiline
        /// </summary>
        public IFieldConfigurationBuilder Mandatory()
        {
            _model.IsMandatory = true;
            return this;
        }
        /// <summary>
        /// Sets display title
        /// </summary>
        /// <param name="title">Display title</param>
        public IFieldConfigurationBuilder Title(string title)
        {
            _model.Title = title;
            return this;
        }
        /// <summary>
        /// Sets default value
        /// </summary>
        /// <param name="value">Default value</param>
        public IFieldConfigurationBuilder DefaultValue(string value)
        {
            _model.Value = value;
            return this;
        }
        /// <summary>
        /// Sets result control reflected type
        /// </summary>
        /// <param name="type">System type</param>
        public IFieldConfigurationBuilder Type(Type type)
        {
            switch (type.Name)
            {
                case "DateTime": return Type(FieldType.Date);
                case "Integer": return Type(FieldType.Number);
                default: return Type(FieldType.Text);
            }
        }
        /// <summary>
        /// Sets result control reflected type
        /// </summary>
        /// <param name="type">Field type</param>
        public IFieldConfigurationBuilder Type(FieldType type)
        {
            return AddOptions("type", type.GetDescription());
        }
    }
}