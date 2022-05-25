using Framework.Domain;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Interface for service validating generic Rail rule
    /// </summary>
    public interface IRuleValidator<TRule> where TRule:Entity
    {
        /// <summary>
        /// Determines whether the specified rule is duplicated
        /// </summary>
        /// <param name="rule">The rule</param>
        bool IsDuplicate(TRule rule);      
    }
}