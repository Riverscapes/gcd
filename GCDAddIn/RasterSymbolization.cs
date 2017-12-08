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
                RasterStretchColorRampRenderer stretchRenderer = new RasterStretchColorRampRenderer();
                IRasterRenderer rasterRenderer = (IRasterRenderer)stretchRenderer;
                IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
                IRaster raster = rasterDataset.CreateDefaultRaster();
                rasterRenderer.Raster = raster;
                IColorRamp pColorRamp = null;
                IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(ref pColorRamp, sColorRampName);
                IRasterRendererColorRamp pRenderColorRamp = (IRasterRendererColorRamp)rasterRenderer;
                pRenderColorRamp.ColorScheme = pStyleItem.Name;
                IRasterStretchMinMax pStretchInfo = (IRasterStretchMinMax)stretchRenderer;
                pStretchInfo.CustomStretchMin = 0;
                int iRound = GetMagnitude(gRaster.Maximum);
                pStretchInfo.CustomStretchMax = Math.Round(gRaster.Maximum, Math.Abs(iRound));
                pStretchInfo.UseCustomStretchMinMax = true;
                stretchRenderer.LabelHigh = Math.Round(gRaster.Maximum, Math.Abs(iRound)).ToString();
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

        private static IStyleGalleryItem GetESRIStyleColorRamp(ref IColorRamp pColorRamp, string sColorRampName)
        {
            IStyleGallery pStyleGallery = new ESRI.ArcGIS.Framework.StyleGallery();
            IStyleGalleryStorage pStyleStorage;
            pStyleStorage = pStyleGallery as ESRI.ArcGIS.Display.IStyleGalleryStorage;
            string pStylePath = pStyleStorage.DefaultStylePath + "ESRI.style";
            pStyleStorage.AddFile(pStylePath);
            ESRI.ArcGIS.esriSystem.IEnumBSTR eESRIRampCategories = pStyleGallery.Categories["Color Ramps"];
            string sESRIRampCategoryName = eESRIRampCategories.Next();
            IStyleGalleryItem pStyleItem;
            bool bFound = false;

            while (string.IsNullOrEmpty(sESRIRampCategoryName))
            {
                ESRI.ArcGIS.Display.IEnumStyleGalleryItem eESRIColorRamps = pStyleGallery.Items["Color Ramps", pStylePath, sESRIRampCategoryName];
                pStyleItem = eESRIColorRamps.Next();
                while (pStyleItem != null)
                {
                    if (string.Compare(pStyleItem.Name, sColorRampName) == 0)
                    {
                        pColorRamp = pStyleItem.Item;
                        return pStyleItem;
                    }

                    pStyleItem = eESRIColorRamps.Next();
                }

                sESRIRampCategoryName = eESRIRampCategories.Next();
            }

            throw new Exception("The name of the color ramp provided does not exist.");
        }

        public static IRasterRenderer CreateClassifyRenderer(Raster gRaster, int iClassCount, string sColorRampName, bool bInvert = false)
        {
            IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRenderer();
            IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
            IFillSymbol fillSymbol = new SimpleFillSymbol();
            IRasterDataset rasterDataset = ArcMapUtilities.GetRasterDataset(gRaster);
            IRaster raster = rasterDataset.CreateDefaultRaster();
            rasterRenderer.Raster = raster;
            IColorRamp pColorRamp = null;
            IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(ref pColorRamp, sColorRampName);
            classifyRenderer.ClassCount = iClassCount;
            rasterRenderer.Update();
            CreateClassBreaks(gRaster.Maximum, iClassCount, classifyRenderer);
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

        private static double GetFormattedRange(double fRange)
        {
            int iRound = GetMagnitude(fRange);
            return fRange = Math.Round(fRange * 2, Math.Abs(iRound) + 1, MidpointRounding.AwayFromZero) / 2;
        }

        public static void CreateClassBreaks(double fMax, int iClassCount, ref IRasterClassifyColorRampRenderer pRasterClassify)
        {
            fMax = GetFormattedRange(fMax);
            double interval = fMax / iClassCount;
            interval = GetFormattedRange(interval);

            int iRound = GetMagnitude(fMax);
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

        public static IRasterRenderer CreateGrainSizeStatisticColorRamp(GISDataStructures.Raster gRaster, NumberFormatting.LinearUnits eUnits)
        {
            try
            {
                IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
                IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
                ESRI.ArcGIS.Geodatabase.IRasterDataset rasterDataset = gRaster.RasterDataset;
                ESRI.ArcGIS.Geodatabase.IRaster raster = rasterDataset.CreateDefaultRaster();
                rasterRenderer.Raster = raster;
                classifyRenderer.ClassCount = 5;
                rasterRenderer.Update();
                classifyRenderer.Break(0) = 0;
                classifyRenderer.Label(0) = "Fines, Sand (0 to 2 mm)";
                classifyRenderer.Break(1) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 2);
                classifyRenderer.Label(1) = "Fine Gravel (2 mm to 16 mm)";
                classifyRenderer.Break(2) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 16);
                classifyRenderer.Label(2) = "Coarse Gravel (16 mm to 64 mm)";
                classifyRenderer.Break(3) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 64);
                classifyRenderer.Label(3) = "Cobbles (64 mm to 256 mm)";
                classifyRenderer.Break(4) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 256);
                classifyRenderer.Label(4) = "Boulders (> 256 mm)";
                List<ESRI.ArcGIS.Display.RgbColor> lColors = new List<ESRI.ArcGIS.Display.RgbColor>;
                lColors.Add(CreateRGBColor(194, 82, 60));
                lColors.Add(CreateRGBColor(240, 180, 17));
                lColors.Add(CreateRGBColor(123, 237, 0));
                lColors.Add(CreateRGBColor(27, 168, 124));
                lColors.Add(CreateRGBColor(11, 44, 122));
                ESRI.ArcGIS.Display.IFillSymbol fillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
                {
                    Debug.WriteLine(string.Format("Red: {0}, Green: {1}, Blue: {2}", lColors(i).Red, lColors(i).Green, lColors(i).Blue));
                    fillSymbol.Color = lColors(i);
                    classifyRenderer.Symbol(i) = fillSymbol;
                }

                return rasterRenderer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
