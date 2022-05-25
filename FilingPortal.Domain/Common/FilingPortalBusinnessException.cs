using System;
using System.Runtime.Serialization;

namespace FilingPortal.Domain.Common
{
    public class FilingPortalBusinnessException : Exception
    {
        public FilingPortalBusinnessException(string message)
            : base(message)
        { }

        public FilingPortalBusinnessException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected FilingPortalBusinnessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
