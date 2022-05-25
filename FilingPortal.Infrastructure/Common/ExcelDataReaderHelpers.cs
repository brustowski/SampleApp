using System;
using System.Globalization;
using ExcelDataReader;

namespace FilingPortal.Infrastructure.Common
{
    /// <summary>
    /// Additional methods for excel data reader
    /// </summary>
    public static class ExcelDataReaderHelpers
    {
        private static CultureInfo enUS = new CultureInfo("en-US");

        /// <summary>
        /// Gets value in correct type
        /// </summary>
        /// <param name="reader">Excel Data Reader</param>
        /// <param name="columnIndex">Column index</param>
        /// <param name="t">Required type</param>
        public static object GetAutoValue(this IExcelDataReader reader, int columnIndex, Type t)
        {
            string stringValue = $"{reader.GetValue(columnIndex)}";
            if (string.IsNullOrWhiteSpace(stringValue)) return null;

            Type covertType = Nullable.GetUnderlyingType(t) ?? t;

            switch (covertType.Name)
            {
                case "DateTime":
                {
                    if (DateTime.TryParseExact(stringValue, FormatsContainer.UI_DATETIME_FORMAT, enUS,
                        DateTimeStyles.None, out var dt))
                        return dt;
                    if (DateTime.TryParseExact(stringValue, FormatsContainer.UI_DATETIME_SHORT_FORMAT, enUS,
                        DateTimeStyles.None, out var shortDt))
                        return shortDt;
                    return reader.GetDateTime(columnIndex); // In case that we have datetime object in excel, but in wrong format
                }
                default:
                    {
                        object safeValue = (string.IsNullOrEmpty(stringValue)) ? null : Convert.ChangeType(stringValue, covertType, CultureInfo.InvariantCulture);
                        return safeValue;
                    }
            }
        }
    }
}
