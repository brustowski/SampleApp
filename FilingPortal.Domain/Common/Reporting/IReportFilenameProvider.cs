namespace FilingPortal.Domain.Common.Reporting
{
    public interface IReportFilenameProvider
    {
        string GetFilenameInFileStorage(string filename, string baseFolder);
    }
}
