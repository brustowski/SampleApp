using ExcelDataReader;
using FilingPortal.Domain.Common.Parsing;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FilingPortal.Infrastructure.Common;

namespace FilingPortal.Infrastructure.Parsing
{
    /// <summary>
    /// Represents simple Excel file parser
    /// </summary>
    public class ExcelFileSimpleParser : IFileParser
    {
        private const string ErrorMessageTemplate = "{0}. Row: {1}. Column name: {2}. Type: {3}. Value: {4}.";
        
        /// <summary>
        /// Register of Parsing Models
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;

        private IParseModelMap _modelMap;
        public void SetModelMap(IParseModelMap modelMap)
        {
            _modelMap = modelMap;
        }

        private IParseModelMap GetModelMap<T>()
        {
            return _modelMap ?? _parseModelMapRegistry.Get<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileSimpleParser"/> class
        /// </summary>
        /// <param name="parseModelMapRegistry">Register for Parse Model Map</param>
        public ExcelFileSimpleParser(IParseModelMapRegistry parseModelMapRegistry)
        {
            _parseModelMapRegistry = parseModelMapRegistry;
        }

        /// <summary>
        /// Parses file according to specified parsing data model
        /// </summary>
        /// <typeparam name="T">Type of the parsing data model</typeparam>
        /// <param name="path">Fully qualified file path</param>
        public ParsingResult<T> Parse<T>(string path) where T : new()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", Path.GetFileName(path));
            }

            using (FileStream fileStream = File.OpenRead(path))
            {
                return Parse<T>(fileStream);
            }
        }

        /// <summary>
        /// Parses the stream according to specified parsing data model
        /// </summary>
        /// <typeparam name="T">Type of the parsing data model</typeparam>
        /// <param name="stream">Stream to parse</param>
        public ParsingResult<T> Parse<T>(Stream stream) where T : new()
        {
            var result = new ParsingResult<T>();
            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            try
            {
                IParseModelMap modelMap = GetModelMap<T>();

                if (!SetSheet(reader, modelMap.SheetName))
                {
                    result.AddError($"Sheet with name \'{modelMap.SheetName}\' not found.");
                    return result;
                }

                Dictionary<string, int> headerModels = ParseHeader(reader);

                ParseBody<T>(reader, headerModels, modelMap, ref result);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Unhandled Error during excel parsing");
                throw new FileParserException(ex.Message, ex);
            }
            finally
            {
                reader?.Close();
            }
            return result;
        }
        /// <summary>
        /// Set the excel reader to the sheet with specified name
        /// </summary>
        /// <param name="reader">The reader to be set</param>
        /// <param name="sheetName">Name of the sheet</param>
        private static bool SetSheet(IExcelDataReader reader, string sheetName)
        {
            do
            {
                if (reader.Name.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
            } while (reader.NextResult());

            return !string.IsNullOrWhiteSpace(reader.Name);
        }
        /// <summary>
        /// Parse excel file first row as table header and prepare mapping between column name and column index 
        /// </summary>
        /// <param name="reader">The excel file reader</param>
        private static Dictionary<string, int> ParseHeader(IExcelDataReader reader)
        {
            var headerModels = new Dictionary<string, int>();
            reader.Read();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.IsDBNull(i))
                {
                    continue;
                }

                var value = reader.GetString(i);

                if (headerModels.ContainsKey(value))
                {
                    throw new FileParserException("Headers should be unique.");
                }

                headerModels.Add(value, i);
            }

            return headerModels;
        }
        /// <summary>
        /// Parse main part of the excel file
        /// </summary>
        /// <typeparam name="T">The type of the expected data</typeparam>
        /// <param name="reader">The excel file reader</param>
        /// <param name="headers">The columnName to index mapping dictionary</param>
        /// <param name="configuration">Parsing model map configuration</param>
        private static void ParseBody<T>(IExcelDataReader reader, Dictionary<string, int> headers, IParseModelMap configuration, ref ParsingResult<T> result) where T : new()
        {
            var rowCount = 2;
            while (reader.Read())
            {
                var row = new T();
                var parsingModel = row as IParsingDataModel;
                if (parsingModel != null)
                {
                    parsingModel.RowNumberInFile = rowCount++;
                }
                var columnsCount = 0;
                foreach (KeyValuePair<string, int> header in headers)
                {
                    if (reader.IsDBNull(header.Value))
                    {
                        continue;
                    }

                    columnsCount++;
                    IPropertyMapInfo propertyMap = configuration.GetMapInfo(header.Key);
                    if (propertyMap == null) continue;

                    var rawValue = $"{reader.GetValue(header.Value)}";

                    try
                    {
                        object rowValue = reader.GetAutoValue(header.Value, propertyMap.PropertyType);

                        propertyMap.Setter(row, rowValue is string val ? string.IsNullOrWhiteSpace(val) ? null : val.Trim() : rowValue);
                    }
                    catch (InvalidCastException ex)
                    {
                        const string errorMessage = "Value conversion error";
                        AppLogger.Error(ex, string.Format(ErrorMessageTemplate, errorMessage, parsingModel?.RowNumberInFile, header.Key, propertyMap?.PropertyType.Name, rawValue));
                        result.AddError(new RowError(parsingModel?.RowNumberInFile, configuration.SheetName, ErrorLevel.Warning, header.Key, errorMessage));
                    }
                    catch (FormatException ex)
                    {
                        const string errorMessage = "Value conversion error";
                        AppLogger.Error(ex, string.Format(ErrorMessageTemplate, errorMessage, parsingModel?.RowNumberInFile, header.Key, propertyMap?.PropertyType.Name, rawValue));
                        result.AddError(new RowError(parsingModel?.RowNumberInFile, configuration.SheetName, ErrorLevel.Warning, header.Key, errorMessage));
                    }
                    catch (Exception ex)
                    {
                        const string errorMessage = "Unhandled error during parsing value";
                        AppLogger.Error(ex, string.Format(ErrorMessageTemplate, errorMessage, parsingModel?.RowNumberInFile, header.Key, propertyMap?.PropertyType.Name, rawValue));
                        result.AddError(new RowError(parsingModel?.RowNumberInFile, configuration.SheetName, ErrorLevel.Warning, header.Key, errorMessage));
                    }
                }

                if (columnsCount == 0 || result.RowErrors.Any(x => x.LineNumber == parsingModel?.RowNumberInFile))
                {
                    continue;
                }

                result.ParsedData.Add(row);
            }
        }
    }
}
