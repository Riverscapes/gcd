using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data.Raster;
using ArcGIS.Core.Internal.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace GCDViewer.GIS
{
    public class MapRenderers
    {
        //public static async Task<RasterLayer> ApplyClassifyRenderer(RasterLayer rasterLayer, int classCount, string colorRampName, bool invert = false)
        //{
        //    if (rasterLayer == null)
        //        return null;

        //    return await QueuedTask.Run(() =>
        //    {
        //        // Ensure statistics exist
        //        Raster raster = rasterLayer.GetRaster();
        //        raster.ComputeStatistics();

        //        // Get min/max from raster info
        //        var rasterInfo = raster.GetRasterInfo();
        //        double minValue = rasterInfo.Minimum;
        //        double maxValue = rasterInfo.Maximum;

        //        // Create class breaks
        //        double interval = (maxValue - minValue) / classCount;
        //        double[] breaks = new double[classCount + 1];
        //        for (int i = 0; i <= classCount; i++)
        //            breaks[i] = minValue + i * interval;

        //        // Get the color ramp from the project style
        //        var styles = Project.Current.GetItems<StyleProjectItem>();
        //        foreach (var style in styles)
        //        {
        //            // Search recursively for all ColorRamp items
        //            var colorRampItems = style.SearchItem(colorRampName);
        //            foreach (var item in colorRampItems)
        //            {
        //                if (item.Item is CIMColorRamp ramp)
        //                    ramps.Add(ramp);
        //            }
        //        }


        //        CIMColorRamp colorRamp = null;
        //        var colorRamps = Project.Current.GetItems<ColorRampStyleItem>();
        //        var styleItem = colorRamps.FirstOrDefault(cr => cr.Name.Equals(colorRampName));
        //        if (styleItem != null)
        //            colorRamp = styleItem.Item as CIMColorRamp;

        //        if (colorRamp == null)
        //            throw new System.Exception($"Color ramp '{colorRampName}' not found");

        //        // Invert colors if requested
        //        if (invert)
        //        {
        //            var colors = colorRamp.Colors.ToList();
        //            colors.Reverse();
        //            colorRamp.Colors = colors.ToArray();
        //        }

        //        // Create the CIM Classify Renderer
        //        var classifyRenderer = new CIMRasterClassifyRenderer
        //        {
        //            Raster = rasterLayer.GetRaster(),
        //            ClassificationMethod = RasterClassificationMethod.EqualInterval,
        //            BreakCount = classCount,
        //            BreakValues = breaks,
        //            ColorRamp = colorRamp,
        //            StretchType = RasterStretchType.None
        //        };

        //        // Apply renderer to the raster layer
        //        rasterLayer.SetRenderer(classifyRenderer);

        //        return rasterLayer;
        //    });
        //}

        //public static async Task<List<CIMColorRamp>> GetAllColorRampsAsync()
        //{
        //    return await QueuedTask.Run(() =>
        //    {
        //        List<CIMColorRamp> ramps = new List<CIMColorRamp>();

        //        // Loop through all style items in the project
        //        var styles = Project.Current.GetItems<StyleProjectItem>();

        //        foreach (var style in styles)
        //        {
        //            // Search recursively for all ColorRamp items
        //            var colorRampItems = style.SearchItem("ColorRamp");
        //            foreach (var item in colorRampItems)
        //            {
        //                if (item.Item is CIMColorRamp ramp)
        //                    ramps.Add(ramp);
        //            }
        //        }

        //        return ramps;
        //    });
        //}

        ///// <summary>
        ///// Apply a classified renderer to a raster layer using a named built-in color ramp.
        ///// </summary>
        ///// <param name="rasterLayer">The raster layer to symbolize.</param>
        ///// <param name="classCount">Number of classes for classification.</param>
        ///// <param name="colorRampName">Name of one of the ArcGIS Pro built-in color ramps.</param>
        //public static void ApplyClassifiedColorRamp(RasterLayer rasterLayer, int classCount, string colorRampName)
        //{
        //    if (rasterLayer == null)
        //        throw new System.ArgumentNullException(nameof(rasterLayer));

        //    QueuedTask.Run(() =>
        //    {
        //        // Get the CIMRasterLayer
        //        CIMRasterLayer cimRasterLayer = rasterLayer.GetDefinition() as CIMRasterLayer;
        //        if (cimRasterLayer == null)
        //            return;

        //        //IEnumerable<StyleProjectItem> items = Project.Current.GetItems<StyleProjectItem>();
        //        //IEnumerable<ColorRampStyleItem> colorRamps = items.SelectMany<StyleProjectItem, ColorRampStyleItem>(style => style is ColorRampStyleItem);


        //        // Get all style items in the project
        //        var styles = Project.Current.GetItems<StyleProjectItem>();

        //        ColorRampStyleItem colorRampItem = null;

        //        // Loop through styles to find a color ramp by name
        //        foreach (var style in styles)
        //        {
        //            var ramps = style.GetItems();
        //            colorRampItem = ramps
        //                .OfType<ColorRampStyleItem>()
        //                .FirstOrDefault(cr => cr.Name.Equals(colorRampName, System.StringComparison.OrdinalIgnoreCase));

        //            if (colorRampItem != null)
        //                break;
        //        }

        //        if (colorRampItem == null)
        //            throw new System.ArgumentException($"Color ramp '{colorRampName}' not found.");
        //        // Get raster min/max
        //        var raster = rasterLayer.GetRaster();
        //        var band = raster.GetBand(0);
        //        double min = 0, max = 0;
        //        band.GetStatistics(out min, out max, out _, out _);

        //        // Compute breaks
        //        double interval = (max - min) / classCount;
        //        List<CIMClassBreak> classBreaks = new List<CIMClassBreak>();
        //        var colorRamp = colorRampItem.ColorRamp;

        //        for (int i = 0; i < classCount; i++)
        //        {
        //            double upper = min + interval * (i + 1);
        //            CIMPolygonSymbol polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(colorRamp.PreviewColors[i * colorRamp.PreviewColors.Count / classCount]);
        //            CIMClassBreak cb = new CIMClassBreak()
        //            {
        //                UpperBound = upper,
        //                Label = $"{min + interval * i:0.##} - {upper:0.##}",
        //                Symbol = polygonSymbol.MakeSymbolReference()
        //            };
        //            classBreaks.Add(cb);
        //        }

        //        // Create the renderer
        //        CIMClassBreaksRenderer renderer = new CIMClassBreaksRenderer()
        //        {
        //            Field = null,
        //            ClassBreakType = ClassBreakType.GraduatedColor,
        //            ClassificationMethod = ClassificationMethod.Manual, // because we specify breaks manually
        //            Breaks = classBreaks.ToArray()
        //        };

        //        // Apply renderer
        //        cimRasterLayer.Renderer = renderer;
        //        rasterLayer.SetDefinition(cimRasterLayer);
        //    });
        //}


        //public static void ApplyStretchedColorRamp(RasterLayer rasterLayer, string colorRampName)
        //{
        //    if (rasterLayer == null)
        //        throw new System.ArgumentNullException(nameof(rasterLayer));

        //    QueuedTask.Run(() =>
        //    {
        //        CIMRasterLayer cimRasterLayer = rasterLayer.GetDefinition() as CIMRasterLayer;
        //        if (cimRasterLayer == null)
        //            return;

        //        // Find color ramp


        //        if (colorRampItem == null)
        //            throw new System.ArgumentException($"Color ramp '{colorRampName}' not found.");

        //        // Create a stretched renderer
        //        CIMStretchRenderer stretchRenderer = new CIMStretchRenderer()
        //        {
        //            ColorRamp = colorRampItem.ColorRamp,
        //            StretchType = StretchType.StandardDeviations, // or MinMax, PercentClip
        //            Gamma = 1.0,
        //            NumberOfStandardDeviations = 2.0, // only used if StretchType = StandardDeviations
        //            UseGamma = false
        //        };

        //        // Apply the renderer
        //        cimRasterLayer.Renderer = stretchRenderer;
        //        rasterLayer.SetDefinition(cimRasterLayer);
        //    });
        //}

        //public ColorRampStyleItem FindColorRampByName(string colorRampName)
        //{
        //    var styles = Project.Current.GetItems<StyleProjectItem>();

        //    ColorRampStyleItem colorRampItem = null;

        //    // Loop through styles to find a color ramp by name
        //    foreach (var style in styles)
        //    {
        //        var ramps = style.GetItems();
        //        colorRampItem = ramps
        //            .OfType<ColorRampStyleItem>()
        //            .FirstOrDefault(cr => cr.Name.Equals(colorRampName, System.StringComparison.OrdinalIgnoreCase));

        //        if (colorRampItem != null)
        //            return colorRampItem;
        //    }

        //    return null;
        //}


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
            });
        }

        private static CIMColorRamp CreateAlgorithmicRamp(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            return new ArcGIS.Core.CIM.CIMLinearContinuousColorRamp
            {
                FromColor = ColorFactory.Instance.CreateRGBColor(r1, g1, b1),
                ToColor = ColorFactory.Instance.CreateRGBColor(r2, g2, b2),
                //Algorithm = ColorRampAlgorithm.LinearContinuous
            };
        }


        public static async Task ApplyRoughnessColorRampAsync(RasterLayer rasterLayer)
        {
            await QueuedTask.Run(() =>
            {
                // Define the breaks and labels
                double[] breaks = { 0, 0.1, 0.25, 0.5, 0.75, 1, 1.5, 2, 3, 5 };
                string[] labels = {
                    "0 to 0.1", "0.1 to 0.25", "0.25 to 0.5", "0.5 to 0.75",
                    "0.75 to 1.0", "1.0 to 1.5", "1.5 to 2.0", "2.0 to 3.0",
                    "3.0 to 5.0", "> 5.0"
                };

                // Define the colors
                List<CIMColor> colors = new List<CIMColor>
                {
                    ColorFactory.Instance.CreateRGBColor(255, 255, 179),
                    ColorFactory.Instance.CreateRGBColor(252, 241, 167),
                    ColorFactory.Instance.CreateRGBColor(252, 230, 157),
                    ColorFactory.Instance.CreateRGBColor(250, 218, 145),
                    ColorFactory.Instance.CreateRGBColor(250, 208, 135),
                    ColorFactory.Instance.CreateRGBColor(237, 191, 126),
                    ColorFactory.Instance.CreateRGBColor(219, 167, 118),
                    ColorFactory.Instance.CreateRGBColor(201, 147, 111),
                    ColorFactory.Instance.CreateRGBColor(184, 127, 106),
                    ColorFactory.Instance.CreateRGBColor(166, 101, 101)
                };

                // Build the class breaks
                List<CIMRasterClassBreak> classBreaks = new List<CIMRasterClassBreak>();
                for (int i = 0; i < breaks.Length; i++)
                {
                    classBreaks.Add(new CIMRasterClassBreak
                    {
                        UpperBound = breaks[i],
                        Label = labels[i],
                        Color = colors[i]
                    });
                }

                // Create the classify colorizer
                var classifyColorizer = new CIMRasterClassifyColorizer
                {
                    ClassificationMethod = ClassificationMethod.Manual,
                    ClassBreaks = classBreaks.ToArray() // use ClassBreaks, not Breaks
                };

                // Apply the colorizer to the raster layer
                rasterLayer.SetColorizer(classifyColorizer);
            });
        }

        public static async Task ApplySlopeDegreesColorRampAsync(RasterLayer rasterLayer)
        {
            await QueuedTask.Run(() =>
            {
                // Define breaks and labels
                double[] breaks = { 0, 2, 5, 10, 15, 25, 35, 45, 60, 80, 90 };
                string[] labels = {
            "0 to 2", "2 to 5", "5 to 10", "10 to 15", "15 to 25",
            "25 to 35", "35 to 45", "45 to 60", "60 to 80", "80 to 90"
        };

                // Define colors (replace with your slope ramp)
                List<CIMColor> colors = CreateSlopeColorRamp(); // Returns List<CIMColor> with 10 entries

                // Build the class breaks
                List<CIMRasterClassBreak> classBreaks = new List<CIMRasterClassBreak>();
                for (int i = 0; i < labels.Length; i++)
                {
                    classBreaks.Add(new CIMRasterClassBreak
                    {
                        UpperBound = breaks[i + 1], // Upper bound is the next break
                        Label = labels[i],
                        Color = colors[i]
                    });
                }

                // Create the classify colorizer
                var classifyColorizer = new CIMRasterClassifyColorizer
                {
                    ClassificationMethod = ClassificationMethod.Manual,
                    ClassBreaks = classBreaks.ToArray()
                };

                // Apply the colorizer
                rasterLayer.SetColorizer(classifyColorizer);
            });
        }

        public static async Task ApplySlopePercentRiseColorRampAsync(RasterLayer rasterLayer)
        {
            await QueuedTask.Run(() =>
            {
                // Define the breaks and labels
                double[] breaks = { 0, 3.5, 8.75, 15, 25, 45, 70, 100, 175, 565 };
                string[] labels = {
            "0 to 3.5%", "3.5% to 8.75%", "8.75% to 15%", "15% to 25%",
            "25% to 45%", "45% to 70%", "70% to 100%", "100% to 175%",
            "175% to 565%", "> 565%"
        };

                // Define the colors for the ramp
                List<CIMColor> colors = CreateSlopeColorRamp(); // Returns List<CIMColor> with 10 entries

                // Build the class breaks
                List<CIMRasterClassBreak> classBreaks = new List<CIMRasterClassBreak>();
                for (int i = 0; i < labels.Length; i++)
                {
                    classBreaks.Add(new CIMRasterClassBreak
                    {
                        UpperBound = breaks[i], // Upper bound is the break value
                        Label = labels[i],
                        Color = colors[i]
                    });
                }

                // Create the classify colorizer
                var classifyColorizer = new CIMRasterClassifyColorizer
                {
                    ClassificationMethod = ClassificationMethod.Manual,
                    ClassBreaks = classBreaks.ToArray()
                };

                // Apply the colorizer to the raster layer
                rasterLayer.SetColorizer(classifyColorizer);
            });
        }

        private static List<CIMColor> CreateSlopeColorRamp()
        {
            var colors = new List<CIMColor> {
                ColorFactory.Instance.CreateRGBColor(255, 235, 176),
                ColorFactory.Instance.CreateRGBColor(255, 219, 135),
                ColorFactory.Instance.CreateRGBColor(255, 202, 97),
                ColorFactory.Instance.CreateRGBColor(255, 186, 59),
                ColorFactory.Instance.CreateRGBColor(255, 170, 0),
                ColorFactory.Instance.CreateRGBColor(255, 128, 0),
                ColorFactory.Instance.CreateRGBColor(255, 85, 0),
                ColorFactory.Instance.CreateRGBColor(255, 42, 0),
                ColorFactory.Instance.CreateRGBColor(161, 120, 120),
                ColorFactory.Instance.CreateRGBColor(130, 130, 130)
            };

            return colors;
        }

        public static async Task ApplyDoDClassifyColorRampAsync(RasterLayer rasterLayer, int classCount, double rampRange = 0)
        {
            await QueuedTask.Run(() =>
            {
                // Determine min/max values
                double rMin = -2.5;
                double rMax = 2.5;

                if (rampRange == 0)
                {
                    // Assuming rasterLayer has statistics; otherwise calculate via ComputeStatistics()
                    var raster = rasterLayer.GetRaster();
                    // TODO: Get Raster Statistics
                    //var stats = raster.GetStatistics(); // returns Dictionary<string, double>
                    //rMin = stats["min"];
                    //rMax = stats["max"];
                }
                else
                {
                    rMin = -rampRange;
                    rMax = rampRange;
                }

                // Handle no-data case
                if (double.IsInfinity(rMin) || double.IsInfinity(rMax) || double.IsNaN(rMin) || double.IsNaN(rMax))
                {
                    var singleBreak = new CIMRasterClassBreak
                    {
                        UpperBound = 0,
                        Label = "No Data (no change detected)",
                        Color = ColorFactory.Instance.CreateRGBColor(255, 255, 255, 0) // transparent white
                    };

                    var colorizer = new CIMRasterClassifyColorizer
                    {
                        ClassificationMethod = ClassificationMethod.Manual,
                        ClassBreaks = new[] { singleBreak }
                    };

                    rasterLayer.SetColorizer(colorizer);
                    return;
                }

                // Create evenly spaced class breaks
                var classBreaks = new List<CIMRasterClassBreak>();
                double step = (rMax - rMin) / classCount;
                List<CIMColor> colors = CreateDoDColorRamp(); // should return classCount CIMColor entries

                for (int i = 0; i < classCount; i++)
                {
                    double upperBound = rMin + step * (i + 1);
                    classBreaks.Add(new CIMRasterClassBreak
                    {
                        UpperBound = upperBound,
                        Label = $"{rMin + step * i:0.##} to {upperBound:0.##}",
                        Color = colors[i]
                    });
                }

                // Create classify colorizer
                var classifyColorizer = new CIMRasterClassifyColorizer
                {
                    ClassificationMethod = ClassificationMethod.Manual,
                    ClassBreaks = classBreaks.ToArray()
                };

                // Apply to raster layer
                rasterLayer.SetColorizer(classifyColorizer);
            });
        }

        private static List<CIMRasterClassBreak> CreateDoDClassBreaks(double dMax, double dMin, int classCount)
        {
            double fMax = Math.Max(Math.Abs(dMax), Math.Abs(dMin));
            double fRange = 2 * fMax;
            double interval = fRange / classCount;
            int iRound = GetMagnitude(fRange);
            interval = Math.Round(interval * Math.Abs(iRound), Math.Abs(iRound) + 2) / Math.Abs(iRound);

            string sFormat;
            if (iRound < 0)
            {
                int decimals = Math.Abs(iRound) + 2;
                sFormat = "#,#0." + new string('0', decimals);
            }
            else
            {
                sFormat = "#,#0.00";
            }

            var classBreaks = new List<CIMRasterClassBreak>();

            for (int i = 0; i < classCount; i++)
            {
                double lower = -fMax + interval * i;
                double upper = -fMax + interval * (i + 1);
                string label;

                if (i == 0)
                    label = "< " + upper.ToString(sFormat);
                else if (i == classCount - 1)
                    label = "> " + lower.ToString(sFormat);
                else
                    label = lower.ToString(sFormat) + " to " + upper.ToString(sFormat);

                classBreaks.Add(new CIMRasterClassBreak
                {
                    UpperBound = upper,
                    Label = label
                    // Color will be assigned separately when building the colorizer
                });
            }

            return classBreaks;
        }

        private static List<CIMColor> CreateDoDColorRamp()
        {
            var colors = new List<CIMColor>
    {
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