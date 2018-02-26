using ESRI.ArcGIS.Carto;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.esriSystem;

namespace GCDAddIn
{
    public class VectorSymbolization
    {
        public static void AddToMapVector(FileInfo sSource, string sDisplayName, IGroupLayer pGrpLyr, string displayField, IFeatureRenderer pRenderer, string queryFilter, string labelField, short fTransparency = -1)
        {
            if (string.IsNullOrEmpty(sDisplayName))
            {
                throw new ArgumentNullException("Display Name", "Null or empty display Name");
            }

            IFeatureLayer pResultLayer = IsFeatureLayerInGroupLayer(sSource.FullName, pGrpLyr);


            IFeatureWorkspace pWS = (IFeatureWorkspace)ArcMapUtilities.GetWorkspace(sSource);
            IFeatureClass pFC = pWS.OpenFeatureClass(Path.GetFileNameWithoutExtension(sSource.FullName));

            pResultLayer = new FeatureLayer();
            pResultLayer.FeatureClass = pFC;
            if (fTransparency >= 0)
            {
                ILayerEffects pLayerEffects = (ILayerEffects)pResultLayer;
                pLayerEffects.Transparency = fTransparency;
            }

            if (!string.IsNullOrEmpty(sDisplayName))
            {
                pResultLayer.Name = sDisplayName;
            }

            IMapLayers pMapLayers = (IMapLayers)ArcMap.Document.FocusMap;
            if (pGrpLyr == null)
            {
                pMapLayers.InsertLayer(pResultLayer, true, 0);
            }
            else
            {
                pMapLayers.InsertLayerInGroup(pGrpLyr, pResultLayer, true, 0);
            }

            if (pRenderer != null)
            {
                ((IGeoFeatureLayer)pResultLayer).Renderer = pRenderer;
            }

            if (!string.IsNullOrEmpty(labelField))
            {
                IGeoFeatureLayer pGFL = pResultLayer as IGeoFeatureLayer;
            
                // This first attempt seems to set the label field, but doesn't
                // cause the labeling to refresh and take effect
                //IDisplayString displayString = pGFL as IDisplayString;
                //IDisplayExpressionProperties properties = displayString.ExpressionProperties;
                //properties.Expression = string.Format("[{0}]", labelField);

                // This second attempt takes more effort but actually works
                // https://community.esri.com/thread/19005
                IAnnotateLayerPropertiesCollection labelPropsCollection = pGFL.AnnotationProperties;
                labelPropsCollection.Clear();

                //IBasicOverposterLayerProperties pBOLayerProps = new BasicOverposterLayerProperties();
                //pBOLayerProps.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolygon
                //pBOLayerProps.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerShape
                //pBOLayerProps.FeatureWeight = esriBasicOverposterWeight.esriNoWeight
                //pBOLayerProps.LabelWeight = esriBasicOverposterWeight.esriLowWeight

                //Dim tSym As ITextSymbol
                //Set tSym = New TextSymbol
                //Dim font As IFontDisp
                //Set font = tSym.font
                //font.Bold = False
                //font.size = 6
                //tSym.font = font

                ILabelEngineLayerProperties aLELayerProps = (ILabelEngineLayerProperties)new LabelEngineLayerProperties();
                aLELayerProps.Expression = string.Format("[{0}]", labelField);

                //Set aLELayerProps.Symbol = tSym
                //Set aLELayerProps.BasicOverposterLayerProperties = pBOLayerProps
                IAnnotateLayerProperties lProps = aLELayerProps as IAnnotateLayerProperties;
                lProps.Class = "Any Name";
                labelPropsCollection.Add(lProps);

                pGFL.DisplayAnnotation = true;
            }

            if (pRenderer is IUniqueValueRenderer)
            {
                // If you didn't use a color ramp that was predefined in a style,
                // you need to use "Custom" here, otherwise use the name of the color ramp you chose.
                ((IGeoFeatureLayer)pResultLayer).DisplayField = displayField;

                // This makes the layer properties symbology tab show the correct interface
                IUID pUID = new UIDClass();
                pUID.Value = "{683C994E-A17B-11D1-8816-080009EC732A}";
                ((IGeoFeatureLayer)pResultLayer).RendererPropertyPageClassID = pUID as UIDClass;
            }

            if (!string.IsNullOrEmpty(queryFilter))
            {
                ((IFeatureLayerDefinition)pResultLayer).DefinitionExpression = queryFilter;
            }

            int refsLeft = 0;
            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pFC);
            }
            while (refsLeft > 0);

            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pResultLayer);
            }
            while (refsLeft > 0);

            ArcMap.Document.UpdateContents();
            ArcMap.Document.ActiveView.Refresh();
            ArcMap.Document.CurrentContentsView.Refresh(null);
        }

        private static IFeatureLayer IsFeatureLayerInGroupLayer(string sSource, IGroupLayer pGrpLyr)
        {
            if (pGrpLyr == null)
                return null;

            ESRI.ArcGIS.ArcMapUI.IMxDocument pMXDoc = ArcMap.Document;
            IMap pMap = pMXDoc.FocusMap;
            ICompositeLayer compositeLayer = pGrpLyr as ICompositeLayer;
            if (compositeLayer != null & compositeLayer.Count > 0)
            {
                for (int i = 0; i <= compositeLayer.Count - 1; i++)
                {
                    if (compositeLayer.Layer[i] is IFeatureLayer)
                    {
                        ESRI.ArcGIS.Geodatabase.IDataset pLayer = (ESRI.ArcGIS.Geodatabase.IDataset)compositeLayer.Layer[i];
                        string pLayerPath = Path.Combine(pLayer.Workspace.PathName, pLayer.BrowseName + ".shp");
                        if (string.Compare(pLayerPath, sSource, true) == 0)
                        {
                            return (IFeatureLayer)pLayer;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        /// <remarks>https://gis.stackexchange.com/questions/58728/set-unique-values-to-different-groups-programmatically
        /// http://edndoc.esri.com/arcobjects/9.0/ComponentHelp/esriCarto/IUniqueValueRenderer_Example.htm
        /// </remarks>
        public static IUniqueValueRenderer GetRegularMaskRenderer(GCDCore.Project.Masks.RegularMask mask)
        {
            ISimpleFillSymbol symbol = new SimpleFillSymbol();
            symbol.Style = esriSimpleFillStyle.esriSFSSolid;
            symbol.Outline.Width = 0.4;

            // Make the color ramp we will use for the symbols in the renderer
            IRandomColorRamp rx = new RandomColorRamp();
            rx.MinSaturation = 20;
            rx.MaxSaturation = 40;
            rx.MinValue = 85;
            rx.MaxValue = 100;
            rx.StartHue = 76;
            rx.EndHue = 188;
            rx.UseSeed = true;
            rx.Seed = 43;

            // These properties should be set prior to adding values
            IUniqueValueRenderer pRender = new UniqueValueRenderer();
            pRender.FieldCount = 1;
            pRender.Field[0] = mask._Field;
            pRender.DefaultSymbol = (ISymbol)symbol;
            pRender.UseDefaultSymbol = true;

            foreach (GCDCore.Project.Masks.MaskItem item in mask._Items.Where(x => x.Include))
            {
                ISimpleFillSymbol symx = new SimpleFillSymbol();
                symx.Style = esriSimpleFillStyle.esriSFSSolid;
                symx.Outline.Width = 0.4;

                pRender.AddValue(item.FieldValue, string.Empty, (ISymbol)symx);
                pRender.Label[item.FieldValue] = item.Label;
                pRender.Symbol[item.FieldValue] = (ISymbol)symx;
            }

            // now that we know how many unique values there are we can size the color ramp and assign the colors
            rx.Size = pRender.ValueCount;
            bool bOK;
            rx.CreateRamp(out bOK);
            IEnumColors RColors = rx.Colors;
            RColors.Reset();

            for (int i = 0; i < pRender.ValueCount; i++)
            {
                string xv = pRender.Value[i];
                ISimpleFillSymbol jsv = (ISimpleFillSymbol)pRender.Symbol[xv];
                jsv.Color = RColors.Next();
                pRender.Symbol[xv] = (ISymbol)jsv;
            }

            // If you didn't use a color ramp that was predefined in a style, you need to use "Custom" here, otherwise
            // use the name of the color ramp you chose.
            pRender.ColorScheme = "Custom";
            pRender.set_FieldType(0, true);

            return pRender;
        }

        public static ISimpleRenderer GetAOIRenderer(GCDCore.Project.Masks.AOIMask mask)
        {
            RgbColor rgb = new RgbColor();
            rgb.Red = 255;
            rgb.Blue = 0;
            rgb.Green = 0;

            ISimpleFillSymbol symbol = new SimpleFillSymbol();
            symbol.Style = esriSimpleFillStyle.esriSFSHollow;
            symbol.Outline.Width = 1.0;
            ILineSymbol pLineSymbol = symbol.Outline;
            pLineSymbol.Color = rgb;
            pLineSymbol.Width = 1;
            symbol.Outline = pLineSymbol;

            // These properties should be set prior to adding values
            ISimpleRenderer pRender = new SimpleRenderer();
            pRender.Label = "Area of Interest";
            pRender.Symbol = symbol as ISymbol;

            return pRender;
        }

        public static ISimpleRenderer GetDirectionalMaskRenderer(GCDCore.Project.Masks.DirectionalMask mask)
        {
            RgbColor rgb = new RgbColor();
            rgb.Red = 0;
            rgb.Blue = 0;
            rgb.Green = 0;

            ISimpleFillSymbol symbol = new SimpleFillSymbol();
            symbol.Style = esriSimpleFillStyle.esriSFSHollow;
            symbol.Outline.Width = 1.0;
            ILineSymbol pLineSymbol = symbol.Outline;
            pLineSymbol.Color = rgb;
            pLineSymbol.Width = 1;
            symbol.Outline = pLineSymbol;

            // These properties should be set prior to adding values
            ISimpleRenderer pRender = new SimpleRenderer();
            pRender.Label = "Directional Mask";
            pRender.Symbol = symbol as ISymbol;

            return pRender;
        }
    }
}
