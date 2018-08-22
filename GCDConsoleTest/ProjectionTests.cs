using GCDConsoleTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class ProjectionTests
    {

        [TestMethod()]
        [TestCategory("Unit")]
        public void ProjectionTestWKT()
        {
            string sUnitWKT = "PROJCS[\"NAD_1983_StatePlane_California_II_FIPS_0402\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"False_Easting\",2000000.0],PARAMETER[\"False_Northing\",500000.0],PARAMETER[\"Central_Meridian\",-122.0],PARAMETER[\"Standard_Parallel_1\",38.33333333333334],PARAMETER[\"Standard_Parallel_2\",39.83333333333334],PARAMETER[\"Latitude_Of_Origin\",37.66666666666666],UNIT[\"meter\",1.0]]";
            string sNoUnitWKT1 = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            Projection sUnitP1 = new Projection(sUnitWKT);
            Projection sNoUnitP1 = new Projection(sNoUnitWKT1);

            Assert.IsNotNull(sUnitP1);
            Assert.IsNotNull(sNoUnitP1);

            // Majke sure we throw on a junk projection
            try
            {
                Projection junk = new Projection("junk");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ApplicationException));
            }

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void ProjectionTestEPSG()
        {
            Assert.Inconclusive();
            Projection sP1 = new Projection(4269);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IsSameBasicTest()
        {
            // Ok. Easy test first. Minor changes to the same WKT
            string sWKTTest1 = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            string sWKTTest2 = "geogcs[\"NAD83\",datum[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            string sWKTTest3 = "GEOGCS[     \"NAD83\"    ,    DATUM[    \"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";

            string sDiff = "PROJCS[\"NAD_1983_StatePlane_California_II_FIPS_0402\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"False_Easting\",2000000.0],PARAMETER[\"False_Northing\",500000.0],PARAMETER[\"Central_Meridian\",-122.0],PARAMETER[\"Standard_Parallel_1\",38.33333333333334],PARAMETER[\"Standard_Parallel_2\",39.83333333333334],PARAMETER[\"Latitude_Of_Origin\",37.66666666666666],UNIT[\"meter\",1.0]]";


            Projection pWKTTest1 = new Projection(sWKTTest1);
            Projection pWKTTest2 = new Projection(sWKTTest2);
            Projection pWKTTest3 = new Projection(sWKTTest3);

            Projection pWKTTestDiff = new Projection(sDiff);

            // Positive Test
            Assert.IsTrue(pWKTTest1.IsSame(pWKTTest2));
            Assert.IsTrue(pWKTTest1.IsSame(pWKTTest3));

            // Negative Test
            Assert.IsFalse(pWKTTestDiff.IsSame(pWKTTest2));
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void HorizontalUnitTest()
        {
            // Let's set up a WKT and then swap out the units
            string sUnitWKT = "PROJCS[\"NAD_1983_StatePlane_California_II_FIPS_0402\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"False_Easting\",2000000.0],PARAMETER[\"False_Northing\",500000.0],PARAMETER[\"Central_Meridian\",-122.0],PARAMETER[\"Standard_Parallel_1\",38.33333333333334],PARAMETER[\"Standard_Parallel_2\",39.83333333333334],PARAMETER[\"Latitude_Of_Origin\",37.66666666666666],UNIT[\"{0}\",1.0]]";

            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "meter")).HorizontalUnit, UnitsNet.Units.LengthUnit.Meter);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "Meter")).HorizontalUnit, UnitsNet.Units.LengthUnit.Meter);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "m")).HorizontalUnit, UnitsNet.Units.LengthUnit.Meter);

            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "Feet")).HorizontalUnit, UnitsNet.Units.LengthUnit.Foot);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "Foot")).HorizontalUnit, UnitsNet.Units.LengthUnit.Foot);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "ft")).HorizontalUnit, UnitsNet.Units.LengthUnit.Foot);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "ft-us")).HorizontalUnit, UnitsNet.Units.LengthUnit.Foot);

            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "Centimeter")).HorizontalUnit, UnitsNet.Units.LengthUnit.Centimeter);
            Assert.AreEqual(new Projection(String.Format(sUnitWKT, "cm")).HorizontalUnit, UnitsNet.Units.LengthUnit.Centimeter);

            try
            {
                Projection s1 = new Projection(String.Format(sUnitWKT, "gallops"));
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Not just any exception will do.
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }



        [TestMethod()]
        [TestCategory("Unit")]
        public void IsSameComplexTest()
        {
            Assert.Inconclusive();
            // Now the Hard stuff: 3 different ways of doing the same WKT
            string sWKT1 = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            string sWKT2 = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
            string sWKT3 = @"GEOGCS[""NAD83"",
    DATUM[""North_American_Datum_1983"",
        SPHEROID[""GRS 1980"",6378137,298.257222101,
            AUTHORITY[""EPSG"",""7019""]],
        AUTHORITY[""EPSG"",""6269""]],
    PRIMEM[""Greenwich"",0,
        AUTHORITY[""EPSG"",""8901""]],
    UNIT[""degree"",0.01745329251994328,
        AUTHORITY[""EPSG"",""9122""]],
    AUTHORITY[""EPSG"",""4269""]]";
            Projection junk = new Projection("junk");

            Projection pWKT1 = new Projection(sWKT1);
            Projection pWKT2 = new Projection(sWKT2);
            Projection pWKT3 = new Projection(sWKT3);


            // Positive Test
            Assert.IsTrue(pWKT1.IsSame(pWKT2));
            Assert.IsTrue(pWKT1.IsSame(pWKT3));

            // Negative Test
            Assert.IsFalse(pWKT1.IsSame(junk));

        }




        [TestMethod()]
        [TestCategory("Long")]
        public void ValidityCheck()
        {
            Dictionary<string, Projection> projdict = new Dictionary<string, Projection>();

            Dictionary<string, Raster> rasters = new Dictionary<string, Raster>()
            { //"C:\dev\gcd\extlib\TestData\projectiontests\Dee\2011Dee1mDEM_BEFORE.tif"
                { "Dee2011", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Dee/2011Dee1mDEM.tif"))) },
                { "Dee2011Weird", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Dee/2011Dee1mDEM_weird.tif"))) },
                { "Grilliot", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Grilliot/WB2013610r.tif"))) },
                { "Rees", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Rees/Event08DEM.tif"))) },
                { "Shotover2011", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Shotover/Pr2011DEM.tif"))) },
                { "ShotoverPre2011", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Shotover/Pre2011DEM.tif"))) },
                { "Sulpher2005", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/SulpherCreek/2005DecDEM.tif"))) },
                { "SulpherPointDenisty", new Raster(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/SulpherCreek/dec05allpoints.tif"))) },
            };

            Dictionary<string, Vector> vectors = new Dictionary<string, Vector>()
            {
                { "Dee", new Vector(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Dee/OSWaterAreaReachType.shp"))) },
                { "ShotoverAOI", new Vector(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Shotover/AoI.shp"))) },
                { "ShotoverCells", new Vector(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Shotover/Cells.shp"))) },
                { "ShotoverSections", new Vector(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/Shotover/Sections.shp"))) },
                { "SulpherCreek", new Vector(new FileInfo(DirHelpers.GetTestRootPath("projectiontests/SulpherCreek/DoDGeomorphicInterpretation.shp"))) }
            };

            foreach (KeyValuePair<string, Raster> kvp in rasters)
                projdict.Add(kvp.Value.GISFileInfo.FullName, kvp.Value.Proj);

            foreach (KeyValuePair<string, Vector> kvp in vectors)
                projdict.Add(kvp.Value.GISFileInfo.FullName, kvp.Value.Proj);

            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {

                foreach (KeyValuePair<string, Raster> kvp in rasters)
                {
                    string thing;
                    kvp.Value.Proj.mSRef.ExportToPrettyWkt(out thing, 0);

                    Raster rCopy = RasterOperators.ExtendedCopy(kvp.Value, new FileInfo(Path.Combine(tmp.Name, kvp.Value.GISFileInfo.Name)));
                    string HSName = Path.GetFileNameWithoutExtension(kvp.Value.GISFileInfo.Name) + "_HS" + Path.GetExtension(kvp.Value.GISFileInfo.Name);
                    Raster rHS = RasterOperators.Hillshade(rCopy, new FileInfo(Path.Combine(tmp.Name, HSName)));

                    projdict.Add(rCopy.GISFileInfo.FullName, rCopy.Proj);
                    projdict.Add(rHS.GISFileInfo.FullName, rHS.Proj);

                    Assert.IsTrue(kvp.Value.Proj.IsSame(rCopy.Proj));
                    Assert.IsTrue(kvp.Value.Proj.IsSame(rHS.Proj));
                }

                Debug.WriteLine("DONE");
            }

            Debug.WriteLine("Local, Projected, Geographic, GeoCentric, Path, HasWKT, HasProj, LinearUnits, WKTLength, HasProj4");
            foreach(KeyValuePair<string, Projection> kvp in projdict)
            {
                Projection Proj = kvp.Value;
                string proj4;
                Proj.mSRef.ExportToProj4(out proj4);
                Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                    Proj.mSRef.IsLocal(), Proj.mSRef.IsProjected(), Proj.mSRef.IsGeographic(), 
                    Proj.mSRef.IsGeocentric(), kvp.Key, Proj.mSRef.GetLinearUnitsName(),
                    Proj.Wkt.Length, !String.IsNullOrEmpty(proj4)));

            }

            Debug.WriteLine("DONE");
        }

    }
}