using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using System.IO;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes template processing service for parsing data model and result entity
    /// </summary>
    /// <typeparam name="TParsingDataModel">Parsing data model</typeparam>
    /// <typeparam name="TEntity">Rule entity</typeparam>
    public interface IFormDataTemplateProcessingService<TParsingDataModel, TEntity> : IFormDataTemplateProcessingService
        where TParsingDataModel : IParsingDataModel, new()
        where TEntity : BaseDefValuesManual, new()
    {

    }

    /// <summary>
    /// Describes the Form data template processing service
    /// </summary>
    public interface IFormDataTemplateProcessingService
    {
        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="filePath">File path</param>
        /// <param name="configuration">Import configuration</param>
        FileProcessingDetailedResult Process(string fileName, string filePath, IFormImportConfiguration configuration);

        /// <summary>
        /// Processing the stream
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="configuration">Import configuration</param>
        FileProcessingDetailedResult Process(string fileName, Stream fileStream, IFormImportConfiguration configuration);
    }
}