using FilingPortal.Domain.Common.Reporting;
using System;
using System.IO;

namespace FilingPortal.Infrastructure.Report
{
    internal class ReportFilenameProvider : IReportFilenameProvider
    {
        private readonly TimeSpan _maxAgeForOldFiles = new TimeSpan(hours: -1, minutes: 0, seconds: 0);

        public ReportFilenameProvider() { }

        public string GetFilenameInFileStorage(string filename, string baseFolder)
        {
            string workFilename = PrepareWorkFile(filename, baseFolder);
            return workFilename;
        }

        private string PrepareWorkFile(string filename, string baseFolder)
        {
            var absoluteBaseFolderPath = CreateWorkDirectory(baseFolder);

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            var filenameExtension = Path.GetExtension(filename);
            var workFilename = $"{filenameWithoutExtension}_{Guid.NewGuid()}{filenameExtension}";
            var fullFilePath = Path.Combine(absoluteBaseFolderPath, workFilename);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            return fullFilePath;
        }

        private string CreateWorkDirectory(string baseFolder)
        {
            var basePath = Path.GetTempPath();

            var absoluteBaseFolderPath = Path.Combine(basePath, baseFolder);

            if (!Directory.Exists(absoluteBaseFolderPath))
            {
                Directory.CreateDirectory(absoluteBaseFolderPath);
            }

            ClearDirectory(absoluteBaseFolderPath);

            return absoluteBaseFolderPath;
        }

        private void ClearDirectory(string absoluteBaseFolderPath)
        {
            foreach (var fileName in Directory.GetFiles(absoluteBaseFolderPath))
            {
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.LastWriteTime < DateTime.Now.Add(_maxAgeForOldFiles))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception) // ignored
                    {
                    }
                }
            }
        }

    }
}
