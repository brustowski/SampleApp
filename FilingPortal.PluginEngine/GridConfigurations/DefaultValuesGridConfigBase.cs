using System;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Class describing the configuration for the Default Values grid
    /// </summary>
    public abstract class DefaultValuesGridConfigBase : GridConfiguration<DefValuesViewModel>
    {
        /// <summary>
        /// Table names data provider
        /// </summary>
        protected abstract Type TableNamesDataProvider { get; }
        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected abstract Type TableColumnsDataProvider { get; }

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected abstract Type DependsOnDataProvider { get; }

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected sealed override void ConfigureColumns()
        {
            AddColumn(x => x.DisplayOnUI).DisplayName("Display On UI").MinWidth(150).EditableNumber();
            AddColumn(x => x.ValueLabel).DisplayName("Label").MinWidth(200).DefaultSorted().EditableText();
            AddColumn(x => x.DefaultValue).DisplayName("Default value").MinWidth(200).EditableText();
            AddColumn(x => x.ValueDesc).DisplayName("Description").MinWidth(200).EditableText();
            AddColumn(x => x.TableName).DisplayName("Table Name").MinWidth(200).EditableLookup().DataSourceFrom(TableNamesDataProvider);
            AddColumn(x => x.ColumnName).DisplayName("Column Name").MinWidth(250).EditableLookup().DataSourceFrom(TableColumnsDataProvider).DependsOn<DefValuesViewModel>(x => x.TableName);
            AddColumn(x => x.Manual).DisplayName("Manual").MinWidth(150).EditableNumber();
            AddColumn(x => x.Editable).DisplayName("Editable").MinWidth(150).EditableBoolean();
            AddColumn(x => x.UISection).DisplayName("UI Section").MinWidth(200);
            AddColumn(x => x.Mandatory).DisplayName("Mandatory").MinWidth(150).EditableBoolean();
            AddColumn(x => x.SingleFilingOrder).DisplayName("Single Filing").MinWidth(150).EditableNumber();
            AddColumn(x => x.PairedFieldTable).DisplayName("Display With: Table").MinWidth(200).EditableLookup().DataSourceFrom(TableNamesDataProvider);
            AddColumn(x => x.PairedFieldColumn).DisplayName("Display With: Column").MinWidth(250).EditableLookup().DataSourceFrom(TableColumnsDataProvider).DependsOn<DefValuesViewModel>(x => x.PairedFieldTable);
            AddColumn(x => x.HandbookName).DisplayName("Handbook").MinWidth(150).EditableLookup().DataSourceFrom<HandbooksDataProvider>();
            AddColumn(x => x.DependsOn).DisplayName("Depends On").MinWidth(250)
                .EditableLookup().DataSourceFrom(DependsOnDataProvider).KeyField(x => x.DependsOnId)
                .DependsOn<DefValuesViewModel>(x => x.TableName);
            AddColumn(x => x.OverriddenType).DisplayName("Overridden Type").MinWidth(150)
                .EditableLookup().DataSourceFrom<FieldTypeDataProvider>().DependsOnProperty(x => x.ValueType);
            AddColumn(x => x.ConfirmationNeeded).DisplayName("Confirmation Needed").MinWidth(150).EditableBoolean();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected sealed override void ConfigureFilters()
        {
            NumberFilterFor(x => x.DisplayOnUI).Title("Display On UI");
            TextFilterFor(x => x.ValueLabel).Title("Label");
            TextFilterFor(x => x.DefaultValue).Title("Default Value");
            TextFilterFor(x => x.ValueDesc).Title("Description");
            TextFilterFor(x => x.TableName).Title("Table Name");
            TextFilterFor(x => x.ColumnName).Title("Column Name");
            NumberFilterFor(x => x.Manual).Title("Manual");
            SelectFilterFor(x => x.Editable).Title("Editable").DataSourceFrom<YesNoFilterDataProvider>().NotSearch();
            TextFilterFor(x => x.UISection).Title("UI Section");
            SelectFilterFor(x => x.Mandatory).Title("Mandatory").DataSourceFrom<YesNoFilterDataProvider>().NotSearch();
            TextFilterFor(x => x.DependsOn).Title("Depends On");
            TextFilterFor(x => x.OverriddenType).Title("Overridden Type");
            SelectFilterFor(x => x.ConfirmationNeeded).Title("Confirmation").DataSourceFrom<YesNoFilterDataProvider>().NotSearch();
        }
    }
}