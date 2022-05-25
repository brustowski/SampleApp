using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Packs unit of measure data 
    /// </summary>
    public class PacksUnitOfMeasureDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="PacksUnitOfMeasureDataProvider"/>
        /// </summary>
        /// <param name="repository">Data repository</param>
        public PacksUnitOfMeasureDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.PacksUnitOfMeasure;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "cw_packtype";
    }
}