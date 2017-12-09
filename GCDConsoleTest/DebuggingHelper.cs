using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using GCDConsoleLib.Tests.Utility;
using GCDConsoleLib.Common.Extensons;
using System.IO;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;
using System.Collections.Generic;

namespace GCDConsoleLib.Tests
{
    /// <summary>
    /// NOTE: WE ARE ONLY TESTING THE INTERFACE HERE. 
    /// 
    /// DO NOT TEST ANY VALUES PRODUCED HERE!!!!!!!
    /// </summary>
    [TestClass()]
    public class DEBUGGINGHELPER
    {
        [TestMethod()]
        public void EXTENTDEDCOPY()
        {
            //"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\2006FebDEM.tif"
            //FileInfo fisFile = new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\2006FebDEM.tif");
            //Raster Input = new Raster(fisFile);
            //Vector mask = new Vector(new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\Masks\2006FebDEMMask.shp"));

            //Dictionary<string, ErrorRasterProperties> props = new Dictionary<string, ErrorRasterProperties>() {
            //        { "Unknown",  new ErrorRasterProperties(0.2m) },
            //        { "Total Station",  new ErrorRasterProperties(0.15m) },
            //        { "LiDAR",  new ErrorRasterProperties(0.09m) }
            //    };

            //Raster rTemplateOutput2 = RasterOperators.CreateErrorRaster(Input, mask, "Method", props, new FileInfo(@"C:\dev\MultMethodUniform\inputs\MMUNIFORMTEST.tif"));


            //Assert.Fail();

            //Assert.Inconclusive();
            //using (ITempDir tmp = TempDir.Create())
            //{
            //    //"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\2006FebDEM.tif"
            //    FileInfo fisFile = new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\2006FebDEM.tif");
            //    Raster Input = new Raster(fisFile);
            //    Vector mask = new Vector(new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\Masks\2006FebDEMMask.shp"));

            //    Dictionary<string, ErrorRasterProperties> props = new Dictionary<string, ErrorRasterProperties>() {
            //        { "Unknown",  new ErrorRasterProperties(0.09m) },
            //        { "Total Station",  new ErrorRasterProperties(0.15m) },
            //        { "LiDAR",  new ErrorRasterProperties(0.09m) }
            //    };

            //    Raster rTemplateOutput2 = RasterOperators.CreateErrorRaster(Input, mask, "Method", props , new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            //    Assert.Fail();
            //}
        }

        [TestMethod()]
        public void BINRASTERPOLYGON()
        {
            //FileInfo fisFile = new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\2006FebDEM.tif");
            //Raster Input = new Raster(fisFile);
            //Vector mask = new Vector(new FileInfo(@"C:\dev\MultMethodUniform\inputs\2006FebDEM\Masks\2006FebDEMMask.shp"));

            //Dictionary<string, Histogram> thebins = RasterOperators.BinRaster(Input, 100, mask, "Method");
            //Assert.Fail();
        }
    }
}