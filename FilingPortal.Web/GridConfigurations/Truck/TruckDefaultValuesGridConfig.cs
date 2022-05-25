using System;
using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Class describing the configuration for the Default Values grid
    /// </summary>
    public class TruckDefaultValuesGridConfig : DefaultValuesGridConfigBase
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckDefaultValues;
        /// <summary>
        /// Table names data provider
        /// </summary>
        protected override Type TableNamesDataProvider { get; } = typeof(TruckTableNamesDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type TableColumnsDataProvider { get; } = typeof(TruckTableColumnsDataProvider);

        /// <summary>
        /// Table columns data provider
        /// </summary>
        protected override Type DependsOnDataProvider { get; } = typeof(TruckFormConfigurationDataProvider);
    }
}