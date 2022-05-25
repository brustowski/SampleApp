namespace FilingPortal.Domain.Common.Reporting.Model
{
    public class Cell
    {
        public Cell(string content, int span = 1, CellStyle style = CellStyle.Default)
        {
            Content = content;
            Span = span;
            Style = style;
        }

        public string Content { get; private set; }
        public int Span { get; private set; }
        public CellStyle Style { get; private set; }
    }
}
