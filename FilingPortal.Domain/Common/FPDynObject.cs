using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Dynamic object to create models safely
    /// </summary>
    public class FPDynObject : DynamicObject
    {
        private readonly Dictionary<string, object> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="FPDynObject"/> class.
        /// </summary>
        /// <param name="properties">The properties of the object</param>
        public FPDynObject(Dictionary<string, object> properties) => _properties = properties;

        /// <summary>
        /// Initializes a new instance of empty <see cref="FPDynObject"/> class.
        /// </summary>
        public FPDynObject()
        {
            _properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FPDynObject"/> class.
        /// </summary>
        /// <param name="properties">The properties of the object</param>
        public static dynamic GetDynamicObject(Dictionary<string, object> properties) => new FPDynObject(properties);

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
    }
}
