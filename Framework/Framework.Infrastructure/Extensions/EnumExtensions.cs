using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for enums
    /// </summary>
    public static class EnumExtensions
    {
        #region Private members

        /// <summary>
        /// Gets the description of the field of the type by the specified field name 
        ///  </summary>
        /// <param name="value">The field name</param>
        /// <param name="t">The type</param>
        static string GetDescriptionInternal(object value, Type t)
        {
            if (t == null) t = value.GetType();
            DescriptionAttribute[] attributes = (DescriptionAttribute[])t.GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : Enum.GetName(t, value);
        }

        #endregion

        /// <summary>
        /// Gets the description from DescriptionAttribute
        /// </summary>
        /// <param name="enumObj">The enum object</param>
        public static string GetDescription(this Enum enumObj)
        {
            return GetDescriptionInternal(enumObj, enumObj.GetType());
        }
    }
}