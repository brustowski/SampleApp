using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Service for generic Rail rule validation
    /// </summary>
    internal class RuleValidator<TRule> : IRuleValidator<TRule> where TRule : Entity
    {
        /// <summary>
        /// The Rail rule repository
        /// </summary>
        private readonly IRuleRepository<TRule> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidator{TRule}"/> class.
        /// </summary>
        /// <param name="repository">The Rail rule repository</param>
        public RuleValidator(IRuleRepository<TRule> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Determines whether the specified rule is duplicated
        /// </summary>
        /// <param name="rule">The rule</param>
        public bool IsDuplicate(TRule rule)
        {
            return _repository.IsDuplicate(rule);
        }
    }
}