using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Reporting.Model
{
    public class Row
    {
        public Row()
        {
            Cells = new List<Cell>();
        }

        public Row CreateCell(string content, int span = 1, CellStyle style = CellStyle.Default)
        {
            var cell = new Cell(content, span, style);
            Cells.Add(cell);
            return this;
        }

        public Row SkipNColumns(int span, CellStyle style = CellStyle.Default)
        {
            var cell = new Cell(string.Empty, span, style);
            Cells.Add(cell);
            return this;
        }

        public List<Cell> Cells { get; private set; }
    }
}
