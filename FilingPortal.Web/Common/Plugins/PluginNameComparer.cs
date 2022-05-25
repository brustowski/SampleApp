using System.Collections.Generic;

namespace FilingPortal.Web.Common.Plugins
{
    /// <summary>
    /// Ensures loading of plugins in correct order
    /// </summary>
    public class PluginNameComparer : IComparer<string>
    {
        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(string x, string y)
        {
            var xValue = getValue(x);
            var yValue = getValue(y);

            return xValue == yValue ? 0 : xValue < yValue ? -1 : 1;
        }

        private int getValue(string x)
        {
            if (x == null) return 0;
            x = x.ToLower();
            if (x.EndsWith("domain.dll")) return 1;
            if (x.EndsWith("datalayer.dll")) return 2;
            if (x.EndsWith("web.dll")) return 3;
            return 0;
        }
    }
}