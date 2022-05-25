using System.Collections.Generic;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Class converter from db value type to ui value type
    /// </summary>
    public class ValueTypeConverter : IValueTypeConverter
    {
        /// <summary>
        /// The list of mapped types
        /// </summary>
        private readonly Dictionary<string, string> _listOfMappedTypes = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeConverter"/> class that
        /// converts types using list at https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-2017 
        /// </summary>
        public ValueTypeConverter()
        {
            AddExactNumerics();
            AddDateAndTime();
            AddCustomTypes();
        }

        /// <summary>
        /// Adds the exact numerics types
        /// </summary>
        private void AddExactNumerics()
        {
            var list = new[] {
                "bigint", "numeric",
                "smallint",
                "decimal", "smallmoney",
                "int", "tinyint",
                "money"
            };
            AddItemsToDictionary(list, UIValueTypes.Numeric);
            _listOfMappedTypes.Add("bit", UIValueTypes.Boolean);
        }
        /// <summary>
        /// Adds the date and time types
        /// </summary>
        private void AddDateAndTime()
        {
            var list = new[]
            {
                "date", "datetimeoffset",
                "datetime2", "smalldatetime",
                "datetime", "time"
            };
            AddItemsToDictionary(list, UIValueTypes.Date);
        }
        /// <summary>
        /// Adds the custom types
        /// </summary>
        private void AddCustomTypes()
        {
            _listOfMappedTypes.Add(UIValueTypes.Address.ToLower(), UIValueTypes.Address);
            _listOfMappedTypes.Add(UIValueTypes.Confirmation.ToLower(), UIValueTypes.Confirmation);
        }
        /// <summary>
        /// Adds the items to list of mapping
        /// </summary>
        /// <param name="listOfTypes">The list of types</param>
        /// <param name="uiType">Type of the UI</param>
        private void AddItemsToDictionary(string[] listOfTypes, string uiType)
        {
            foreach (var type in listOfTypes)
            {
                _listOfMappedTypes.Add(type, uiType);
            }
        }

        /// <summary>
        /// Converts the specified db value type into ui value type
        /// </summary>
        /// <param name="valueType">Db type of the value</param>
        public string Convert(string valueType)
        {
            return _listOfMappedTypes.ContainsKey(valueType.ToLower())
                ? _listOfMappedTypes[valueType.ToLower()]
                : UIValueTypes.Text;
        }
    }
}