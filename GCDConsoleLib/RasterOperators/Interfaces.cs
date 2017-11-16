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
        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(rInput, sOutputRaster), rInput.Extent
            });
        }

        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(rInput, sOutputRaster), newRect
            });
        }

        public static Raster ExtendedCopy(Raster rInput, Raster rOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, rOutputRaster, newRect
            });
        }

        /// <summary>
        /// Raster Math: 1 raster and one operand (double)
        /// </summary>
        public static Raster Add(Raster rInput, double dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Addition, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }
        public static Raster Subtract(Raster rInput, double dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Subtraction, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }
        public static Raster Multiply(Raster rInput, double dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Multipication, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }
        public static Raster Divide(Raster rInput, double dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Division, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }


        public static Raster Add(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Addition, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        public static Raster Subtract(Raster rInputA, Raster rInputB, System.IO.FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Subtraction, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        public static Raster Multiply(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Multipication, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }
        public static Raster Divide(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Division, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }


        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="minLoD">Minimum Level of Detection</param>
        /// <returns></returns>
        public static DoDStats GetStatsMinLoD(Raster rawDoD, Raster thrDoD, float minLoD,
             Area cellArea, UnitGroup units)
        {
            GetDodMinLodStats theStatsOp = new GetDodMinLodStats(rawDoD, thrDoD, minLoD, new DoDStats(cellArea, units));
            theStatsOp.Run();
            return theStatsOp.Stats;
        }

        /// <summary>
        /// Retrieve the segregated Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="minLoD">Minimum Level of Detection</param>
        /// <param name="PolygonMask">Vector layer containing the mask polygons</param>
        /// <param name="FieldName">Name of the field in the PolygonMask that contains the distinguishing property on which to group statistics</param>
        /// <returns></returns>
        public static Dictionary<string, DoDStats> GetStatsMinLoD(Raster rawDoD, Raster thrDoD, float minLoD,
            Vector PolygonMask, string FieldName,
             Area cellArea, UnitGroup units)
        {
            GetDodMinLodStats theStatsOp = new GetDodMinLodStats(rawDoD, thrDoD, minLoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
            theStatsOp.Run();
            return theStatsOp.SegStats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <param name="PolygonMask">Vector layer containing the mask polygons</param>
        /// <param name="FieldName">Name of the field in the PolygonMask that contains the distinguishing property on which to group statistics</param>
        /// <returns></returns>
        public static DoDStats GetStatsPropagated(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
            Area cellArea, UnitGroup units)
        {
            GetDoDPropStats theStatsOp = new GetDoDPropStats(rawDoD, thrDoD, new DoDStats(cellArea, units));
            theStatsOp.Run();
            return theStatsOp.Stats;
        }

        /// <summary>
        /// Retrieve the segragated Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static Dictionary<string, DoDStats> GetStatsPropagated(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
          Vector PolygonMask, string FieldName,
          Area cellArea, UnitGroup units)
        {
            GetDoDPropStats theStatsOp = new GetDoDPropStats(rawDoD, thrDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
            theStatsOp.Run();
            return theStatsOp.SegStats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a probabilistic thresholding
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsProbalistic(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
            Area cellArea, UnitGroup units)
        {
            GetChangeStats raw = new GetChangeStats(rawDoD, new DoDStats(cellArea, units));
            GetChangeStats thr = new GetChangeStats(thrDoD, new DoDStats(cellArea, units));
            GetChangeStats err = new GetChangeStats(propErrRaster, thrDoD, new DoDStats(cellArea, units));

            return new GCD.DoDStats(
                raw.Stats.ErosionRaw.GetArea(cellArea), raw.Stats.DepositionRaw.GetArea(cellArea),
                thr.Stats.ErosionRaw.GetArea(cellArea), thr.Stats.DepositionRaw.GetArea(cellArea),
                raw.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                raw.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                err.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                cellArea, units
                );
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a probabilistic thresholding
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <param name="PolygonMask">Vector layer containing the mask polygons</param>
        /// <param name="FieldName">Name of the field in the PolygonMask that contains the distinguishing property on which to group statistics</param>
        /// <returns></returns>
        enum statsType : byte { raw, thr, err };
        public static Dictionary<string, DoDStats> GetStatsProbalistic(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
            Vector PolygonMask, string FieldName,
            Area cellArea, UnitGroup units)
        {
            GetChangeStats raw = new GetChangeStats(rawDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
            GetChangeStats thr = new GetChangeStats(thrDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
            GetChangeStats err = new GetChangeStats(propErrRaster, thrDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);

            // Create an empty stats object we will use wherever we need to
            DoDStats empty = new DoDStats(cellArea, units);

            // Now turn the dictionaries inside out [fieldvalue][raw/thr/err][DoDstats]
            Dictionary<string, Dictionary<statsType, DoDStats>> statsflddic = new Dictionary<string, Dictionary<statsType, DoDStats>>();

            foreach (KeyValuePair<string, DoDStats> kvp in raw.SegStats)
                if (statsflddic.ContainsKey(kvp.Key))
                    statsflddic[kvp.Key][statsType.raw] = kvp.Value;
                else
                    statsflddic[kvp.Key] = new Dictionary<statsType, DoDStats>() { { statsType.raw, kvp.Value } };

            foreach (KeyValuePair<string, DoDStats> kvp in thr.SegStats)
                if (statsflddic.ContainsKey(kvp.Key))
                    statsflddic[kvp.Key][statsType.thr] = kvp.Value;
                else
                    statsflddic[kvp.Key] = new Dictionary<statsType, DoDStats>() { { statsType.thr, kvp.Value } };

            foreach (KeyValuePair<string, DoDStats> kvp in err.SegStats)
                if (statsflddic.ContainsKey(kvp.Key))
                    statsflddic[kvp.Key][statsType.err] = kvp.Value;
                else
                    statsflddic[kvp.Key] = new Dictionary<statsType, DoDStats>() { { statsType.err, kvp.Value } };

            // Now we're ready to combine things and return a dictionary of values
            Dictionary<string, DoDStats> retVal = new Dictionary<string, DoDStats>();
            foreach (KeyValuePair<string, Dictionary<statsType, DoDStats>> kvp in statsflddic)
            {
                // Now we need to fill in the blanks with an empty DoDstats object so we get 0's where 
                // we expect them.
                foreach (statsType tp in Enum.GetValues(typeof(statsType)))
                    if (!kvp.Value.ContainsKey(tp))
                        kvp.Value[tp] = empty;

                retVal[kvp.Key] = new GCD.DoDStats(
                            kvp.Value[statsType.raw].ErosionRaw.GetArea(cellArea),
                            kvp.Value[statsType.raw].DepositionRaw.GetArea(cellArea),
                            kvp.Value[statsType.thr].ErosionRaw.GetArea(cellArea),
                            kvp.Value[statsType.thr].DepositionRaw.GetArea(cellArea),
                            kvp.Value[statsType.raw].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.raw].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.raw].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.raw].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.err].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.raw].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            cellArea, units);
            }

            return retVal;
        }

        public static Raster BilinearResample(Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Hillshade(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(OSGeo.GDAL.DataType.GDT_Int16));
            Hillshade theHSOp = new Hillshade(rInput, outputRaster);
            return theHSOp.RunWithOutput();
        }

        public static Raster SlopePercent(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(float)));
            Slope theSlopeOp = new Slope(rInput, outputRaster, Slope.SlopeType.Percent);
            return theSlopeOp.RunWithOutput();
        }

        public static Raster SlopeDegrees(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(float)));
            Slope theSlopeOp = new Slope(rInput, outputRaster, Slope.SlopeType.Degrees);
            return theSlopeOp.RunWithOutput();
        }

        public enum KernelShapes
        {
            Square,
            Circle
        }

        public static Raster PointDensity(Raster rInput, Vector vPointCloud, string sOutputRaster, KernelShapes eKernel, double fSize)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster Uniform<T>(Raster rInput, FileInfo sOutputRaster, T value)
        {
            UniformRaster<T> theUniformOp = new UniformRaster<T>(rInput, new Raster(rInput, sOutputRaster), value);
            return theUniformOp.RunWithOutput();
        }

        public static Raster Mosaic(List<System.IO.FileInfo> sInputs, FileInfo sOutputRaster)
        {
            List<Raster> rlInputs = new List<Raster>();

            foreach (FileInfo sRa in sInputs)
                rlInputs.Add(new Raster(sRa));
            Raster r0 = rlInputs[0];

            return (Raster)GenericRunWithOutput(typeof(Mosaic<>), rlInputs[0].Datatype.CSType, new object[] {
                rlInputs, new Raster( r0, sOutputRaster)
            });
        }

        public static Raster Mask(Raster rUnmasked, Raster rMask, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(Mask<>), rUnmasked.Datatype.CSType, new object[] {
                rUnmasked, rMask, new Raster( rUnmasked, sOutputRaster)
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
        public static Raster FISRaster(Dictionary<string, FileInfo> fisInputs, FileInfo sFISRuleFile, Raster rReference, FileInfo sOutputRaster)
        {
            List<string> rFisInputNames = new List<string>();
            List<Raster> rFisInputRasters = new List<Raster>();

            // Load up our input rasters
            foreach (KeyValuePair<string, FileInfo> inp in fisInputs)
            {
                rFisInputNames.Add(inp.Key);
                rFisInputRasters.Add(new Raster(inp.Value));
            }

            Raster outputRaster = new Raster(rReference, sOutputRaster, new GdalDataType(typeof(float)));
            FISRaster theSlopeOp = new FISRaster(rFisInputNames, rFisInputRasters, sFISRuleFile, outputRaster);

            throw new NotImplementedException();
            return theSlopeOp.RunWithOutput();
        }

        public static Raster RootSumSquares(Raster raster1, Raster raster2, System.IO.FileInfo sOutputRaster)
        {
            RootSumSquare theUniformOp = new RootSumSquare(raster1, raster2, new Raster(raster1, sOutputRaster));
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
        public static Histogram BinRaster(Raster rInput, int numberofBins)
        {
            BinRaster histOp = new BinRaster(rInput, numberofBins);
            histOp.Run();
            return histOp.theHistogram;
        }

        public static Dictionary<string, Histogram> BinRaster(ref Raster rInput, int numberofBins, ref Vector polygonMask, string FieldName)
        {
            BinRaster histOp = new BinRaster(rInput, numberofBins, polygonMask, FieldName);
            histOp.Run();
            return histOp.SegHistograms;
        }

        public enum ThresholdOps
        {
            LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual,
        }

        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp,
            float fThreshold, FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(rInput, fThresholdOp, fThreshold);
            return threshOp.RunWithOutput();
        }

        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp, Raster rThreshold, System.IO.FileInfo sOutputRaster)
        {
            throw new NotImplementedException("threshold is defined by a raster instead of constant.");
        }

        public static Raster SetNull(Raster rInput, ThresholdOps fBottomOp, float fBottom, ThresholdOps fTopOp, float fTop, System.IO.FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(rInput, fBottomOp, fBottom, fTopOp, fTop);
            return threshOp.RunWithOutput();
        }

        public static Raster CreatePriorProbabilityRaster(Raster rawDoD, Raster newError, Raster oldError, string sOutputRaster)
        {
            throw new NotImplementedException();
            return null;
        }

        public static Raster ThresholdDoDProbWithSpatialCoherence(Raster rawDoD, string thrDoDPath, Raster newError, Raster OldError,
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

        public static Raster ThresholdDoDProbability(Raster rawDoD, string thrHistPath, Raster newError, Raster oldError, string sPriorProbRaster, double fThreshold)
        {
            throw new NotImplementedException();
            return null;
        }

        public static void BuildPyramids(System.IO.FileInfo rInput)
        {
            Raster rRa = new Raster(rInput);
            rRa.BuildPyramids("AVERAGE");
        }


        ////////////////////////////////////    EVERYTHING BELOW HERE IS PRIVATE ////////////////////////////////////


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





