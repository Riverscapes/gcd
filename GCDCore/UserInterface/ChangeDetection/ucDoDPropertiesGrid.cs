using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDPropertiesGrid : UserControl
    {
        public DoDBase DoD { get; internal set; }
        private naru.ui.SortableBindingList<DoDProperty> DoDProperties;

        public ucDoDPropertiesGrid()
        {
            InitializeComponent();
            DoDProperties = new naru.ui.SortableBindingList<DoDProperty>();
        }

        private void ucDoDPropertiesGrid_Load(object sender, EventArgs e)
        {
            grdData.AutoGenerateColumns = false;
            grdData.DataSource = DoDProperties;
        }

        public void AddDoDProperty(string property, string value)
        {
            DoDProperties.Add(new DoDProperty(property, value));
            DoDProperties.ResetBindings();
        }

        public void Initialize(DoDBase dod)
        {
            DoDProperties.Add(new DoDPropertyRaster("New Surface", dod.NewSurface.Name, dod.NewSurface.Raster.GISFileInfo));
            DoDProperties.Add(new DoDPropertyRaster("Old Surface", dod.NewSurface.Name, dod.OldSurface.Raster.GISFileInfo));
            DoDProperties.Add(new DoDProperty("Uncertainty Analysis", dod.UncertaintyAnalysisLabel));

            if (dod is DoDPropagated)
            {
                DoDPropagated dodProp = dod as DoDPropagated;
                DoDProperties.Add(new DoDPropertyRaster("New Error Surface", dodProp.NewError.Name, dodProp.NewError.Raster.GISFileInfo));
                DoDProperties.Add(new DoDPropertyRaster("Old Error Surface", dodProp.OldError.Name, dodProp.OldError.Raster.GISFileInfo));
                DoDProperties.Add(new DoDPropertyRaster("Propagated Error Surface", dodProp.PropagatedError.GISFileInfo));

                if (dod is DoDProbabilistic)
                {
                    DoDProbabilistic dodProb = dod as DoDProbabilistic;

                    if (dodProb.SpatialCoherence == null)
                    {
                        DoDProperties.Add(new DoDPropertyRaster("Probability Raster", dodProb.PriorProbability.GISFileInfo));
                    }
                    else
                    {
                        DoDProperties.Add(new DoDProperty("Spatial Coherence", string.Format("Bayesian updating with filter size of {0} X {0} cells", ((DoDProbabilistic)dod).SpatialCoherence.BufferSize)));
                        DoDProperties.Add(new DoDPropertyRaster("Probability Raster", dodProb.PriorProbability.GISFileInfo));
                        DoDProperties.Add(new DoDPropertyRaster("Posterior Raster", dodProb.PosteriorProbability.GISFileInfo));
                        DoDProperties.Add(new DoDPropertyRaster("Conditional Raster", dodProb.ConditionalRaster.GISFileInfo));
                        DoDProperties.Add(new DoDPropertyRaster("Surface Lowering Count", dodProb.SpatialCoherenceErosion.GISFileInfo));
                        DoDProperties.Add(new DoDPropertyRaster("Surface Raising Count", dodProb.SpatialCoherenceDeposition.GISFileInfo));
                    }
                }
            }    
        }

        private class DoDProperty
        {
            public string Property { get; set; }
            public string Value { get; set; }

            /// <summary>
            /// Default constructor required for grid binding
            /// </summary>
            public DoDProperty()
            {

            }

            public DoDProperty(string prop, string value)
            {
                Property = prop;
                Value = value;
            }
        }

        private class DoDPropertyRaster : DoDProperty
        {
            public System.IO.FileInfo RasterPath { get; set; }

            public DoDPropertyRaster(string prop, string value, System.IO.FileInfo rasterPath)
            {
                Property = prop;
                Value = value;
                RasterPath = rasterPath;
            }

            public DoDPropertyRaster(string prop, System.IO.FileInfo rasterPath)
            {
                Property = prop;
                Value = GCDCore.Project.ProjectManager.Project.GetRelativePath(rasterPath);
                RasterPath = rasterPath;
            }
        }
    }
}
