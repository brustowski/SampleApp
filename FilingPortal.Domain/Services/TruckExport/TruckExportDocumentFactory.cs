using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.TruckExport;
using System;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Truck Export document creation service
    /// </summary>
    public class TruckExportDocumentFactory : IDocumentFactory<TruckExportDocument>
    {
        /// <summary>
        /// Creates Truck document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public TruckExportDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            TruckExportDocument truckDocument = Mapper.Map<TDocumentDto, TruckExportDocument>(dto);
            truckDocument.CreatedUser = username;
            truckDocument.CreatedDate = DateTime.Now;
            return truckDocument;
        }
    }
}
