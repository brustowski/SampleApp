using System;
using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Implements base synchronizable model. All models that are synchronized with CargoWise should be inherited from this class
    /// </summary>
    public abstract class BaseSyncEntity: Entity
    {
        /// <summary>
        /// Gets or sets the record's last updated time
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }
    }
}
