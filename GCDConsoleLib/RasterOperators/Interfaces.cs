using System;
using System.Collections.Generic;
using System.Reflection;
using GCDConsoleLib.Internal.Operators;
namespace GCDConsoleLib
{
    public class RasterOperators
    {
        public enum MathOpType : byte { Addition, Subtraction, Division, Multipication };

        /// <summary>
        /// EXTENDED COPY
        /// </summary>
        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(ref rInput, sOutputRaster), rInput.Extent
            });
        }

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(ref rInput, sOutputRaster), newRect
            });
        }

        public static Raster ExtendedCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, rOutputRaster, newRect
            });
        }

        /// <summary>
        /// Raster Math
        /// </summary>
        public static Raster Add<T>(ref Raster rInput, T dOperand, string sOutputRaster)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Addition, rInput, dOperand, new Raster(ref rInput, sOutputRaster)
            });
        }
        public static Raster Add(ref Raster rInputA, ref Raster rInputB, string sOutputRaster)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Addition, rInputA, rInputB, new Raster(ref rInputA, sOutputRaster)
            });
        }

        public static Raster Multiply(ref Raster rInputA, ref Raster rInputB, string sOutputRaster)
        {
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Multipication, rInputA, rInputB, new Raster(ref rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="minLoD">Minimum Level of Detection</param>
        /// <returns></returns>
        public static DoDStats GetStatsMinLoD(ref Raster rawDoD, ref Raster thrDoD, double minLoD)
        {
            throw new NotImplementedException("See old public C method GetDoDMinLoDStats()");

            return new DoDStats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsPropagated(string rawDoD, string thrDoD, string propErrRaster)
        {
            throw new NotImplementedException("See old public C method GetDoDPropStats()");

            return new DoDStats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a probabilistic thresholding
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsProbalistic(string rawDoD, string thrDoD, string propErrRaster)
        {
            throw new NotImplementedException("See old public C method GetDoDPropStats()");

            return new DoDStats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        public static Raster BilinearResample(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Hillshade(ref Raster rInput, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster SlopePercent(ref Raster rInput, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster SlopeDegrees(ref Raster rInput, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public enum KernelShapes
        {
            Square,
            Circle
        }

        public static Raster PointDensity(ref Raster rInput, ref Vector vPointCloud, string sOutputRaster, KernelShapes eKernel, double fSize)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Uniform(ref Raster rInput, string sOutputRaster, double fValue)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Mosaic(ref List<string> sInputs, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Mask(ref Raster rUnmasked, ref Raster rMask, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fisInputs">Key is FIS input name, value is corresponding raster path</param>
        /// <param name="sFISRuleFile">Path to FIS rule file (*.fis)</param>
        /// <param name="rReference"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster FISRaster(ref Dictionary<string, string> fisInputs, string sFISRuleFile, ref Raster rReference, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Subtract(ref Raster raster1, ref Raster raster2, System.IO.FileInfo sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// Default histogram generator
        /// </summary>
        /// <param name="rInput"></param>
        /// <returns></returns>
        /// <remarks>The goal of this operation is to bin raster values into a set number of bins.
        /// We need to discuss how those bins are defined. Ideally the first and last bin would
        /// contain zero cell count, so that the caller has confidence that the histogram has
        /// captured the full range of the raster values.</remarks>
        public static Histogram BinRaster(ref Raster rInput, int numberofBins)
        {
            throw new NotImplementedException();
            return null;
        }
        
        public static Raster SetNullLessThan(ref Raster rInput, double fThreshold, System.IO.FileInfo sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        




        ////////////////////////////////////    EVERYTHING BELOW HERE IS PRIVATE


        /// <summary>
        /// Generic function to get a generic type
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object MakeGenericType(Type generic, Type innerType,
            params object[] args)
        {
            Type specificType = generic.MakeGenericType(new Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }

        /// <summary>
        /// This method just calls the previous two in succession. Basically we're instantiating a generic 
        /// operator and then we're returning its "Run" method.
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object GenericRunner(Type generic, Type innerType, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("Run",
                BindingFlags.Public | BindingFlags.Instance);
            return method.Invoke(myGenericClass, null);
        }


    }

}





