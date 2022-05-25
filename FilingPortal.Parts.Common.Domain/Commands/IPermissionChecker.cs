using System.Collections.Generic;

namespace FilingPortal.Parts.Common.Domain.Commands
{
    /// <summary>
    /// Defines method for Permission Check
    /// </summary>
    public interface IPermissionChecker
    {
        /// <summary>
        /// Checks if user has specified permissions
        /// </summary>
        /// <param name="permissions">Numeric value of permission</param>
        bool HasPermissions(IEnumerable<int> permissions);
    }
}
