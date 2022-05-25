using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface describing Def Value Service  
    /// </summary>
    /// <typeparam name="TDefValue">Specific DefValue type</typeparam>
    public interface IDefValueService<in TDefValue> where TDefValue: BaseDefValue
    {
        /// <summary>
        /// Add a new record
        /// </summary>
        /// <param name="defValue">New value to add</param>
        /// <param name="tableName">Corresponding table name</param>
        /// <param name="value">Default value for this form param</param>
        void Create(TDefValue defValue, string tableName, string value);

        /// <summary>
        /// Updates the record
        /// </summary>
        /// <param name="defValue">New value to add</param>
        /// <param name="tableName">Corresponding table name</param>
        /// <param name="value">Default value for this form param</param>
        OperationResult Update(TDefValue defValue, string tableName, string value);

        /// <summary>
        /// Delete record with specified identifier
        /// </summary>
        /// <param name="id">Identifier of the record to delete</param>
        OperationResult Delete(int id);
    }
}
