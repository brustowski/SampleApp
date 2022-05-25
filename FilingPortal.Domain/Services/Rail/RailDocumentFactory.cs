using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using System;
using System.IO;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DocumentTypes;
using FilingPortal.Domain.DocumentTypes.Entities;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Rail document creation service
    /// </summary>
    public class RailDocumentFactory : IRailDocumentFactory
    {
        /// <summary>
        /// The document type name
        /// </summary>
        private const string TypeName = "MAN";

        /// <summary>
        /// Document type repository
        /// </summary>
        private readonly IDocumentTypesRepository _documentTypesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailDocumentFactory"/> class
        /// </summary>
        /// <param name="documentTypesRepository">The document type repository</param>
        public RailDocumentFactory(IDocumentTypesRepository documentTypesRepository)
        {
            _documentTypesRepository = documentTypesRepository;
        }

        /// <summary>
        /// Creates Rail document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public RailDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            RailDocument railDocument = Mapper.Map<TDocumentDto, RailDocument>(dto);
            railDocument.CreatedUser = username;
            railDocument.CreatedDate = DateTime.Now;
            return railDocument;
        }

        /// <summary>
        /// Creates the Rail document manifest from specified file model and with specified creator name
        /// </summary>
        /// <param name="file">The file model</param>
        /// <param name="creatorName">The creator name</param>
        public RailDocument CreateManifest(BinaryFileModel file, string creatorName)
        {
            DocumentType fileDescription = _documentTypesRepository.GetByTypeName(TypeName);

            return new RailDocument
            {
                DocumentType = TypeName,
                Description = fileDescription?.Description,
                Extension = Path.GetExtension(file.FileName),
                Content = file.Content,
                FileName = file.FileName,
                IsManifest = 1,
                CreatedUser = creatorName,
                CreatedDate = DateTime.Now
            };
        }
    }
}
