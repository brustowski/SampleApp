using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Common.OperationResult
{
    /// <summary>
    /// Provides result of the operation with additional error data
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Operation result
        /// </summary>
        public bool IsValid => !Errors.Any();
        
        /// <summary>
        /// Collection of errors
        /// </summary>
        public ICollection<string> Errors { get; private set; }

        public OperationResult()
        {
            Errors = new HashSet<string>();
        }

        /// <summary>
        /// Add error the errors collection and set result status to Error
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        public void AddErrorMessage(string errorMessage)
        {
            Errors.Add(errorMessage);
        }

    }
}
