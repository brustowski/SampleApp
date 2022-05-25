using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Truck;
using System;

namespace FilingPortal.Domain.Services.Truck
{
    /// <summary>
    /// Truck document creation service
    /// </summary>
    public class TruckDocumentFactory : IDocumentFactory<TruckDocument>
    {
        /// <summary>
        /// Creates Truck document from specified DTO and with specified creator name
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="username">The creator name</param>
        public TruckDocument CreateFromDto<TDocumentDto>(TDocumentDto dto, string username)
            where TDocumentDto : BaseDocumentDto
        {
            TruckDocument truckDocument = Mapper.Map<TDocumentDto, TruckDocument>(dto);
            truckDocument.CreatedUser = username;
            truckDocument.CreatedDate = DateTime.Now;
            return truckDocument;
        }
    }
}
