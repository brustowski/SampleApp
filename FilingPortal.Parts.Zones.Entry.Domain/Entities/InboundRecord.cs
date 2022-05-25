using System;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents the Inbound record
    /// </summary>
    public class InboundRecord : InboundEntityNew<FilingHeader>
    {
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets importer id
        /// </summary>
        public Guid? ImporterId { get; set; }
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
        public DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// Gets orr sets FIRMs code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Owner Ref / File No
        /// </summary>
        public string OwnerRef { get; set; }

        /// <summary>
        /// Gets or sets Filer Code
        /// </summary>
        public string FilerCode { get; set; }

        /// <summary>
        /// Gets or sets Entry No
        /// </summary>
        public string EntryNo { get; set; }

        /// <summary>
        /// Gets or sets Vessel Name
        /// </summary>
        public string VesselName { get; set; }

        /// <summary>
        /// Gets or sets if record is 7501 (update of existing CW record)
        /// </summary>
        public bool Is7501 { get; set; } = false;
        /// <summary>
        /// Gets or sets the binary content of the file
        /// </summary>
        public byte[] XmlFile { get; set; }
        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string XmlFileName { get; set; }
        /// <summary>
        /// Gets or sets XML type
        /// </summary>
        public string XmlType { get; set; }

        /// <summary>
        /// Gets or sets inbound note
        /// </summary>
        public virtual ICollection<InboundNote> Notes { get; set; }

        /// <summary>
        /// Gets or sets inbound lines
        /// </summary>
        public virtual ICollection<InboundLine> InboundLines { get; set; }
        /// <summary>
        /// Gets or sets documents list
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; }

        /// <summary>
        /// Gets autofile config Importer or Exporter
        /// </summary>
        public override string AutoFileConfigId => Importer?.ClientCode;

        /// <summary>
        /// Gets or sets inbound parsed data
        /// </summary>
        public virtual InboundParsedData ParsedData { get; set; }
    }
}