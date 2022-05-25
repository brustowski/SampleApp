using System;
using Framework.Domain;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Entities
{
    /// <summary>
    /// Represents the Inbound record
    /// </summary>
    public class InboundParsedData : Entity
    {
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Gets or sets inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }

        #region ADMISSION
        public string CompanyCode { get; set; }
        public string OfficeCode { get; set; }
        public string FileNo { get; set; }
        public string FilerCode { get; set; }
        public string RecordExistsAction { get; set; }
        public string PartnerKey { get; set; }
        public string PartnerKey2 { get; set; }
        public string PartnerKey3 { get; set; }
        public string PartnerKey4 { get; set; }
        public string AdmissionNo { get; set; }
        public string AdmissionYear { get; set; }
        public string ZoneNo { get; set; }
        public string ZonePort { get; set; }
        public string Firms { get; set; }
        public string DirectDelivery { get; set; }
        public string AbiRouting { get; set; }
        public string SubmitterIrsNo { get; set; }
        public string ApplicantIrsNo { get; set; }
        public string SentToCensus { get; set; }
        public string AuthorizedGoodsDesc { get; set; }
        public string ExceptionsExist { get; set; }
        public string ExceptGoodsDesc { get; set; }
        #endregion

        #region Conveyance
        public int? ConvNo { get; set; }
        public string AdmissionType { get; set; }
        public string UnladingPort { get; set; }
        public string Mot { get; set; }
        public string ImpCarrierCode { get; set; }
        public string ImpCarrierName { get; set; }
        public string ImpVessel { get; set; }
        public string ImpVesselCountryCode { get; set; }
        public string FltVoyTrip { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? ImportDate { get; set; }
        public DateTime? EstArrDate { get; set; }
        public string UserProvidedSfTransactionNo { get; set; }
        public DateTime? RcptRprDate { get; set; }
        public string FilingStatus { get; set; }
        #endregion

        #region PTT_INBOND
        public string PttFirms { get; set; }
        public string PttIrsNo { get; set; }
        public string ItNo { get; set; }
        public DateTime? ItDate { get; set; }
        public string ItCarrierCode { get; set; }
        public string ItCarrierName { get; set; }
        public string ItFromPort { get; set; }
        public string ItFromZoneNo { get; set; }
        #endregion

        #region BILL
        public int? LineNo { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public int? Qty { get; set; }
        public string Qtyuom { get; set; }
        public string Ce { get; set; }
        public string ForeignPort { get; set; }
        public string ForeignPortCode { get; set; }
        public string ForeignPortName { get; set; }
        public string BtaIndicator { get; set; }
        public string Container { get; set; }
        #endregion
    }
}