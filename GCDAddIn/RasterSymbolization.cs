using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using GCDConsoleLib;

namespace GCDAddIn
{
    public class RasterSymbolization
    {
        public static IRasterRenderer CreateESRIDefinedContinuousRenderer(GCDConsoleLib.Raster gRaster, int iClassCount, string sColorRampName, bool bInvert = false)
        {
            try
            {
                gRaster.ComputeStatistics();
                decimal maxValue = gRaster.GetStatistics()["max"];
                int iRound = GetMagnitude(maxValue);
                double maxValueRounded = Math.Round((double)maxValue, Math.Abs(iRound));

                RasterStretchColorRampRenderer stretchRenderer = new RasterStretchColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)stretchRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();
                rasterRenderer.Raster = raster;
                IColorRamp pColorRamp = null;
                IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(out pColorRamp, sColorRampName);
                IRasterRendererColorRamp pRenderColorRamp = (IRasterRendererColorRamp)rasterRenderer;
                pRenderColorRamp.ColorScheme = pStyleItem.Name;
                IRasterStretchMinMax pStretchInfo = (IRasterStretchMinMax)stretchRenderer;
                pStretchInfo.CustomStretchMin = 0;
                pStretchInfo.CustomStretchMax = maxValueRounded;
                pStretchInfo.UseCustomStretchMinMax = true;
                stretchRenderer.LabelHigh = maxValueRounded.ToString();
                stretchRenderer.LabelLow = "0.0";
                if (bInvert)
                {
                    IRasterStretch2 pStretch = (IRasterStretch2)stretchRenderer;
                    pStretch.Invert = true;
                }

                rasterRenderer.Update();
                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static IStyleGalleryItem GetESRIStyleColorRamp(out IColorRamp pColorRamp, string sColorRampName)
        {
            IStyleGallery pStyleGallery = new ESRI.ArcGIS.Framework.StyleGalleryClass();
            IStyleGalleryStorage pStyleStorage;
            pStyleStorage = pStyleGallery as IStyleGalleryStorage;
            string pStylePath = pStyleStorage.DefaultStylePath + "ESRI.style";
            pStyleStorage.AddFile(pStylePath);
            ESRI.ArcGIS.esriSystem.IEnumBSTR eESRIRampCategories = pStyleGallery.Categories["Color Ramps"];
            string sESRIRampCategoryName = eESRIRampCategories.Next();
            IStyleGalleryItem pStyleItem;

            while (!string.IsNullOrEmpty(sESRIRampCategoryName))
            {
                IEnumStyleGalleryItem eESRIColorRamps = pStyleGallery.Items["Color Ramps", pStylePath, sESRIRampCategoryName];
                pStyleItem = eESRIColorRamps.Next();
                while (pStyleItem != null)
                {
                    if (string.Compare(pStyleItem.Name, sColorRampName) == 0)
                    {
                        pColorRamp = (IColorRamp)pStyleItem.Item;
                        return pStyleItem;
                    }

                    pStyleItem = eESRIColorRamps.Next();
                }

                sESRIRampCategoryName = eESRIRampCategories.Next();
            }

            throw new Exception(string.Format("Cannot find the color ramp with the name '{0}'", sColorRampName));
        }

        public static IAlgorithmicColorRamp CreateAlgorithmicColorRamp(IColor pStartColor, IColor pEndColor, esriColorRampAlgorithm eColorAlgorithm = esriColorRampAlgorithm.esriHSVAlgorithm, int iSize = 500)
        {
            IAlgorithmicColorRamp pAlgorithmicColorRamp = new AlgorithmicColorRampClass();
            pAlgorithmicColorRamp.FromColor = pStartColor;
            pAlgorithmicColorRamp.ToColor = pEndColor;
            pAlgorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            pAlgorithmicColorRamp.Size = iSize;
            bool bOK = true;
            pAlgorithmicColorRamp.CreateRamp(out bOK);
            return pAlgorithmicColorRamp;
        }

        public static IMultiPartColorRamp CreateMultiPartColorRamp(List<IColorRamp> lColorRamps, int iSize = 500)
        {
            IMultiPartColorRamp pMultiPartColorRamp = new MultiPartColorRampClass();
            foreach (IColorRamp pColorRamp in lColorRamps)
            {
                pMultiPartColorRamp.AddRamp(pColorRamp);
            }

            pMultiPartColorRamp.Size = iSize;
            bool bOK = true;
            pMultiPartColorRamp.CreateRamp(out bOK);
            return pMultiPartColorRamp;
        }

        public static IRasterRenderer CreateContinuousRenderer(Raster gRaster, IColorRamp pColorRamp, bool bInvert = false)
        {
            try
            {
                gRaster.ComputeStatistics();
                Dictionary<string, decimal> stats = gRaster.GetStatistics();
                double rMin = (double)stats["min"];
                double rMax = (double)stats["max"];

                RasterStretchColorRampRenderer stretchRenderer = new RasterStretchColorRampRenderer();
                IRasterRenderer rasterRenderer = (IRasterRenderer)stretchRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();
                rasterRenderer.Raster = raster;

                IRasterRendererColorRamp pRenderColorRamp = (IRasterRendererColorRamp)rasterRenderer;
                pRenderColorRamp.ColorRamp = pColorRamp;
                int iRound = GetMagnitude(rMin);
                stretchRenderer.LabelHigh = Math.Round(rMax, Math.Abs(iRound)).ToString();
                stretchRenderer.LabelLow = Math.Round(rMin, Math.Abs(iRound)).ToString();

                if (bInvert)
                {
                    IRasterStretch2 pStretch = (IRasterStretch2)stretchRenderer;
                    pStretch.Invert = true;
                }

                rasterRenderer.Update();
                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static IRasterRenderer CreateClassifyRenderer(GCDConsoleLib.Raster gRaster, int iClassCount, string sColorRampName, bool bInvert = false)
        {
            gRaster.ComputeStatistics();
            decimal maxValue = gRaster.GetStatistics()["max"];

            IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
            IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
            IFillSymbol fillSymbol = new SimpleFillSymbol();
            IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
            IRaster raster = rasterDataset.CreateDefaultRaster();
            rasterRenderer.Raster = raster;
            IColorRamp pColorRamp = null;
            IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(out pColorRamp, sColorRampName);
            classifyRenderer.ClassCount = iClassCount;
            rasterRenderer.Update();
            CreateClassBreaks((double)maxValue, iClassCount, classifyRenderer);
            pColorRamp.Size = iClassCount;
            bool bSucess = true;
            pColorRamp.CreateRamp(out bSucess);
            List<IColor> lColors = new List<IColor>();

            for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
            {
                lColors.Add(pColorRamp.Color[i]);
            }

            if (bInvert)
            {
                lColors.Reverse();
            }

            for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
            {
                fillSymbol.Color = lColors[i];
                classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
            }

            return rasterRenderer;
        }


        public static IRasterRenderer CreateClassifyRenderer(GCDConsoleLib.Raster gRaster, int iClassCount, string sColorRampName, double dMax, bool bInvert = false)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                IFillSymbol fillSymbol = new SimpleFillSymbol();
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();

                rasterRenderer.Raster = raster;
                IColorRamp pColorRamp;
                IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(out pColorRamp, sColorRampName);
                classifyRenderer.ClassCount = iClassCount;
                rasterRenderer.Update();
                CreateClassBreaks(dMax, iClassCount, classifyRenderer);
                pColorRamp.Size = iClassCount;
                bool bOK = false;
                pColorRamp.CreateRamp(out bOK);
                List<IColor> lColors = new List<IColor>();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    lColors.Add(pColorRamp.Color[i]);
                }

                if (bInvert)
                {
                    lColors.Reverse();
                }

                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    fillSymbol.Color = lColors[i];
                    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static int GetMagnitude(double range, int iMagnitudeMinimum = -4, int iMagnitudeMaximum = 4)
        {
            for (int i = iMagnitudeMinimum; i <= iMagnitudeMaximum; i += 1)
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

        private static int GetMagnitude(decimal range, int iMagnitudeMinimum = -4, int iMagnitudeMaximum = 4)
        {
            return GetMagnitude((double)range, iMagnitudeMinimum, iMagnitudeMaximum);
        }

        private static double GetFormattedRange(double fRange)
        {
            int iRound = GetMagnitude(fRange);
            return fRange = Math.Round(fRange * 2, Math.Abs(iRound) + 1, MidpointRounding.AwayFromZero) / 2;
        }

        public static void CreateClassBreaks(double maxRasterValue, int iClassCount, IRasterClassifyColorRampRenderer pRasterClassify)
        {
            maxRasterValue = GetFormattedRange(maxRasterValue);
            double interval = maxRasterValue / iClassCount;
            interval = GetFormattedRange(interval);

            int iRound = GetMagnitude(maxRasterValue);
            interval = Math.Round(interval * Math.Abs(iRound), Math.Abs(iRound) + 1) / Math.Abs(iRound);

            string sFormat = "#,#0";
            if (iRound < 0)
            {
                sFormat = string.Format("{0}.{1}", sFormat, new string('0', Math.Abs(iRound) + 1));
            }
            else
            {
                sFormat = "#,#0.00";
            }

            if (pRasterClassify.ClassCount != iClassCount)
                pRasterClassify.ClassCount = iClassCount;

            for (int i = 0; i <= iClassCount; i++)
            {
                double fBreak = Math.Round(interval * i, iRound);
                pRasterClassify.Break[i] = fBreak;

                if (i < iClassCount - 1)
                {
                    pRasterClassify.Label[i] = string.Format("{0} to {1}", fBreak.ToString(sFormat), Math.Round(fBreak + interval, iRound).ToString(sFormat));
                }
                else if (i == iClassCount - 1)
                {
                    pRasterClassify.Label[i] = "> " + fBreak.ToString(sFormat);
                }
            }
        }

        public static IRasterRenderer CreateGrainSizeStatisticColorRamp(GCDConsoleLib.Raster gRaster, UnitsNet.Units.LengthUnit eUnits)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();

                rasterRenderer.Raster = raster;
                classifyRenderer.ClassCount = 5;
                rasterRenderer.Update();
                classifyRenderer.Break[0] = 0;
                classifyRenderer.Label[0] = "Fines, Sand (0 to 2 mm)";
                classifyRenderer.Break[1] = UnitsNet.Length.From(2, UnitsNet.Units.LengthUnit.Millimeter).As(eUnits);
                classifyRenderer.Label[1] = "Fine Gravel (2 mm to 16 mm)";
                classifyRenderer.Break[2] = UnitsNet.Length.From(16, UnitsNet.Units.LengthUnit.Millimeter).As(eUnits);
                classifyRenderer.Label[2] = "Coarse Gravel (16 mm to 64 mm)";
                classifyRenderer.Break[3] = UnitsNet.Length.From(64, UnitsNet.Units.LengthUnit.Millimeter).As(eUnits);
                classifyRenderer.Label[3] = "Cobbles (64 mm to 256 mm)";
                classifyRenderer.Break[4] = UnitsNet.Length.From(256, UnitsNet.Units.LengthUnit.Millimeter).As(eUnits);
                classifyRenderer.Label[4] = "Boulders (> 256 mm)";

                List<RgbColor> lColors = new List<RgbColor>();
                lColors.Add(CreateRGBColor(194, 82, 60));
                lColors.Add(CreateRGBColor(240, 180, 17));
                lColors.Add(CreateRGBColor(123, 237, 0));
                lColors.Add(CreateRGBColor(27, 168, 124));
                lColors.Add(CreateRGBColor(11, 44, 122));

                IFillSymbol fillSymbol = new SimpleFillSymbol();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    fillSymbol.Color = lColors[i];
                    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static IRasterRenderer CreateDEMColorRamp(Raster gRaster)
        {
            IColor pStartColor = (IColor)CreateRGBColor(255, 235, 176);
            IColor pEndColor = (IColor)CreateRGBColor(38, 115, 0);
            IAlgorithmicColorRamp pColorRamp1 = CreateAlgorithmicColorRamp(pStartColor, pEndColor);

            pStartColor = (IColor)CreateRGBColor(38, 115, 0);
            pEndColor = (IColor)CreateRGBColor(115, 77, 0);
            IAlgorithmicColorRamp pColorRamp2 = CreateAlgorithmicColorRamp(pStartColor, pEndColor);

            pStartColor = (IColor)CreateRGBColor(115, 77, 0);
            pEndColor = (IColor)CreateRGBColor(255, 255, 255);
            IAlgorithmicColorRamp pColorRamp3 = CreateAlgorithmicColorRamp(pStartColor, pEndColor);
            List<IColorRamp> lColorRamps = new List<IColorRamp>(new IColorRamp[] { pColorRamp1, pColorRamp2, pColorRamp3 });

            IMultiPartColorRamp pMultiPartColorRamp = CreateMultiPartColorRamp(lColorRamps);
            return CreateContinuousRenderer(gRaster, pMultiPartColorRamp);
        }

        public static IRasterRenderer CreateRoughnessColorRamp(GCDConsoleLib.Raster gRaster)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();

                rasterRenderer.Raster = raster;
                classifyRenderer.ClassCount = 10;
                rasterRenderer.Update();
                classifyRenderer.Break[0] = 0;
                classifyRenderer.Label[0] = "0 to 0.1";
                classifyRenderer.Break[1] = 0.1;
                classifyRenderer.Label[1] = "0.1 to 0.25";
                classifyRenderer.Break[2] = 0.25;
                classifyRenderer.Label[2] = "0.25 to 0.5";
                classifyRenderer.Break[3] = 0.5;
                classifyRenderer.Label[3] = "0.5 to 0.75";
                classifyRenderer.Break[4] = 0.75;
                classifyRenderer.Label[4] = "0.75 to 1.0";
                classifyRenderer.Break[5] = 1;
                classifyRenderer.Label[5] = "1.0 to 1.5";
                classifyRenderer.Break[6] = 1.5;
                classifyRenderer.Label[6] = "1.5 to 2.0";
                classifyRenderer.Break[7] = 2;
                classifyRenderer.Label[7] = "2.0 to 3.0";
                classifyRenderer.Break[8] = 3;
                classifyRenderer.Label[8] = "3.0 to 5.0";
                classifyRenderer.Break[9] = 5;
                classifyRenderer.Label[9] = "> 5.0";

                List<RgbColor> lColors = new List<RgbColor>();
                lColors.Add(CreateRGBColor(255, 255, 179));
                lColors.Add(CreateRGBColor(252, 241, 167));
                lColors.Add(CreateRGBColor(252, 230, 157));
                lColors.Add(CreateRGBColor(250, 218, 145));
                lColors.Add(CreateRGBColor(250, 208, 135));
                lColors.Add(CreateRGBColor(237, 191, 126));
                lColors.Add(CreateRGBColor(219, 167, 118));
                lColors.Add(CreateRGBColor(201, 147, 111));
                lColors.Add(CreateRGBColor(184, 127, 106));
                lColors.Add(CreateRGBColor(166, 101, 101));

                IFillSymbol fillSymbol = new SimpleFillSymbol();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    fillSymbol.Color = lColors[i];
                    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public static IRasterRenderer CreateSlopeDegreesColorRamp(GCDConsoleLib.Raster gRaster)
        {
            try
            {
                //Open raster file workspace.
                IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesRaster.RasterWorkspaceFactoryClass();
                ESRI.ArcGIS.DataSourcesRaster.IRasterWorkspace rasterWorkspace = (ESRI.ArcGIS.DataSourcesRaster.IRasterWorkspace)workspaceFactory.OpenFromFile(@"D:\Testing\GCD\test\inputs\2005DecDEM\AssociatedSurfaces\SlopeDegrees", 0);

                //Open file raster dataset. 
                IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset("SlopeDegrees.tif");

                ////Create the classify renderer.
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;

                //Set up the renderer properties.
                gRaster.ComputeStatistics(true);
                //IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();
                rasterRenderer.Raster = raster;
                classifyRenderer.ClassCount = 3;
                rasterRenderer.Update();

                //Set the color ramp for the symbology.
                IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRampClass();
                colorRamp.Size = 3;
                bool createColorRamp;
                colorRamp.CreateRamp(out createColorRamp);

                //Create the symbol for the classes.
                IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                for (int i = 0; i < classifyRenderer.ClassCount; i++)
                {
                    fillSymbol.Color = colorRamp.get_Color(i);
                    classifyRenderer.set_Symbol(i, (ISymbol)fillSymbol);
                    classifyRenderer.set_Label(i, Convert.ToString(i));
                }
                //IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                //IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                //IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                //IRaster raster = rasterDataset.CreateDefaultRaster();

                ////gRaster.ComputeStatistics();
                ////double rasterMin = (double)gRaster.GetStatistics()["min"];

                //rasterRenderer.Raster = raster;
                //classifyRenderer.ClassCount = 1;
                //rasterRenderer.Update();

                //classifyRenderer.Break[0] = 0f;
                ////classifyRenderer.Label[0] = "0 to 2";
                //classifyRenderer.set_Label(0, "test");
                ////classifyRenderer.Break[1] = 2;
                ////classifyRenderer.Label[1] = "2 to 5";
                ////classifyRenderer.Break[2] = 5;
                ////classifyRenderer.Label[2] = "5 to 10";
                ////classifyRenderer.Break[3] = 10;
                ////classifyRenderer.Label[3] = "10 to 15";
                ////classifyRenderer.Break[4] = 15;
                ////classifyRenderer.Label[4] = "15 to 25";
                ////classifyRenderer.Break[5] = 25;
                ////classifyRenderer.Label[5] = "25 to 35";
                ////classifyRenderer.Break[6] = 35;
                ////classifyRenderer.Label[6] = "35 to 45";
                ////classifyRenderer.Break[7] = 45;
                ////classifyRenderer.Label[7] = "45 to 60";
                ////classifyRenderer.Break[8] = 60;
                ////classifyRenderer.Label[8] = "60 to 80";
                ////classifyRenderer.Break[9] = 80;
                ////classifyRenderer.Label[9] = "80 to 90";
                ////classifyRenderer.Break[10] = 90;
                ////classifyRenderer.set_Label(10, "temp");

                ////List<RgbColor> lColors = CreateSlopeColorRamp();
                ////IFillSymbol fillSymbol = new SimpleFillSymbol();
                ////for (int i = 0; i < classifyRenderer.ClassCount; i++)
                ////{
                ////    fillSymbol.Color = lColors[i];
                ////    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                ////}
                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static List<RgbColor> CreateSlopeColorRamp()
        {
            List<ESRI.ArcGIS.Display.RgbColor> lColors = new List<RgbColor>();
            lColors.Add(CreateRGBColor(255, 235, 176));
            lColors.Add(CreateRGBColor(255, 219, 135));
            lColors.Add(CreateRGBColor(255, 202, 97));
            lColors.Add(CreateRGBColor(255, 186, 59));
            lColors.Add(CreateRGBColor(255, 170, 0));
            lColors.Add(CreateRGBColor(255, 128, 0));
            lColors.Add(CreateRGBColor(255, 85, 0));
            lColors.Add(CreateRGBColor(255, 42, 0));
            lColors.Add(CreateRGBColor(161, 120, 120));
            lColors.Add(CreateRGBColor(130, 130, 130));
            return lColors;
        }

        public static IRasterRenderer CreateSlopePrecentRiseColorRamp(Raster gRaster)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();

                rasterRenderer.Raster = raster;
                classifyRenderer.ClassCount = 10;
                rasterRenderer.Update();
                classifyRenderer.Break[0] = 0;
                classifyRenderer.Label[0] = "0 to 3.5%";
                classifyRenderer.Break[1] = 3.5;
                classifyRenderer.Label[1] = "3.5% to 8.75%";
                classifyRenderer.Break[2] = 8.75;
                classifyRenderer.Label[2] = "8.75% to 15%";
                classifyRenderer.Break[3] = 15;
                classifyRenderer.Label[3] = "15% to 25%";
                classifyRenderer.Break[4] = 25;
                classifyRenderer.Label[4] = "25% to 45%";
                classifyRenderer.Break[5] = 45;
                classifyRenderer.Label[5] = "45% to 70%";
                classifyRenderer.Break[6] = 70;
                classifyRenderer.Label[6] = "70% to 100%";
                classifyRenderer.Break[7] = 100;
                classifyRenderer.Label[7] = "100% to 175%";
                classifyRenderer.Break[8] = 175;
                classifyRenderer.Label[8] = "175% to 565%";
                classifyRenderer.Break[9] = 565;
                classifyRenderer.Label[9] = "> 565%";
                List<RgbColor> lColors = CreateSlopeColorRamp();
                IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    fillSymbol.Color = lColors[i];
                    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public static IRasterRenderer CreateDoDClassifyRenderer(Raster gRaster, int iClassCount)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();

                gRaster.ComputeStatistics();
                Dictionary<string, decimal> rasterStats = gRaster.GetStatistics();
                double rMin = (double)rasterStats["min"];
                double rMax = (double)rasterStats["max"];

                rasterRenderer.Raster = raster;
                if ((rMin == double.MinValue & rMax == double.MaxValue) | (rMin == double.MaxValue & rMax == double.MinValue) | (rMin == float.MinValue & rMax == float.MaxValue) | (rMin == float.MaxValue & rMax == float.MinValue))
                {
                    classifyRenderer.ClassCount = 1;
                    ESRI.ArcGIS.Display.IRgbColor rgbColor = CreateRGBColor(255, 255, 255);
                    rgbColor.Transparency = 0;
                    fillSymbol.Color = rgbColor;
                    classifyRenderer.Symbol[0] = (ISymbol)fillSymbol;
                    classifyRenderer.Label[0] = "No Data (no change detected)";
                    return rasterRenderer;
                }

                classifyRenderer.ClassCount = iClassCount;
                rasterRenderer.Update();
                CreateDoDClassBreaks(rMax, rMin, iClassCount, ref classifyRenderer);
                List<IColor> lColors = CreateDoDColorRamp();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    fillSymbol.Color = lColors[i];
                    classifyRenderer.Symbol[i] = (ISymbol)fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        private static void CreateDoDClassBreaks(double dMax, double dMin, int iClassCount, ref IRasterClassifyColorRampRenderer pRasterClassify)
        {
            double fMax = Math.Max(Math.Abs(dMax), Math.Abs(dMin));
            double fRange = 2 * fMax;
            double interval = fRange / iClassCount;
            int iRound = GetMagnitude(fRange);
            interval = Math.Round(interval * Math.Abs(iRound), Math.Abs(iRound) + 2) / Math.Abs(iRound);
            string sFormat = "#,#0";
            if (iRound < 0)
            {
                iRound = Math.Abs(iRound) + 2;
                for (int i = 0; i <= iRound; i++)
                {
                    if (i == 0)
                    {
                        sFormat += ".";
                    }

                    sFormat += "0";
                }
            }
            else
            {
                sFormat = "#,#0.00";
            }

            //Debug.Print("Class count: " + pRasterClassify.ClassCount);
            //Debug.Print("Interval: {0}", interval);
            //Debug.Print("Max: {0}", fMax);
            for (int i = 0; i <= iClassCount; i++)
            {
                double fBreak = Math.Round(-1 * fMax + (interval * i), iRound);
                //Debug.Print("Break " + i.ToString + " = " + fBreak);
                pRasterClassify.Break[i] = fBreak;
                if (i == 0)
                {
                    pRasterClassify.Break[i] = fBreak;
                    fBreak = Math.Round(-1 * fMax + (interval * 1), iRound);
                    pRasterClassify.Label[i] = "< " + fBreak.ToString(sFormat);
                }
                else if (i < iClassCount - 1)
                {
                    if (i == 10)
                    {
                        pRasterClassify.Break[i] = 0;
                        pRasterClassify.Label[i] = (0).ToString(sFormat) + " to " + Math.Round(fBreak + interval, iRound).ToString(sFormat);
                        continue;
                    }

                    pRasterClassify.Label[i] = (fBreak).ToString(sFormat) + " to " + Math.Round(fBreak + interval, iRound).ToString(sFormat);
                }
                else if (i == iClassCount - 1)
                {
                    pRasterClassify.Label[i] = "> " + fBreak.ToString(sFormat);
                }
            }
        }

        private static List<IColor> CreateDoDColorRamp()
        {
            List<IColor> lColors = new List<IColor>();
            IRgbColor rgbColor = CreateRGBColor(230, 0, 0);

            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(235, 45, 23);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(240, 67, 41);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(242, 88, 61);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(245, 108, 81);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(245, 131, 105);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(245, 151, 130);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(242, 171, 155);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(237, 190, 180);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(230, 208, 207);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(218, 218, 224);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(197, 201, 219);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(176, 183, 214);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(155, 166, 207);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(135, 150, 201);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(110, 131, 194);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(92, 118, 189);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(72, 105, 184);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(49, 91, 176);
            lColors.Add(rgbColor);
            rgbColor = CreateRGBColor(2, 77, 168);
            lColors.Add(rgbColor);
            return lColors;
        }

        public static RgbColor CreateRGBColor(UInt16 iRed, UInt16 iGreen, UInt16 iBlue)
        {
            RgbColor RGBColor = new RgbColor();
            RGBColor.Red = iRed;
            RGBColor.Green = iGreen;
            RGBColor.Blue = iBlue;
            return RGBColor;
        }
    }
}
