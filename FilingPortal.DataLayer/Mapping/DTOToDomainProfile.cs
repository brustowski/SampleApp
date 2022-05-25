using AutoMapper;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.DataLayer.Mapping
{
    /// <summary>
    /// Class describing mapping of the dto to the domain entities
    /// </summary>
    public class DTOToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the dto to the domain entities
        /// </summary>
        public DTOToDomainProfile()
        {
            CreateMap<FieldConfiguration, BaseDefValuesManualReadModel>()
                .Include<FieldConfiguration, RailDefValuesManualReadModel>()
                .Include<FieldConfiguration, TruckExportDefValuesManualReadModel>()
                .Include<FieldConfiguration, TruckDefValueManualReadModel>()
                .Include<FieldConfiguration, PipelineDefValueManualReadModel>()
                .Include<FieldConfiguration, VesselImportDefValuesManualReadModel>()
                .Include<FieldConfiguration, VesselExportDefValuesManualReadModel>()
                .ForMember(x => x.ModificationDate, opt => opt.Ignore());
            CreateMap<FieldConfiguration, RailDefValuesManualReadModel>();
            CreateMap<FieldConfiguration, TruckExportDefValuesManualReadModel>();
            CreateMap<FieldConfiguration, TruckDefValueManualReadModel>();
            CreateMap<FieldConfiguration, PipelineDefValueManualReadModel>();
            CreateMap<FieldConfiguration, VesselImportDefValuesManualReadModel>();
            CreateMap<FieldConfiguration, VesselExportDefValuesManualReadModel>();
        }
    }
}