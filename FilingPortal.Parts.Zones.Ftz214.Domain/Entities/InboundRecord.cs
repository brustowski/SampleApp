using System;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Entities
{
    /// <summary>
    /// Represents the Inbound record
    /// </summary>
    public class InboundRecord : InboundEntityNew<FilingHeader>
    {
        /// <summary>
        /// Gets or sets the applicant
        /// </summary>
        public Guid? ApplicantId { get; set; }
        /// <summary>
        /// Gets or sets the applicant
        /// </summary>
        public Client Applicant { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }
        /// <summary>
        /// Gets or sets the FTZ Operator
        /// </summary>
        public Guid? FtzOperatorId { get; set; }
        /// <summary>
        /// Gets or sets the FTZ Operator
        /// </summary>
        public Client FtzOperator { get; set; }
        /// <summary>
        /// Gets or sets the Zone Id
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Gets or sets the Admin.Type ???
        /// </summary>
        public string AdmissionType { get; set; }

        /// <summary>
        /// Gets or sets the InboundParsedData
        /// </summary>
        public virtual InboundParsedData InboundParsedData { get; set; }

        /// <summary>
        /// Gets or sets inbound lines
        /// </summary>
        public virtual ICollection<InboundParsedLine> InboundParsedLines { get; set; }
        /// <summary>
        /// Gets or sets documents list
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; }

        /// <summary>
        /// Gets autofile config Importer or Exporter
        /// </summary>
        public override string AutoFileConfigId { get; }

        #region XML File
        /// <summary>
        /// Gets or sets the binary content of the file
        /// </summary>
        public byte[] XmlFile { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string XmlFileName { get; set; }
        #endregion
    }
}