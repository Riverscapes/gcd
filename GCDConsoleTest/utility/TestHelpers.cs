using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GCDConsoleLib.Tests
{
    /// <summary>
    /// Summary description for TestHelpers
    /// </summary>
    public struct TestHelpers
    {
        public static string AssemblyDir
        {
            get
            {
                var executingAssemblyFile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
                return Path.GetDirectoryName(executingAssemblyFile);
            }
        }
        public static string GetTestRootPath(string rName)
        {
            string[] dirs = new string[] { AssemblyDir, @"TestData", rName };
            return Path.Combine(dirs);
        }

        public static string GetTestRasterPath(string rName)
        {
            string[] dirs = new string[] { AssemblyDir, @"TestData\rasters", rName };
            return Path.Combine(dirs);
        }
        public static string GetTestVectorPath(string rName)
        {
            string[] dirs = new string[] { AssemblyDir, @"TestData\vectors", rName };
            return Path.Combine(dirs);
        }

    }
}
