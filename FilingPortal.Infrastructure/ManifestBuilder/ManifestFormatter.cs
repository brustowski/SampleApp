using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services.Rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilingPortal.Domain.DTOs.Rail.Manifest;

namespace FilingPortal.Infrastructure.ManifestBuilder
{
    /// <summary>
    /// Rail manifest formatter implementation
    /// </summary>
    internal class ManifestFormatter : IManifestFormatter
    {
        private readonly StringBuilder _sb = new StringBuilder();

        private void AddField(string displayName, string value)
        {
            _sb.Append($"<b>{displayName}</b>: {value}<br />");
        }

        private void AddLine(string value = "")
        {
            _sb.Append($"{value}<br />");
        }
        /// <summary>
        /// Formats model to formatted text
        /// </summary>
        /// <param name="manifest">Rail manifest</param>
        public string Format(Manifest manifest)
        {
            _sb.Clear();

            AddField("Importer", manifest.InvolvedEntities.FirstOrDefault(x=>x.EntityType.Equals("SH"))?.Name);
            AddField("Supplier", manifest.InvolvedEntities.FirstOrDefault(x => x.EntityType.Equals("CN"))?.Name);
            AddField("Description 1", manifest.C4Details.FirstOrDefault()?.Description);
            AddField("Equipment Initial", manifest.EquipmentDetails.FirstOrDefault()?.EquipmentNumber.Substring(0,4));
            AddField("Equipment Number", manifest.EquipmentDetails.FirstOrDefault()?.EquipmentNumber.Substring(4));
            AddField("Issuer Code", manifest.ManifestHeader.CarrierCode);
            AddField("Bill of Lading", manifest.BillOfLading.BillOfLadingNumber);
            AddField("Port of Unlading", manifest.ManifestHeader.DistrictPortOfUnlading);
            AddField("Manifest Units", manifest.BillOfLading.ManifestQuantityUnit);
            AddField("Weight", manifest.BillOfLading.Weight.ToString());
            AddField("Weight Unit", manifest.BillOfLading.WeightUnit);
            AddField("Reference Number 1", manifest.AdditionalReferences.FirstOrDefault()?.ReferenceNumber);

            return _sb.ToString();
        }

        /// <summary>
        /// Gets additional styles if any
        /// </summary>
        public string GetManifestStyles()
        {
            return string.Empty;
        }
    }
}