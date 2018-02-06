using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using GCDConsoleTest.Utility;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class VectorTests
    {
        [TestMethod()]
        public void VectorTest()
        {
            string sFilepath = TestHelpers.GetTestVectorPath("StressTest.shp");
            Vector rVector = new Vector(new FileInfo(sFilepath));
            Assert.IsTrue(rVector.Features.Count > 0);
            Assert.IsTrue(rVector.Fields.Count > 0);
            Assert.AreEqual(rVector.LayerName, "StressTest");
            Assert.AreEqual(rVector.GISFileInfo.FullName, sFilepath);
            Assert.IsNotNull(rVector.Proj);
        }

        [TestMethod()]
        public void VectorCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Vector rVector = new Vector(new FileInfo(TestHelpers.GetTestVectorPath("StressTest.shp")));
                rVector.Copy(new FileInfo(Path.Combine(tmp.Name, "CopyShapefile.shp")));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shp")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.dbf")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.prj")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shx")));
            }
        }

        [TestMethod()]
        public void FileTypes()
        {
            Vector shp = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"vectors\StressTest.shp")));
            Assert.IsTrue(shp.Features.Count > 10);

            Vector geojson = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"geojson\3squares.json")));
            Assert.IsTrue(geojson.Features.Count > 2);

        }

        [TestMethod()]
        public void VectorDeleteTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                string sOrigPath = TestHelpers.GetTestVectorPath("StressTest.shp");
                string sDeletePath = Path.Combine(tmp.Name, "DeleteShapefile.shp");

                Vector rVector = new Vector(new FileInfo(sOrigPath));
                rVector.Copy(new FileInfo(sDeletePath));
                //Make sure our setup is good
                Assert.IsTrue(File.Exists(sDeletePath));

                // Now delete what we just copied
                Vector rVectorCopy = new Vector(new FileInfo(sDeletePath));
                rVectorCopy.Delete();

                // Make sure we're good.
                Assert.IsFalse(File.Exists(sDeletePath));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.dbf")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.prj")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.shx")));
            }
        }


        [TestMethod()]
        public void StringVector()
        {
            string geoJSON = @"
{
    'type': 'FeatureCollection',
    'crs': {
        'type': 'name',
        'properties': {
            'name': 'urn:ogc:def:crs:OGC:1.3:CRS84'
        }
    },
    'features': [{
            'type': 'Feature',
            'properties': {
                'id': 1,
                'name': 'FeatA'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.252403253361436, 30.888533802761863],
                        [-114.252403223569075, 30.888531408978231],
                        [-114.252400449711814, 30.888531507935365],
                        [-114.252400393960841, 30.888533878831986],
                        [-114.252403253361436, 30.888533802761863]
                    ]
                ]
            }
        },
        {
            'type': 'Feature',
            'properties': {
                'id': 2,
                'name': 'FeatB'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.2524031592272, 30.888531092526087],
                        [-114.252403186661326, 30.888528722140837],
                        [-114.252400238159936, 30.888528628797079],
                        [-114.252400356460214, 30.888531167573458],
                        [-114.2524031592272, 30.888531092526087]
                    ]
                ]
            }
        },
        {
            'type': 'Feature',
            'properties': {
                'id': 3,
                'name': 'FeatC'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.25240312113354, 30.888528356846415],
                        [-114.252403033521759, 30.888525915243243],
                        [-114.252400202438167, 30.88852599080198],
                        [-114.252400343125714, 30.888528284855536],
                        [-114.25240312113354, 30.888528356846415]
                    ]
                ]
            }
        }
    ]
}  ";
            // First we test using it on its own. Just a temporary file all alone in the world
            string filepath1 = "";
            using (var tmp = new TempGeoJSONFile(geoJSON))
            {
                filepath1 = tmp.fInfo.FullName;
                Vector stringVector = new Vector(tmp.fInfo);
                Assert.IsTrue(stringVector.Features.Count > 2);
                Assert.IsTrue(new FileInfo(filepath1).Exists);
            }
            // Usage is done now file is cleaned up
            Assert.IsFalse(new FileInfo(filepath1).Exists);

            // Now let's test using it as part of a temporary directory 
            string filepath2 = "";
            using (ITempDir tmpfolder = TempDir.Create())
            {
                using (var tmpfile = new TempGeoJSONFile(geoJSON, Path.Combine(tmpfolder.Name, "geojson.json")))
                {
                    filepath2 = tmpfile.fInfo.FullName;
                    Vector stringVector = new Vector(tmpfile.fInfo);
                    Assert.IsTrue(stringVector.Features.Count > 2);
                    Assert.IsTrue(new FileInfo(filepath2).Exists);
                }
                // Still here
                Assert.IsTrue(new FileInfo(filepath2).Exists);
            }
            // Now it's gone 
            Assert.IsFalse(new FileInfo(filepath2).Exists);

        }
    }
}