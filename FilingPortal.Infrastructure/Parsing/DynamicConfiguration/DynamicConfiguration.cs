using System.Collections.Generic;

namespace FilingPortal.Infrastructure.Parsing.DynamicConfiguration
{
    internal class DynamicConfiguration : IDynamicConfiguration
    {
        private readonly Dictionary<string, DynamicMapInfo> _mappings;

        public DynamicConfiguration()
        {
            _mappings = new Dictionary<string, DynamicMapInfo>();
        }

        public void AddPropertyMap(DynamicMapInfo propInfo)
        {
            _mappings.Add(propInfo.FieldName, propInfo);
        }

        public DynamicMapInfo GetMapInfo(string internalFieldName)
        {
            return _mappings.ContainsKey(internalFieldName) ? _mappings[internalFieldName] : null;
        }
    }
}
