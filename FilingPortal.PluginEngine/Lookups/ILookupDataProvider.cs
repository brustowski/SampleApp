using FilingPortal.PluginEngine.GridConfigurations.Filters;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.Lookups
{
    /// <summary>
    /// Interface for Column DataProvider
    /// </summary>
    public interface ILookupDataProvider : IDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Column data provider with ability to add new values
    /// </summary>
    public interface IEditableLookupDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Adds new value to handbook
        /// </summary>
        /// <param name="modelValue">Option value</param>
        /// <param name="dependValue">Additional data</param>
        LookupItem Add(string modelValue, object dependValue = null);
    }
}
