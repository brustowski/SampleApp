using FilingPortal.Parts.Recon.Domain.Enums;
using Framework.Domain;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Represents the FTA Recon entity
    /// </summary>
    public class FtaRecon : AuditableEntity
    {
        #region CW data
        /// <summary>
        /// Gets or sets the Entry Line Origin Number
        /// </summary>
        public int? EntryLineOrigNo { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Entry Duty
        /// </summary>
        public decimal? ReconEntryDuty { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Entry Fee
        /// </summary>
        public decimal? ReconEntryFee { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Entry Line Duty
        /// </summary>
        public decimal? ReconEntryLineDuty { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Entry Line MPF
        /// </summary>
        public decimal? ReconEntryLineMpf { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Entry Line SPI
        /// </summary>
        public string ReconEntryLineSpi { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Issue Code
        /// </summary>
        public string ReconIssueCode { get; set; }
        
        /// <summary>
        /// Gets or sets the Recon Nf Job Number
        /// </summary>
        public string ReconNfJobNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the Import Declaration Entry Number
        /// </summary>
        public string ImportDeclarationEntryNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the Import Job Declaration Reference
        /// </summary>
        public string ImportJobDeclarationReference { get; set; }
        
        /// <summary>
        /// Gets or sets the Declaration FTA Recon
        /// </summary>
        public string DeclarationFtaRecon { get; set; }
        #endregion
        
        #region Client Data
        /// <summary>
        /// Gets or sets the FTA Eligibility
        /// </summary>
        public string FtaEligibility { get; set; }

        /// <summary>
        /// Gets or sets the client note
        /// </summary>
        public string ClientNote { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public int Status { get; set; } = (int)FtaReconStatusValue.Open;

        /// <summary>
        /// Gets or sets the Inbound record
        /// </summary>
        public virtual InboundRecord Inbound { get; set; }
        #endregion
    }
}
