using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    using System;
    using System.Collections.Generic;
    using FilingPortal.Domain.Enums;
    using Framework.Infrastructure.Extensions;

    /// <summary>
    /// Represents Rail Inbound Read model Record Item
    /// </summary>
    public class RailInboundReadModel : InboundReadModelOld
    {
        /// <summary>
        /// Gets or sets the Rail Inbound Description 1
        /// </summary>
        public string BdParsedDescription1 { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Importer
        /// </summary>
        public string BdParsedImporterConsignee { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Supplier
        /// </summary>
        public string BdParsedSupplier { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Number
        /// </summary>
        public string BOLNumber { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Container Number
        /// </summary>
        public string ContainerNumber { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Filing Status Title
        /// </summary>
        public string FilingStatusTitle { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound HTS
        /// </summary>
        public string HTS { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Issuer Code
        /// </summary>
        public string IssuerCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Rail Inbound marked as Duplicated
        /// </summary>
        public bool IsDuplicated { get; set; }

        /// <summary>
        /// Gets the value indicating whether Rail Inbound record marked as Archived
        /// </summary>
        public bool IsArchived => CreatedDate < DateTime.Now.AddDays(-60).Date;

        /// <summary>
        /// Gets or sets the Rail Inbound Manifest Record Id
        /// </summary>
        public int? ManifestRecordId { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Mapping Status Title
        /// </summary>
        public string MappingStatusTitle { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Port Code
        /// </summary>
        public string PortCode { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Rule Port Port
        /// </summary>
        public string RulePortPort { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the Rail Inbound Train Number
        /// </summary>
        public string TrainNumber { get; set; }
        /// <summary>
        /// Gets or sets destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Determines whether this record can be restored
        /// </summary>
        public bool CanBeRestored()
        {
            return IsDeleted;
        }

        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public override bool CanBeEdited()
        {
            if (IsDeleted || IsArchived || IsDuplicated)
            {
                return false;
            }

            if (FilingStatus == Parts.Common.Domain.Enums.FilingStatus.Filed || FilingStatus == Parts.Common.Domain.Enums.FilingStatus.InProgress)
            {
                return false; // Should never modify filed or in progress
            }

            if (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.Error || FilingStatus == Parts.Common.Domain.Enums.FilingStatus.Error)
            {
                return true;
            }

            // Filing status is Open

            if (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.InReview) // Working with saved item
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this model can be filed
        /// </summary>
        public override bool CanBeFiled()
        {
            return !FilingHeaderId.HasValue && !(IsDeleted
                || IsDuplicated
                || IsArchived
                || (MappingStatus.HasValue && MappingStatus != Parts.Common.Domain.Enums.MappingStatus.Open)
                );
        }

        /// <summary>
        /// Determines whether this model can be selected
        /// </summary>
        public override bool CanBeSelected()
        {
            return !(IsDuplicated || IsArchived) && HasAllRequiredRules;
        }

        /// <summary>
        /// Gets Rail Inbound record Status string value
        /// </summary>
        public string Status
        {
            get
            {
                var result = new List<string>();
                if (IsDeleted) result.Add(RailInboundRecordStatus.Deleted.GetDescription());
                if (IsDuplicated) result.Add(RailInboundRecordStatus.Duplicates.GetDescription());
                if (IsArchived) result.Add(RailInboundRecordStatus.Archived.GetDescription());
                if (result.Count == 0) result.Add(RailInboundRecordStatus.Open.GetDescription());

                return string.Join("; ", result);
            }
        }

        /// <summary>
        /// Returns if this record can be edited
        /// </summary>
        public bool CanEditInitialRecord()
        {
            return !ManifestRecordId.HasValue && (!FilingHeaderId.HasValue || (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.Open && FilingStatus == Parts.Common.Domain.Enums.FilingStatus.Open));
        }
    }
}
