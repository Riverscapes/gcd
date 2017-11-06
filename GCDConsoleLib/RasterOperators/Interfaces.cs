using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GCDConsoleLib.Internal.Operators;
using System.IO;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib
{
    public class RasterOperators
    {
        public enum MathOpType : byte { Addition, Subtraction, Division, Multipication };

        /// <summary>
        /// EXTENDED COPY
        /// </summary>
        public static Raster ExtendedCopy(ref Raster rInput, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(ref rInput, sOutputRaster), rInput.Extent
            });
        }

        public static Raster ExtendedCopy(ref Raster rInput, FileInfo sOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(ref rInput, sOutputRaster), newRect
            });
        }

        public static Raster ExtendedCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, rOutputRaster, newRect
            });
        }

        /// <summary>
        /// Raster Math
        /// </summary>
        public static Raster Add<T>(ref Raster rInput, T dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Addition, rInput, dOperand, new Raster(ref rInput, sOutputRaster)
            });
        }
        public static Raster Add(ref Raster rInputA, ref Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Addition, rInputA, rInputB, new Raster(ref rInputA, sOutputRaster)
            });
        }

        public static Raster Multiply(ref Raster rInputA, ref Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInputA.Datatype.CSType, new object[] {
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
        public static DoDStats GetStatsMinLoD(ref Raster rawDoD, ref Raster thrDoD, float minLoD,
             Area cellArea, UnitGroup units)
        {
            GetDodMinLodStats theStatsOp = new GetDodMinLodStats(ref rawDoD, ref thrDoD, minLoD, new DoDStats(cellArea, units));
            theStatsOp.Run();
            return theStatsOp.Stats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsPropagated(ref Raster rawDoD, ref Raster thrDoD, ref Raster propErrRaster,
            Area cellArea, UnitGroup units)
        {
            GetDoDPropStats theStatsOp = new GetDoDPropStats(ref rawDoD, ref thrDoD, new DoDStats(cellArea, units));
            theStatsOp.Run();
            return theStatsOp.Stats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a probabilistic thresholding
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsProbalistic(ref Raster rawDoD, ref Raster thrDoD, ref Raster propErrRaster,
            Area cellArea, UnitGroup units)
        {
            GetChangeStats raw = new GetChangeStats(ref rawDoD, new DoDStats(cellArea, units));
            GetChangeStats thr = new GetChangeStats(ref thrDoD, new DoDStats(cellArea, units));
            GetChangeStats err = new GetChangeStats(ref propErrRaster, ref thrDoD, new DoDStats(cellArea, units));

            return new GCD.DoDStats(
                raw.Stats.ErosionRaw.GetArea(cellArea), raw.Stats.DepositionRaw.GetArea(cellArea),
                thr.Stats.ErosionRaw.GetArea(cellArea), thr.Stats.DepositionRaw.GetArea(cellArea),
                raw.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                raw.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                err.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                cellArea, units
                );
        }

        public static Raster BilinearResample(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Hillshade(ref Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(ref rInput, sOutputRaster, new GdalDataType(OSGeo.GDAL.DataType.GDT_Int16));
            Hillshade theHSOp = new Hillshade(ref rInput, outputRaster);
            return theHSOp.RunWithOutput();
        }

        public static Raster SlopePercent(ref Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(ref rInput, sOutputRaster, new GdalDataType(typeof(float)));
            Slope theSlopeOp = new Slope(ref rInput, outputRaster, Slope.SlopeType.Percent);
            return theSlopeOp.RunWithOutput();
        }

        public static Raster SlopeDegrees(ref Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(ref rInput, sOutputRaster, new GdalDataType(typeof(float)));
            Slope theSlopeOp = new Slope(ref rInput, outputRaster, Slope.SlopeType.Degrees);
            return theSlopeOp.RunWithOutput();
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

        public static Raster Uniform<T>(ref Raster rInput, FileInfo sOutputRaster, T value)
        {
            UniformRaster<T> theUniformOp = new UniformRaster<T>(ref rInput, new Raster(ref rInput, sOutputRaster), value);
            return theUniformOp.RunWithOutput();
        }

        public static Raster Mosaic(ref List<System.IO.FileInfo> sInputs, FileInfo sOutputRaster)
        {
            List<Raster> rlInputs = new List<Raster>();

            foreach (FileInfo sRa in sInputs)
                rlInputs.Add(new Raster(sRa));
            Raster r0 = rlInputs[0];

            return (Raster)GenericRunWithOutput(typeof(Mosaic<>), rlInputs[0].Datatype.CSType, new object[] {
                rlInputs, new Raster(ref r0, sOutputRaster)
            });
        }

        public static Raster Mask(ref Raster rUnmasked, ref Raster rMask, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(Mask<>), rUnmasked.Datatype.CSType, new object[] {
                rUnmasked, rMask, new Raster(ref rUnmasked, sOutputRaster)
            });
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
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), raster1.Datatype.CSType, new object[] {
                MathOpType.Subtraction, raster1, raster2, new Raster(ref raster1, sOutputRaster)
            });
        }

        public static Raster RootSumSquares(ref Raster raster1, ref Raster raster2, System.IO.FileInfo sOutputRaster)
        {
            RootSumSquare theUniformOp = new RootSumSquare(ref raster1, ref raster2, new Raster(ref raster1, sOutputRaster));
            return theUniformOp.RunWithOutput();
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
            BinRaster histOp = new BinRaster(ref rInput, numberofBins);
            histOp.Run();
            return histOp.theHistogram;
        }

        public enum ThresholdOps
        {
            LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual,
        }

        public static Raster SetNull(ref Raster rInput, ThresholdOps fThresholdOp,
            Single fThreshold, System.IO.FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(ref rInput, fThresholdOp, fThreshold);
            return threshOp.RunWithOutput();
        }

        public static Raster SetNull(ref Raster rInput, ThresholdOps fThresholdOp, ref Raster rThreshold, System.IO.FileInfo sOutputRaster)
        {
            throw new NotImplementedException("threshold is defined by a raster instead of constant.");
        }

        public static Raster SetNull(ref Raster rInput, ThresholdOps fBottomOp, Single fBottom, ThresholdOps fTopOp, Single fTop, System.IO.FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(ref rInput, fBottomOp, fBottom, fTopOp, fTop);
            return threshOp.RunWithOutput();
        }

        public static Raster CreatePriorProbabilityRaster(ref Raster rawDoD, ref Raster newError, ref Raster oldError, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster ThresholdDoDProbWithSpatialCoherence(ref Raster rawDoD, string thrDoDPath, ref Raster newError, ref Raster OldError,
                                                            string sPriorProbRaster,
                                                            string sPosteriorRaster,
                                                            string sConditionalRaster,
                                                            string sSpatialCoErosionRaster,
                                                            string sSpatialCoDepositionRaster,
                                                             int nMovingWindowWidth, int nMovingWindowHeight, double fThreshold)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster ThresholdDoDProbability(ref Raster rawDoD, string thrHistPath, ref Raster newError, ref Raster oldError, string sPriorProbRaster, double fThreshold)
        {
            throw new NotImplementedException();
            return null;
        }

        public static void BuildPyramids(System.IO.FileInfo rInput)
        {
            Raster rRa = new Raster(rInput);
            rRa.BuildPyramids("AVERAGE");
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
        private static object GenericRun(Type generic, Type innerType, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("Run",
                BindingFlags.Public | BindingFlags.Instance);
            method.Invoke(myGenericClass, null);
            return myGenericClass;
        }
        private static object GenericRunWithOutput(Type generic, Type innerType, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("RunWithOutput",
                BindingFlags.Public | BindingFlags.Instance);
            return method.Invoke(myGenericClass, null);
        }

    }

}





