using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="BaseDefValueReadModel"/>
    /// </summary>
    public interface IDefValuesReadModelRepository<TDefValueModel> :
        IAgileConfiguration<TDefValueModel>
        , ISearchRepository<TDefValueModel>
    where TDefValueModel : BaseDefValueReadModel
    {

    }
}