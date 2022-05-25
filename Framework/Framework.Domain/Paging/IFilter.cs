using System.Collections.Generic;

namespace Framework.Domain.Paging
{
    public interface IFilter
    {
        string FieldName { get; set; }
        
        List<LookupItem> Values { get; set; }

        string Operand { get; set; }
    }
}
