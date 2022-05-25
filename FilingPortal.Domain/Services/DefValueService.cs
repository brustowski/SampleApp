using System;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Provides methods for creating, updating and deleting DefValue records
    /// </summary>
    public class DefValueService<TDefValue, TSection, TTable> : IDefValueService<TDefValue>
    where TDefValue : BaseDefValueWithSection<TSection>
    where TSection : BaseSection
    where TTable : BaseTable
    {
        /// <summary>
        /// The DefValues repository
        /// </summary>
        private readonly IDefValueRepository<TDefValue> _repository;

        /// <summary>
        /// The repository for Table repository
        /// </summary>
        private readonly ITablesRepository<TTable> _tableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefValueService{TDefValue, TSection, TTable}" /> class
        /// </summary>
        /// <param name="repository">The DefValues repository</param>
        /// <param name="tableRepository">The table repository</param>
        public DefValueService(IDefValueRepository<TDefValue> repository
            , ITablesRepository<TTable> tableRepository)
        {
            _repository = repository;
            _tableRepository = tableRepository;
        }

        /// <summary>
        /// Add a new record
        /// </summary>
        /// <param name="defValue">New value to add</param>
        /// <param name="tableName">Corresponding table name</param>
        /// <param name="value">Default value for this form param</param>
        public void Create(TDefValue defValue, string tableName, string value)
        {
            try
            {
                int sectionId = _tableRepository.GetSectionIdByTableName(tableName);
                defValue.SectionId = sectionId;
                _repository.Add(defValue);
                _repository.UpdateColumnConstraint(defValue, value);
                _repository.Save();
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.RuleCommonAddError);
                throw;
            }
        }

        /// <summary>
        /// Updates the record
        /// </summary>
        /// <param name="defValue">New value to add</param>
        /// <param name="tableName">Corresponding table name</param>
        /// <param name="value">Default value for this form param</param>
        public OperationResult Update(TDefValue defValue, string tableName, string value)
        {
            try
            {
                var result = new OperationResult();
                if (_repository.IsExist(defValue.Id))
                {
                    int sectionId = _tableRepository.GetSectionIdByTableName(tableName);
                    defValue.SectionId = sectionId;
                    _repository.Update(defValue);
                    _repository.UpdateColumnConstraint(defValue, value);
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
        /// <param name="id">Identifier of the Rule to delete</param>
        public OperationResult Delete(int id)
        {
            try
            {
                var result = new OperationResult();
                if (_repository.IsExist(id))
                {
                    _repository.DeleteById(id);
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
