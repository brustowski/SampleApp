using FilingPortal.Domain.Services.GridExport.Formatters;
using System;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    internal interface IDefaultFormattersRegistry
    {
        IValueFormatter Get(Type type);
    }
}
