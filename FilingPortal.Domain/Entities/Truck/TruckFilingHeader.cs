using FilingPortal.Domain.Enums;
using Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{
    /// <summary>
    /// Defines the <see cref="TruckFilingHeader" /> - model for uniting truck records for filing
    /// </summary>
    public class TruckFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets the Truck Documents
        /// </summary>
        public virtual ICollection<TruckDocument> TruckDocuments { get; set; } = new List<TruckDocument>();

        /// <summary>
        /// Gets or sets the TruckInbounds records, united by this model
        /// </summary>
        public virtual ICollection<TruckInbound> TruckInbounds { get; set; } = new List<TruckInbound>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<TruckDocument> documents)
        {
            foreach (TruckDocument railDocument in documents)
                TruckDocuments.Add(railDocument);
        }

        /// <summary>
        /// Adds the truck inbound records
        /// </summary>
        /// <param name="truckInbounds">The truckInbounds<see cref="IEnumerable{TruckInbound}"/></param>
        public void AddTruckInbounds(IEnumerable<TruckInbound> truckInbounds)
        {
            foreach (TruckInbound truckInbound in truckInbounds)
            {
                TruckInbounds.Add(truckInbound);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => TruckInbounds.Select(x => x.Id);
    }
}
