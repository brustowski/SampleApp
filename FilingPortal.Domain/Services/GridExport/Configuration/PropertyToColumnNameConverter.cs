using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    internal class PropertyToColumnNameConverter
    {
        private static Dictionary<int, string> DomainTerms => new Dictionary<int, string>()
        {
            { 1, "HTS" }
        };

        private static Dictionary<string, string> Shortenings => new Dictionary<string, string>()
        {
        };

        private const string Separator = "|";
        private static string SplittingRegularExpression => $@"([A-Z]|\{Separator}[0-9]+)";

        public string Convert(string propertyName)
        {
            propertyName = SplitIntoWords(propertyName);

            propertyName = ReplaceShortenings(propertyName);

            return propertyName;
        }

        private static string ReplaceShortenings(string propertyName)
        {
            foreach (var shortName in Shortenings.Keys)
            {
                var longName = Shortenings[shortName];
                if (!propertyName.Contains(longName))
                    propertyName = propertyName.Replace(shortName, longName);
            }
            return propertyName;
        }

        private static string SplitIntoWords(string propertyName)
        {
            propertyName = EncodeDomainTerms(propertyName);

            var splitted = SplitCamelCaseIntoWords(propertyName);

            propertyName = DecodeDomainTerms(splitted);
            return propertyName;
        }

        private static string DecodeDomainTerms(string propertyName)
        {
            foreach (var key in DomainTerms.Keys)
            {
                propertyName = propertyName.Replace(Separator + key, DomainTerms[key]);
            }
            return propertyName;
        }

        private static string EncodeDomainTerms(string propertyName)
        {
            foreach (var key in DomainTerms.Keys)
            {
                propertyName = propertyName.Replace(DomainTerms[key], Separator + key);
            }
            return propertyName;
        }

        private static string SplitCamelCaseIntoWords(string input)
        {
            return Regex
                .Replace(input, SplittingRegularExpression, " $1", RegexOptions.Compiled).Trim();
        }
    }
}
