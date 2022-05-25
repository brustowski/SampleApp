using System;
using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Vessel;

namespace FilingPortal.Domain.Services.Vessel
{
    /// <summary>
    /// Vessel Import document creation service
    /// </summary>
    public class VesselDocumentFactory : IDocumentFactory<VesselImportDocument>
    {
        /// <summary>
        /// Creates Vessel document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public VesselImportDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            VesselImportDocument truckDocument = Mapper.Map<TDocumentDto, VesselImportDocument>(dto);
            truckDocument.CreatedUser = username;
            truckDocument.CreatedDate = DateTime.Now;
            return truckDocument;
        }
    }
}
