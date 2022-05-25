using System;
using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Common.Lookups.Providers;

namespace FilingPortal.Web.GridConfigurations.Vessel
{
    /// <summary>
    /// Class describing the configuration for the Vessel Default Values grid
    /// </summary>
    public class VesselDefaultValuesGridConfig : DefaultValuesGridConfigBase
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselDefaultValues;
        /// <summary>
        /// Table names data provider
        /// </summary>
        protected override Type TableNamesDataProvider { get; } = typeof(VesselTableNamesDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type TableColumnsDataProvider { get; } = typeof(VesselTableColumnsDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type DependsOnDataProvider { get; } = typeof(VesselImportFormConfigurationDataProvider);
    }
}