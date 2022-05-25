using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Common.OperationResult
{
    /// <summary>
    /// Provides result of operation with additional error data and value
    /// </summary>
    /// <typeparam name="TValue">Type of the Operations result value</typeparam>
    public class OperationResultWithValue<TValue> : OperationResult
    {
        /// <summary>
        /// Operation result value
        /// </summary>
        public TValue Value { get; set; }

      
        public OperationResultWithValue(): base ()
        { }
        
    }
}
