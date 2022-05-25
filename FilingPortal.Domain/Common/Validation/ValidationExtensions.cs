using FilingPortal.Domain.Mapping;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FilingPortal.Domain.Common.Validation
{
    /// <summary>
    /// Provides extensions methods for various validations
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Decimal format validation expression
        /// </summary>
        private static readonly Regex DecimalRegex = new Regex(ValidatorConstants.Decimal, RegexOptions.Compiled);
        
        /// <summary>
        /// Checks whether the string is a valid formatted decimal value
        /// </summary>
        /// <param name="value">Checked value</param>
        public static bool IsDecimalFormat(this string value) => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) && DecimalRegex.IsMatch(v.ToString(CultureInfo.InvariantCulture));
        
        /// <summary>
        /// Checks whether the string is a valid int value
        /// </summary>
        /// <param name="value">Checked value</param>
        public static bool IsIntFormat(this string value) => int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v);

        /// <summary>
        /// Checks whether the string is a valid formatted date time value
        /// </summary>
        /// <param name="value">Checked value</param>
        public static bool IsDateTimeFormat(this string value)
        {
            return DateTime.TryParseExact(value, FormatsContainer.US_DATETIME_FORMAT, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out DateTime d)
                || DateTime.TryParseExact(value, FormatsContainer.US_DATETIME_FORMAT_SHORT_YEAR, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out d);

        }
        
        /// <summary>
        /// Checks whether the string is a valid formatted guid value
        /// </summary>
        /// <param name="value">Checked value</param>
        public static bool IsGuidFormat(this string value)
        {
            return Guid.TryParse(value, out Guid result);
        }

        /// <summary>
        /// Checks whether the string is a valid boolean value
        /// </summary>
        /// <param name="value">Checked value</param>
        public static bool IsBoolFormat(this string value) => bool.TryParse(value, out var v);
    }
}
