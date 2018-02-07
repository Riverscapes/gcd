using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GCDConsoleLib.Extensions.Tests
{
    [TestClass()]
    public class BiDictionaryTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void BiDictionaryTest()
        {
            BiDictionary<int, string, double> test1 = new BiDictionary<int, string, double>()
            {
                {1,"first", 1.0 },
                {2,"second", 2.0 },
                {3, "third", 3.0 }
            };
            test1.Add(4, "fourth", 4.0);
            
            Assert.AreEqual(test1.ByKey1[1], 1.0);
            Assert.AreEqual(test1.ByKey2["first"], 1.0);

            Assert.IsTrue(test1.ContainsKey(1));
            Assert.IsTrue(test1.ContainsKey("first"));
            Assert.IsTrue(test1.ContainsKey(2));
            Assert.IsTrue(test1.ContainsKey(3));
            Assert.IsTrue(test1.ContainsKey(4));

            Assert.IsTrue(test1.ContainsValue(4.0));

        }


    }
}