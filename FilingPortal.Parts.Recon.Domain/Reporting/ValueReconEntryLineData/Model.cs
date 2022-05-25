namespace FilingPortal.Parts.Recon.Domain.Reporting.ValueReconEntryLineData
{
    /// <summary>
    /// Represents the Line Data Recon Entry Report Model
    /// </summary>
    internal class Model
    {
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Import Entry Ref
        /// </summary>
        public string ImportEntryRef { get; set; }
        /// <summary>
        /// Gets or sets the Origin Line Number
        /// </summary>
        public string OriginLineNumber { get; set; }
        /// <summary>
        /// Gets or sets the Origin Customs Value
        /// </summary>
        public decimal? OriginCustomsValue { get; set; }
        /// <summary>
        /// Gets or sets the Org
        /// </summary>
        public string Org { get; set; }
        /// <summary>
        /// Gets or sets the Origin Tariff
        /// </summary>
        public string OriginTariff { get; set; }
        /// <summary>
        /// Gets or sets the Origin Prov Prog Tariff
        /// </summary>
        public string OriginProvProgTariff { get; set; }
        /// <summary>
        /// Gets or sets the Origin Qty 1
        /// </summary>
        public decimal? OriginQty1 { get; set; }
        /// <summary>
        /// Gets or sets the Origin UQ 1
        /// </summary>
        public string OriginUq1 { get; set; }
        /// <summary>
        /// Gets or sets the Origin SPI
        /// </summary>
        public string OriginSpi { get; set; }
        /// <summary>
        /// Gets or sets the Origin Duty
        /// </summary>
        public decimal? OriginDuty { get; set; }
        /// <summary>
        /// Gets or sets the Origin HMf Amount
        /// </summary>
        public decimal? OriginHmfAmount { get; set; }
        /// <summary>
        /// Gets or sets the Origin MPF Amount
        /// </summary>
        public decimal? OriginMpfAmount { get; set; }
        /// <summary>
        /// Gets or sets the Recon Tariff
        /// </summary>
        public string ReconTariff { get; set; }
        /// <summary>
        /// Gets or sets the Recon Tariff Rec Prov Prog Tariff
        /// </summary>
        public string ReconTariffRecProvProgTariff { get; set; }
        /// <summary>
        /// Gets or sets the Recon Qty 1
        /// </summary>
        public decimal? ReconQty1 { get; set; }
        /// <summary>
        /// Gets or sets the Recon UQ 1
        /// </summary>
        public string ReconUq1 { get; set; }
        /// <summary>
        /// Gets or sets the Recon SPI
        /// </summary>
        public string ReconSpi { get; set; }
    }
}