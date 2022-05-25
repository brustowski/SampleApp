using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FilingPortal.Parts.Common.DataLayer.Conventions
{
    public class DatetimeConvention : Convention
    {
        public DatetimeConvention()
        {
            Properties<DateTime>()
                .Configure(config => config.HasColumnType("datetime"));
        }
    }
}