using AutoMapper;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DocumentTypes;
using FilingPortal.Domain.DocumentTypes.Entities;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Pipeline;
using System;
using System.IO;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Pipeline document creation service
    /// </summary>
    public class PipelineDocumentFactory : IPipelineDocumentFactory
    {
        /// <summary>
        /// The document type name
        /// </summary>
        private const string TypeName = "CAL";

        /// <summary>
        /// Document type repository
        /// </summary>
        private readonly IDocumentTypesRepository _documentTypesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDocumentFactory"/> class
        /// </summary>
        /// <param name="documentTypesRepository">The document type repository</param>
        public PipelineDocumentFactory(IDocumentTypesRepository documentTypesRepository)
        {
            _documentTypesRepository = documentTypesRepository;
        }

        /// <summary>
        /// Creates Pipeline document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public PipelineDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            PipelineDocument truckDocument = Mapper.Map<TDocumentDto, PipelineDocument>(dto);
            truckDocument.CreatedUser = username;
            truckDocument.CreatedDate = DateTime.Now;
            return truckDocument;
        }

        /// <summary>
        /// Creates the pipeline api calculator document from specified file model and with specified creator name
        /// </summary>
        /// <param name="file">The file model</param>
        /// <param name="creatorName">The creator name</param>
        public PipelineDocument CreateApiCalculator(BinaryFileModel file, string creatorName)
        {
            DocumentType fileDescription = _documentTypesRepository.GetByTypeName(TypeName);

            return new PipelineDocument
            {
                DocumentType = TypeName,
                Description = fileDescription?.Description,
                Extension = Path.GetExtension(file.FileName),
                Content = file.Content,
                FileName = file.FileName,
                CreatedUser = creatorName,
                CreatedDate = DateTime.Now
            };
        }
    }
}
