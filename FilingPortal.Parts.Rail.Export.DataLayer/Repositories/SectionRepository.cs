using FilingPortal.Parts.Rail.Export.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
{
    /// <summary>
    /// Represents repository of the Section entity
    /// </summary>
    class SectionRepository : SearchRepository<Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public SectionRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }
    }
}