using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class ProjectionTests
    {

        [TestMethod()]
        public void ProjectionTestWKT()
        {
            string sWKT1 = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            Projection sP1 = new Projection(sWKT1);
            Projection junk = new Projection("junk");
            Assert.IsNotNull(sWKT1);
            Assert.IsNotNull(junk);
        }

        [TestMethod()]
        public void ProjectionTestEPSG()
        {
            Assert.Inconclusive();
            Projection sP1 = new Projection(4269);
        }

        [TestMethod()]
        public void IsSameBasicTest()
        {
            // Ok. Easy test first. Minor changes to the same WKT
            string sWKTTest1 = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            string sWKTTest2 = "geogcs[\"NAD83\",datum[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            string sWKTTest3 = "GEOGCS[     \"NAD83\"    ,    DATUM[    \"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
            Projection junk = new Projection("junk");

            Projection pWKTTest1 = new Projection(sWKTTest1);
            Projection pWKTTest2 = new Projection(sWKTTest2);
            Projection pWKTTest3 = new Projection(sWKTTest3);

            // Positive Test
            Assert.IsTrue(pWKTTest1.IsSame(pWKTTest2));
            Assert.IsTrue(pWKTTest1.IsSame(pWKTTest3));

            // Negative Test
            Assert.IsFalse(pWKTTest1.IsSame(junk));
        }

        [TestMethod()]
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
    }
}