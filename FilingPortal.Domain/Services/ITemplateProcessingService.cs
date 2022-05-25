using System.IO;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes template processing service for parsing data model and result entity
    /// </summary>
    /// <typeparam name="TParsingDataModel">Parsing data model</typeparam>
    /// <typeparam name="TRuleEntity">Rule entity</typeparam>
    public interface ITemplateProcessingService<TParsingDataModel, TRuleEntity> : ITemplateProcessingService
        where TParsingDataModel : IParsingDataModel, new()
        where TRuleEntity : AuditableEntity, new()
    {
        
    }

    /// <summary>
    /// Provides file processing method
    /// </summary>
    public interface ITemplateProcessingService
    {
        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="path">Fully qualified file path</param>
        /// <param name="userName">Current user name</param>
        FileProcessingResult Process(string fileName, string path, string userName = null);

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="filesStream">File stream</param>
        /// <param name="userName">Current user name</param>
        FileProcessingResult Process(string fileName, Stream filesStream, string userName = null);

        /// <summary>
        /// Verify the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        FileProcessingDetailedResult Verify(AppDocument document);

        /// <summary>
        /// Import data from the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        /// <param name="userName">Current user name</param>
        FileProcessingDetailedResult Import(AppDocument document, string userName);
    }
}
