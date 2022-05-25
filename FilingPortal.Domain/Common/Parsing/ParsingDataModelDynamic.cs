using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Dynamic object to create models safely
    /// </summary>
    public class ParsingDataModelDynamic : DynamicObject, IParsingDataModel
    {
        private readonly Dictionary<string, object> _properties;
        
        /// <summary>
        /// Gets or sets the section
        /// </summary>
        public string Section { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingDataModelDynamic"/> class.
        /// </summary>
        /// <param name="properties">The properties of the object</param>
        public ParsingDataModelDynamic(Dictionary<string, object> properties) => _properties = properties;

        /// <summary>
        /// Initializes a new instance of empty <see cref="ParsingDataModelDynamic"/> class.
        /// </summary>
        public ParsingDataModelDynamic()
        {
            _properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ParsingDataModelDynamic"/> class.
        /// </summary>
        /// <param name="properties">The properties of the object</param>
        public static dynamic GetDynamicObject(Dictionary<string, object> properties) => new ParsingDataModelDynamic(properties);

        /// <summary>
        /// Gets all property names
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames() => _properties.Keys;

        /// <summary>
        /// Tries to get member value. Returns false if property doesn't exist
        /// </summary>
        /// <param name="binder">The binder <see cref="GetMemberBinder"/></param>
        /// <param name="result">The result <see cref="object"/></param>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public object GetMember(string memberName)
        {
            if (_properties.ContainsKey(memberName))
            {
                return _properties[memberName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to set member value. Returns false if property doesn't exist
        /// </summary>
        /// <param name="binder">The binder<see cref="SetMemberBinder"/></param>
        /// <param name="value">The value<see cref="object"/></param>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                _properties[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Adds property to dynamic object
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Property value</param>
        public void SetMember(string propertyName, object value)
        {
            if (_properties.ContainsKey(propertyName))
            {
                _properties[propertyName] = value;
            }
            else
            {
                _properties.Add(propertyName, value);
            }
        }

        /// <summary>
        /// Returns copy of object properties
        /// </summary>
        public IDictionary<string, object> GetProperties() => _properties.ToDictionary(entry => entry.Key, entry => entry.Value);

        /// <summary>
        /// Gets or sets corresponding row number in the file
        /// </summary>
        public int RowNumberInFile { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            var values = _properties.Select(x => x.Value?.ToString() ?? string.Empty).ToArray();
            return string.Join("|", values);
        }
    }
}
