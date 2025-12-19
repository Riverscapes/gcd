using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data.Raster;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Core.Internal.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace GCDViewer.GIS
{
    public class MapRenderers
    {
        /// <summary>
        /// Apply one of the built-in ESRI color ramps to a raster layer
        /// </summary>
        /// <param name="rasterLayer"></param>
        /// <param name="rampName"></param>
        /// <remarks>Strategy learned from one of the ESRI SDK examples on GitHub
        /// https://github.com/Esri/arcgis-pro-sdk-community-samples/blob/5c532b40b33b3b5474c5235c8768e56773ff7e9f/Map-Authoring/CIMExamples/CreateCIMRasterStretchColorizerFromScratch.cs#L39
        /// </remarks>
        public static async void ApplyStretchRenderer(RasterLayer rasterLayer, string rampName, Tuple<double, double> range = null)
        {
            double min = 0;
            double max = 0;

            if (range is null)
            {
                int attempts = 0;
                while (attempts < 2)
                {
                    attempts++;

                    if (attempts > 1)
                    {
                        // Calculating stats is slow. Only do it if the process has failed one time already.
                        var calcParams = Geoprocessing.MakeValueArray(rasterLayer);
                        await Geoprocessing.ExecuteToolAsync("CalculateStatistics_management", calcParams);
                    }

                    var parametersMin = Geoprocessing.MakeValueArray(rasterLayer, "MINIMUM");
                    var parametersMax = Geoprocessing.MakeValueArray(rasterLayer, "MAXIMUM");
                    var minRes = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", parametersMin);
                    var maxRes = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", parametersMax);

                    if (minRes.ErrorCode == 0 || maxRes.ErrorCode == 0)
                    {
                        min = Convert.ToDouble(minRes.Values[0]);
                        max = Convert.ToDouble(maxRes.Values[0]);
                        attempts = 99;
                    }
                }
            }
            else
            {
                max = range.Item2;
                min = range.Item1;
            }

            var cimColorRamp = GetNamedColorRamp(rampName);

            // Create a stretch colorizer
            var stretchColorizer = new CIMRasterStretchColorizer
            {
                ColorRamp = cimColorRamp,
                StretchType = RasterStretchType.MinimumMaximum,
                UseGammaStretch = false,
                GammaValue = 1.0,
                StretchClasses = new CIMRasterStretchClass[2]
            };

            stretchColorizer.StretchClasses[0] = new CIMRasterStretchClass() { Value = min, Label = min.ToString() };
            stretchColorizer.StretchClasses[1] = new CIMRasterStretchClass() { Value = max, Label = max.ToString() };

            rasterLayer.SetColorizer(stretchColorizer);
        }

        public static CIMColorRamp GetNamedColorRamp(string rampName)
        {
            var style = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name.Equals("ArcGIS Colors", StringComparison.OrdinalIgnoreCase));
            var colorRamps = style.SearchColorRamps(rampName);

            foreach (var ramp in colorRamps)
            {
                if (string.Compare(ramp.Name, rampName, true) == 0)
                {
                    return ramp.ColorRamp;
                }
            }

            throw new Exception(String.Format("Failed to find built-in colour ramp called {0}", rampName));
        }

        public static async Task ApplyDEMColorRampAsync(RasterLayer rasterLayer)
        {
            await QueuedTask.Run(() =>
            {
                // Multipart color ramp
                var ramps = new List<CIMColorRamp>
                {
                    CreateAlgorithmicRamp(255, 235, 176, 38, 115, 0),
                    CreateAlgorithmicRamp(38, 115, 0, 115, 77, 0),
                    CreateAlgorithmicRamp(115, 77, 0, 255, 255, 255)
                };

                var multiRamp = new CIMMultipartColorRamp
                {
                    ColorRamps = ramps.ToArray()
                };

                // Create stretch colorizer
                var stretch = new CIMRasterStretchColorizer
                {
                    ColorRamp = multiRamp,
                    StretchType = RasterStretchType.MinimumMaximum,
                    UseGammaStretch = false,
                    GammaValue = 1.0
                };

                rasterLayer.SetColorizer(stretch);

                // Get the current CIM definition of the raster layer
                var cimRasterLayer = rasterLayer.GetDefinition() as CIMRasterLayer;

                if (cimRasterLayer != null)
                {
                    cimRasterLayer.ShowLegends = true;

                    // OPTIONAL: Ensure the layer is expanded by default in the TOC
                    // If you want the legend to show immediately without clicking the triangle
                    // cimRasterLayer.Expanded = true;

                    // Apply the modified definition back to the layer
                    rasterLayer.SetDefinition(cimRasterLayer);
                }
            });
        }

        private static CIMColorRamp CreateAlgorithmicRamp(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            return new ArcGIS.Core.CIM.CIMLinearContinuousColorRamp
            {
                FromColor = ColorFactory.Instance.CreateRGBColor(r1, g1, b1),
                ToColor = ColorFactory.Instance.CreateRGBColor(r2, g2, b2),
                PrimitiveName = "Custom Ramp",
                //Algorithm = ColorRampAlgorithm.LinearContinuous
            };
        }

        /// <summary>
        /// Applies a named, pre-defined color ramp from the project styles to a RasterLayer.
        /// </summary>
        /// <param name="rasterLayer">The RasterLayer to symbolize.</param>
        /// <param name="colorRampName">The name of the color ramp (e.g., "Elevation #1", "Green-Yellow-Red").</param>
        /// <param name="styleName">The name of the style containing the color ramp (e.g., "ArcGIS Colors").</param>
        public static async Task ApplyNamedColorRampToRasterAsync(
            RasterLayer rasterLayer,
            string colorRampName,
            string styleName = "ArcGIS Colors")
        {
            // 1. Find the Style in the current Project
            StyleProjectItem style =
                Project.Current.GetItems<StyleProjectItem>()
                    .FirstOrDefault(s => s.Name.Equals(styleName, System.StringComparison.OrdinalIgnoreCase));

            if (style == null)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Style '{styleName}' not found in the project.", "Symbology Error");
                return;
            }

            // 2. Search for the Color Ramp by name (Must be on the QueuedTask.Run thread)
            var colorRampList = await QueuedTask.Run(() => style.SearchColorRamps(colorRampName));

            if (colorRampList == null || colorRampList.Count == 0)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Color Ramp '{colorRampName}' not found in style '{styleName}'.", "Symbology Error");
                return;
            }

            // Retrieve the CIMColorRamp object from the first result
            CIMColorRamp cimColorRamp = colorRampList[0].ColorRamp;

            // 3. Create and Apply the Raster Stretch Colorizer (Must be on the QueuedTask.Run thread)
            await QueuedTask.Run(() =>
            {
                var stretchColorizer = new CIMRasterStretchColorizer
                {
                    ColorRamp = cimColorRamp,
                    // Configure the stretch type (MinimumMaximum is a common default for DEMs)
                    StretchType = RasterStretchType.MinimumMaximum,
                    UseGammaStretch = false,
                    GammaValue = 1.0
                };

                // Apply the new colorizer to the layer
                rasterLayer.SetColorizer(stretchColorizer);

                var cimRasterLayer = rasterLayer.GetDefinition() as CIMRasterLayer;
                if (cimRasterLayer == null)
                    return;

                cimRasterLayer.Colorizer = new CIMRasterStretchColorizer
                {
                    ColorRamp = cimColorRamp,
                    StretchType = RasterStretchType.MinimumMaximum
                };

                cimRasterLayer.ShowLegends = true;

                rasterLayer.SetDefinition(cimRasterLayer);
            });
        }

        public static async Task<(double? min, double? max)> GetRasterMinMax(IEnumerable<ProjectTree.Raster> rasters)
        {
            double? minElevation = null;
            double? maxElevation = null;

            foreach (var raster in rasters)
            {
                var minParams = Geoprocessing.MakeValueArray(raster.GISPath, "MINIMUM");
                var maxParams = Geoprocessing.MakeValueArray(raster.GISPath, "MAXIMUM");

                var minResult = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", minParams);
                var maxResult = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", maxParams);

                if (minResult.IsFailed || maxResult.IsFailed)
                    continue;

                // GP tools return strings
                double rasterMin = double.Parse(minResult.ReturnValue, CultureInfo.InvariantCulture);
                double rasterMax = double.Parse(maxResult.ReturnValue, CultureInfo.InvariantCulture);

                if (!minElevation.HasValue || rasterMin < minElevation)
                    minElevation = rasterMin;

                if (!maxElevation.HasValue || rasterMax > maxElevation)
                    maxElevation = rasterMax;
            }

            return (minElevation, maxElevation);
        }

        public static void ApplyDoDClassifyColorRampAsync(RasterLayer rasterLayer, double rampRange)
        {
            double maxRange = Math.Abs(rampRange);
            double minRange = -1 * maxRange;

            var colors = CreateDoDColorRamp();
            var upperLabels = CreateDoDClassBreaks(minRange, maxRange, colors.Count);

            var classBreaks = new List<CIMRasterClassBreak>();
            for (int i = 0; i < colors.Count; i++)
            {
                classBreaks.Add(new CIMRasterClassBreak
                {
                    UpperBound = upperLabels[i].Item1,
                    Label = upperLabels[i].Item2,
                    Color = colors[i]
                });
            }

            //        double step = (maxRange - minRange) / colors.Count;

            //        for (int i = 0; i<colors.Count; i++)
            //        {
            //            double upperBound = minRange + step * (i + 1);
            //    classBreaks.Add(new CIMRasterClassBreak
            //            {
            //                UpperBound = upperBound,
            //                Label = $"{minRange + step * i:0.##} to {upperBound:0.##}",
            //                Color = colors[i]
            //});
            //        }

            // Create classify colorizer
            var classifyColorizer = new CIMRasterClassifyColorizer
            {
                ClassificationMethod = ClassificationMethod.Manual,
                ClassBreaks = classBreaks.ToArray()
            };

            // Apply to raster layer
            rasterLayer.SetColorizer(classifyColorizer);
        }

        /// <summary>
        /// Creates symmetric raster class breaks around zero for DoD-style rasters.
        /// The range is rounded outward to 2 significant figures.
        /// </summary>
        public static List<Tuple<double, string>> CreateDoDClassBreaks(double dMax, double dMin, int classCount)
        {
            if (classCount <= 0)
                throw new ArgumentException("classCount must be greater than zero");

            // 1. Determine symmetric max and round outward to 2 significant figures
            double rawMax = Math.Max(Math.Abs(dMax), Math.Abs(dMin));
            double fMax = RoundUpToSignificantFigures(rawMax, 1);

            double fRange = 2 * fMax;

            // 2. Calculate interval
            double rawInterval = fRange / classCount;

            // Determine decimal precision based on interval magnitude
            int decimals = Math.Max(0, -(int)Math.Floor(Math.Log10(rawInterval)) + 1);
            double interval = Math.Round(rawInterval, decimals);

            // 3. Label format
            string format = "#,#0." + new string('0', decimals);

            var result = new List<Tuple<double, string>>(classCount);

            double lower = -fMax;

            // 4. Build class breaks
            for (int i = 0; i < classCount; i++)
            {
                double upper = (i == classCount - 1)
                    ? fMax            // force exact end to avoid rounding drift
                    : lower + interval;

                string label;
                if (i == 0)
                    label = "< " + upper.ToString(format);
                else if (i == classCount - 1)
                    label = "> " + lower.ToString(format);
                else
                    label = $"{lower.ToString(format)} to {upper.ToString(format)}";

                // Hack, when the bin edge is extremely close to zero it messes up the class break
                if (Math.Abs(upper) < 0.00001)
                    upper = 0;

                label = label.Replace("-0.00", "0.00");

                System.Diagnostics.Debug.Print(string.Format("Upper: {0}, Label: {1}", upper, label));

                result.Add(new Tuple<double, string>(upper, label));
                lower = upper;
            }

            return result;
        }

        /// <summary>
        /// Rounds a value away from zero to the specified number of significant figures.
        /// </summary>
        private static double RoundUpToSignificantFigures(double value, int sigFigs)
        {
            if (value == 0)
                return 0;

            double scale = Math.Pow(
                10,
                sigFigs - 1 - Math.Floor(Math.Log10(Math.Abs(value)))
            );

            return Math.Ceiling(value * scale) / scale;
        }

        private static List<CIMColor> CreateDoDColorRamp()
        {
            var colors = new List<CIMColor> {
                ColorFactory.Instance.CreateRGBColor(230, 0, 0),
                ColorFactory.Instance.CreateRGBColor(235, 45, 23),
                ColorFactory.Instance.CreateRGBColor(240, 67, 41),
                ColorFactory.Instance.CreateRGBColor(242, 88, 61),
                ColorFactory.Instance.CreateRGBColor(245, 108, 81),
                ColorFactory.Instance.CreateRGBColor(245, 131, 105),
                ColorFactory.Instance.CreateRGBColor(245, 151, 130),
                ColorFactory.Instance.CreateRGBColor(242, 171, 155),
                ColorFactory.Instance.CreateRGBColor(237, 190, 180),
                ColorFactory.Instance.CreateRGBColor(230, 208, 207),
                ColorFactory.Instance.CreateRGBColor(218, 218, 224),
                ColorFactory.Instance.CreateRGBColor(197, 201, 219),
                ColorFactory.Instance.CreateRGBColor(176, 183, 214),
                ColorFactory.Instance.CreateRGBColor(155, 166, 207),
                ColorFactory.Instance.CreateRGBColor(135, 150, 201),
                ColorFactory.Instance.CreateRGBColor(110, 131, 194),
                ColorFactory.Instance.CreateRGBColor(92, 118, 189),
                ColorFactory.Instance.CreateRGBColor(72, 105, 184),
                ColorFactory.Instance.CreateRGBColor(49, 91, 176),
                ColorFactory.Instance.CreateRGBColor(2, 77, 168)
            };

            return colors;
        }

        private static int GetMagnitude(double range, int iMagnitudeMinimum = -4, int iMagnitudeMaximum = 4)
        {
            for (int i = iMagnitudeMinimum; i <= iMagnitudeMaximum; i++)
            {
                double tempVal = range / Math.Pow(10, i);
                if (Math.Floor(Math.Abs(tempVal)) >= 1 & Math.Floor(Math.Abs(tempVal)) < 10)
                {
                    if (i == 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return i;
                    }
                }
            }

            return 1;
        }


        //If a raster dataset has statistics, you can create a raster layer and get these statistics by accessing the colorizer.
        //       public  await QueuedTask.Run(() =>
        //{
        //            //Accessing the raster layer
        //            var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BasicRasterLayer>().FirstOrDefault();
        //            //Getting the colorizer
        //            var colorizer = lyr.GetColorizer() as CIMRasterStretchColorizer;
        //            //Accessing the statistics
        //            var stats = colorizer.StretchStats;
        //            var max = stats.max;
        //            var min = stats.min;
        //        });
    }
}