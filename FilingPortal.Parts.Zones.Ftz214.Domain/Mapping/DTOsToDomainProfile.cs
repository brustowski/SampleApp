using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Parts.Zones.Ftz214.Domain.Converters;
using FilingPortal.Parts.Zones.Ftz214.Domain.Dtos;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Mapping
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

            CreateMap<FTZ_214FTZ_ADMISSION, InboundRecord>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Ein, opt => opt.MapFrom(x => x.APPLICANT_IRS_NO))
                .ForMember(x => x.ApplicantId, opt
                    => opt.ResolveUsing<ClientIdByClientNumberResolver, string>(x => x.APPLICANT_IRS_NO))
                .ForMember(x => x.FtzOperatorId, opt
                    => opt.ResolveUsing<ClientIdByClientNumberResolver, string>(x => x.SUBMITTER_IRS_NO))
                .ForMember(x => x.ZoneId, opt => opt.MapFrom(x => x.ZONE_NO))
                .ForMember(x => x.AdmissionType, opt 
                    => opt.MapFrom(x => x.CONVEYANCE.FirstOrDefault().ADMISSION_TYPE))
                .AfterMap((src, dest, context) =>
                {
                    FTZ_214FTZ_ADMISSIONCONVEYANCEPTT_INBONDBILL bill = src.CONVEYANCE.FirstOrDefault()?.PTT_INBOND.FirstOrDefault()?.BILL.FirstOrDefault();

                    if (bill != null && bill.ITEM.SafeAny())
                    {
                        dest.InboundParsedLines =
                            context.Mapper.Map<FTZ_214FTZ_ADMISSIONCONVEYANCEPTT_INBONDBILLITEM[], ICollection<InboundParsedLine>>(bill.ITEM);
                    }


                    dest.InboundParsedData = new InboundParsedData { InboundRecord = dest };

                    context.Mapper.Map(src, dest.InboundParsedData);

                });

            CreateMap<FTZ_214FTZ_ADMISSION, InboundParsedData>()
                .ForMember(x => x.CompanyCode, opt => opt.MapFrom(x => x.COMPANY_CODE))
                .ForMember(x => x.OfficeCode, opt => opt.MapFrom(x => x.OFFICE_CODE))
                .ForMember(x => x.FileNo, opt => opt.MapFrom(x => x.FILE_NO))
                .ForMember(x => x.FilerCode, opt => opt.MapFrom(x => x.FILER_CODE))
                .ForMember(x => x.RecordExistsAction, opt => opt.MapFrom(x => x.RECORD_EXISTS_ACTION))
                .ForMember(x => x.PartnerKey, opt => opt.MapFrom(x => x.PARTNER_KEY))
                .ForMember(x => x.PartnerKey2, opt => opt.MapFrom(x => x.PARTNER_KEY_2))
                .ForMember(x => x.PartnerKey3, opt => opt.MapFrom(x => x.PARTNER_KEY_3))
                .ForMember(x => x.PartnerKey4, opt => opt.MapFrom(x => x.PARTNER_KEY_4))
                .ForMember(x => x.AdmissionNo, opt => opt.MapFrom(x => x.ADMISSION_NO))
                .ForMember(x => x.AdmissionYear, opt => opt.MapFrom(x => x.ADMISSION_YEAR))
                .ForMember(x => x.ZoneNo, opt => opt.MapFrom(x => x.ZONE_NO))
                .ForMember(x => x.DirectDelivery, opt
                    => opt.MapFrom(s => s.DIRECT_DELIVERY == "T" ? "Y" : "N"))
                .ForMember(x => x.AbiRouting, opt => opt.MapFrom(x => x.ABI_ROUTING))
                .ForMember(x => x.ApplicantIrsNo, opt => opt.MapFrom(x => x.APPLICANT_IRS_NO))
                .ForMember(x => x.SubmitterIrsNo, opt => opt.MapFrom(x => x.SUBMITTER_IRS_NO))
                .ForMember(x => x.SentToCensus, opt => opt.MapFrom(x => x.SENT_TO_CENSUS))
                .ForMember(x => x.AuthorizedGoodsDesc, opt => opt.MapFrom(x => x.AUTHORIZED_GOODS_DESC))
                .ForMember(x => x.ExceptionsExist, opt => opt.MapFrom(x => x.EXCEPTIONS_EXIST))
                .ForMember(x => x.ExceptGoodsDesc, opt => opt.MapFrom(x => x.EXCEPT_GOODS_DESC))
                .AfterMap((src, dest, context) => context.Mapper.Map(src.CONVEYANCE.FirstOrDefault(), dest));

            CreateMap<FTZ_214FTZ_ADMISSIONCONVEYANCE, InboundParsedData>()
                .ForMember(x => x.ConvNo, opt => opt.MapFrom(x => x.CONV_NO))
                .ForMember(x => x.AdmissionType, opt => opt.MapFrom(x => x.ADMISSION_TYPE))
                .ForMember(x => x.UnladingPort, opt => opt.MapFrom(x => x.UNLADING_PORT))
                .ForMember(x => x.Mot, opt => opt.MapFrom(x => x.MOT))
                .ForMember(x => x.ImpCarrierCode, opt => opt.MapFrom(x => x.IMP_CARRIER_CODE))
                .ForMember(x => x.ImpCarrierName, opt => opt.MapFrom(x => x.IMP_CARRIER_NAME))
                .ForMember(x => x.ImpVessel, opt => opt.MapFrom(x => x.IMP_VESSEL))
                .ForMember(x => x.ImpVesselCountryCode, opt => opt.MapFrom(x => x.IMP_VESSEL_COUNTRY_CODE))
                .ForMember(x => x.FltVoyTrip, opt => opt.MapFrom(x => x.FLT_VOY_TRIP))
                .ForMember(x => x.ExportDate, opt => opt.MapFrom(x => x.EXPORT_DATE))
                .ForMember(x => x.ImportDate, opt => opt.MapFrom(x => x.IMPORT_DATE))
                .ForMember(x => x.EstArrDate, opt => opt.MapFrom(x => x.EST_ARR_DATE))
                .ForMember(x => x.UserProvidedSfTransactionNo, opt => opt.MapFrom(x => x.USER_PROVIDED_SF_TRANSACTION_NO))
                .ForMember(x => x.RcptRprDate, opt => opt.MapFrom(x => x.RCPT_RPT_DATE))
                .ForMember(x => x.FilingStatus, opt => opt.MapFrom(x => x.FILINGSTATUS))
                .AfterMap((src, dest, context) => context.Mapper.Map(src.PTT_INBOND.FirstOrDefault(), dest));

            CreateMap<FTZ_214FTZ_ADMISSIONCONVEYANCEPTT_INBOND, InboundParsedData>()
                .ForMember(x => x.PttFirms, opt => opt.MapFrom(x => x.PTT_FIRMS))
                .ForMember(x => x.PttIrsNo, opt => opt.MapFrom(x => x.PTT_IRS_NO))
                .ForMember(x => x.ItNo, opt => opt.MapFrom(x => x.IT_NO))
                .ForMember(x => x.ItDate, opt => opt.MapFrom(x => x.IT_DATE))
                .ForMember(x => x.ItCarrierCode, opt => opt.MapFrom(x => x.IT_CARRIER_CODE.Trim()))
                .ForMember(x => x.ItCarrierName, opt => opt.MapFrom(x => x.IT_CARRIER_NAME))
                .ForMember(x => x.ItFromPort, opt => opt.MapFrom(x => x.IT_FROM_PORT))
                .ForMember(x => x.ItFromZoneNo, opt => opt.MapFrom(x => x.IT_FROM_ZONE_NO))
                .AfterMap((src, dest, context) => context.Mapper.Map(src.BILL.FirstOrDefault(), dest));

            CreateMap<FTZ_214FTZ_ADMISSIONCONVEYANCEPTT_INBONDBILL, InboundParsedData>()
                .ForMember(x => x.LineNo, opt => opt.MapFrom(x => x.LINE_NO))
                .ForMember(x => x.Master, opt => opt.MapFrom(x => x.MASTER))
                .ForMember(x => x.House, opt => opt.MapFrom(x => x.HOUSE))
                .ForMember(x => x.Qty, opt => opt.MapFrom(x => x.QTY))
                .ForMember(x => x.Qtyuom, opt => opt.MapFrom(x => x.QTYUOM))
                .ForMember(x => x.Qtyuom, opt
                    => opt.MapFrom(s => s.QTYUOM.Contains("LBK") ? "VL" : s.QTYUOM))
                .ForMember(x => x.Ce, opt => opt.MapFrom(x => x.CE))
                .ForMember(x => x.ForeignPort, opt => opt.MapFrom(x => x.FOREIGN_PORT))
                .ForMember(x => x.ForeignPortCode, opt => opt.MapFrom(x => x.FOREIGN_PORT_CODE))
                .ForMember(x => x.ForeignPortName, opt => opt.MapFrom(x => x.FOREIGN_PORT_NAME))
                .ForMember(x => x.BtaIndicator, opt => opt.MapFrom(x => x.BTA_INDICATOR))
                .AfterMap((src, dest, context) =>
                {
                    if (src.CONTAINER.SafeAny())
                    {
                        dest.Container = string.Join(", ", src.CONTAINER.Select(x => x.CONT_NO));
                    }
                });

            CreateMap<FTZ_214FTZ_ADMISSIONCONVEYANCEPTT_INBONDBILLITEM, InboundParsedLine>()
                .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ITEM_NO))
                .ForMember(x => x.SubItem, opt => opt.MapFrom(x => x.SUB_ITEM))
                .ForMember(x => x.Hts, opt => opt.MapFrom(x => x.HTS))
                .ForMember(x => x.AdditionalHts, opt => opt.MapFrom(x => x.ADDITIONAL_HTS))
                .ForMember(x => x.Spi1, opt => opt.MapFrom(x => x.SPI1))
                .ForMember(x => x.Spi1Country, opt => opt.MapFrom(x => x.SPI1_COUNTRY))
                .ForMember(x => x.Spi2, opt => opt.MapFrom(x => x.SPI2))
                .ForMember(x => x.Qty1, opt => opt.MapFrom(x => x.QTY1))
                .ForMember(x => x.Qty1uom, opt => opt.MapFrom(x => x.QTY1UOM))
                .ForMember(x => x.Qty2, opt => opt.MapFrom(x => x.QTY2))
                .ForMember(x => x.Qty2uom, opt => opt.MapFrom(x => x.QTY2UOM))
                .ForMember(x => x.CategoryNo, opt => opt.MapFrom(x => x.CATEGORY_NO))
                .ForMember(x => x.Co, opt => opt.MapFrom(x => x.CO))
                .ForMember(x => x.Mid, opt => opt.MapFrom(x => x.MID))
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.VALUE))
                .ForMember(x => x.AdditionalHtsValue, opt => opt.MapFrom(x => x.ADDITIONAL_HTS_VALUE))
                .ForMember(x => x.GrossWgt, opt => opt.MapFrom(x => x.GROSS_WGT))
                .ForMember(x => x.GrossLbs, opt => opt.MapFrom(x => x.GROSS_LBS))
                .ForMember(x => x.Charges, opt => opt.MapFrom(x => x.CHARGES))
                .ForMember(x => x.Hmf, opt
                    => opt.MapFrom(s => (s.HMF.IsNotEmpty() && double.Parse(s.HMF) > 0) ? "Y" : "N"))
                .ForMember(x => x.ZoneStatus, opt => opt.MapFrom(x => x.ZONE_STATUS))
                .ForMember(x => x.MxCementLicenseNo, opt => opt.MapFrom(x => x.MX_CEMENT_LICENSE_NO))
                .ForMember(x => x.PnDisclaimer, opt => opt.MapFrom(x => x.PN_DISCLAIMER))
                .ForMember(x => x.ImporterAcctno, opt => opt.MapFrom(x => x.IMPORTER_ACCTNO))
                .ForMember(x => x.ImporterAcct, opt => opt.MapFrom(x => x.IMPORTER_ACCT))
                .ForMember(x => x.ImporterIrsno, opt => opt.MapFrom(x => x.IMPORTER_IRSNO))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.PRODUCT_NAME))
                .ForMember(x => x.ProductCountry, opt => opt.MapFrom(x => x.PRODUCT_COUNTRY))
                .ForMember(x => x.ProductQty, opt => opt.MapFrom(x => x.PRODUCT_QTY))
                .AfterMap((src, dest, context) =>
                {
                    if (src.REMARKS.SafeAny())
                    {
                        dest.Remarks = string.Join(", ", src.REMARKS.Select(x => x.REMARK_LINE));
                    }

                    if (src.DESCRIPTION.SafeAny())
                    {
                        dest.Description = string.Join(", ", src.DESCRIPTION.Select(x => x.DESC_LINE));
                    }

                    var valueConverter = new WeightConverter(src.GROSS_WGT, WeightConverter.WeightConversionTypes.DecimalNumber);

                    dest.GrossWgt = (int)valueConverter.Weight;
                });
        }
    }
}