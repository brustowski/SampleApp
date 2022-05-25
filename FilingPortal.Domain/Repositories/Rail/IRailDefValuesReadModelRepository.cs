using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Interface for repository of <see cref="RailDefValuesManualReadModel"/>
    /// </summary>
    public interface IRailDefValuesReadModelRepository : IRepositoryWithTypeId<RailDefValuesReadModel, int>
        , IAgileConfiguration<RailDefValuesReadModel>
        , ISearchRepository
    {

    }
}