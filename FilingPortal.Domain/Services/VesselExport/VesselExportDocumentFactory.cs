using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.VesselExport;
using System;

namespace FilingPortal.Domain.Services.VesselExport
{
    /// <summary>
    /// Vessel Export document creation service
    /// </summary>
    public class VesselExportDocumentFactory : IDocumentFactory<VesselExportDocument>
    {
        /// <summary>
        /// Creates Vessel document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public VesselExportDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            VesselExportDocument truckDocument = Mapper.Map<TDocumentDto, VesselExportDocument>(dto);
            truckDocument.CreatedUser = username;
            truckDocument.CreatedDate = DateTime.Now;
            return truckDocument;
        }
    }
}
