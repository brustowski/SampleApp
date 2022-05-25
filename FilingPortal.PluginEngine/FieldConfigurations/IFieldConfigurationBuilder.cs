using System;
using System.Collections.Generic;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.Fields;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Manifest field builder
    /// </summary>
    public interface IFieldConfigurationBuilder
    {
        /// <summary>
        /// Creates underlying FieldModel
        /// </summary>
        /// <param name="name">Field title</param>
        IFieldConfigurationBuilder Create(string name = "");
        /// <summary>
        /// Adds option to FieldModel
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        IFieldConfigurationBuilder AddOptions(string name, object value);
        /// <summary>
        /// Sets display title
        /// </summary>
        /// <param name="title">Display title</param>
        IFieldConfigurationBuilder Title(string title);
        /// <summary>
        /// Sets default value
        /// </summary>
        /// <param name="value">Default value</param>
        IFieldConfigurationBuilder DefaultValue(string value);
        /// <summary>
        /// Sets result control reflected type
        /// </summary>
        /// <param name="type">Value type</param>
        IFieldConfigurationBuilder Type(Type type);
        /// <summary>
        /// Sets result control reflected type
        /// </summary>
        /// <param name="type">Value type</param>
        IFieldConfigurationBuilder Type(FieldType type);
        /// <summary>
        /// Make field long
        /// </summary>
        IFieldConfigurationBuilder Long();
        /// <summary>
        /// Adds separator after field
        /// </summary>
        IFieldConfigurationBuilder Separator();
        /// <summary>
        /// Makes field multiline
        /// </summary>
        IFieldConfigurationBuilder Multiline();
        /// <summary>
        /// Makes field mandatory
        /// </summary>
        IFieldConfigurationBuilder Mandatory();
        /// <summary>
        /// Sets data provider for field value and set lookup field type
        /// </summary>
        /// <param name="provider">Data provider</param>
        IFieldConfigurationBuilder Lookup(ILookupDataProvider provider);
        /// <summary>
        /// Sets data provider for field value and set lookup field type
        /// </summary>
        /// <param name="providerName">Data provider name</param>
        /// <param name="canAdd">With this flag enabled user can add own values to Data Provider</param>
        IFieldConfigurationBuilder Lookup(string providerName, bool canAdd = false);
        /// <summary>
        /// Sets Address field type
        /// </summary>
        IFieldConfigurationBuilder Address();
        /// <summary>
        /// Sets field dependency and allowed values for field activation
        /// </summary>
        /// <param name="fieldName">The name of the field on which it depends</param>
        /// <param name="allowedValues">List of allowed values at which the field becomes active</param>
        IFieldConfigurationBuilder DependsOn(string fieldName, IEnumerable<string> allowedValues = null);
        /// <summary>
        /// Creates configuration for table field
        /// </summary>
        IFieldConfigurationBuilder Table(FormConfigModel model);
        /// <summary>
        /// Makes field start from new line
        /// </summary>
        IFieldConfigurationBuilder FullLineControl();
        /// <summary>
        /// Builds FieldModel
        /// </summary>
        FieldModel Build();
    }
}
