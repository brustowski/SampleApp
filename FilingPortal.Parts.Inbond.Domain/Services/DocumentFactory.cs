using System;
using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Services
{
    /// <summary>
    /// Document creation service
    /// </summary>
    public class DocumentFactory : IDocumentFactory<Document>
    {
        /// <summary>
        /// Creates document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public Document CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            Document document = Mapper.Map<TDocumentDto, Document>(dto);
            document.CreatedUser = username;
            document.CreatedDate = DateTime.Now;
            return document;
        }
    }
}
