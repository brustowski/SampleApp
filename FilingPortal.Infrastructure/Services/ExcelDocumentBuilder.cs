using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FilingPortal.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilingPortal.Infrastructure.Services
{
    /// <summary>
    /// Provides ability to build excel document
    /// </summary>
    public class ExcelDocumentBuilder : IExcelDocumentBuilder, IDisposable
    {
        private SpreadsheetDocument _document;
        private MemoryStream _stream;

        /// <summary>
        /// Open builder from bytes array
        /// </summary>
        /// <param name="bytes">The content of the excel as byte array</param>
        public IExcelDocumentBuilder Open(byte[] bytes)
        {
            _stream = new MemoryStream();
            _stream.Write(bytes, 0, bytes.Length);
            _document = SpreadsheetDocument.Open(_stream, true);
            return this;
        }

        /// <summary>
        /// Sets the value to the named cell
        /// </summary>
        /// <param name="cellName">Name of the cell</param>
        /// <param name="value">Value</param>
        public IExcelDocumentBuilder SetNamedCellValue(string cellName, object value)
        {
            WorkbookPart wbPart = _document.WorkbookPart;
            IEnumerable<DefinedName> definedNames = wbPart.Workbook.DefinedNames.Descendants<DefinedName>();

            DefinedName definedName = definedNames.FirstOrDefault(x => x.Name.Value == cellName);

            Worksheet worksheet = GetWorksheet(wbPart, definedName);

            Cell cell = GetCell(worksheet, definedName);

            SetCellValue(cell, value);

            return this;
        }

        /// <summary>
        /// Sets the value of the "FullCalculationOnLoad" property
        /// </summary>
        /// <param name="value">True - to enable full calculation on load, otherwise - false</param>
        public IExcelDocumentBuilder SetFullCalculationOnLoad(bool value)
        {
            _document.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            return this;
        }

        private static Worksheet GetWorksheet(WorkbookPart wbPart, DefinedName definedName)
        {
            var worksheetName = definedName.Text.Split('!').First();
            var ws = worksheetName.Trim('\'');
            Sheet worksheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == ws);
            if (worksheet == null)
            {
                throw new KeyNotFoundException($"Worksheet with name '{worksheetName}' not found.");
            }

            var wsPart = (WorksheetPart)wbPart.GetPartById(worksheet.Id);

            return wsPart.Worksheet;
        }

        private static Cell GetCell(Worksheet worksheet, DefinedName definedName)
        {
            var cellReference = definedName.Text.Split('!').Last();
            var cf = cellReference.Replace("$", string.Empty);
            Cell cell = worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference.Value == cf);
            if (cell == null)
            {
                throw new KeyNotFoundException($"Cell with reference '{cellReference}' not found.");
            }

            return cell;
        }

        private static void SetCellValue(Cell cell, object value)
        {
            cell.CellValue = new CellValue(value.ToString());
        }

        /// <summary>
        /// Provides content of the file as byte array
        /// </summary>
        public byte[] ToByteArray()
        {
            _document.Save();
            _document.Close();

            return _stream.ToArray();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _document?.Dispose();
            _stream?.Dispose();
        }
    }
}
