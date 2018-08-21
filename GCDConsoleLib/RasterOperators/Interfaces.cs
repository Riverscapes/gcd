using System;
using System.Collections.Generic;
using System.Reflection;
using GCDConsoleLib.Internal.Operators;
using System.IO;
using UnitsNet;
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
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType,
                "Copying Raster",
                progressHandler,
                new object[] {
                rInput, new Raster(rInput, sOutputRaster), rInput.Extent
            });
        }

        /// <summary>
        /// Copy a raster by specifying a new extent
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="newRect">New extent recangle</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster ExtendedCopy(Raster rInput, FileInfo sOutputRaster, ExtentRectangle newRect,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(ExtendedCopy<>), rInput.Datatype.CSType,
                "Copying Raster",
                progressHandler,
                new object[] {
                rInput, new Raster(rInput, sOutputRaster), newRect
            });
        }

        /// <summary>
        /// Add a value to a Raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="dOperand">scalar decimal operand</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Add(Raster rInput, decimal dOperand, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType,
                "Adding decimal value",
                progressHandler,
                new object[] {
                MathOpType.Addition, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Subtract a value from a Raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="dOperand">scalar decimal operand</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Subtract(Raster rInput, decimal dOperand, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType,
                "Subtracting decimal value",
                progressHandler,
                new object[] {
                MathOpType.Subtraction, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Multiply a Raster and a Value
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="dOperand">scalar decimal operand</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Multiply(Raster rInput, decimal dOperand, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType,
                "Multiplying by decimal value",
                progressHandler,
                new object[] {
                MathOpType.Multipication, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Divide a Raster and a Value
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="dOperand">scalar decimal operand</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Divide(Raster rInput, decimal dOperand, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInput.Datatype.CSType,
                "Dividing by decimal value",
                progressHandler,
                new object[] {
                MathOpType.Division, rInput, dOperand, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Add Two Rasters
        /// </summary>
        /// <param name="rInputA">First raster object</param>
        /// <param name="rInputB">Second raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Add(Raster rInputA, Raster rInputB, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                "Adding decimal value",
                progressHandler,
                new object[] {
                MathOpType.Addition, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Substract two rasters
        /// </summary>
        /// <param name="rInputA">First raster object</param>
        /// <param name="rInputB">Second raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Subtract(Raster rInputA, Raster rInputB, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                "Subtracting decimal value",
                progressHandler,
                new object[] {
                MathOpType.Subtraction, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }


        /// <summary>
        /// Substract two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster SubtractWithMask(Raster rInputA, Raster rInputB, Vector PolygonMask,
            FileInfo sOutputRaster, bool RasterizeFirst = true,
            EventHandler<OpStatus> progressHandler = null)
        {
            Raster retval;
            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rInputA, PolygonMask))
                {
                    retval = (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                        "Subtracting with mask",
                        progressHandler,
                        new object[] {
                        MathOpType.Subtraction, rInputA, rInputB, tmp, new Raster(rInputA, sOutputRaster)
                    });
                }
            }
            else
            {
                retval = (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                    "Subtracting with mask",
                    progressHandler,
                    new object[] {
                    MathOpType.Subtraction, rInputA, rInputB, PolygonMask, new Raster(rInputA, sOutputRaster)
                });
            }
            return retval;
        }

        /// <summary>
        /// Multiply two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Multiply(Raster rInputA, Raster rInputB, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                "Multiplying two rasters",
                progressHandler,
                new object[] {
                MathOpType.Multipication, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }

        /// <summary>
        /// Divide two rasters
        /// </summary>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Divide(Raster rInputA, Raster rInputB, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(RasterMath<>), rInputA.Datatype.CSType,
                "Dividing two rasters",
                progressHandler,
                new object[] {
                MathOpType.Division, rInputA, rInputB, new Raster(rInputA, sOutputRaster)
            });
        }



        public enum MultiMathOpType : byte { Maximum, Minimum, Mean, Addition, StandardDeviation, RootSumSquares }
        public enum MultiMathErrOpType : byte { Maximum, Minimum }

        /// <summary>
        /// Maximum of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Maximum(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.Maximum, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);

            op.OpDescription = "Calculating maximum raster";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Root sum squares of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster MultiRootSumSquares(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.RootSumSquares, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating root-sum-square";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Returns the error raster value corresponding to the maximum value of a group of rasters
        /// </summary>
        /// <param name="rasters">Input values to be used for the max and min</param>
        /// <param name="rasters">Error Rasters that will be used for the value of the output raster</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster MaximumErr(List<Raster> rasters, List<Raster> errors, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMathError op = new RasterMultiMathError(MultiMathErrOpType.Maximum, rasters, errors, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating maximum error raster";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Minimum of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Minimum(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.Minimum, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating minimum raster";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Returns the error raster value corresponding to the minimum value of a group of rasters
        /// </summary>
        /// <param name="rasters">Input values to be used for the max and min</param>
        /// <param name="rasters">Error Rasters that will be used for the value of the output raster</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster MinimumErr(List<Raster> rasters, List<Raster> errors, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMathError op = new RasterMultiMathError(MultiMathErrOpType.Minimum, rasters, errors, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating minimum error raster";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Mean of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Mean(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.Mean, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating mean error raster";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Addition of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster MultiAdd(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.Addition, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Adding rasters";
            return op.RunWithOutput();
        }

        /// <summary>
        /// StandardDeviation of a series of rasters
        /// </summary>
        /// <param name="rasters"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster StandardDeviation(List<Raster> rasters, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RasterMultiMath op = new RasterMultiMath(MultiMathOpType.StandardDeviation, rasters, new Raster(rasters[0], sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating standard deviation";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Create a BilinearReseample Raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="outputExtent"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster BilinearResample(Raster rInput, FileInfo sOutputRaster, ExtentRectangle outputExtent,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(BilinearResample<>), rInput.Datatype.CSType,
                "Resampling raster",
                progressHandler,
                new object[] {
                rInput, outputExtent, new Raster(rInput, sOutputRaster)
            });
        }

        /// <summary>
        /// Create a Root Sum Squares Calculation Raster
        /// </summary>
        /// <param name="raster1"></param>
        /// <param name="raster2"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster RootSumSquares(Raster raster1, Raster raster2, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            RootSumSquare op = new RootSumSquare(raster1, raster2, new Raster(raster1, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating root-sum-squares";
            return op.RunWithOutput();
        }

        #endregion

        #region Raster Combination methods (Mosaic, Mask) --------------------------------------------------------------------------------

        /// <summary>
        /// Mosaic a list of Rasters into an output Raster
        /// </summary>
        /// <param name="sInputs"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Mosaic(List<FileInfo> sInputs, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            List<Raster> rlInputs = new List<Raster>();

            foreach (FileInfo sRa in sInputs)
                rlInputs.Add(new Raster(sRa));

            Raster r0 = rlInputs[0];

            return (Raster)GenericRunWithOutput(typeof(Mosaic<>), rlInputs[0].Datatype.CSType,
                "Calculating mosaic",
                progressHandler,
                new object[] {
                rlInputs, new Raster( r0, sOutputRaster)
            });
        }

        /// <summary>
        /// Mask a raster with another raster
        /// </summary>
        /// <param name="rUnmasked"></param>
        /// <param name="rMask"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Mask(Raster rUnmasked, Raster rMask, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            return (Raster)GenericRunWithOutput(typeof(Mask<>), rUnmasked.Datatype.CSType,
                "Masking raster",
                progressHandler,
                new object[] {
                rUnmasked, rMask, new Raster( rUnmasked, sOutputRaster)
            });
        }

        #endregion

        #region Statistics Calculation Operation (DoD stuff and Histogram Binning) --------------------------------------------------------------------------------

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="minLoD"></param>
        /// <param name="units"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static DoDStats GetStatsMinLoD(Raster rawDoD, decimal minLoD, UnitGroup units,
            EventHandler<OpStatus> progressHandler = null)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDodMinLodStats op = new GetDodMinLodStats(rawDoD, minLoD, new DoDStats(cellArea, units));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating MinLoD";
            op.Run();
            return op.Stats;
        }

        /// <summary>
        /// Retrieve the segregated Change Statistics from a pair of DoD rasters that were thresholded using minimum level of detection
        /// THIS IS THE PURE VECTOR APPROACH
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="minLoD">Minimum Level of Detection</param>
        /// <param name="PolygonMask">Vector layer containing the mask polygons</param>
        /// <param name="FieldName">Name of the field in the PolygonMask that contains the distinguishing property on which to group statistics</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <param name="RasterizeFirst">Set to false to allow overlaps (much slower)</param>
        /// <returns></returns>
        public static Dictionary<string, DoDStats> GetStatsMinLoD(Raster rawDoD, decimal minLoD,
            Vector PolygonMask, string FieldName, UnitGroup units,
            EventHandler<OpStatus> progressHandler = null,
            bool RasterizeFirst = true)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDodMinLodStats theStatsOp;

            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rawDoD, PolygonMask, FieldName))
                {
                    theStatsOp = new GetDodMinLodStats(rawDoD, minLoD, new DoDStats(cellArea, units), tmp, FieldName);
                    theStatsOp.AddProgressEvent(progressHandler);
                    theStatsOp.OpDescription = "Calculating MinLoD";
                    theStatsOp.Run();
                }
            }
            else
            {
                theStatsOp = new GetDodMinLodStats(rawDoD, minLoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
                theStatsOp.AddProgressEvent(progressHandler);
                theStatsOp.OpDescription = "Calculating MinLoD";
                theStatsOp.Run();
            }
            return theStatsOp.SegStats;
        }

        /// <summary>
        /// Retrieve the segragated Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static DoDStats GetStatsPropagated(Raster rawDoD, Raster propErrRaster, UnitGroup units,
            EventHandler<OpStatus> progressHandler = null)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDoDPropStats theStatsOp = new GetDoDPropStats(rawDoD, propErrRaster, new DoDStats(cellArea, units));
            theStatsOp.AddProgressEvent(progressHandler);
            theStatsOp.OpDescription = "Calculating propagated stats";
            theStatsOp.Run();
            return theStatsOp.Stats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a propagated error raster
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <param name="PolygonMask">Vector layer containing the mask polygons</param>
        /// <param name="FieldName">Name of the field in the PolygonMask that contains the distinguishing property on which to group statistics</param>
        /// <param name="RasterizeFirst">Set to false to allow overlaps (much slower)</param>
        /// <returns></returns>
        public static Dictionary<string, DoDStats> GetStatsPropagated(Raster rawDoD, Raster propErrRaster,
          Vector PolygonMask, string FieldName, UnitGroup units,
          EventHandler<OpStatus> progressHandler = null,
          bool RasterizeFirst = true)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetDoDPropStats theStatsOp;
            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rawDoD, PolygonMask, FieldName))
                {
                    theStatsOp = new GetDoDPropStats(rawDoD, propErrRaster, new DoDStats(cellArea, units), tmp, FieldName);
                    theStatsOp.AddProgressEvent(progressHandler);
                    theStatsOp.OpDescription = "Calculating propagated stats";
                    theStatsOp.Run();
                }
            }
            else
            {
                theStatsOp = new GetDoDPropStats(rawDoD, propErrRaster, new DoDStats(cellArea, units), PolygonMask, FieldName);
                theStatsOp.AddProgressEvent(progressHandler);
                theStatsOp.OpDescription = "Calculating propagated stats";
                theStatsOp.Run();
            }
            return theStatsOp.SegStats;
        }

        /// <summary>
        /// Retrieve the Change Statistics from a pair of DoD rasters that were thresholded using a probabilistic thresholding
        /// </summary>
        /// <param name="rawDoD">Raw DoD Raster Path</param>
        /// <param name="thrDoD">Thresholded DoD Raster Path</param>
        /// <param name="propErrRaster">Propagated Error Raster Path</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static DoDStats GetStatsProbalistic(Raster rawDoD, Raster thrDoD,
            Raster propErrRaster, UnitGroup units,
            EventHandler<OpStatus> progressHandler = null)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);

            GetChangeStats raw = new GetChangeStats(rawDoD, new DoDStats(cellArea, units));
            GetChangeStats thr = new GetChangeStats(thrDoD, new DoDStats(cellArea, units));
            GetChangeStats err = new GetChangeStats(propErrRaster, thrDoD, new DoDStats(cellArea, units));

            raw.AddProgressEvent(progressHandler);
            thr.AddProgressEvent(progressHandler);
            err.AddProgressEvent(progressHandler);

            raw.OpDescription = "Calculating raw DoD";
            thr.OpDescription = "Calculating thresholded DoD";
            err.OpDescription = "Calculating error DoD";

            raw.Run();
            thr.Run();
            err.Run();

            return new DoDStats(
                raw.Stats.ErosionRaw.GetArea(cellArea), raw.Stats.DepositionRaw.GetArea(cellArea),
                thr.Stats.ErosionRaw.GetArea(cellArea), thr.Stats.DepositionRaw.GetArea(cellArea),
                raw.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), raw.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                thr.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), thr.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
                err.Stats.ErosionRaw.GetVolume(cellArea, units.VertUnit), err.Stats.DepositionRaw.GetVolume(cellArea, units.VertUnit),
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
        /// <param name="progressHandler">Optional Event handler</param>
        /// <param name="RasterizeFirst">Set to false to allow overlaps (much slower)</param>
        /// <returns></returns>
        enum statsType : byte { raw, thr, err };
        public static Dictionary<string, DoDStats> GetStatsProbalistic(Raster rawDoD, Raster thrDoD, Raster propErrRaster,
            Vector PolygonMask, string FieldName, UnitGroup units,
            EventHandler<OpStatus> progressHandler = null,
            bool RasterizeFirst = true)
        {
            Area cellArea = rawDoD.Extent.CellArea(units);
            GetChangeStats raw;
            GetChangeStats thr;
            GetChangeStats err;

            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rawDoD, PolygonMask, FieldName))
                {
                    raw = new GetChangeStats(rawDoD, new DoDStats(cellArea, units), tmp, FieldName);
                    thr = new GetChangeStats(thrDoD, new DoDStats(cellArea, units), tmp, FieldName);
                    err = new GetChangeStats(propErrRaster, thrDoD, new DoDStats(cellArea, units), tmp, FieldName);

                    raw.AddProgressEvent(progressHandler);
                    thr.AddProgressEvent(progressHandler);
                    err.AddProgressEvent(progressHandler);

                    raw.OpDescription = "Calculating raw DoD";
                    thr.OpDescription = "Calculating thresholded DoD";
                    err.OpDescription = "Calculating error DoD";

                    raw.Run();
                    thr.Run();
                    err.Run();
                }
            }
            else
            {
                raw = new GetChangeStats(rawDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
                thr = new GetChangeStats(thrDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);
                err = new GetChangeStats(propErrRaster, thrDoD, new DoDStats(cellArea, units), PolygonMask, FieldName);

                raw.OpDescription = "Calculating raw DoD";
                thr.OpDescription = "Calculating thresholded DoD";
                err.OpDescription = "Calculating error DoD";

                raw.Run();
                thr.Run();
                err.Run();
            }
            return StatsProbabilisticCombine(raw, thr, err, cellArea, units);
        }


        /// <summary>
        /// This is a helper method for the above GetStatsProbalistic
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="thr"></param>
        /// <param name="err"></param>
        /// <param name="cellArea"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public static Dictionary<string, DoDStats> StatsProbabilisticCombine(GetChangeStats raw, GetChangeStats thr, GetChangeStats err,
            Area cellArea, UnitGroup units)
        {
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

                // Note that the raw, thr and err stats calculated above are all stored in 
                // the "raw" object of the returned stats dictionary. So the "thr" and "err"
                // stats are **assigned** to the appropriate types but **originate** from the "raw" part of the object.
                retVal[kvp.Key] = new DoDStats(
                            kvp.Value[statsType.raw].ErosionRaw.GetArea(cellArea),
                            kvp.Value[statsType.raw].DepositionRaw.GetArea(cellArea),
                            kvp.Value[statsType.thr].ErosionRaw.GetArea(cellArea),
                            kvp.Value[statsType.thr].DepositionRaw.GetArea(cellArea),
                            kvp.Value[statsType.raw].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.raw].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.thr].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.thr].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.err].ErosionRaw.GetVolume(cellArea, units.VertUnit),
                            kvp.Value[statsType.err].DepositionRaw.GetVolume(cellArea, units.VertUnit),
                            cellArea, units);

            }

            return retVal;
        }

        /// <summary>
        /// Default histogram generator
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="numberofBins">number of bins to use</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        /// <remarks>The goal of this operation is to bin raster values into a set number of bins.
        /// We need to discuss how those bins are defined. Ideally the first and last bin would
        /// contain zero cell count, so that the caller has confidence that the histogram has
        /// captured the full range of the raster values.</remarks>
        public static Histogram BinRaster(Raster rInput, int numberofBins,
            EventHandler<OpStatus> progressHandler = null)
        {
            BinRaster op = new BinRaster(rInput, numberofBins);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating histograms";
            op.Run();
            return op.theHistogram;
        }


        #endregion  


        #region Error Raster stuff --------------------------------------------------------------------------------

        /// <summary>
        /// Single method error calculation
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputPath"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster CreateErrorRaster(Raster rawDEM, ErrorRasterProperties props, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            CreateErrorRaster theStatsOp = new CreateErrorRaster(rawDEM, props, new Raster(rawDEM, sOutputRaster));
            theStatsOp.AddProgressEvent(progressHandler);
            theStatsOp.OpDescription = "Creating error raster";
            if (progressHandler != null)
                theStatsOp.ProgressEvent += progressHandler;
            return theStatsOp.RunWithOutput();
        }

        /// <summary>
        /// Multimethod error calculation (Pure Vector Method)
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputPath"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <param name="RasterizeFirst">Set to false to allow overlaps (much slower)</param>
        /// <returns></returns>
        public static Raster CreateErrorRaster(Raster rawDEM, Vector PolygonMask, string MaskFieldName,
            Dictionary<string, ErrorRasterProperties> props, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null,
            bool RasterizeFirst = true)
        {
            Raster returnRaster;
            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rawDEM, PolygonMask, MaskFieldName))
                {
                    CreateErrorRaster theStatsOp = new CreateErrorRaster(rawDEM, tmp, MaskFieldName, props, new Raster(rawDEM, sOutputRaster));
                    theStatsOp.AddProgressEvent(progressHandler);
                    theStatsOp.OpDescription = "Creating error raster";
                    returnRaster = theStatsOp.RunWithOutput();
                }
            }
            else
            {
                CreateErrorRaster theStatsOp = new CreateErrorRaster(rawDEM, PolygonMask, MaskFieldName, props, new Raster(rawDEM, sOutputRaster));
                theStatsOp.AddProgressEvent(progressHandler);
                theStatsOp.OpDescription = "Creating error raster";
                returnRaster = theStatsOp.RunWithOutput();
            }
            return returnRaster;

        }


        /// <summary>
        /// Generate a Uniform Raster
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="value">Value for the raster to be</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Uniform<T>(Raster rInput, FileInfo sOutputRaster, T value,
            EventHandler<OpStatus> progressHandler = null)
        {
            UniformRaster<T> op = new UniformRaster<T>(rInput, new Raster(rInput, sOutputRaster), value);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating uniform raster";
            return op.RunWithOutput();
        }

        #endregion  


        /// <summary>
        /// Create a Hillshade raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster Hillshade(Raster rInput, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(int)));
            Hillshade op = new Hillshade(rInput, outputRaster);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating hillshade";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Create a Slope Raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster SlopePercent(Raster rInput, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(double)));
            Slope op = new Slope(rInput, outputRaster, Slope.SlopeType.Percent);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating percent slope";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Create a Slope Raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster SlopeDegrees(Raster rInput, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Raster outputRaster = new Raster(rInput, sOutputRaster, new GdalDataType(typeof(double)));
            Slope op = new Slope(rInput, outputRaster, Slope.SlopeType.Degrees);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating degree slope";
            return op.RunWithOutput();
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
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="eKernel"></param>
        /// <param name="fSize"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster PointDensity(Raster rDEM, Vector vPointCloud, FileInfo sOutputRaster,
            KernelShapes eKernel, decimal fSize, EventHandler<OpStatus> progressHandler = null)
        {
            Raster outputRaster = new Raster(rDEM, sOutputRaster, new GdalDataType(typeof(double)));
            PointDensity op = new PointDensity(rDEM, vPointCloud, outputRaster, eKernel, fSize);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating point Density";
            if (progressHandler != null)
                op.ProgressEvent += progressHandler;

            return op.RunWithOutput();
        }

        /// <summary>
        /// Extract every cell of the raster that intersects with all points on any line of an input vector
        /// </summary>
        /// <param name="rDEM"></param>
        /// <param name="vPointCloud"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="eKernel"></param>
        /// <param name="fSize"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        //public static void LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo sOutCSV, EventHandler<int> progressHandler = null)
        //{
        //    LinearExtractor<double> theExtractOp = new LinearExtractor<double>(vLineShp, rRasters, sOutCSV);

        //    if (progressHandler != null)
        //        theExtractOp.ProgressEvent += progressHandler;

        //    theExtractOp.Run();
        //}

        /// <summary>
        /// Extract the values of the raster that occur under regular intervals of the input vector lines
        /// </summary>
        /// <param name="vLineShp"></param>
        /// <param name="rRasters"></param>
        /// <param name="sOutCSV"></param>
        /// <param name="intervallength"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        public static void LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo sOutCSV,
            decimal intervallength, string sFieldName,
            EventHandler<OpStatus> progressHandler = null)
        {
            GenericRunWithSpacing(typeof(LinearExtractor<>), rRasters[0].Datatype.CSType,
                "Running linear extraction",
                progressHandler,
                new object[] {
                 vLineShp, rRasters, sOutCSV, intervallength, sFieldName
            });
        }

        /// <summary>
        /// Extract the values of the raster that occur under regular intervals of the input vector lines
        /// </summary>
        /// <param name="vLineShp"></param>
        /// <param name="rRasters"></param>
        /// <param name="sOutCSV"></param>
        /// <param name="intervallength"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        public static void LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo sOutCSV,
            decimal intervallength, EventHandler<OpStatus> progressHandler = null)
        {
            LinearExtractor<double> op = new LinearExtractor<double>(vLineShp, rRasters, sOutCSV, intervallength);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Running linear extraction";

            //This is the magic sauce. This method doesn't use the base operator's run method at all.
            op.RunWithSpacing();
        }

        /// <summary>
        /// Create a FIS Raster
        /// </summary>
        /// <param name="fisInputs">Key is FIS input name, value is corresponding raster path</param>
        /// <param name="sFISRuleFile">Path to FIS rule file (*.fis)</param>
        /// <param name="rReference">Reference Raster to use.</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster FISRaster(Dictionary<string, FileInfo> fisInputs, FileInfo sFISRuleFile,
            Raster rReference, FileInfo sOutputRaster, EventHandler<OpStatus> progressHandler = null)
        {
            Dictionary<string, Raster> rFISInputs = new Dictionary<string, Raster>();

            // Load up our input rasters
            foreach (KeyValuePair<string, FileInfo> inp in fisInputs)
                rFISInputs[inp.Key] = new Raster(inp.Value);

            Raster outputRaster = new Raster(rReference, sOutputRaster, new GdalDataType(typeof(double)));
            FISRasterOp op = new FISRasterOp(rFISInputs, sFISRuleFile, outputRaster);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating FIS error surface";

            return op.RunWithOutput();
        }

        /// <summary>
        /// Bin a Raster into a Histogram (pure vector)
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="numberofBins"></param>
        /// <param name="polygonMask"></param>
        /// <param name="FieldName"></param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <param name="RasterizeFirst">Set to false to allow overlaps (much slower)</param>
        /// <returns></returns>
        public static Dictionary<string, Histogram> BinRaster(Raster rInput, int numberofBins,
            Vector polygonMask, string FieldName,
            EventHandler<OpStatus> progressHandler = null,
            bool RasterizeFirst = true)
        {
            BinRaster op;
            if (RasterizeFirst)
            {
                using (VectorRaster tmp = new VectorRaster(rInput, polygonMask, FieldName))
                {
                    op = new BinRaster(rInput, numberofBins, tmp, FieldName);
                    op.AddProgressEvent(progressHandler);
                    op.OpDescription = "Binning Raster";
                    op.Run();
                }
            }
            else
            {
                op = new BinRaster(rInput, numberofBins, polygonMask, FieldName);
                op.AddProgressEvent(progressHandler);
                op.OpDescription = "Binning Raster";
                op.Run();
            }
            return op.SegHistograms;
        }


        public enum ThresholdOps { LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="fThresholdOp">LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual</param>
        /// <param name="fThreshold">Threshold Value</param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp,
            decimal fThreshold, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Threshold op = new Threshold(rInput, fThresholdOp, fThreshold, new Raster(rInput, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Setting null";
            return op.RunWithOutput();
        }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="fThresholdOp">LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual</param>
        /// <param name="rThreshold"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fThresholdOp, Raster rThreshold,
            FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Threshold op = new Threshold(rInput, fThresholdOp, rThreshold, new Raster(rInput, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Setting null";
            return op.RunWithOutput();
        }

        /// <summary>
        /// SetNull based on a requested method
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="fBottomOp"></param>
        /// <param name="fBottom"></param>
        /// <param name="fTopOp"></param>
        /// <param name="fTop"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster SetNull(Raster rInput, ThresholdOps fBottomOp, decimal fBottom,
            ThresholdOps fTopOp, decimal fTop, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            Threshold op = new Threshold(rInput, fBottomOp, fBottom, fTopOp, fTop, new Raster(rInput, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Setting null";
            return op.RunWithOutput();
        }

        /// <summary>
        /// SetNull based on a requested method, after performing Absolute() on the rInput raster 
        /// </summary>
        /// <param name="rInput">Input raster object</param>
        /// <param name="fThresholdOp">LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual</param>
        /// <param name="rThreshold"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster AbsoluteSetNull(Raster rInput, ThresholdOps fThresholdOp, Raster rThreshold,
            FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            ThresholdAbs op = new ThresholdAbs(rInput, fThresholdOp, rThreshold, new Raster(rInput, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Setting absolute null";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Prior Probability Raster Generation
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="newError"></param>
        /// <param name="oldError"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster CreatePriorProbabilityRaster(Raster rawDoD, Raster propError, FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            PriorProbRaster op = new PriorProbRaster(rawDoD, propError, new Raster(rawDoD, sOutputRaster));
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Creating prior probability raster";
            return op.RunWithOutput();
        }


        public enum GCDWindowType { Erosion, Deposition, All };
        /// <summary>
        /// Function designed to separate erosion and deposition
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="wType"></param>
        /// <param name="sOutputRaster">Output raster fileInfo object</param>
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster NeighbourCount(Raster rawDoD, GCDWindowType wType, int nMovingWindowWidth,
            FileInfo sOutputRaster,
            EventHandler<OpStatus> progressHandler = null)
        {
            GCDNeighbourCount op = new GCDNeighbourCount(rawDoD, new Raster(rawDoD, sOutputRaster, new GdalDataType(typeof(int))), nMovingWindowWidth, wType);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Counting nearest neighbors";
            return op.RunWithOutput();
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
            int xMin,
            int xMax,
            EventHandler<OpStatus> progressHandler = null)
        {
            PosteriorProbability op = new PosteriorProbability(rawDoD, priorProb,
                sSpatialCoErosionRaster, sSpatialCoDepositionRaster,
                new Raster(rawDoD, sPosteriorRaster),
                new Raster(rawDoD, sConditionalRaster),
                xMin, xMax);

            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating posterior probability";

            if (progressHandler != null)
                op.ProgressEvent += progressHandler;

            return op.RunWithOutput();
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
        /// <param name="progressHandler">Optional Event handler</param>
        /// <returns></returns>
        public static Raster ThresholdDoDProbability(Raster rawDoD, Raster rPriorProb, FileInfo thrHistPath, decimal fThreshold,
            EventHandler<OpStatus> progressHandler = null)
        {
            CITThresholdRaster op = new CITThresholdRaster(rawDoD, rPriorProb, new Raster(rawDoD, thrHistPath), fThreshold);
            op.AddProgressEvent(progressHandler);
            op.OpDescription = "Calculating thresholded DoD probability";
            return op.RunWithOutput();
        }

        /// <summary>
        /// Build the Pyramids for a raster
        /// </summary>
        /// <param name="rInput">Input raster object</param>
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
        private static object GenericRun(Type generic, Type innerType, string sDesc,
            EventHandler<OpStatus> progressHandler, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("Run",
                BindingFlags.Public | BindingFlags.Instance);

            if (progressHandler != null)
            {
                MethodInfo addProgressMethod = myGenericClass.GetType().GetMethod("AddProgressEvent",
                    BindingFlags.Public | BindingFlags.Instance);
                addProgressMethod.Invoke(myGenericClass, new object[] { progressHandler });
            }

            TrySetProperty(myGenericClass, "OpDescription", sDesc);

            method.Invoke(myGenericClass, null);
            return myGenericClass;
        }


        private static object GenericRunWithOutput(Type generic, Type innerType, string sDesc,
            EventHandler<OpStatus> progressHandler, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("RunWithOutput",
                BindingFlags.Public | BindingFlags.Instance);

            if (progressHandler != null)
            {
                MethodInfo addProgressMethod = myGenericClass.GetType().GetMethod("AddProgressEvent",
                    BindingFlags.Public | BindingFlags.Instance);
                addProgressMethod.Invoke(myGenericClass, new object[] { progressHandler });
            }

            TrySetProperty(myGenericClass, "OpDescription", sDesc);

            return method.Invoke(myGenericClass, null);

        }
        /// <summary>
        /// The linear extractor is a special case and needs its own method
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object GenericRunWithSpacing(Type generic, Type innerType, string sDesc,
            EventHandler<OpStatus> progressHandler, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType, args);
            MethodInfo method = myGenericClass.GetType().GetMethod("RunWithSpacing",
                BindingFlags.Public | BindingFlags.Instance);

            if (progressHandler != null)
            {
                MethodInfo addProgressMethod = myGenericClass.GetType().GetMethod("AddProgressEvent",
                    BindingFlags.Public | BindingFlags.Instance);
                addProgressMethod.Invoke(myGenericClass, new object[] { progressHandler });
            }

            TrySetProperty(myGenericClass, "OpDescription", sDesc);

            return method.Invoke(myGenericClass, null);

        }

        /// <summary>
        /// Setting a property on a generic object is tricky. This will set the property if it can find it.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }

        #endregion  
    }

}