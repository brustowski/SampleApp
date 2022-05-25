using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of the <see cref="RailDefValues"/>
    /// </summary>
    public class RailDefValuesRepository : BaseDefValuesRepository<RailDefValues, RailSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
