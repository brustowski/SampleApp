using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.Common
{
    /// <summary>
    /// Represents the service to resolve collection of the compatible data types
    /// </summary>
    internal class CompatibleDataTypeService : ICompatibleDataTypeService
    {
        /// <summary>
        /// The list of mapped types
        /// </summary>
        private readonly Dictionary<string, string[]> _listOfCompatibleTypes = new Dictionary<string, string[]>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompatibleDataTypeService"/> class that
        /// converts types using list at https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-2017 
        /// </summary>
        public CompatibleDataTypeService()
        {
            AddOther();
        }

        /// <summary>
        /// Adds the other types
        /// </summary>
        private void AddOther()
        {
            _listOfCompatibleTypes.Add("int", new[] { UIValueTypes.Address });
            _listOfCompatibleTypes.Add("bit", new[] { UIValueTypes.Confirmation });
        }

        /// <summary>
        /// Gets collection of the compatible data types for the specified type
        /// </summary>
        /// <param name="type">The type</param>
        public IEnumerable<string> Get(string type)
        {
            return _listOfCompatibleTypes.ContainsKey(type.ToLower())
                ? _listOfCompatibleTypes[type.ToLower()]
                : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets collection of the compatible data types
        /// </summary>
        public IEnumerable<string> GetAll()
        {
            return _listOfCompatibleTypes.Values.SelectMany(x => x).Distinct();
        }
    }
}
