using System.Data.Entity;
using Framework.DataLayer.Interceptors;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    /// <summary>
    /// DB configuration
    /// </summary>
    public class FpConfiguration : DbConfiguration
    {
        /// <summary>
        /// initialize a new instance of the <see cref="FpConfiguration"/> class;
        /// </summary>
        public FpConfiguration()
        {
            AddInterceptor(new StringTrimmerInterceptor());
        }
    }
}
