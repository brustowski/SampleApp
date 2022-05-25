using AutoMapper;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Mapping.Resolvers;

namespace FilingPortal.Parts.Recon.Domain.Mapping
{
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the DTO models
        /// </summary>
        public DomainToDTOProfile()
        {
            CreateMap<InboundRecord, Reporting.CargoWiseInternal.Model>();
            CreateMap<InboundRecord, Reporting.CargoWiseClientFta.Model>()
                .ForMember(d => d.FtaEligibility, opt => opt.Ignore())
                .ForMember(d => d.ClientNote, opt => opt.Ignore());
            CreateMap<InboundRecord, Reporting.CargoWiseClientValue.Model>()
                .ForMember(d => d.FinalUnitPrice, opt => opt.Ignore())
                .ForMember(d => d.FinalTotalValue, opt => opt.Ignore())
                .ForMember(d => d.ClientNote, opt => opt.Ignore());

            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.ValueFlagMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.ReconciliationIndicator, opt => opt.MapFrom(s => s.AceReconIndicator))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchReconValueFlag
                        ? s.AceReconIndicator == null || s.AceReconIndicator == "" ? "Reconciliation Indicator doesn’t exist."
                            : s.AceReconIndicator == "Y" ? "Incorrect - No VL Recon Flag" : "Incorrect - VL Recon Flag"
                        : s.AceReconIndicator == "Y" ? "Correct - VL Recon Flag" : "Correct - No VL Recon Flag"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.FtaFlagMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.NaftaReconciliationIndicator, opt => opt.MapFrom(s => s.AceNaftaReconIndicator))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchReconFtaFlag
                        ? s.AceNaftaReconIndicator == null || s.AceNaftaReconIndicator == "" ? "NAFTA Reconciliation Indicator doesn’t exist"
                            : s.AceNaftaReconIndicator == "Y" ? "Incorrect - No FTA Recon Flag" : "Incorrect - FTA Recon Flag"
                        : s.AceNaftaReconIndicator == "Y" ? "Correct - FTA Recon Flag" : "Correct - No FTA Recon Flag"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.EntryValueMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.LineGoodsValueAmount, opt => opt.MapFrom(s => s.AceLineGoodsValueAmount))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchEntryValue
                        ? s.AceLineGoodsValueAmount == 0 ? "Line Goods Value Amount doesn’t exist" : "Value Doesn't Match"
                        : "Value Matches"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.DutyMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.LineDutyAmount, opt => opt.MapFrom(s => s.AceLineDutyAmount))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchDuty
                        ? s.AceLineDutyAmount == 0 ? "Line Duty Amount doesn’t exist" : "Duty Doesn't Match"
                        : "Duty Matches"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.MpfMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.LineMpfAmount, opt => opt.MapFrom(s => s.AceLineMpfAmount))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchMpf
                        ? s.AceLineMpfAmount == 0 ? "Line MPF Amount doesn’t exist" : "MPF Doesn't Match"
                        : "MPF Matches"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.PayableMpfMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.TotalPaidMpfAmount, opt => opt.MapFrom(s => s.AceTotalPaidMpfAmount))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchPayableMpf
                        ? s.AceTotalPaidMpfAmount == 0 ? "Total Paid MPF Amount doesn’t exist" : "Payable MPF Doesn't Match"
                        : "Payable MPF Matches"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.QuantityMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.LineTariffQuantity, opt => opt.MapFrom(s => s.AceLineTariffQuantity))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchQuantity
                        ? s.AceLineTariffQuantity == 0 ? "Line Tariff Quantity (1) doesn’t exist" : "Quantity Doesn't Match"
                        : "Quantity Matches"));
            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.HtsMismatchModel>()
                .ForMember(d => d.EntrySummaryNumber, opt => opt.MapFrom(s => s.AceEntrySummaryNumber))
                .ForMember(d => d.Line, opt => opt.MapFrom(s => s.LineNumber7501))
                .ForMember(d => d.HtsNumberFull, opt => opt.MapFrom(s => s.AceHtsNumberFull))
                .ForMember(d => d.Message, opt
                    => opt.MapFrom(s => s.MismatchHts
                        ? s.AceHtsNumberFull == null || s.AceHtsNumberFull == "" ? "HTS Number doesn’t exist" : "HTS Doesn't Match"
                        : "HTS Matches"));

            CreateMap<InboundRecordReadModel, Reporting.CargoWiseRecon.AllMismatchesModel>()
                .ForMember(d => d.Message, opt => opt.ResolveUsing<ReconValidationErrorsResolver>());

            CreateMap<FtaReconReadModel, Reporting.ReconEntryAssociation.Model>()
                .ForMember(d => d.FilerEntryNo, opt => opt.MapFrom(s => s.Filer + s.EntryNo))
                .ForMember(d => d.DutyFees, opt => opt.UseValue("Y"));

            CreateMap<ValueReconReadModel, Reporting.ValueReconEntry.Model>()
                .ForMember(d => d.FilerEntryNo, opt => opt.MapFrom(s => s.Filer + s.EntryNo))
                .ForMember(d => d.DutyFees, opt => opt.UseValue("Y"));

            CreateMap<ValueReconReadModel, Reporting.ValueReconEntryLineData.Model>()
                .ForMember(d => d.ImportEntryRef, opt => opt.MapFrom(s => s.Filer + s.EntryNo))
                .ForMember(d => d.OriginLineNumber, opt => opt.MapFrom(s=>s.LineNumber7501))
                .ForMember(d => d.OriginCustomsValue, opt => opt.MapFrom(s => s.LineEnteredValue))
                .ForMember(d => d.Org, opt => opt.MapFrom(s => s.CO))
                .ForMember(d => d.OriginTariff, opt => opt.MapFrom(s=>s.Tariff))
                .ForMember(d => d.OriginProvProgTariff, opt => opt.MapFrom(x=>x.ProvProgTariff))
                .ForMember(d => d.OriginQty1, opt => opt.MapFrom(s => s.CustomsQty1))
                .ForMember(d => d.OriginUq1, opt => opt.MapFrom(s => s.CustomsUq1))
                .ForMember(d => d.OriginSpi, opt => opt.MapFrom(s => s.Spi))
                .ForMember(d => d.OriginDuty, opt => opt.MapFrom(s => s.Duty))
                .ForMember(d => d.OriginHmfAmount, opt => opt.MapFrom(s => s.Hmf))
                .ForMember(d => d.OriginMpfAmount, opt => opt.MapFrom(s => s.Mpf))
                .ForMember(d=>d.ReconTariff, opt=>opt.MapFrom(s=>s.Tariff))
                .ForMember(d => d.ReconTariffRecProvProgTariff, opt => opt.MapFrom(x => x.ProvProgTariff))
                .ForMember(d => d.ReconQty1, opt => opt.MapFrom(s => s.CustomsQty1))
                .ForMember(d => d.ReconUq1, opt => opt.MapFrom(s=>s.CustomsUq1))
                .ForMember(d => d.ReconSpi, opt 
                    => opt.MapFrom(s=>s.FtaEligibility == "Y" && s.ReconNfJobNumber != null ? s.ReconEntryLineSpi : s.Spi))
                ;
        }
    }
}
