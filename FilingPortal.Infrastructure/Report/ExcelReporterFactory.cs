using FilingPortal.Domain.Common.Reporting;

namespace FilingPortal.Infrastructure.Report
{
    /// <summary>
    /// Represents the Excel Reporter Factory
    /// </summary>
    internal class ExcelReporterFactory : IReporterFactory
    {
        /// <summary>
        /// The file name provider
        /// </summary>
        private readonly IReportFilenameProvider _reportFilenameProvider;
        /// <summary>
        /// Initialize a new instance of the <see cref="ExcelReporterFactory"/> class
        /// </summary>
        /// <param name="reportFilenameProvider"></param>
        public ExcelReporterFactory(IReportFilenameProvider reportFilenameProvider)
        {
            _reportFilenameProvider = reportFilenameProvider;
        }
        /// <summary>
        /// Create a new Excel file with specified name
        /// </summary>
        /// <param name="filename">The file name</param>
        public IReporter Create(string filename)
        {
            return new ExcelReporter(filename, _reportFilenameProvider);
        }
        /// <summary>
        /// Create a new Excel file with specified name and sheet name
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="sheetName">The sheet name</param>
        public IReporter Create(string filename, string sheetName)
        {
            return new ExcelReporter(filename, _reportFilenameProvider, sheetName);
        }
    }
}
