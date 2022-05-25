using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.TemplateEngine;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;

namespace FilingPortal.Infrastructure.TemplateEngine
{
    /// <summary>
    /// Template service 
    /// </summary>
    public class TemplateService : ITemplateService
    {
        /// <summary>
        /// The model map registry
        /// </summary>
        private readonly IParseModelMapRegistry _registry;
        /// <summary>
        /// Template file factory
        /// </summary>
        private readonly IReporterFactory _fileFactory;

        /// <summary>
        /// Initialize a new instance of the <see cref="TemplateService"/> class.
        /// </summary>
        /// <param name="registry">The model map registry</param>
        /// <param name="fileFactory">File creation factory</param>
        public TemplateService(IParseModelMapRegistry registry, IReporterFactory fileFactory)
        {
            _registry = registry;
            _fileFactory = fileFactory;
        }

        /// <summary>
        /// Creates the template file based on provided configuration
        /// </summary>
        /// <param name="configuration">Template configuration</param>
        public FileExportResult Create(IImportConfiguration configuration)
        {
            IParseModelMap modelMap = _registry.Get(configuration.ModelType);

            var row = new Row();
            foreach (IPropertyMapInfo info in modelMap.MapInfos)
            {
                row.CreateCell(info.FieldName);
            }

            var fileName = $"{configuration.FileName}.xlsx";
            IReporter excelReporter = _fileFactory.Create(fileName, modelMap.SheetName);
            excelReporter.AddPartOfData(new[] { row });
            var fullFilePath = excelReporter.SaveToFile();

            return new FileExportResult
            {
                DocumentExternalName = fileName,
                FileName = fullFilePath
            };
        }
    }
}
