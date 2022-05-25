using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface for document creation service
    /// </summary>
    public interface IDocumentFactory<TDocument>
        where TDocument : BaseDocument
    {
        /// <summary>
        /// Creates document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        TDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto;
    }
}