using AutoMapper;
using FilingPortal.Domain.DTOs.Rail.Manifest;
using FilingPortal.Domain.Entities.Rail;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Provides method to convert Rail EDI message record to the Manifest object
    /// </summary>
    public class RailEdiMessageToManifestConverter : ITypeConverter<RailEdiMessage, Manifest>
    {
        private const string DateFormat = "MMddyy";

        /// <summary>Performs conversion from source to destination type</summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public Manifest Convert(RailEdiMessage source, Manifest destination, ResolutionContext context)
        {
            List<string> message = source.EmMessageText.SplitBySize(80).ToList();

            var manifest = new Manifest
            {
                ManifestHeader = GetManifestHeader(message),
                BillOfLading = GetBillOfLading(message),
                AdditionalReferences = GetAdditionalReferences(message),
                InvolvedEntities = GetInvolvedEntities(message),
                EquipmentDetails = GetEquipmentDetails(message),
                C4Details = GetC4Details(message),
                MarksAndNumbers = GetMarksAndNumbers(message),
                HazardousMaterials = GetHazardousMaterials(message),
                TariffDetails = GetTariffDetails(message)
            };
            return manifest;
        }

        private static ManifestHeader GetManifestHeader(List<string> messageLines)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var m1Line = messageLines.FirstOrDefault(x => x.StartsWith("1M", StringComparison.CurrentCultureIgnoreCase));
            var m2Line = messageLines.FirstOrDefault(x => x.StartsWith("2M", StringComparison.CurrentCultureIgnoreCase));
            var p1Line = messageLines.FirstOrDefault(x => x.StartsWith("1P", StringComparison.CurrentCultureIgnoreCase));
            var header = new ManifestHeader
            {
                CarrierCode = m1Line?.Substring(2, 4).Trim(),
                CountryCodeOfImportingConveyance = m1Line?.Substring(8, 2).Trim(),
                ImportingConveyanceName = m1Line?.Substring(10, 23).Trim(),
                ManifestSequenceNumber = int.TryParse(m1Line?.Substring(43, 6), out var sn) ? sn : 0,
                ManifestTypeCode = m1Line?.Substring(57, 1).Trim(),
                CarrierAssignedBatchNumber = m2Line?.Substring(2, 30).Trim(),
                DistrictPortOfUnlading = p1Line?.Substring(2, 4).Trim(),
                EstimatedArrivalDateTime =
                    DateTime.TryParseExact(p1Line?.Substring(6, 6), DateFormat, provider, DateTimeStyles.None, out DateTime dt)
                    ? dt : DateTime.MinValue
            };
            return header;
        }

        private static BillOfLading GetBillOfLading(List<string> messageLines)
        {
            var b1Line = messageLines.FirstOrDefault(x => x.StartsWith("1B", StringComparison.CurrentCultureIgnoreCase));
            var b2Line = messageLines.FirstOrDefault(x => x.StartsWith("2B", StringComparison.CurrentCultureIgnoreCase));
            var j1Line = messageLines.FirstOrDefault(x => x.StartsWith("1J", StringComparison.CurrentCultureIgnoreCase));
            return new BillOfLading
            {
                BillOfLadingNumber = b1Line?.Substring(2, 12).Trim(),
                IssuerScacCode = j1Line?.Substring(2, 4).Trim(),
                ForeignPortOfLading = b1Line?.Substring(14, 5).Trim(),
                ManifestQuantity = int.TryParse(b1Line?.Substring(19, 10), out var mq) ? mq : 0,
                ManifestQuantityUnit = b1Line?.Substring(29, 3).Trim(),
                Weight = int.TryParse(b1Line?.Substring(34, 10), out var w) ? w : 0,
                WeightUnit = b1Line?.Substring(44, 3).Trim(),
                BillOfLadingStatus = int.TryParse(b1Line?.Substring(46, 1), out var s) ? s : 0,
                MasterInBondIndicator = string.IsNullOrWhiteSpace(b1Line?.Substring(47, 1))
                    ? "N"
                    : b1Line?.Substring(47, 1),
                Measurement = int.TryParse(b2Line?.Substring(2, 10), out var result) ? result : 0,
                MeasurementUnit = b2Line?.Substring(12, 2).Trim(),
                PlaceOfReceiptByPreCarrier = b2Line?.Substring(14, 17).Trim()
            };
        }

        private static List<AdditionalReference> GetAdditionalReferences(List<string> messageLines)
        {
            IEnumerable<string> b4Lines = messageLines.Where(x => x.StartsWith("4B", StringComparison.InvariantCultureIgnoreCase));
            return b4Lines.Select(b4Line => new AdditionalReference
            {
                ReferenceType = b4Line.Substring(2, 3).Trim(),
                ReferenceNumber = b4Line.Substring(5, 30).Trim()
            }).ToList();
        }

        private static List<InvolvedEntity> GetInvolvedEntities(List<string> messageLines)
        {
            var involvedEntities = new List<InvolvedEntity>();
            InvolvedEntity entity = null;
            foreach (var mes in messageLines)
            {
                if (entity == null && !mes.StartsWith("0N", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (mes.StartsWith("0N", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (entity != null)
                    {
                        involvedEntities.Add(entity);
                    }

                    entity = new InvolvedEntity
                    {
                        EntityType = mes.Substring(2, 3).Trim(),
                        Name = mes.Substring(5, 35).Trim(),
                        CodeQualifierId = mes.Substring(40, 2).Trim(),
                        CodeId = mes.Substring(42, 17).Trim()
                    };
                };

                if (mes.StartsWith("2N", StringComparison.CurrentCultureIgnoreCase))
                {
                    entity.Address += $"{mes.Substring(2, 35).Trim()} ";
                    entity.Address += $"{mes.Substring(37, 35).Trim()} ";
                }

                if (mes.StartsWith("3N", StringComparison.CurrentCultureIgnoreCase))
                {
                    entity.Address += $"{mes.Substring(2, 19).Trim()} ";
                    entity.Address += $"{mes.Substring(21, 2).Trim()} ";
                    entity.Address += $"{mes.Substring(23, 9).Trim()} ";
                    entity.Address += $"{mes.Substring(32, 2).Trim()} ";
                }

                if (mes.StartsWith("4N", StringComparison.CurrentCultureIgnoreCase))
                {
                    entity.ContactName = mes.Substring(2, 23);
                }
            }

            if (entity != null)
            {
                involvedEntities.Add(entity);
            }
            return involvedEntities;
        }

        private static List<EquipmentDetail> GetEquipmentDetails(List<string> messageLines)
        {
            IEnumerable<string> b4Lines = messageLines.Where(x => x.StartsWith("1C", StringComparison.InvariantCultureIgnoreCase));
            return b4Lines.Select(b4Line => new EquipmentDetail
            {
                EquipmentNumber = b4Line.Substring(2, 4).Trim() + b4Line.Substring(6, 10).Trim(),
                SealNumber1 = b4Line.Substring(16, 15).Trim(),
                SealNumber2 = b4Line.Substring(31, 15).Trim(),
                ContainerEquipmentTypeCode = b4Line.Substring(46, 2).Trim(),
                LoadEmptyStatusCode = b4Line.Substring(73, 1).Trim()
            }).ToList();
        }

        private static List<C4Detail> GetC4Details(List<string> messageLines)
        {
            IEnumerable<string> d1Lines = messageLines.Where(x => x.StartsWith("1D", StringComparison.InvariantCultureIgnoreCase));
            return d1Lines.Select(d1Line => new C4Detail
            {
                PieceCount = d1Line.Substring(2, 10).Trim(),
                Description = d1Line.Substring(12, 45).Trim(),
                C4Number = d1Line.Substring(57, 14).Trim(),
                Country = d1Line.Substring(74, 2).Trim(),
                PieceCountUnit = d1Line.Substring(71, 3).Trim()
            }).ToList();
        }

        private static List<string> GetMarksAndNumbers(List<string> messageLines)
        {
            IEnumerable<string> d2Lines = messageLines.Where(x => x.StartsWith("2D", StringComparison.InvariantCultureIgnoreCase));
            List<string> marksNumbers = d2Lines
                .Select(d2Line => d2Line.Substring(2, 45).Trim())
                .Where(value => !string.IsNullOrWhiteSpace(value) && !value.StartsWith("."))
                .ToList();
            if (marksNumbers.Count == 0)
            {
                marksNumbers.Add("No Marks or Numbers");
            }
            return marksNumbers;
        }

        private static List<HazardousMaterial> GetHazardousMaterials(List<string> messageLines)
        {
            IEnumerable<string> v1Lines = messageLines.Where(x => x.StartsWith("1V", StringComparison.InvariantCultureIgnoreCase));
            return v1Lines.Select(v1Line => new HazardousMaterial
            {
                Code = v1Line.Substring(2, 10).Trim(),
                Class = v1Line.Substring(12, 4).Trim(),
                CodeQualifier = v1Line.Substring(16, 1).Trim(),
                Description = v1Line.Substring(17, 30).Trim(),
                ContactNumber = v1Line.Substring(47, 24).Trim()
            }).ToList();
        }

        private static List<TariffDetail> GetTariffDetails(List<string> messageLines)
        {
            IEnumerable<string> d0Lines = messageLines.Where(x => x.StartsWith("0D", StringComparison.InvariantCultureIgnoreCase));
            return d0Lines.Select(d0Line => new TariffDetail
            {
                HarmonizedTariffNumber = d0Line.Substring(2, 11).Trim(),
                Value = d0Line.Substring(13, 8).Trim(),
                Weight = int.TryParse(d0Line.Substring(21, 10), out var result) ? result : 0,
                WeightUnit = d0Line.Substring(31, 2).Trim()
            }).ToList();
        }
    }
}
