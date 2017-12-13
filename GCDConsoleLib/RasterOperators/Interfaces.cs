using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GCDConsoleLib.Internal.Operators;
using System.IO;
using UnitsNet;
using UnitsNet.Units;
using System.Diagnostics;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib
{
    public static class RasterOperators
    {
        #region Math and Copy Operations --------------------------------------------------------------------------------

        public enum MathOpType : byte { Addition, Subtraction, Division, Multipication };

        /// <summary>
        /// Copy a Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(rInput, sOutputRaster), rInput.Extent
            });
        }

        /// <summary>
        /// Copy a raster by specifying a new extent
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="newRect"></param>
        /// <returns></returns>
        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster, ExtentRectangle newRect)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType, new object[] {
                rInput, new Raster(rInput, sOutputRaster), newRect
            });
        }

        /// <summary>
        /// Add a value to a Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Add(Raster rInput, decimal dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Addition, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Subtract a value from a Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Subtract(Raster rInput, decimal dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Subtraction, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Multiply a Raster and a Value
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Multiply(Raster rInput, decimal dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Multipication, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Divide a Raster and a Value
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Divide(Raster rInput, decimal dOperand, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType, new object[] {
                MathOpType.Division, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Add Two Rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Add(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Addition, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Substract two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Subtract(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Subtraction, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Multiply two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Multiply(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Multipication, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Divide two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Divide(Raster rInputA, Raster rInputB, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType, new object[] {
                MathOpType.Division, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Create a BilinearReseample Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="outputExtent"></param>
        /// <returns></returns>
        public static Raster BilinearResample(Raster rInput, FileInfo sOutputRaster, ExtentRectangle outputExtent)
        {
            return (Raster)GenericRunWithOutput(typeof(BilinearResample<>), rInput.Datatype.CSType, new object[] {
                rInput, outputExtent, new Raster(rInput, sOutputRaster)
            });
        }


        /// <summary>
        /// Create a Root Sum Squares Calculation Raster
        /// </summary>
        /// <param name="raster1"></param>
        /// <param name="raster2"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster RootSumSquares(Raster raster1, Raster raster2, FileInfo sOutputRaster)
        {
            RootSumSquare theUniformOp = new RootSumSquare(raster1, raster2, new Raster(raster1, sOutputRaster));
            return theUniformOp.RunWithOutput();
        }

        #endregion

        #region Raster Combination methods (Mosaic, Mask) --------------------------------------------------------------------------------

        /// <summary>
        /// Mosaic a list of Rasters into an output Raster
        /// </summary>
        /// <param name="sInputs"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Mosaic(List<FileInfo> sInputs, FileInfo sOutputRaster)
        {
            List<Raster> rlInputs = new List<Raster>();

            foreach (FileInfo sRa in sInputs)
                rlInputs.Add(new Raster(sRa));

            Raster r0 = rlInputs[0];

            return (Raster)GenericRunWithOutput(typeof(Mosaic<>), rlInputs[0].Datatype.CSType, new object[] {
                rlInputs, new Raster( r0, sOutputRaster)
            });
        }

        /// <summary>
        /// Mask a raster with another raster
        /// </summary>
        /// <param name="rUnmasked"></param>
        /// <param name="rMask"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Mask(Raster rUnmasked, Raster rMask, FileInfo sOutputRaster)
        {
            return (Raster)GenericRunWithOutput(typeof(Mask<>), rUnmasked.Datatype.CSType, new object[] {
                rUnmasked, rMask, new Raster( rUnmasked, sOutputRaster)
            });
        }

        #endregion

        #region Statistics Calculation Operation (DoD stuff and Histogram Binning) --------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="minLoD">Minimum Level of Detection</param>
        /// <returns></returns>
        public static DoDStats GetStatsMinLoD(Raster rawDoD, Raster thrDoD, decimal minLoD, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
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
        public static Dictionary<string, DoDStats> GetStatsMinLoD(Raster rawDoD, Raster thrDoD, decimal minLoD,
            Vector PolygonMask, string FieldName, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDodMinLodStats theStatsOp = new GetDodMinLodStats(rawDoD, thrDoD, minLoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
            theStatsOp.Run();
            return theStatsOp.SegStats;
        }


        /// <summary>
        /// Retrieve the segragated Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <returns></returns>
        public static DoDStats GetStatsPropagated(Raster rawDoD, Raster propErrRaster, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDoDPropStats theStatsOp = new GetDoDPropStats(rawDoD, propErrRaster, new DoDStats(cellArea, units));
            theStatsOp.Run();
            return theStatsOp.Stats;
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
        public static Dictionary<string, DoDStats> GetStatsPropagated(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
          Vector PolygonMask, string FieldName, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
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
        public static DoDStats GetStatsProbalistic(Raster rawDoD, Raster thrDoD, Raster propErrRaster, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
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
            Vector PolygonMask, string FieldName, UnitGroup units)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
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
            rInput.Dispose();
            return histOp.theHistogram;
        }


        #endregion  


        #region Error Raster stuff --------------------------------------------------------------------------------

        /// <summary>
        /// Single method error calculation
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static Raster CreateErrorRaster(Raster rawDEM, ErrorRasterProperties props, FileInfo sOutputRaster, EventHandler<int> progressHandler = null)
        {
            CreateErrorRaster theStatsOp = new CreateErrorRaster(rawDEM, props, new Raster(rawDEM, sOutputRaster));
            if (progressHandler != null)
                theStatsOp.ProgressEvent += progressHandler;
            return theStatsOp.RunWithOutput();
        }

        /// <summary>
        /// Multimethod error calculation
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static Raster CreateErrorRaster(Raster rawDEM, Vector PolygonMask, string MaskFieldName,
            Dictionary<string, ErrorRasterProperties> props, FileInfo sOutputRaster, EventHandler<int> progressHandler = null)
        {
            //https://stackoverflow.com/questions/2560258/how-to-pass-an-event-to-a-method
            CreateErrorRaster theStatsOp = new CreateErrorRaster(rawDEM, PolygonMask, MaskFieldName, props, new Raster(rawDEM, sOutputRaster));
            if (progressHandler != null)
                theStatsOp.ProgressEvent += progressHandler;
            return theStatsOp.RunWithOutput();
        }

        /// <summary>
        /// Generate a Uniform Raster
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="value">Value for the raster to be</param>
        /// <returns></returns>
        public static Raster Uniform<T>(Raster rInput, FileInfo sOutputRaster, T value)
        {
            UniformRaster<T> theUniformOp = new UniformRaster<T>(rInput, new Raster(rInput, sOutputRaster), value);
            return theUniformOp.RunWithOutput();
        }

        #endregion  


        /// <summary>
        /// Create a Hillshade raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Hillshade(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(int)));
            Hillshade theHSOp = new Hillshade(rInput, outputRaster);
            return theHSOp.RunWithOutput();
        }

        /// <summary>
        /// Create a Slope Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster SlopePercent(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(double)));
            Slope theSlopeOp = new Slope(rInput, outputRaster, Slope.SlopeType.Percent);
            return theSlopeOp.RunWithOutput();
        }

        /// <summary>
        /// Create a Slope Raster
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster SlopeDegrees(Raster rInput, FileInfo sOutputRaster)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(double)));
            Slope theSlopeOp = new Slope(rInput, outputRaster, Slope.SlopeType.Degrees);
            return theSlopeOp.RunWithOutput();
        }

        public enum KernelShapes
        {
            Square,
            Circle
        }

        /// <summary>
        /// Create a point density Raster
        /// </summary>
        /// <param name="rDEM"></param>
        /// <param name="vPointCloud"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="eKernel"></param>
        /// <param name="fSize"></param>
        /// <param name="progressHandler">Event handler to hook into this so that you can update a progress bar</param>
        /// <returns></returns>
        public static Raster PointDensity(Raster rDEM, Vector vPointCloud, FileInfo sOutputRaster,
            KernelShapes eKernel, decimal fSize, EventHandler<int> progressHandler = null)
        {
            Raster outputRaster = new Raster(rDEM, sOutputRaster, new GdalDataType(typeof(double)));
            PointDensity theSlopeOp = new PointDensity(rDEM, vPointCloud, outputRaster, eKernel, fSize);

            if (progressHandler != null)
                theSlopeOp.ProgressEvent += progressHandler;

            return theSlopeOp.RunWithOutput();
        }



        /// <summary>
        /// Create a FIS Raster
        /// </summary>
        /// <param name="fisInputs">Key is FIS input name, value is corresponding raster path</param>
        /// <param name="sFISRuleFile">Path to FIS rule file (*.fis)</param>
        /// <param name="rReference">Reference Raster to use.</param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster FISRaster(Dictionary<string, FileInfo> fisInputs, FileInfo sFISRuleFile,
            Raster rReference, FileInfo sOutputRaster, EventHandler<int> progressHandler = null)
        {
            Dictionary<string, Raster> rFISInputs = new Dictionary<string, Raster>();

            // Load up our input rasters
            foreach (KeyValuePair<string, FileInfo> inp in fisInputs)
                rFISInputs[inp.Key] = new Raster(inp.Value);

            Raster outputRaster = new Raster(rReference, sOutputRaster, new GdalDataType(typeof(double)));
            FISRasterOp theSlopeOp = new FISRasterOp(rFISInputs, sFISRuleFile, outputRaster);

            if (progressHandler != null)
                theSlopeOp.ProgressEvent += progressHandler;

            return theSlopeOp.RunWithOutput();
        }

        /// <summary>
        /// Bin a Raster into a Histogram
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="numberofBins"></param>
        /// <param name="polygonMask"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static Dictionary<string, Histogram> BinRaster(Raster rInput, int numberofBins, Vector polygonMask, string FieldName)
        {
            BinRaster histOp = new BinRaster(rInput, numberofBins, polygonMask, FieldName);
            histOp.Run();
            return histOp.SegHistograms;
        }

        public enum ThresholdOps { LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="fThresholdOp">LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual</param>
        /// <param name="fThreshold">Threshold Value</param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp,
            decimal fThreshold, FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(rInput, fThresholdOp, fThreshold, new Raster(rInput, sOutputRaster));
            return threshOp.RunWithOutput();
        }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="fThresholdOp">LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual</param>
        /// <param name="rThreshold"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp, Raster rThreshold, FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(rInput, fThresholdOp, rThreshold, new Raster(rInput, sOutputRaster));
            return threshOp.RunWithOutput();
        }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="fBottomOp"></param>
        /// <param name="fBottom"></param>
        /// <param name="fTopOp"></param>
        /// <param name="fTop"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fBottomOp, decimal fBottom, ThresholdOps fTopOp, decimal fTop, FileInfo sOutputRaster)
        {
            Threshold threshOp = new Threshold(rInput, fBottomOp, fBottom, fTopOp, fTop, new Raster(rInput, sOutputRaster));
            return threshOp.RunWithOutput();
        }

        /// <summary>
        /// Prior Probability Raster Generation
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="newError"></param>
        /// <param name="oldError"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster CreatePriorProbabilityRaster(Raster rawDoD, Raster propError, FileInfo sOutputRaster)
        {
            PriorProbRaster thePriorProb = new PriorProbRaster(rawDoD, propError, new Raster(rawDoD, sOutputRaster));
            return thePriorProb.RunWithOutput();
        }


        public enum GCDWindowType { Erosion, Deposition, All };
        /// <summary>
        /// Function designed to separate erosion and deposition
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="wType"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster NeighbourCount(Raster rawDoD, GCDWindowType wType, int nMovingWindowWidth, FileInfo sOutputRaster)
        {
            GCDNeighbourCount theCountRaster = new GCDNeighbourCount(rawDoD, new Raster(rawDoD, sOutputRaster, new GdalDataType(typeof(int))), nMovingWindowWidth, wType);
            return theCountRaster.RunWithOutput();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Raster PosteriorProbability(
            Raster rawDoD, 
            Raster priorProb,
            Raster sSpatialCoErosionRaster,
            Raster sSpatialCoDepositionRaster,
            FileInfo sPosteriorRaster,
            FileInfo sConditionalRaster,
            int nMovingWindowWidth,
            int inflectionA,
            int inflectionB,
            EventHandler<int> progressHandler = null)
        {
            PosteriorProbability thePostProb = new PosteriorProbability(rawDoD, priorProb,
                sSpatialCoErosionRaster, sSpatialCoDepositionRaster,
                new Raster(rawDoD, sPosteriorRaster),
                new Raster(rawDoD, sConditionalRaster),                
                nMovingWindowWidth, inflectionA, inflectionB);

            if (progressHandler != null)
                thePostProb.ProgressEvent += progressHandler;

            return thePostProb.RunWithOutput();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrHistPath"></param>
        /// <param name="newError"></param>
        /// <param name="oldError"></param>
        /// <param name="sPriorProbRaster"></param>
        /// <param name="fThreshold"></param>
        /// <returns></returns>
        public static Raster ThresholdDoDProbability(Raster rawDoD, Raster rPriorProb, FileInfo thrHistPath, decimal fThreshold)
        {
            CITThresholdRaster thePriorProb = new CITThresholdRaster(rawDoD, rPriorProb, new Raster(rawDoD, thrHistPath), fThreshold);
            return thePriorProb.RunWithOutput();
        }

        /// <summary>
        /// Build the Pyramids for a raster
        /// </summary>
        /// <param name="rInput"></param>
        public static void BuildPyramids(FileInfo rInput)
        {
            Raster rRa = new Raster(rInput);
            rRa.BuildPyramids("average");
        }


        #region  ////////////////////////////////////    EVERYTHING BELOW HERE IS PRIVATE ////////////////////////////////////


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

        #endregion  
    }

}





