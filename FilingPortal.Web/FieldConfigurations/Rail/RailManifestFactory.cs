using System.Collections.Generic;
using FilingPortal.Domain.DTOs;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.FieldConfigurations.Rail
{
    /// <summary>
    /// Class for Import Rail Manifest factory
    /// </summary>
    public class RailManifestFactory : IManifestFactory
    {
        /// <summary>
        /// Field Model builder
        /// </summary>
        private readonly IFieldConfigurationBuilder _builder;

        /// <summary>
        /// Creates new instance of the <see cref="RailManifestFactory"/>
        /// </summary>
        /// <param name="builder">Field Model builder</param>
        public RailManifestFactory(IFieldConfigurationBuilder builder) => _builder = builder;

        /// <summary>
        /// Creates field set from manifest object
        /// </summary>
        /// <param name="manifest">Inbound manifest to get information from</param>
        public ManifestModel CreateFrom(Manifest manifest)
        {
            return new ManifestModel
            {
                ManifestText = manifest.ManifestText,
                Fields = new List<FieldModel> {
                    _builder.Create("Consignee").DefaultValue(manifest.Consignee).Build(),
                    _builder.Create("Supplier").DefaultValue(manifest.Supplier).Build(),
                    _builder.Create("Importer").DefaultValue(manifest.Importer).Separator().Build(),
                    _builder.Create("Description 1").DefaultValue(manifest.Description1).Long().Separator().Multiline().Build(),
                    _builder.Create("Equipment Initial").DefaultValue(manifest.EquipmentInitial).Build(),
                    _builder.Create("Equipment Number").DefaultValue(manifest.EquipmentNumber).Build(),
                    _builder.Create("Issuer Code").DefaultValue(manifest.IssuerCode).Build(),
                    _builder.Create("Bill of Lading").DefaultValue(manifest.BillofLading).Build(),
                    _builder.Create("Port of Unlading").DefaultValue(manifest.PortofUnlading).Build(),
                    _builder.Create("Destination").DefaultValue(manifest.Destination).Build(),
                    _builder.Create("Weight").DefaultValue(manifest.Weight).Build(),
                    _builder.Create("Weight Unit").DefaultValue(manifest.WeightUnit).Build(),
                    _builder.Create("Reference Number 1").DefaultValue(manifest.ReferenceNumber1).Build(),
                    _builder.Create("Manifest Units").DefaultValue(manifest.ManifestUnits).Build(),
                }
            };
        }
       
    }
}
