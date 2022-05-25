using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Defines the Generic Rule Repository
    /// </summary>
    /// <typeparam name="TRuleEntity">Rule type</typeparam>
    public interface IRuleRepository<TRuleEntity> : ISearchRepository, IRepository<TRuleEntity> where TRuleEntity : Entity
    {
        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The rule to be checked</param>
        bool IsDuplicate(TRuleEntity rule);
        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        int GetId(TRuleEntity rule);
        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        bool IsExist(int id);
    }
}
