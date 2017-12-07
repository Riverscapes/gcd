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
    public struct RasterSymbolization
    {
        public IRasterRenderer CreateESRIDefinedContinuousRenderer(GCDConsoleLib.Raster gRaster, int iClassCount, string sColorRampName, bool bInvert = false)
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

        private IStyleGalleryItem GetESRIStyleColorRamp(ref IColorRamp pColorRamp, string sColorRampName)
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

        public IRasterRenderer CreateClassifyRenderer(Raster gRaster, int iClassCount, string sColorRampName, bool bInvert = false)
        {
            IRasterClassifyColorRampRenderer classifyRenderer = new RasterClassifyColorRampRendererClass();
            IRasterRenderer rasterRenderer = (IRasterRenderer)classifyRenderer;
            ESRI.ArcGIS.Display.IFillSymbol fillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
            ESRI.ArcGIS.Geodatabase.IRasterDataset rasterDataset = gRaster.RasterDataset;
            ESRI.ArcGIS.Geodatabase.IRaster raster = rasterDataset.CreateDefaultRaster();
            rasterRenderer.Raster = raster;
            ESRI.ArcGIS.Display.IColorRamp pColorRamp = null;
            ESRI.ArcGIS.Display.IStyleGalleryItem pStyleItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName);
            classifyRenderer.ClassCount = iClassCount;
            rasterRenderer.Update();
            CreateClassBreaks(gRaster.Maximum, iClassCount, classifyRenderer);
            pColorRamp.Size = iClassCount;
            pColorRamp.CreateRamp(true);
            List<ESRI.ArcGIS.Display.IColor> lColors = new List<ESRI.ArcGIS.Display.IColor>;
            for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
            {
                lColors.Add(pColorRamp.Color(i));
            }

            if (bInvert)
            {
                lColors.Reverse();
            }

            for (int i = 0; i <= classifyRenderer.ClassCount - 1; i += i + 1)
            {
                fillSymbol.Color = lColors.Item(i);
                classifyRenderer.Symbol(i) = fillSymbol;
            }

            return rasterRenderer;

        }


        private int GetMagnitude(double range, int iMagnitudeMinimum = -4, int iMagnitudeMaximum = 4)
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
    }
}
