using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static int GetLevenshteinDistance(this string first, string second)
        {
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            {
                return 0;
            }

            var lengthFirst = first.Length;
            var lengthSecond = second.Length;
            var distances = new int[lengthFirst + 1, lengthSecond + 1];

            for (var i = 0; i <= lengthFirst; distances[i, 0] = i++)
            {
            }
            for (var j = 0; j <= lengthSecond; distances[0, j] = j++)
            {
            }

            for (var i = 1; i <= lengthFirst; i++)
            {
                for (var j = 1; j <= lengthSecond; j++)
                {
                    var cost = second[j - 1] == first[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost);
                }
            }

            return distances[lengthFirst, lengthSecond];
        }

        public static string JoinBySeparator(this IEnumerable<string> list, char separator)
        {
            return string.Join(separator.ToString(CultureInfo.CurrentCulture), list);
        }

        public static string Join(this IEnumerable<string> list)
        {
            return string.Join(string.Empty, list);
        }

        /// <summary>
        /// Splits string by required number of characters
        /// </summary>
        /// <param name="source">initial string</param>
        /// <param name="maxChunkSize">max chunk size</param>
        public static IEnumerable<string> SplitBySize(this string source, int maxChunkSize)
        {
            for (var i = 0; i < source.Length; i += maxChunkSize)
            {
                yield return source.Substring(i, Math.Min(maxChunkSize, source.Length - i));
            }
        }

        public static string GetClosestValue(this string value, IEnumerable<string> listOfValues)
        {
            var strings = listOfValues.ToArray();
            return strings.Count() > 1
                ? strings.Aggregate(
                    (s1, s2) => s1.GetLevenshteinDistance(value) < s2.GetLevenshteinDistance(value) ? s1 : s2)
                : strings.FirstOrDefault();
        }

        public static string RemoveSeparators(this string s, char[] separators)
        {
            return string.IsNullOrEmpty(s) ? s : string.Concat(s.Split(separators, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        public static string TruncateEnd(this string str, int endingLength)
        {
            return str.Substring(0, Math.Max(str.Length - endingLength, 0));
        }

        public static string SubstringFromEnd(this string s, int lenght)
        {
            return s.Substring(s.Length - lenght, lenght);
        }

        public static string SubstringFromBegin(this string s, int lenght)
        {
            return s.Substring(0, lenght);
        }

        public static DateTime? ToDate(this string dateTimeStr, string dateFmt)
        {
            DateTime dt;
            if (DateTime.TryParseExact(dateTimeStr, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dt))
            {
                return dt;
            }
            return null;
        }

        public static int? ToInt(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i))
            {
                return i;
            }
            return null;
        }

        public static decimal? ToDecimal(this string s)
        {
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
            {
                return i;
            }
            return null;
        }
        /// <summary>
        /// Convert string value to nullable Guid
        /// </summary>
        /// <param name="s">String value to convert</param>
        public static Guid? ToNullableGuid(this string s)
        {
            if (Guid.TryParse(s, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static byte? ToByte(this string s)
        {
            if (byte.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
            {
                return i;
            }
            return null;
        }

        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// Generates the MD5 hash of the string
        /// </summary>
        /// <param name="value">The source string</param>
        public static string ToMD5HashString(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            return BitConverter.ToString(data);
        }

        /// <summary>
        /// Trims string with string beginning
        /// </summary>
        /// <param name="target">Initial string</param>
        /// <param name="trimString">String to trim from the beginning</param>
        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return target;

            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }
    }
}