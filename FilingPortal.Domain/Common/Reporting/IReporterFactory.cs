namespace FilingPortal.Domain.Common.Reporting
{
    public interface IReporterFactory
    {
        IReporter Create(string filename);
        IReporter Create(string filename, string sheetName);
    }
}
