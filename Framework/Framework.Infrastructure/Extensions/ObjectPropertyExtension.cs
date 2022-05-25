using System;
using System.ComponentModel;
using System.Reflection;

namespace Framework.Infrastructure.Extensions
{
    public static class ObjectPropertyExtension
    {
        public static string GetDescription(this PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(DescriptionAttribute))
                        ? (Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute).Description
                        : property.Name;
        }
    }
}
