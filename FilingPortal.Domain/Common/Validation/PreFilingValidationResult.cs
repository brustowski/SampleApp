using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common.Validation
{
    /// <summary>
    /// Represents the Pre-Filing record validation result
    /// </summary>
    [TsClass(IncludeNamespace = false)]
    public class PreFilingValidationResult
    {
        public int Id { get; set; }
        public bool IsValid => ErrorType == PreFilingValidationErrorType.None;
        public string Error { get; set; }
        public PreFilingValidationErrorType ErrorType { get; set; }
    }
}
