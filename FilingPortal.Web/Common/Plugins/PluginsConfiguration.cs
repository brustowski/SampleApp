using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FilingPortal.Web.Common.Plugins
{
    /// <summary>
    /// Get assemblies information
    /// </summary>
    public static class PluginsConfiguration
    {
        /// <summary>
        /// Gets assemblies list
        /// </summary>
        public static Assembly[] GetAssemblies(string overridePath = null)
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = overridePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Plugins");

            if (Directory.Exists(path))
            {
                foreach (string dll in Directory.EnumerateFiles(path)
                    .Where(x => x.EndsWith(".dll"))
                    .OrderBy(x => x, new PluginNameComparer()))
                    allAssemblies.Add(Assembly.LoadFrom(dll));
            }

            return allAssemblies.ToArray();
        }


    }
}