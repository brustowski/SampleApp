using FilingPortal.Parts.Zones.Ftz214.Domain.Config;
using FilingPortal.Parts.Zones.Ftz214.Web.Common.Providers;
using FilingPortal.PluginEngine.GridConfigurations;
using System;

namespace FilingPortal.Parts.Zones.Ftz214.Web.GridConfigs
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