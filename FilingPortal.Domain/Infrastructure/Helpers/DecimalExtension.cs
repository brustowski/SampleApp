using System;
using System.Globalization;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Infrastructure.Helpers
{
    /// <summary>
    /// Extension class for decimal formatting and conversion
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// The count of maximal digits floor
        /// </summary>
        public const int MaximalDigitsFloor = 11;
        /// <summary>
        /// The count of maximal digits scale
        /// </summary>
        public const int MaximalDigitsScale = 6;
        /// <summary>
        /// The count of maximal scale for view
        /// </summary>
        public const int MaximalScaleForView = 2;

        /// <summary>
        /// The factor
        /// </summary>
        private static readonly decimal[] Factor = { 1M, 10M, 100M, 1000M, 10000M, 100000M, 1000000M, 10000000M, 100000000M };
        /// <summary>
        /// The scale
        /// </summary>
        private static readonly decimal[] Scale = { 0M, .9M, .99M, .999M, .9999M, .99999M, .999999M, .9999999M, .99999999M };
        /// <summary>
        /// The precision
        /// </summary>
        private static readonly decimal[] Precision = { 9999999999M, 99999999999M, 999999999999M };

        /// <summary>
        /// Truncates the specified value to specified count of symbols before dot and specified count of digits
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="m">Count of ymbols before dot</param>
        /// <param name="n">Count of digits</param>
        private static decimal TruncateToMDotNDigits(this decimal value, int m, int n)
        {
            Check.InRange(m, "m", 10, MaximalDigitsFloor);
            Check.InRange(n, "n", 0, MaximalDigitsScale);
            return Truncate(value, m, n);
        }

        /// <summary>
        /// Rounds the specified value for view
        /// </summary>
        /// <param name="value">The value</param>
        public static decimal RoundForView(this decimal value)
            => Round(value, MaximalScaleForView, GetMaxValueForPrecision(MaximalDigitsFloor, MaximalScaleForView));

        /// <summary>
        /// Truncates the specified value for view
        /// </summary>
        /// <param name="value">The value</param>
        public static decimal TruncateForView(this decimal value)
            => Truncate(value, MaximalDigitsFloor, MaximalScaleForView);

        /// <summary>
        /// Truncates the specified value
        /// </summary>
        /// <param name="value">The value</param>
        public static decimal Truncate(this decimal value)
            => Truncate(value, MaximalDigitsFloor, MaximalDigitsScale);

        /// <summary>
        /// Rounds the specified value
        /// </summary>
        /// <param name="value">The value</param>
        public static decimal Round(this decimal value)
            => Round(value, MaximalDigitsScale, GetMaxValueForPrecision(MaximalDigitsFloor, MaximalDigitsScale));

        /// <summary>
        /// Compares the decimal to another decimal with the specified scale
        /// </summary>
        /// <param name="x">The current decimal value</param>
        /// <param name="y">The decimal to compare with</param>
        /// <param name="scale">The scale</param>
        public static int CompareTo(this decimal x, decimal y, int scale)
            => Math.Sign(x.Truncate(MaximalDigitsFloor, scale) - y.Truncate(MaximalDigitsFloor, scale));

        /// <summary>
        /// Performs safe overflow for the specified expression
        /// </summary>
        /// <param name="expression">The expression</param>
        public static decimal SafeOverflow(Func<decimal> expression)
        {
            try
            {
                return expression();
            }
            catch (DivideByZeroException)
            {
                return decimal.MaxValue;
            }
            catch (OverflowException)
            {
                return decimal.MaxValue;
            }
        }

        /// <summary>
        /// Truncates the decimal value with precision and scale values
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="m">The precision value</param>
        /// <param name="n">The scale value</param>
        private static decimal Truncate(this decimal value, int m, int n)
            => Truncate(value, Factor[n], GetMaxValueForPrecision(m, n));

        /// <summary>
        /// Gets the maximum value for precision
        /// </summary>
        /// <param name="m">The precision value</param>
        /// <param name="n">The scale value</param>
        private static decimal GetMaxValueForPrecision(int m, int n)
            => Precision[m - 10] + Scale[n];

        /// <summary>
        /// Truncates the decimal with specified factor and maximal value
        /// </summary>
        /// <param name="value">The decimal value</param>
        /// <param name="factor">The factor</param>
        /// <param name="maxValue">The maximum value</param>
        private static decimal Truncate(this decimal value, decimal factor, decimal maxValue)
        {
            var floor = Math.Truncate(value);
            var truncated = Math.Truncate((value - floor) * factor) / factor + floor;
            return Math.Min(truncated, maxValue);
        }

        /// <summary>
        /// Rounds  the decimal with specified scale and maximal value
        /// </summary>
        /// <param name="value">The decimal value</param>
        /// <param name="scale">The scale</param>
        /// <param name="maxValue">The maximal value</param>
        private static decimal Round(this decimal value, int scale, decimal maxValue)
        {
            var roundedValue = Math.Round(value, scale, MidpointRounding.AwayFromZero);
            return Math.Min(roundedValue, maxValue);
        }

        /// <summary>
        /// Converts the decimal to string without trailing zeroes using the specified culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="culture">The culture</param>
        public static string WithoutTrailingZeroes(this decimal dt, CultureInfo culture = null)
        {
            var decimalPlacesCount = dt.CountDecimalPlaces();
            var format = "0." + (decimalPlacesCount > 0 ? new string('#', dt.CountDecimalPlaces()) : "#");
            return dt.ToString(format, culture ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the decimal to string without trailing zeroes using current culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        public static string ToCurrentCultureWithoutTrailingZeroes(this decimal dt)
            => ToCurrentCultureWithoutTrailingZeroes(dt, CultureInfo.CurrentCulture, MaximalDigitsScale);

        /// <summary>
        /// Converts the decimal to string without trailing zeroes for view using current culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        public static string ToCurrentCultureWithoutTrailingZeroesForView(this decimal dt)
            => ToCurrentCultureWithoutTrailingZeroes(dt, CultureInfo.CurrentCulture, MaximalScaleForView);

        /// <summary>
        /// Converts the decimal to string without trailing zeroes using invariant culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        public static string ToInvariantCultureWithoutTrailingZeroes(this decimal dt)
            => ToInvariantCultureWithoutTrailingZeroes(dt, MaximalDigitsScale);

        /// <summary>
        /// Converts the nullable decimal to string without trailing zeroes using invariant culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        public static string ToInvariantCultureWithoutTrailingZeroes(this decimal? dt)
            => ToInvariantCultureWithoutTrailingZeroes(dt, MaximalDigitsScale);

        /// <summary>
        ///  Converts the decimal to string without trailing zeroes for view using invariant culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        public static string ToInvariantCultureWithoutTrailingZeroesForView(this decimal dt)
            => ToInvariantCultureWithoutTrailingZeroes(dt, MaximalDigitsScale);

        /// <summary>
        ///  Converts the nullable decimal to string without trailing zeroes using invariant culture and specified fractional count
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        private static string ToInvariantCultureWithoutTrailingZeroes(this decimal? dt, int fractionalCount)
        {
            if (dt == null)
            {
                return null;
            }
            var usedCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            usedCulture.NumberFormat.CurrencyGroupSeparator = string.Empty;
            usedCulture.NumberFormat.NumberGroupSeparator = string.Empty;
            var decimalSeparator = usedCulture.NumberFormat.CurrencyDecimalSeparator;
            var formatString = $"N{fractionalCount}";
            var truncatedValue = TruncateToMDotNDigits(dt.Value, MaximalDigitsFloor, fractionalCount);
            var result = truncatedValue.ToString(formatString, usedCulture);
            return TrimTrailingZeroesAfterComma(result, decimalSeparator);
        }

        /// <summary>
        ///  Converts the decimal to string without trailing zeroes using specified culture and fractional count
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="usedCulture">The used culture</param>
        /// <param name="fractionalCount">The fractional count</param>
        private static string ToCurrentCultureWithoutTrailingZeroes(this decimal dt, CultureInfo usedCulture, int fractionalCount)
        {
            var decimalSeparator = usedCulture.NumberFormat.CurrencyDecimalSeparator;
            var formatString = $"N{fractionalCount}";
            var truncatedValue = TruncateToMDotNDigits(dt, MaximalDigitsFloor, fractionalCount);
            var result = truncatedValue.ToString(formatString, usedCulture);
            return TrimTrailingZeroesAfterComma(result, decimalSeparator);
        }

        /// <summary>
        /// Converts the decimal to string with specified fractional count after comma with current culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        public static string ToNDigitsAfterCommaWithCurrentCulture(this decimal dt, int fractionalCount)
            => ToNDigitsAfterCommaProcessor(dt.TruncateToMDotNDigits(MaximalDigitsFloor, fractionalCount), fractionalCount, true);

        /// <summary>
        /// Converts the decimal to string with specified fractional count after comma with invariant culture
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        public static string ToNDigitsAfterCommaWithInvariantCulture(this decimal dt, int fractionalCount)
            => ToNDigitsAfterCommaProcessor(dt.TruncateToMDotNDigits(MaximalDigitsFloor, fractionalCount), fractionalCount, false);


        /// <summary>
        /// Converts the decimal to string with specified fractional count after comma
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        public static string ToNDigitsAfterCommaPlain(this decimal dt, int fractionalCount)
        {
            Check.InRange(fractionalCount, "fractionalCount", 0, MaximalDigitsScale);
            return Math.Truncate(dt * Factor[fractionalCount]).ToString(new string('0', fractionalCount + 1));
        }

        /// <summary>
        /// Converts the decimal to string rounded to specified fractional count after comma
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        public static string RoundToNDigitsAfterCommaPlain(this decimal dt, int fractionalCount)
        {
            Check.InRange(fractionalCount, "fractionalCount", 0, MaximalDigitsScale);
            var roundedValue = Math.Round(dt, fractionalCount);
            return Math.Truncate(roundedValue * Factor[fractionalCount]).ToString(new string('0', fractionalCount + 1));
        }

        /// <summary>
        /// Converts the decimal to string as positive with 2 digits after comma
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>System.String.</returns>
        public static string ToPositivePlain2DigitsAfterComma(this decimal dt)
            => dt > 0 ? ToNDigitsAfterCommaPlain(dt, 2) : null;

        /// <summary>
        /// Converts the decimal to string as positive with specified fractional count using current culture depending on specified flag
        /// </summary>
        /// <param name="dt">The decimal value</param>
        /// <param name="fractionalCount">The fractional count</param>
        /// <param name="useCulture">Flag determining whether current culture should be used</param>
        private static string ToNDigitsAfterCommaProcessor(decimal dt, int fractionalCount, bool useCulture)
        {
            var usedCulture = useCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            var decimalSeparator = usedCulture.NumberFormat.CurrencyDecimalSeparator;
            var formatString = useCulture
                                   ? $"N{fractionalCount}"
                                   : $"0{decimalSeparator}{new string('0', fractionalCount)}";
            return dt.ToString(formatString, usedCulture);
        }

        /// <summary>
        /// Removes the trailing zeroes in specified string source after specified decimal separator
        /// </summary>
        /// <param name="source">The source</param>
        /// <param name="decimalSeparator">The decimal separator</param>
        private static string TrimTrailingZeroesAfterComma(string source, string decimalSeparator)
            => source.Contains(decimalSeparator)
                   ? source.TrimEnd('0').TrimEnd(decimalSeparator[0])
                   : source;

        /// <summary>
        /// Counts the decimal places in decimal
        /// </summary>
        /// <param name="value">The value</param>
        public static int CountDecimalPlaces(this decimal value)
        {
            if (value == 0)
                return 0;
            int[] bits = Decimal.GetBits(value);
            int exponent = bits[3] >> 16;
            int result = exponent;
            long lowDecimal = bits[0] | (bits[1] >> 8);
            while ((lowDecimal % 10) == 0)
            {
                result--;
                lowDecimal /= 10;
            }
            return result;
        }

        /// <summary>
        /// Counts the integer places in decimal
        /// </summary>
        /// <param name="value">The value</param>
        public static int CountIntegerPlaces(this decimal value)
        {
            return Math.Abs(Decimal.Truncate(value)).ToString(CultureInfo.InvariantCulture).Length;
        }

        /// <summary>
        /// Performs safe divide using the specified divinder
        /// </summary>
        /// <param name="dividend">The dividend</param>
        /// <param name="divider">The divider</param>
        public static decimal DivideSafe(this decimal dividend, decimal divider)
        {
            return (divider > decimal.Zero) ? (dividend / divider) : decimal.Zero;
        }
    }
}
