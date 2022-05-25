using System.IO;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes file parser with specified parsing data model
    /// </summary>
    public interface IFileParser
    {
        /// <summary>
        /// Sets the specific model map
        /// </summary>
        void SetModelMap(IParseModelMap modelMap);
        /// <summary>
        /// Parses the stream according to specified parsing data model
        /// </summary>
        /// <typeparam name="T">Type of the parsing data model</typeparam>
        /// <param name="stream">Stream to parse</param>
        ParsingResult<T> Parse<T>(Stream stream) where T : new();

        /// <summary>
        /// Parses the stream according to specified parsing data model
        /// </summary>
        /// <typeparam name="T">Type of the parsing data model</typeparam>
        /// <param name="filePath">File path</param>
        ParsingResult<T> Parse<T>(string filePath) where T : new();
    }
}
