using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Parts.Zones.Entry.Domain.Converters;
using FilingPortal.Parts.Zones.Entry.Domain.Dtos;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.RuleImporter;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Zones.Entry.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DTOsToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DTOsToDomainProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DTOsToDomainProfile()
        {
            CreateMap<DocumentDto, Document>()
                .ForMember(x => x.Content, opt => opt.MapFrom(s => s.FileContent))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.FileDesc))
                .ForMember(x => x.Extension, opt => opt.MapFrom(s => s.FileExtension))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaderId, opt => opt.Ignore())
                .ForMember(x => x.FilingHeader, opt => opt.Ignore());

            CreateMap<CUSTOMS_ENTRY_FILEENTRY, InboundRecord>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Ein, 
                    opt => opt.MapFrom(x => x.HEADER_ADDRESS
                        .FirstOrDefault(h => h.TYPE == "IM").MID_OR_IRS_NO))
                .ForMember(x => x.ImporterId,
                    opt => opt.ResolveUsing<ClientIdByClientNumberResolver, string>(x =>
                        x.HEADER_ADDRESS.FirstOrDefault(h => h.TYPE == "IM").MID_OR_IRS_NO))
                .ForMember(x => x.EntryPort, opt => opt.MapFrom(x => x.US_ENTRY_PORT))
                .ForMember(x => x.ArrivalDate, opt => opt.MapFrom(x => x.ESTIMATED_ARRIVAL_DATE))
                .ForMember(x => x.FirmsCode, opt => opt.MapFrom(x => x.FIRMS_CODE))
                .ForMember(x => x.OwnerRef, opt => opt.MapFrom(x => x.FILE_NO))
                .ForMember(x => x.EntryNo, opt => opt.MapFrom(x => $"{x.ENTRY_NO}{x.CHECK_DIGIT}"))
                .ForMember(x => x.VesselName, opt => opt.MapFrom(x => x.VESSEL_NAME.TrimStart("FTZ")))
                .ForMember(x => x.FilerCode, opt => opt.MapFrom(x => x.FILER_CODE))

                .AfterMap((src, dest, context) =>
                {
                    CUSTOMS_ENTRY_FILEENTRYINVOICE invoice = src.INVOICE?.FirstOrDefault();

                    if (src.CARGO_RELEASE_ITEM.SafeAny())
                    {
                        dest.InboundLines =
                            context.Mapper.Map<CUSTOMS_ENTRY_FILEENTRYCARGO_RELEASE_ITEM[], ICollection<InboundLine>>(
                                src.CARGO_RELEASE_ITEM);
                    }

                    if (invoice != null && invoice.ITEM.SafeAny())
                    {
                        dest.InboundLines =
                            context.Mapper.Map<CUSTOMS_ENTRY_FILEENTRYINVOICEITEM[], ICollection<InboundLine>>(
                                invoice.ITEM);
                    }

                    if (src.ENTRY_NOTES.SafeAny())
                    {
                        dest.Notes = context.Mapper.Map<CUSTOMS_ENTRY_FILEENTRYENTRY_NOTES[], ICollection<InboundNote>>(
                            src.ENTRY_NOTES);
                    }

                    dest.ParsedData = new InboundParsedData { InboundRecord = dest };

                    context.Mapper.Map(src, dest.ParsedData);

                });

            CreateMap<CUSTOMS_ENTRY_FILEENTRY, InboundParsedData>()
                .ForMember(x => x.FilerCode, opt => opt.MapFrom(x => x.FILER_CODE))
                .ForMember(x => x.EntryNumber, opt => opt.MapFrom(x => x.ENTRY_NO))
                .ForMember(x => x.CheckDigit, opt => opt.MapFrom(x => x.CHECK_DIGIT))
                .ForMember(x => x.ImportDate, opt => opt.MapFrom(x => x.IMPORT_DATE))
                .ForMember(x => x.TeamNo, opt => opt.MapFrom(x => x.TEAM_NO))
                .ForMember(x => x.NaftaRecon, opt => opt.MapFrom(x => x.NAFTA_RECON.IsNotEmpty() ? x.NAFTA_RECON.ToUpper() == "T" : (bool?)null))
                .ForMember(x => x.ReconIssue, opt => opt.MapFrom(x => x.RECON_ISSUE))
                .ForMember(x => x.UltimateDestinationState, opt => opt.MapFrom(x => x.ULTIMATE_DESTINATION_STATE))
                .ForMember(x => x.ApplicationBeginDate, opt => opt.MapFrom(x => x.APPLICATION_BEGIN_DATE))
                .ForMember(x => x.ApplicationEndDate, opt => opt.MapFrom(x => x.APPLICATION_END_DATE))
                .ForMember(x => x.TotalEnteredValue, opt => opt.MapFrom(x => x.TOTAL_ENTERED_VALUE))
                .ForMember(x => x.FtzNumber, opt => opt.MapFrom(x => x.VESSEL_NAME.TrimStart("FTZ")))
                .AfterMap((src, dest, context) =>
                {
                    var valueConverter = new WeightConverter(src.GROSS_WGT,
                        WeightConverter.WeightConversionTypes.DecimalNumber);

                    dest.GrossWgt = valueConverter.Weight;
                    dest.GrossWgtUnit = valueConverter.Measure;

                    dest.Charges = dest.InboundRecord?.InboundLines?.Sum(x => x.Charges);
                });

            CreateMap<CUSTOMS_ENTRY_FILEENTRYCARGO_RELEASE_ITEM, InboundLine>()
                .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ITEM_NO))
                .ForMember(x => x.ItemValue, opt => opt.MapFrom(x => x.ITEM_VALUE))
                .ForMember(x => x.Hts, opt => opt.MapFrom(x => x.HTS))
                .ForMember(x => x.CountryOfOrigin, opt => opt.MapFrom(x => x.COUNTRY_OF_ORIGIN))
                .ForMember(x => x.ManufacturersIdNo, opt => opt.MapFrom(x => x.MANUFACTURERS_ID_NO))
                .ForMember(x => x.FtzManifestQty, opt => opt.MapFrom(x => x.FTZ_MANIFEST_QTY))
                .ForMember(x => x.FtzStatus, opt => opt.MapFrom(x => x.FTZ_STATUS))
                .ForMember(x => x.FtzDate, opt => opt.MapFrom(x => x.FTZ_DATE))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.PRODUCT_NAME))
                .AfterMap((src, dest, context) =>
                {
                    if (src.CARGO_RELEASE_ITEM_PGA_EPA.SafeAny())
                    {
                        var pga = src.CARGO_RELEASE_ITEM_PGA_EPA.Where(x => x.GOVERNMENT_AGENCY_PROGRAM_CODE != "TS1").ToList();
                        if (pga.Count != 0)
                        {
                            dest.DeclarationCode = string.Join("-", pga.Select(x => x.DECLARATION_CODE));
                            dest.ProgramCode = string.Join("-", pga.Select(x => x.GOVERNMENT_AGENCY_PROGRAM_CODE));
                            dest.Disclaimer=string.Join("-", pga.Select(x => x.DISCLAIMER));
                        }
                    }

                })
                    ;

    

            CreateMap<CUSTOMS_ENTRY_FILEENTRYINVOICEITEM, InboundLine>()
                .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ITEM_NO))
                .ForMember(x => x.Hts, opt => opt.MapFrom(x => x.HTS))
                .ForMember(x => x.ItemValue, opt => opt.MapFrom(x => x.HTS_VALUE))
                .ForMember(x => x.CountryOfOrigin, opt => opt.MapFrom(x => x.COUNTRY_OF_ORIGIN))
                .ForMember(x => x.ManufacturersIdNo, opt => opt.MapFrom(x => x.MANUFACTURERS_ID_NO))
                .ForMember(x => x.FtzManifestQty, opt => opt.MapFrom(x => x.FTZ_MANIFEST_QTY))
                .ForMember(x => x.Qty_1_UOM, opt => opt.MapFrom(x => x.QTY_1_UOM))
                .ForMember(x => x.FtzStatus, opt => opt.MapFrom(x => x.FTZ_STATUS))
                .ForMember(x => x.FtzDate, opt => opt.MapFrom(x => x.FTZ_DATE))
                .ForMember(x => x.Spi, opt => opt.MapFrom(x => x.SPECIAL_PROGRAMS_INDICATOR_1))
                .ForMember(x => x.TransactionRelated,
                    opt => opt.MapFrom(x =>
                        x.RELATED_FLAG.IsNotEmpty() ? x.RELATED_FLAG.ToUpper() == "T" ||  x.RELATED_FLAG.ToUpper() == "Y" ? "Y" : "N" : ""))
                .ForMember(x => x.Charges, opt => opt.MapFrom(x => x.CHARGES))
                .AfterMap((src, dest, context) =>
                {
                    var valueConverter = new WeightConverter(src.GROSS_WGT,
                        WeightConverter.WeightConversionTypes.DecimalNumber);

                    dest.GrossWeight = valueConverter.Weight;
                    dest.GrossWeightUnit = valueConverter.Measure;
                })
                ;
            CreateMap<ImportModel, RuleImporter>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.ImporterId,
                    opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.ImporterCode))
                .ForMember(x => x.Importer, opt => opt.Ignore())
                ;
            CreateMap<CUSTOMS_ENTRY_FILEENTRYENTRY_NOTES, InboundNote>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.TITLE))
                .ForMember(x => x.Date, opt => opt.MapFrom(x => x.DATE_TIME))
                .ForMember(x => x.Message, opt => opt.MapFrom(x => x.MESSAGE))
                ;
        }
    }
}