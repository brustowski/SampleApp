using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services
{
    using FilingPortal.Domain.Common.OperationResult;
    using FilingPortal.Domain.Repositories;
    using Framework.Domain;
    using Framework.Infrastructure;
    using System;

    internal class RuleService<TRule> : IRuleService<TRule> where TRule : Entity
    {
        /// <summary>
        /// The repository for Rail Rule
        /// </summary>
        private readonly IRuleRepository<TRule> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleService{TRule}" /> class
        /// </summary>
        /// <param name="repository">Rail Rule repository</param>
        public RuleService(IRuleRepository<TRule> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Add new rule record
        /// </summary>
        /// <param name="rule">Rule to add</param>
        public void Create(TRule rule)
        {
            try
            {
                _repository.Add(rule);
                _repository.Save();
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.RuleCommonAddError);
                throw;
            }
        }

        /// <summary>
        /// Updates rule record
        /// </summary>
        /// <param name="rule">Rule to update</param>
        public OperationResult Update(TRule rule)
        {
            try
            {
                var result = new OperationResult();
                if (_repository.IsExist(rule.Id))
                {
                    _repository.Update(rule);
                    _repository.Save();
                }
                else
                {
                    result.AddErrorMessage(ErrorMessages.RuleDoesNotExist);
                }
                return result;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.RuleCommonUpdateError);
                throw;
            }
        }

        /// <summary>
        /// Delete rule record with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        public OperationResult Delete(int ruleId)
        {
            try
            {
                var result = new OperationResult();
                if (_repository.IsExist(ruleId))
                {
                    _repository.DeleteById(ruleId);
                    _repository.Save();
                }
                else
                {
                    result.AddErrorMessage(ErrorMessages.RuleDoesNotExist);
                }
                return result;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.RuleCommonDeleteError);
                throw;
            }
        }
    }
}
