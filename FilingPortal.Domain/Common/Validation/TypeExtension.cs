using System;
using System.Collections.Generic;
using System.Data;

namespace FilingPortal.Domain.Common.Validation
{
    /// <summary>
    /// Static class for type conversion mapping
    /// </summary>
    public static class TypeExtension
    {
        private static Dictionary<string, Type> Mappings;
        static TypeExtension()
        {
            Mappings = new Dictionary<string, Type>
            {
                { "bigint", typeof(long) },
                { "binary", typeof(byte[]) },
                { "bit", typeof(int) },
                { "char", typeof(string) },
                { "date", typeof(DateTime) },
                { "datetime", typeof(DateTime) },
                { "datetime2", typeof(DateTime) },
                { "datetimeoffset", typeof(DateTimeOffset) },
                { "decimal", typeof(decimal) },
                { "float", typeof(double) },
                { "image", typeof(byte[]) },
                { "int", typeof(int) },
                { "money", typeof(decimal) },
                { "nchar", typeof(string) },
                { "ntext", typeof(string) },
                { "numeric", typeof(decimal) },
                { "nvarchar", typeof(string) },
                { "real", typeof(float) },
                { "rowversion", typeof(byte[]) },
                { "smalldatetime", typeof(DateTime) },
                { "smallint", typeof(short) },
                { "smallmoney", typeof(decimal) },
                { "text", typeof(string) },
                { "time", typeof(TimeSpan) },
                { "timestamp", typeof(byte[]) },
                { "tinyint", typeof(byte) },
                { "uniqueidentifier", typeof(Guid) },
                { "varbinary", typeof(byte[]) },
                { "varchar", typeof(string) },
                { "Address", typeof(int) }, // custom type
                { "Confirmation", typeof(byte) }, // custom type
            };

        }

        /// <summary>
        /// Returns CLR type mapped from sql type name
        /// </summary>
        /// <param name="sqlType"><see cref="string"/> sql type name</param>
        public static Type ToClrType(this string sqlType)
        {
            if (sqlType == null) return null;
            if (Mappings.TryGetValue(sqlType, out Type datatype))
                return datatype;
            return null;
        }

    }
}
