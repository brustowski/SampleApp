using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;

namespace FilingPortal.Web.Tests.Common
{
    internal class FakeResourceRequestor : IPermissionChecker
    {
        private readonly bool _result;
        public FakeResourceRequestor(bool result = true)
        {
            _result = result;
        }
        public bool HasPermissions(IEnumerable<Permissions> permissions)
        {
            return _result;
        }

        /// <summary>
        /// Checks if user has specified permissions
        /// </summary>
        /// <param name="permissions">Numeric value of permission</param>
        public bool HasPermissions(IEnumerable<int> permissions)
        {
            return _result;
        }
    }
}
