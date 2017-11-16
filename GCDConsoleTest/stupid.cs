
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
    public class Stupidtests
    {
        [TestMethod()]
        public void PassByRef()
        {
            List<string> A = new List<string>();
            Dictionary<string, List<string>> B = new Dictionary<string, List<string>>();

            A.Add("thingA");
            B["other"] = A;

            B["other"].Add("thingB");
            A.Add("thingC");
            testFunc1(B, A);
            testFunc2(B, A);

            Assert.Fail();
        }

        public void testFunc1(Dictionary<string, List<string>> E, List<string> F)
        {
            E["other"].Add("testFunc1-1");
            F.Add("testFunc1-2");
        }

        public void testFunc2(Dictionary<string, List<string>> E, List<string> F)
        {
            E["other"].Add("testFunc2");
            F.Add("testFunc2");
        }
    }
}

