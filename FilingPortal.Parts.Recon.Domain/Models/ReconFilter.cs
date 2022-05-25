using System;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Domain.Models
{
    /// <summary>
    /// Describes a model with user filters for recon report
    /// </summary>
    public class ReconFilter
    {
        /// <summary>
        /// Gets or sets the  from date
        /// </summary>
        public DateTime? ImportFrom { get; set; }
        /// <summary>
        ///Gets or sets the  Import to date
        /// </summary>
        public DateTime? ImportTo { get; set; }
        /// <summary>
        /// Gets or sets the Recon Issue
        /// </summary>
        public IEnumerable<string> ReconIssue { get; set; }
        /// <summary>
        /// Gets or sets the NAFTA recon
        /// </summary>
        public string NaftaRecon { get; set; }
        /// <summary>
        /// Gets or sets the Importer full name
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets the Preliminary Statement Date From
        /// </summary>
        public DateTime? PreliminaryStatementDateFrom { get; set; }
        /// <summary>
        /// Gets or sets the Preliminary Statement Date To
        /// </summary>
        public DateTime? PreliminaryStatementDateTo { get; set; }
        /// <summary>
        /// Gets or sets the entry number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets the FTA Recon Filing
        /// </summary>
        public IEnumerable<string> FtaReconFiling { get; set; }
        /// <summary>
        /// Gets or sets the Recon Flagged and filed state
        /// </summary>
        public IEnumerable<int> ReconFlaggedFiled { get; set; }
    }
}
