
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class MiscTests
    {
        // without refs
        public void testFunc1(Dictionary<string, List<string>> E, List<string> F)
        {
            E["other"].Add("testFunc1-1");
            F.Add("testFunc1-2");
        }
        // With Refs
        public void testFunc2(ref Dictionary<string, List<string>> E, ref List<string> F)
        {
            E["other"].Add("testFunc2");
            F.Add("testFunc2");
        }

        /// <summary>
        /// Just a quick test to demonstrate how ref keywords work 
        /// (and more specifically how they don't work)
        /// </summary>
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
            testFunc2(ref B, ref A);

            // A rose by any other name is not the same rose.
            Assert.AreEqual(A, B["other"]);
            CollectionAssert.ReferenceEquals(A, B["other"]);
            CollectionAssert.Equals(A, B["other"]);
        }

        /// <summary>
        /// How do we set cultures and read cultural numbers
        /// </summary>
        [TestMethod()]
        public void CultureStuff()
        {
            double x = 123123123.456;

            CultureInfo EnglishCulture = new CultureInfo("en-EN");
            CultureInfo FrenchCulture = new CultureInfo("fr-FR");
            ///The CultureInfo.InvariantCulture property is neither a neutral nor a specific culture. 
            ///It is a third type of culture that is culture-insensitive.
            CultureInfo VancouverCulture = CultureInfo.InvariantCulture;

            // Now let's chenge this thread's culture
            Thread.CurrentThread.CurrentCulture = FrenchCulture;
            Thread.CurrentThread.CurrentUICulture = FrenchCulture;

            Assert.AreEqual(x.ToString(FrenchCulture), "123123123,456");
            Assert.AreEqual(x.ToString(EnglishCulture), "123123123.456");

            // We've defaulted this thread to french so we should see the virgule.
            Assert.AreEqual(x.ToString(), "123123123,456");

            // So this is what you want to use when writing to a file
            Assert.AreEqual(x.ToString(VancouverCulture), "123123123.456");

            // I'm not sure how unit testing works so let's change things 
            // back so it doesn't affect other tests
            Thread.CurrentThread.CurrentCulture = VancouverCulture;
            Thread.CurrentThread.CurrentUICulture = VancouverCulture;
        }

        /// <summary>
        /// Just a quick little test to see how floating point numbers work in C#
        /// </summary>
        [TestMethod()]
        public void floatingPoint()
        {
            // Pick a stupid number first to show how it goes
            string pi = "3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679";
            float piFlt = Convert.ToSingle(pi);
            double piDbl = Convert.ToDouble(pi);
            decimal piDec = Convert.ToDecimal(pi);

            // What happens when we try to parse this as a floating-point?
            Assert.AreEqual(piFlt.ToString(), "3.141593");
            Assert.AreEqual(piDbl.ToString(), "3.14159265358979");
            Assert.AreEqual(piDec.ToString(), "3.1415926535897932384626433833");

            // Now let's try a reasonable but precise number that isn't Pi
            Single reasonalbeFlt = 1.123212321232123210f;
            double reasonableDbl = 1.123212321232123210;
            decimal reasonableDec = 1.123212321232123210m;

            Assert.AreEqual(reasonalbeFlt.ToString(), "1.123212");
            Assert.AreEqual(reasonableDbl.ToString(), "1.12321232123212");
            Assert.AreEqual(reasonableDec.ToString(), "1.123212321232123210");

            // ok. now let's try getting into some trouble with division
            double divisionDbl = 0.7 / 0.025;
            decimal divisionDec = 0.7m / 0.025m;
            float divisionFloat = 0.7f / 0.025f;

            // Here's where we get into trouble
            // You never want to show this on a screen but you might want to write it to a file.
            Assert.AreEqual(divisionDbl, 27.999999999999996); // <== ok in the XML. Not ok on screen.
            Assert.AreEqual(divisionDec, 28);
            Assert.AreEqual(divisionFloat, 28);

            // Even though it looks like 27.999999999999996 in the inspector it renders out properly
            Assert.AreEqual(divisionDbl.ToString(), "28");
            Assert.AreEqual(divisionDec.ToString(), "28");
            Assert.AreEqual(divisionFloat.ToString(), "28");

            // Make sure we can go back and forth:
            /// This format is supported only for the Single and Double types. 
            /// The round-trip specifier guarantees that a numeric value converted 
            /// to a string will be parsed back into the same numeric value.
            Assert.AreEqual(Convert.ToDouble(piDbl.ToString("R")), piDbl);
            Assert.AreEqual(Convert.ToSingle(piFlt.ToString("R")), piFlt);
            Assert.AreEqual(Convert.ToDecimal(piDec.ToString()), piDec);
        }

        /// Notes about Math.Round from https://msdn.microsoft.com/en-us/library/75ks3aby(v=vs.110).aspx
        /// 
        /// Because of the loss of precision that can result from representing decimal values as floating-point 
        /// numbers or performing arithmetic operations on floating-point values, in some cases the 
        /// Round(Double, Int32) method may not appear to round midpoint values to the nearest even value 
        /// in the digits decimal position. This is illustrated in the following example, where 2.135 is rounded 
        /// to 2.13 instead of 2.14. This occurs because internally the method multiplies value by 10digits, and 
        /// the multiplication operation in this case suffers from a loss of precision.
        [TestMethod()]
        public void MathRounding()
        {

            // Look what happens with doubles. Default behaviour is to round to EVEN
            Assert.AreEqual(Math.Round(2.125, 2), 2.12);
            Assert.AreEqual(Math.Round(2.125001, 2), 2.13);
            Assert.AreEqual(Math.Round(2.135, 2), 2.13);
            Assert.AreEqual(Math.Round(2.145, 2), 2.14);
            Assert.AreEqual(Math.Round(3.125, 2), 3.12);

            // Look what happens with doubles using midpoint to AwayFromZero
            Assert.AreEqual(Math.Round(2.125, 2, MidpointRounding.AwayFromZero), 2.13);
            Assert.AreEqual(Math.Round(2.125001, 2, MidpointRounding.AwayFromZero), 2.13);
            Assert.AreEqual(Math.Round(2.135, 2, MidpointRounding.AwayFromZero), 2.13); // <==== WHOA!
            Assert.AreEqual(Math.Round(2.145, 2, MidpointRounding.AwayFromZero), 2.15);
            Assert.AreEqual(Math.Round(3.125, 2, MidpointRounding.AwayFromZero), 3.13);

            // Now do the same thing with ToString()
            Assert.AreEqual((2.125).ToString("#.##"), "2.13");
            Assert.AreEqual((2.125001).ToString("#.##"), "2.13");
            Assert.AreEqual((2.135).ToString("#.##"), "2.14");
            Assert.AreEqual((2.145).ToString("#.##"), "2.15");
            Assert.AreEqual((3.125).ToString("#.##"), "3.13");

            // Now with decimals
            Assert.AreEqual(Math.Round(2.125m, 2), 2.12m);
            Assert.AreEqual(Math.Round(2.12500001m, 2), 2.13m);
            Assert.AreEqual(Math.Round(2.135m, 2), 2.14m);
            Assert.AreEqual(Math.Round(2.145m, 2), 2.14m);
            Assert.AreEqual(Math.Round(3.125m, 2), 3.12m);

            // Long story short. Be really careful about using Math.Round() and avoid if possible. 
            // Use ToString() if you can and never store anything you pass through Round()
        }

    }
}

