 using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Zones.Entry.Domain.Reporting.Inbound
{
    /// <summary>
    /// Represents the Zones Type 06 Report Model
    /// </summary>
    internal class InboundReportModel
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }
        /// <summary>
        /// Gets or sets entry port
        /// </summary>
        public string EntryPort { get; set; }
        /// <summary>
        /// Gets or sets Arrival date
        /// </summary>
        public System.DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// Gets orr sets FIRMs code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Owner Ref / File No
        /// </summary>
        public string OwnerRef { get; set; }
       /// <summary>
        /// Gets or sets Entry No
        /// </summary>
        public string EntryNo { get; set; }

        /// <summary>
        /// Gets or sets Vessel Name
        /// </summary>
        public string VesselName { get; set; }
        
    }
}