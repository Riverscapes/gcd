using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class VectorTests
    {
        [TestMethod()]
        public void VectorTest()
        {
            string sFilepath = TestHelpers.GetTestVectorPath("StressTest.shp");
            Vector rVector = new Vector(sFilepath);
            Assert.IsTrue(rVector.Features.Count > 0);
            Assert.IsTrue(rVector.Fields.Count > 0);
            Assert.AreEqual(rVector.LayerName, "StressTest");
            Assert.AreEqual(rVector.FilePath, sFilepath);
            Assert.IsNotNull(rVector.Proj);
        }

        [TestMethod()]
        public void VectorCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Vector rVector = new Vector(TestHelpers.GetTestVectorPath("StressTest.shp"));
                rVector.Copy(Path.Combine(tmp.Name, "CopyShapefile.shp"));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shp")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.dbf")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.prj")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shx")));
            }
        }


        [TestMethod()]
        public void VectorDeleteTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                string sOrigPath = TestHelpers.GetTestVectorPath("StressTest.shp");
                string sDeletePath = Path.Combine(tmp.Name, "DeleteShapefile.shp");

                Vector rVector = new Vector(sOrigPath);
                rVector.Copy(sDeletePath);
                //Make sure our setup is good
                Assert.IsTrue(File.Exists(sDeletePath));

                // Now delete what we just copied
                Vector rVectorCopy = new Vector(sDeletePath);
                rVectorCopy.Delete();

                // Make sure we're good.
                Assert.IsFalse(File.Exists(sDeletePath));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.dbf")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.prj")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.shx")));
            }
        }

    }
}