using System;
using FilingPortal.Domain.Infrastructure.Helpers;

namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal class DateTimeFormatter : ValueFormatter<DateTime>
    {
        protected override string Format(DateTime value)
        {
            return value.ToUsFormat();
        }
    }
}