using System.Collections.Generic;

namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest
    /// </summary>
    public class Manifest
    {
        /// <summary>
        /// Gets or sets the Manifest Header
        /// </summary>
        public ManifestHeader ManifestHeader { get; set; }
        /// <summary>
        /// Gets or sets the Bill Of Lading
        /// </summary>
        public BillOfLading BillOfLading { get; set; }
        /// <summary>
        /// Gets or sets the Additional References
        /// </summary>
        public List<AdditionalReference> AdditionalReferences { get; set; }
        /// <summary>
        /// Gets or sets the Involved Entities
        /// </summary>
        public List<InvolvedEntity> InvolvedEntities { get; set; }
        /// <summary>
        /// Gets or sets the Equipment Details
        /// </summary>
        public List<EquipmentDetail> EquipmentDetails { get; set; }
        /// <summary>
        /// Gets or sets the Line Details
        /// </summary>
        public List<C4Detail> C4Details { get; set; }
        /// <summary>
        /// Gets or sets the Marks and Numbers
        /// </summary>
        public List<string> MarksAndNumbers { get; set; }
        /// <summary>
        /// Gets or sets the Hazardous Materials
        /// </summary>
        public List<HazardousMaterial> HazardousMaterials { get; set; }
        /// <summary>
        /// Gets or sets the Tariff Details
        /// </summary>
        public List<TariffDetail> TariffDetails { get; set; }
    }
}
