using FilingPortal.Domain.Common.OperationResult;
using Framework.Domain;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface describing Generic Rail Rule Service  
    /// </summary>
    public interface IRuleService<in TRule> where TRule : Entity
    {
        /// <summary>
        /// Add new rule record
        /// </summary>
        /// <param name="rule">Rule to add</param>
        void Create(TRule rule);

        /// <summary>
        /// Updates rule record
        /// </summary>
        /// <param name="rule">Rule to update</param>
        OperationResult Update(TRule rule);

        /// <summary>
        /// Delete rule record with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        OperationResult Delete(int ruleId);
    }
}
