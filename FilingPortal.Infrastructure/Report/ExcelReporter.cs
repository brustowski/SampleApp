using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FilingPortal.Domain.Common.Reporting;
using System.Collections.Generic;
using System.Linq;
using Cell = FilingPortal.Domain.Common.Reporting.Model.Cell;
using ReportRow = FilingPortal.Domain.Common.Reporting.Model.Row;

namespace FilingPortal.Infrastructure.Report
{
    internal class ExcelReporter : IReporter
    {
        private const int MaxRowsInExcelSheet = 1048576;
        private const int OneMillion = 1024 * 1024;
        private const int FlushSymbolCount = 10 * OneMillion;
        private const string BaseFolder = "ReportFiles";

        private readonly ExcelCellRefGenerator _cellRefGenerator;
        private readonly string _fullFilePath;
        private string _sheetName;
        private SpreadsheetDocument _spreadsheetDocument;
        private OpenXmlWriter _writer;
        private uint _rowNumber;
        private UInt32Value _sheetNumber = UInt32Value.FromUInt32(0);
        private UInt32Value _sequentialNumber = UInt32Value.FromUInt32(0);
        private ReportRow _header;
        private uint _headerRowNumber;
        private bool _isAutoFilterEnable;

        private readonly Dictionary<uint, uint> _columnsMaxWidth = new Dictionary<uint, uint>();


        internal ExcelReporter(string filename, IReportFilenameProvider reportFilenameProvider, string sheetName = "Sheet")
        {
            _cellRefGenerator = new ExcelCellRefGenerator();
            _fullFilePath = reportFilenameProvider.GetFilenameInFileStorage(filename, BaseFolder);
            _sheetName = sheetName;

            CreateDocument();
            OpenWorkbook();
            OpenNewSheet();
        }

        #region Private methods

        private void CreateDocument()
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(_fullFilePath, SpreadsheetDocumentType.Workbook, autoSave: false);
        }

        private void OpenWorkbook()
        {
            WorkbookPart workbookPart = _spreadsheetDocument.AddWorkbookPart();
            AddStyles(workbookPart);
            workbookPart.Workbook = new Workbook();
            workbookPart.Workbook.AppendChild(new Sheets());
        }

