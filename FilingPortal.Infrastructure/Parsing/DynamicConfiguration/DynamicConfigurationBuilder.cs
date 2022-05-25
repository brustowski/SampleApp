using FilingPortal.Domain.Common.Parsing;
using System.Collections.Generic;

namespace FilingPortal.Infrastructure.Parsing.DynamicConfiguration
{
    class DynamicConfigurationBuilder
    {
        public DynamicConfiguration Build(IEnumerable<FileColumnInfo> headerModels, IParseModelMap parseModelMap)
        {
            var mapper = new DynamicConfiguration();
            foreach (var model in headerModels)
            {
                var propertyInfo = parseModelMap.GetMapInfo(model.FieldName);
                if (propertyInfo != null)
                {
                    var propInfo = new DynamicMapInfo
                    {
                        Setter = propertyInfo.Setter,
                        FieldName = model.InternalName
                    };
                    mapper.AddPropertyMap(propInfo);
                }
            }
            return mapper;
        }
    }
}
