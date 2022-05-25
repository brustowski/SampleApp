using AutoMapper;
using FilingPortal.Domain.DTOs.Rail.Manifest;
using FilingPortal.Domain.Entities.Rail;
using System.Collections.Generic;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Provides method to convert Rail Broker Download Parsed record to the Manifest object
    /// </summary>
    public class RailBdParsedToManifestConverter : ITypeConverter<RailBdParsed, Manifest>
    {
        /// <summary>Performs conversion from source to destination type</summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public Manifest Convert(RailBdParsed source, Manifest destination, ResolutionContext context)
        {
            var manifest = new Manifest
            {
                ManifestHeader = GetManifestHeader(source),
                BillOfLading = GetBillOfLading(source),
                AdditionalReferences = GetAdditionalReferences(source),
                InvolvedEntities = GetInvolvedEntities(source),
                EquipmentDetails = GetEquipmentDetails(source),
                C4Details = GetC4Details(source),
                MarksAndNumbers = GetMarksAndNumbers(source),
                HazardousMaterials = GetHazardousMaterials(source),
                TariffDetails = GetTariffDetails(source)
            };
            return manifest;
        }

        private static ManifestHeader GetManifestHeader(RailBdParsed source)
        {
            return new ManifestHeader
            {
                CarrierCode = source.IssuerCode,
                CarrierAssignedBatchNumber = source.IssuerCode + source.BillOfLading,
                DistrictPortOfUnlading = source.PortOfUnlading
            };
        }

        private static BillOfLading GetBillOfLading(RailBdParsed source)
        {
            return new BillOfLading
            {
                BillOfLadingNumber = source.BillOfLading,
                IssuerScacCode = source.IssuerCode,
                ManifestQuantityUnit = source.ManifestUnits,
                Weight = int.TryParse(source.Weight, out var s) ? s : 0,
                WeightUnit = source.WeightUnit
            };
        }

        private static List<AdditionalReference> GetAdditionalReferences(RailBdParsed source)
        {
            return new List<AdditionalReference>
            {
                new AdditionalReference
                {
                    ReferenceNumber = source.ReferenceNumber1
                }
            };
        }

        private static List<InvolvedEntity> GetInvolvedEntities(RailBdParsed source)
        {
            return new List<InvolvedEntity>
            {
                new InvolvedEntity
                {
                    Name = source.Importer,
                    EntityType = "SH"
                },
                new InvolvedEntity
                {
                    Name = source.Supplier,
                    EntityType = "CN"
                }
            };
        }

        private static List<EquipmentDetail> GetEquipmentDetails(RailBdParsed source)
        {
            return new List<EquipmentDetail>
            {
                new EquipmentDetail
                {
                    EquipmentNumber = source.EquipmentInitial + source.EquipmentNumber
                }
            };
        }

        private static List<C4Detail> GetC4Details(RailBdParsed source)
        {
            return new List<C4Detail>
            {
                new C4Detail
                {
                    Description = source.Description1
                }
            };
        }

        private static List<string> GetMarksAndNumbers(RailBdParsed source)
        {
            return new List<string>
            {
                "NO MARKS OR NUMBERS"
            };
        }

        private static List<HazardousMaterial> GetHazardousMaterials(RailBdParsed source)
        {
            return new List<HazardousMaterial>();
        }

        private static List<TariffDetail> GetTariffDetails(RailBdParsed source)
        {
            return new List<TariffDetail>();
        }
    }
}
