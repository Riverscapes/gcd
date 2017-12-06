using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.GIS
{
    public enum BrowseGISTypes
    {
        Point,
        Line,
        Polygon,
        Raster,
        TIN,
        Any,
    }

    public class BrowseGISLayerEventArgs : EventArgs
    {
        public readonly BrowseGISTypes LayerType;
        public readonly string MessageBoxTitle;

        public BrowseGISLayerEventArgs(BrowseGISTypes layerType, string msgBoxTitle)
        {
            LayerType = layerType;
            MessageBoxTitle = msgBoxTitle;
        }
    }
}
