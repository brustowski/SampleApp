using System;
using System.IO;

namespace FilingPortal.Parts.Common.DataLayer.Helpers
{
    public class MigrationHelper
    {
        public static string GetResourcePath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources");
            string pathbin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "resources");
            if (Directory.Exists(pathbin))
                path = pathbin;
            return path;
        }
    }
}
