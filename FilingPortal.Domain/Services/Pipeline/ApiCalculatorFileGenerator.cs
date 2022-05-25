using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Provides method that generates the api calculator document based on entity
    /// </summary>
    public class ApiCalculatorFileGenerator : IFileGenerator<PipelineInbound>
    {
        /// <summary>
        /// The Excel file builder
        /// </summary>
        private readonly IExcelDocumentBuilder _fileBuilder;
        
        /// <summary>
        /// The new file factory
        /// </summary>
        private readonly ITemplatesProviderService _templateFileProvider;
        /// <summary>
        /// File Template name
        /// </summary>
        private const string TemplateName = "pipeline_api_calculator_template.xlsx";

        /// <summary>
        /// The file name
        /// </summary>
        private static string FileName => GeneratorFileNames.PipelineApiCalculatorFileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCalculatorFileGenerator"/> class
        /// </summary>
        /// <param name="fileBuilder">The file builder</param>
        /// <param name="templateFileProvider">The new file factory</param>
        public ApiCalculatorFileGenerator(IExcelDocumentBuilder fileBuilder, ITemplatesProviderService templateFileProvider)
        {
            _fileBuilder = fileBuilder;
            _templateFileProvider = templateFileProvider;
        }

        /// <summary>
        /// Generates the API calculator for specified inbound record
        /// </summary>
        /// <param name="inbound">The inbound record</param>
        public BinaryFileModel Generate(PipelineInbound inbound)
        {
            byte[] template = _templateFileProvider.GetTemplateAsByteArray(TemplateName);
            byte[] data = _fileBuilder.Open(template)
                .SetNamedCellValue("api", inbound.API)
                .SetNamedCellValue("quantity", inbound.Quantity)
                .SetFullCalculationOnLoad(true)
                .ToByteArray();

            BinaryFileModel binaryFileModel = CreateBinaryModel(data);

            return binaryFileModel;
        }

        /// <summary>
        /// Creates the binary model using specified data
        /// </summary>
        /// <param name="data">The data</param>
        private BinaryFileModel CreateBinaryModel(byte[] data)
        {
            return new BinaryFileModel
            {
                Content = data,
                FileName = FileName
            };
        }
    }
}