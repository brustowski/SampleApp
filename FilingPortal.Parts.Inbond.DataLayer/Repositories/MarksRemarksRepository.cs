using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="MarksRemarksTemplate"/>
    /// </summary>
    public class MarksRemarksRepository : SearchRepository<MarksRemarksTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarksRemarksRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public MarksRemarksRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork)
        { }
    }
}
