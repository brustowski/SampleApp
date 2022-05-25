using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
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
        public SectionRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork) { }
    }
}