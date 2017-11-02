using System.Collections.Generic;
using System.Linq;

namespace GCDConsoleLib.Common.Extensons
{
    public static class ListExtensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : System.ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
