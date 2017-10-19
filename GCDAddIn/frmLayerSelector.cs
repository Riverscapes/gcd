using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDAddIn
{
    public partial class frmLayerSelector : Form
    {
        public GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes BrowseType { get; internal set; }

        public GCD.GCDLib.Core.GISDataStructures.GISDataSource SelectedLayer
        {
            get
            {
                if (lstLayers.SelectedItem is GCD.GCDLib.Core.GISDataStructures.GISDataSource)
                    return (GCD.GCDLib.Core.GISDataStructures.GISDataSource)lstLayers.SelectedItem;
                else
                    return null;
            }
        }

        public frmLayerSelector(GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes eBrowseType)
        {
            InitializeComponent();
            BrowseType = eBrowseType;
        }

        private void frmLayerSelector_Load(object sender, EventArgs e)
        {
            try
            {
                LoadArcMapLayers();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void LoadArcMapLayers()
        {
            lstLayers.Items.Clear();

            for (int i = 0; i <= ArcMap.Document.FocusMap.LayerCount - 1; i++)
            {
                ILayer pLayer = ArcMap.Document.FocusMap.Layer[i];
                if (!(pLayer is ICompositeLayer))
                {
                    GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes eBrowseType = GetBrowseType(ref pLayer);
                    if (BrowseType == GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Any || eBrowseType == BrowseType)
                    {
                        lstLayers.Items.Add(new LayerInfo(pLayer.Name, ArcMapUtilities.GetPathFromLayer(pLayer), eBrowseType));
                    }
                }
            }
        }

        private GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes GetBrowseType(ref ESRI.ArcGIS.Carto.ILayer pLayer)
        {
            if (pLayer is IGeoFeatureLayer)
            {
                IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pLayer;
                switch (pGFLayer.FeatureClass.ShapeType)
                {
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                        return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Point;

                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine:
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                        return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Line;

                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                        return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Polygon;

                    default:
                        return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Any;
                }
            }
            else if (pLayer is IRasterLayer)
            {
                return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Raster;
            }
            else if (pLayer is ITinLayer)
            {
                return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.TIN;
            }
            else
                return GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Any;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lstLayers.SelectedItems.Count != 1)
            {
                MessageBox.Show("You must select one and only one layer to continue.", GCD.GCDLib.My.Resources.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
            }
        }

        private class LayerInfo
        {
            public string Name { get; internal set; }
            public System.IO.FileSystemInfo FullPath { get; internal set; }
            public GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes BrowseType { get; internal set; }

            public override string ToString()
            {
                return Name;
            }

            public LayerInfo(string sName, System.IO.FileSystemInfo siFullPath, GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes eBrowseType)
            {
                Name = sName;
                FullPath = siFullPath;
                BrowseType = eBrowseType;
            }
        }
    }
}