        private void AddStyles(WorkbookPart workbookPart)
        {
            var stylesheet = new Stylesheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
            stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var fonts = new Fonts { Count = 1U, KnownFonts = true };
            var font = new Font
            (
                new FontSize { Val = 11D },
                new Color { Theme = 1U },
                new FontName { Val = "Calibri" },
                new FontFamilyNumbering { Val = 2 },
                new FontScheme { Val = FontSchemeValues.Minor }
            );
            fonts.AppendChild(font);

            var fills = new Fills(
                // FillId = 0
                new Fill(new PatternFill { PatternType = PatternValues.None }),
                // FillId = 1
                new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),
                // FillId = 2,Gray
                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue { Value = "C0C0C0" } }, new BackgroundColor { Indexed = 64U })
                    { PatternType = PatternValues.Solid })
            );

            var borders = new Borders(new Border(new LeftBorder(), new RightBorder(), new TopBorder(), new BottomBorder(), new DiagonalBorder()));

            var cellStyleFormats = new CellStyleFormats(new CellFormat { NumberFormatId = 0U, FontId = 0U, FillId = 0U, BorderId = 0U });
            var cellFormats = new CellFormats(
                new CellFormat { NumberFormatId = 0U, FontId = 0U, FillId = 0U, BorderId = 0U, FormatId = 0U },
                new CellFormat { NumberFormatId = 0U, FontId = 0U, FillId = 2U, BorderId = 0U, FormatId = 0U, ApplyFill = true });

            var cellStyles = new CellStyles(new CellStyle { Name = "Normal", FormatId = 0U, BuiltinId = 0U });

            stylesheet.Append(fonts, fills, borders, cellStyleFormats, cellFormats, cellStyles);

            WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = stylesheet;

            stylesPart.Stylesheet.Save();
        }

        private void OpenNewSheet()
        {
            WorkbookPart workbookPart = _spreadsheetDocument.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            Sheets sheets = workbookPart.Workbook.Sheets;
            _sheetNumber++;
            _sequentialNumber = 1;
            var sheet = new Sheet { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = _sheetNumber, Name = $"{_sheetName}" };
            sheets.AppendChild(sheet);
            _rowNumber = 1;
            _columnsMaxWidth.Clear();

            _writer = OpenXmlWriter.Create(worksheetPart);

            _writer.WriteStartElement(new Worksheet());
            _writer.WriteStartElement(new SheetData());
        }

        private void AddSheet()
        {
            WorkbookPart workbookPart = _spreadsheetDocument.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            Sheets sheets = workbookPart.Workbook.Sheets;
            _sheetNumber++;
            _sequentialNumber++;
            var sheet = new Sheet { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = _sheetNumber, Name = $"{_sheetName}_{_sequentialNumber}" };
            sheets.AppendChild(sheet);
            _rowNumber = 1;
            _columnsMaxWidth.Clear();

            _writer = OpenXmlWriter.Create(worksheetPart);

            _writer.WriteStartElement(new Worksheet());
            _writer.WriteStartElement(new SheetData());
        }

        private void CloseSheet()
        {
            _writer.WriteEndElement();
            AddAutoFilter();
            _writer.WriteEndElement();
            _writer.Close();
            AddColumns();
        }

        private void AddColumns()
        {
            IEnumerable<Sheet> sheets = _spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>()
                    .Where(s => s.Name == _sheetName);
            var worksheetPart = (WorksheetPart)_spreadsheetDocument
                .WorkbookPart.GetPartById(sheets.Last().Id);
            Worksheet workSheet = worksheetPart.Worksheet;
            Columns columns = workSheet.Elements<Columns>().FirstOrDefault();

            if (columns == null)
            {
                SheetData sheetData = workSheet.Elements<SheetData>().FirstOrDefault();
                if (sheetData != null)
                {
                    columns = workSheet.InsertBefore(new Columns(), sheetData);
                }
                else
                {
                    columns = new Columns();
                    workSheet.AppendChild(columns);
                }
            }

            SetColumnsWidth(columns);

            workSheet.Save();
        }

        private void SetColumnsWidth(Columns columns)
        {
            foreach (KeyValuePair<uint, uint> columnWidth in _columnsMaxWidth)
            {
                var width = (columnWidth.Value * 8 + 5) / (double)8 * 256 / 256 + 3;
                var column = new Column { Min = columnWidth.Key, Max = columnWidth.Key, CustomWidth = true, Width = width };
                columns.AppendChild(column);
            }

        }

        private void AddAutoFilter()
        {
            if (_header == null || !_isAutoFilterEnable)
            {
                return;
            }

            var firstCell = _cellRefGenerator.GetReference(_headerRowNumber, 0);
            var lastCell = _cellRefGenerator.GetReference(_headerRowNumber, (uint)_header.Cells.Count - 1);
            var filter = new AutoFilter { Reference = $"{firstCell}:{lastCell}" };
            _writer.WriteElement(filter);
        }


        private void AddHeader()
        {
            if (_header == null)
            {
                return;
            }
            var r = new Row { RowIndex = new UInt32Value(_rowNumber) };
            _writer.WriteStartElement(r);
            uint columnNumber = 0;
            foreach (Cell cell in _header.Cells)
            {
                SetMaxColumnWidth(columnNumber + 1, cell.Content);
                var cellReference = _cellRefGenerator.GetReference(_rowNumber, columnNumber++);
                var c = new DocumentFormat.OpenXml.Spreadsheet.Cell
                {
                    DataType = CellValues.String,
                    CellReference = cellReference,
                    StyleIndex = 1U,
                    CellValue = new CellValue { Text = cell.Content },
                };
                _writer.WriteElement(c);
            }

            _writer.WriteEndElement();
            _headerRowNumber = _rowNumber++;
        }

        private void SetMaxColumnWidth(uint columnNumber, string value)
        {
            var length = (uint)(value?.Length ?? 0);
            if (_columnsMaxWidth.ContainsKey(columnNumber))
            {
                _columnsMaxWidth[columnNumber] = _columnsMaxWidth[columnNumber] > length ? _columnsMaxWidth[columnNumber] : length;
            }
            else
            {
                _columnsMaxWidth.Add(columnNumber, length);
            }
        }

        #endregion

        #region Public methods

        public void AddSection(string name)
        {
            CloseSheet();
            _sheetName = name;
            _header = null;
            _isAutoFilterEnable = false;
            OpenNewSheet();
        }

        public void AddHeader(ReportRow headerRow, bool enableFilters = false)
        {
            _header = headerRow;
            _isAutoFilterEnable = enableFilters;
            AddHeader();
        }

        public void AddPartOfData(IEnumerable<ReportRow> reportRows)
        {
            var r = new Row();
            var c = new DocumentFormat.OpenXml.Spreadsheet.Cell { DataType = CellValues.String };
            c.AppendChild(new CellValue());

            var symbolCount = 0;

            foreach (ReportRow reportRow in reportRows)
            {
                if (_rowNumber > MaxRowsInExcelSheet)
                {
                    CloseSheet();
                    AddSheet();
                    AddHeader();
                }

                r.RowIndex = _rowNumber;
                _writer.WriteStartElement(r);
                uint columnNumber = 0;
                foreach (Cell reportCell in reportRow.Cells)
                {
                    SetMaxColumnWidth(columnNumber + 1, reportCell.Content);
                    symbolCount += reportCell.Content?.Length ?? 0;
                    var cellReference = _cellRefGenerator.GetReference(_rowNumber, columnNumber++);
                    c.CellValue.Text = reportCell.Content;
                    c.CellReference = cellReference;
                    _writer.WriteElement(c);
                }
                _writer.WriteEndElement();

                if (symbolCount > FlushSymbolCount)
                {
                    symbolCount = 0;
                    _spreadsheetDocument.WorkbookPart.Workbook.Save();
                }

                _rowNumber++;

            }
        }

        public string SaveToFile()
        {
            CloseSheet();
            _spreadsheetDocument.WorkbookPart.Workbook.Save();
            _spreadsheetDocument.Close();

            return _fullFilePath;
        }

        #endregion
    }
}
