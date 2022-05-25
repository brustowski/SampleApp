using System.Collections.Generic;
using System.ComponentModel;

namespace FilingPortal.Domain.Infrastructure.Helpers
{
    /// <summary>
    /// Class for extending every object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts object to properties dictionary
        /// </summary>
        /// <param name="values">underlying object</param>
        public static IDictionary<string, object> GetKeyValueMap(this object values)
        {
            if (values == null)
            {
                return new Dictionary<string, object>();
            }

            var map = values as IDictionary<string, object>;
            if (map != null)
            {
                return map;
            }

            map = new Dictionary<string, object>();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
            {
                map.Add(descriptor.Name, descriptor.GetValue(values));
            }

            return map;
        }
    }
}
