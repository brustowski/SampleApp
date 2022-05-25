using System;
using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Default Values grid
    /// </summary>
    public class DefaultValuesGridConfig : DefaultValuesGridConfigBase
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.DefaultValues;

        /// <summary>
        /// Table names data provider
        /// </summary>
        protected override Type TableNamesDataProvider { get; } = typeof(TableNamesDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type TableColumnsDataProvider { get; } = typeof(TableColumnsDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type DependsOnDataProvider { get; } = typeof(FormConfigurationDataProvider);
    }
}