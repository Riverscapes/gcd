using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDCore.UserInterface.SurveyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary.Tests
{
    [TestClass()]
    public class ExtentImporterTests
    {
        [TestMethod()]
        public void ExtentImporterTest()
        {
            // Turning this inconcul
            Assert.Inconclusive();

            ExtentRectangle input = new ExtentRectangle(100, 10, 1, 1, 500, 400);
            //ExtentImporter ei = new ExtentImporter(ExtentImporter.Purposes.FirstDEM, input, null, 0);

            ExtentRectangle input2 = new ExtentRectangle(100, 10, 0.9999999m, 0.9999999m, 500, 400);
            //ExtentImporter ei2 = new ExtentImporter(ExtentImporter.Purposes.FirstDEM, input, null, 2);

            Assert.Fail();
        }
    }
}