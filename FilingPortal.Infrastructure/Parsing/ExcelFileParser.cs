using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FilingPortal.Domain;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Infrastructure.Parsing.DynamicConfiguration;
using Framework.Infrastructure;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Infrastructure.Parsing
{
    /// <summary>
    /// Provides methods for Excel file parsing
    /// </summary>
    internal class ExcelFileParser : IFileParser
    {
        /// <summary>
        /// <see cref="DynamicConfigurationBuilder"/>
        /// </summary>
        private readonly DynamicConfigurationBuilder _dynamicConfigurationBuilder;
        /// <summary>
        /// Register of Parsing Models
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileParser"/> class
        /// </summary>
        /// <param name="parseModelMapRegistry">Register for Parse Model Map</param>
        public ExcelFileParser(IParseModelMapRegistry parseModelMapRegistry)
        {
            _parseModelMapRegistry = parseModelMapRegistry;
            _dynamicConfigurationBuilder = new DynamicConfigurationBuilder();
        }

        /// <summary>
        /// Parses file according to specified parsing data model
        /// </summary>
        /// <typeparam name="T">Type of the parsing data model</typeparam>
        /// <param name="path">Fully qualified file path</param>
        public ParsingResult<T> Parse<T>(string path) where T : new()
        {
            using (var fileStream = File.OpenRead(path))
            {
                return Parse<T>(fileStream);
            }
        }

        /// <summary>
        /// Sets the specific model map
        /// </summary>
        public void SetModelMap(IParseModelMap modelMap)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse Excel file stream based on the corresponding model type
        /// </summary>
        /// <typeparam name="T">Type of the corresponding model</typeparam>
        /// <param name="fileStream">Excel file stream</param>
        public ParsingResult<T> Parse<T>(Stream fileStream) where T : new()
        {
            var result = new ParsingResult<T>();

            try
            {
                using (var doc = SpreadsheetDocument.Open(fileStream, false))
                {
                    var workbookPart = doc.WorkbookPart;

                    var modelMap = _parseModelMapRegistry.Get<T>();

                    var worksheetPart = GetWorksheetPart(workbookPart, modelMap.SheetName);

                    var rowsCount = GetRowsCount(worksheetPart);

                    using (var reader = OpenXmlReader.Create(worksheetPart))
                    {
                        var headerModels = GetHeader(reader, workbookPart);

                        var config = _dynamicConfigurationBuilder.Build(headerModels, modelMap);

                        var parsingResult = Parse<T>(reader, workbookPart, config);

                        result.ParsedData.AddRange(parsingResult);
                    }

                    return result;
                }
            }

            catch (OpenXmlPackageException ex)
            {
                AppLogger.Error(ex, "Error during excel parsing");
                throw new FileParserException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Unhandled Error during excel parsing");
                throw new FileParserException(ex.Message, ex);
            }
        }

        private static WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
        {
            var sheetsWithName = workbookPart.Workbook.Descendants<Sheet>().Where(x => sheetName.Equals(x.Name)).ToList();
            if (sheetsWithName.Count == 1)
            {
                return (WorksheetPart)workbookPart.GetPartById(sheetsWithName.First().Id);
            }

            if (sheetsWithName.Count == 0)
            {
                throw new FileParserException($"{ValidationMessages.SheetNotFound} Sheet: {sheetName}");
            }

            throw new FileParserException($"Sheet {sheetName} is duplicated.");
        }

        private IEnumerable<T> Parse<T>(
            OpenXmlReader reader,
            WorkbookPart workbookPart,
            IDynamicConfiguration configuration
        ) where T : new()
        {
            var result = new List<T>();
            var rowsProcessed = 0;
            while (reader.ReadNextSibling())
            {
                if (reader.ElementType == typeof(Row))
                {
                    var internalRowNumberString = GetInternalRowNumberString(reader);

                    var currentModel = new T();

                    if (currentModel is IParsingDataModel parsingModel)
                    {
                        parsingModel.RowNumberInFile = Convert.ToInt32(internalRowNumberString);
                    }


                    reader.ReadFirstChild();
                    var anyNotEmpty = false;
                    do
                    {
                        if (reader.ElementType == typeof(Cell))
                        {
                            var c = (Cell)reader.LoadCurrentElement();
                            var cellValue = GetFormattedCellValue(workbookPart, c);
                            anyNotEmpty |= cellValue != null;

                            var internalColumnName =
                                c.CellReference.ToString().RemoveFromEnd(internalRowNumberString);
                            var config = configuration.GetMapInfo(internalColumnName);
                            config?.Setter(currentModel, cellValue);
                        }
                    } while (reader.ReadNextSibling());

                    if (anyNotEmpty)
                    {
                        result.Add(currentModel);
                    }

                    rowsProcessed++;
                }
            }
            return result;
        }
        
        private static int GetRowsCount(WorksheetPart worksheetPart)
        {
            int rowsCount = 0;
            using (var reader = OpenXmlReader.Create(worksheetPart))
            {
                if (reader.Read() && SearchElement(reader, typeof(Worksheet)) && reader.ReadFirstChild() && SearchElement(reader, typeof(SheetDimension)))
                {
                    var loadCurrentElement = (SheetDimension)reader.LoadCurrentElement();
                    rowsCount = ParseRowsCount(loadCurrentElement.Reference.ToString());
                }
            }
            return rowsCount;
        }

        private static bool SearchElement(OpenXmlReader reader, Type elementType)
        {
            bool cont;
            while ((cont = (reader.ElementType != elementType)) && reader.Read())
            {
                ;
            }

            return !cont;
        }

        private static int ParseRowsCount(string sheetDimension)
        {
            try
            {
                var clearValue = string.Join("", sheetDimension.Split(':')[1].Where(char.IsDigit));
                var count = int.Parse(clearValue);
                return count;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "Error during dimensions parsing");
                return 0;
            }
        }

        private IEnumerable<FileColumnInfo> GetHeader(OpenXmlReader reader, WorkbookPart workbookPart)
        {
            var headerModels = new List<FileColumnInfo>();
            var row = 0;
            while (reader.Read())
            {
                if (reader.ElementType != typeof(Row))
                {
                    continue;
                }

                reader.ReadFirstChild();
                do
                {
                    if (reader.ElementType == typeof(Cell))
                    {
                        var c = (Cell)reader.LoadCurrentElement();

                        var cellValue = GetFormattedCellValue(workbookPart, c);
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            if (headerModels.Any(x => x.FieldName == cellValue))
                            {
                                throw new FileParserException("Headers should be unique.");
                            }

                            headerModels.Add(new FileColumnInfo
                            {
                                InternalName = c.CellReference.ToString().TrimEnd('1'),
                                FieldName = cellValue
                            });
                        }
                    }
                } while (reader.ReadNextSibling());
                row++;
                if (row == 1)
                {
                    break;
                }
            }
            return headerModels;
        }

        private static string GetFormattedCellValue(WorkbookPart workbookPart, Cell cell)
        {
            if (cell == null)
            {
                return null;
            }

            string value = cell.InnerText;

            if (cell.DataType == null) // number & dates
            {
                if (cell.StyleIndex != null)
                {
                    int styleIndex = (int)cell.StyleIndex.Value;
                    CellFormat cellFormat =
                        (CellFormat)workbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(styleIndex);
                    uint formatId = cellFormat.NumberFormatId.Value;
                    switch (formatId)
                    {
                        case (uint)Formats.DateShort:
                        case (uint)Formats.DateLong:
                        case (uint)Formats.DateCustom:
                            {
                                if (double.TryParse(cell.InnerText, out var oaDate))
                                {
                                    value = DateTime.FromOADate(oaDate).ToShortDateString();
                                }

                                break;
                            }
                        default:
                            value = ConvertStoredToEnteredValue(cell.InnerText);
                            break;
                    }
                }
            }
            else // Shared string or boolean
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        SharedStringItem ssi = workbookPart.SharedStringTablePart.SharedStringTable
                            .Elements<SharedStringItem>().ElementAt(int.Parse(cell.CellValue.InnerText));
                        value = ssi.InnerText;
                        break;
                    case CellValues.Boolean:
                        value = cell.CellValue.InnerText == "0" ? "false" : "true";
                        break;
                    default:
                        value = cell.CellValue.InnerText;
                        break;
                }
            }

            if (value == string.Empty)
            {
                value = null;
            }

            return value;
        }

        private static string ConvertStoredToEnteredValue(string value)
        {
            try
            {
                var result = Convert.ToDouble(value, CultureInfo.InvariantCulture)
                    .ToString(CultureInfo.InvariantCulture);
                return result;
            }
            catch
            {
                return value;
            }
        }

        private static string GetInternalRowNumberString(OpenXmlReader reader)
        {
            return reader.Attributes.First(a => a.LocalName == "r").Value;
        }
    }
}
