using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    internal interface IReportModelMapContainer
    {
        IReportModelMap GetMap<T>() where T : class;
    }
}
