using DocumentFormat.OpenXml.Packaging;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;
using FilingPortal.Infrastructure.Report;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FilingPortal.Infrastructure.Tests.OpenXml
{
    [TestClass]
    public class ExcelReporterTests
    {
        private ExcelReporter _builder;
        private Mock<IReportFilenameProvider> _reportFilenameProviderMock;

        private const int OneMillion = 1024 * 1024;
        private readonly string _reportFilename = "Test123";
        private readonly string _baseFolder = "ReportFiles";
        private string _returnedFilename;

        [TestInitialize]
        public void TestInitialize()
        {
            _returnedFilename = $"ExcelReporterTests_{Guid.NewGuid()}.xlsx";
            _reportFilenameProviderMock = new Mock<IReportFilenameProvider>();
            _reportFilenameProviderMock.Setup(x => x.GetFilenameInFileStorage(_reportFilename, _baseFolder))
                .Returns(_returnedFilename);

            _builder = new ExcelReporter(_reportFilename, _reportFilenameProviderMock.Object);
        }

        [TestMethod]
        public void AddPart_WhenCall_GetResultFilenameFromProvider()
        {
            List<Row> chunk = GetRows();

            _builder.AddPartOfData(chunk);
            var resultFile = _builder.SaveToFile();

            _reportFilenameProviderMock.VerifyOnce(x => x.GetFilenameInFileStorage(_reportFilename, _baseFolder));

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenCall_ReturnsFilenameGotFromProvider()
        {
            List<Row> chunk = GetRows();

            _builder.AddPartOfData(chunk);
            var resultFile = _builder.SaveToFile();

            resultFile.ShouldBeEqualTo(_returnedFilename);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenFirstCall_CreatesFile()
        {
            List<Row> chunk = GetRows();

            _builder.AddPartOfData(chunk);
            var resultFile = _builder.SaveToFile();

            Assert.IsTrue(File.Exists(resultFile));

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendFiveRows_CreatedSheetWithFiveRows()
        {
            List<Row> chunk = GetRows(5);

            _builder.AddPartOfData(chunk);
            var resultFile = _builder.SaveToFile();

            var rowCount = GetRowCountOnTheFirstSheet(resultFile);
            Assert.AreEqual(5, rowCount);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendThousandRows_CreatedSheetWithThousandRows()
        {
            List<Row> chunk = GetRows(1000);

            _builder.AddPartOfData(chunk);
            var resultFile = _builder.SaveToFile();

            var rowCount = GetRowCountOnTheFirstSheet(resultFile);
            Assert.AreEqual(1000, rowCount);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendOneMillionRows_CreatedOneSheet()
        {
            List<Row> chunk = GetRows();
            var maxRows = OneMillion;

            for (var i = 0; i < maxRows; i++)
            {
                _builder.AddPartOfData(chunk);
            }
            var resultFile = _builder.SaveToFile();

            var sheetCount = GetSheetCount(resultFile);
            Assert.AreEqual(1, sheetCount);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendOneMillionAndOneRows_CreatedTwoSheets()
        {
            List<Row> chunk = GetRows();
            var maxRows = OneMillion + 1;

            for (var i = 0; i < maxRows; i++)
            {
                _builder.AddPartOfData(chunk);
            }
            var resultFile = _builder.SaveToFile();

            var sheetCount = GetSheetCount(resultFile);
            Assert.AreEqual(2, sheetCount);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendTwoMillionAndOneRows_CreatedThreeSheets()
        {
            List<Row> chunk = GetRows();
            var maxRows = 2 * OneMillion + 1;

            for (var i = 0; i < maxRows; i++)
            {
                _builder.AddPartOfData(chunk);
            }
            var resultFile = _builder.SaveToFile();

            var sheetCount = GetSheetCount(resultFile);
            Assert.AreEqual(3, sheetCount);

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        [TestMethod]
        public void AddPart_WhenSendLargeRandomData_Generate500MBFile()
        {
            List<Row> chunk = GetRowsWithLargeRandomData(5000);
            var maxRows = 10;

            for (var i = 0; i < maxRows; i++)
            {
                _builder.AddPartOfData(chunk);
            }
            var resultFile = _builder.SaveToFile();

            Assert.IsTrue(File.Exists(resultFile));

            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
        }

        #region Private methods

        private int GetSheetCount(string filename)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Open(filename, false))
            {
                return spreadsheetDocument.WorkbookPart.WorksheetParts.Count();
            }
        }

        private int GetRowCountOnTheFirstSheet(string filename)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Open(filename, false))
            {
                WorksheetPart sheet = spreadsheetDocument.WorkbookPart.WorksheetParts.First();
                DocumentFormat.OpenXml.OpenXmlElement sheetData = sheet.Worksheet.ChildElements.First();
                return sheetData.ChildElements.Count;
            }
        }

        private List<Row> GetRows(int count = 1)
        {
            var result = new List<Row>();
            for (var i = 0; i < count; i++)
            {
                result.Add(GetRow());
            }
            return result;
        }

        private Row GetRow()
        {
            var i = 1;
            var data = new String('w', 100);
            return new Row
            {
                Cells =
                {
                    new Cell($"Column {i++}"),
                    new Cell(data),
                    new Cell($"Column {i}")
                }
            };
        }

        private List<Row> GetRowsWithLargeRandomData(int count = 1)
        {
            var result = new List<Row>();
            for (var i = 0; i < count; i++)
            {
                result.Add(GetRowWithLargeRandomData());
            }
            return result;
        }

        private Row GetRowWithLargeRandomData()
        {
            var row = new Row();
            for (var j = 0; j < 100; j++)
            {
                var buffer = new StringBuilder(250);
                buffer.Append(Guid.NewGuid());
                buffer.Append(Guid.NewGuid());
                buffer.Append(Guid.NewGuid());
                buffer.Append(Guid.NewGuid());
                buffer.Append(Guid.NewGuid());
                row.Cells.Add(new Cell(buffer.ToString()));
            }
            return row;
        }

        #endregion

    }
}
