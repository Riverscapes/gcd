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

            addToMapToolStripMenuItem.Visible = ProjectManager.IsArcMap;
        }

        public void AddDoDProperty(string property, string value)
        {
            DoDProperties.Add(new DoDProperty(property, value));
            DoDProperties.ResetBindings();
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                DoDProperty prop = (DoDProperty)grdData.Rows[e.RowIndex].DataBoundItem;

                if (prop.Header)
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Font = new Font(grdData.Font, FontStyle.Bold);
                }
                else
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Padding = new Padding(prop.LeftPadding, 0, 0, 0);
                }

                if (prop is DoDPropertyProjectItem || prop is DoDPropertyRaster)
                    grdData.Rows[e.RowIndex].ContextMenuStrip = contextMenuStrip1;
            }
        }

        public void Initialize(DoDBase dod)
        {
            DoDProperties.Add(new DoDProperty("Input Datasets"));
            DoDProperties.Add(new DoDPropertyProjectItem("New Surface", dod.NewSurface.Name, dod.NewSurface));
            DoDProperties.Add(new DoDPropertyProjectItem("Old Surface", dod.OldSurface.Name, dod.OldSurface));
            if (dod is DoDPropagated)
            {
                DoDPropagated dodProp = dod as DoDPropagated;
                DoDProperties.Add(new DoDPropertyProjectItem("New Error Surface", dodProp.NewError.Name, dodProp.NewError));
                DoDProperties.Add(new DoDPropertyProjectItem("Old Error Surface", dodProp.OldError.Name, dodProp.OldError));
            }

            if (dod.AOIMask == null)
                DoDProperties.Add(new DoDProperty("Area Of Interest", dod.AOILabel));
            else
                DoDProperties.Add(new DoDPropertyProjectItem("Area Of Interest", dod.AOILabel, dod.AOIMask));

            DoDProperties.Add(new DoDProperty("Input Parameters"));
            DoDProperties.Add(new DoDProperty("Uncertainty Analysis", dod.UncertaintyAnalysisLabel));

            DoDProperties.Add(new DoDProperty("Output Datasets"));
            DoDProperties.Add(new DoDProperty("DoD Analysis Folder", ProjectManager.Project.GetRelativePath(dod.Folder.FullName)));
            DoDProperties.Add(new DoDPropertyProjectItem("Thresholded DoD", ProjectManager.Project.GetRelativePath(dod.ThrDoD.Raster.GISFileInfo), dod.ThrDoD));
            DoDProperties.Add(new DoDPropertyProjectItem("Raw DoD", ProjectManager.Project.GetRelativePath(dod.RawDoD.Raster.GISFileInfo), dod.RawDoD));

            DoDProperties.Add(new DoDProperty("Intermediate Datasets"));
            if (dod is DoDPropagated)
            {
                DoDPropagated dodProp = dod as DoDPropagated;
                DoDProperties.Add(new DoDPropertyRaster("Propagated Error Surface", dodProp.PropagatedError));

                if (dod is DoDProbabilistic)
                {
                    DoDProbabilistic dodProb = dod as DoDProbabilistic;

                    if (dodProb.SpatialCoherence == null)
                    {
                        DoDProperties.Add(new DoDPropertyRaster("Probability Raster", dodProb.PriorProbability));
                    }
                    else
                    {
                        DoDProperties.Add(new DoDProperty("Spatial Coherence", string.Format("Bayesian updating with filter size of {0} X {0} cells", ((DoDProbabilistic)dod).SpatialCoherence.BufferSize)));
                        DoDProperties.Add(new DoDPropertyRaster("Probability Raster", dodProb.PriorProbability));
                        DoDProperties.Add(new DoDPropertyRaster("Posterior Raster", dodProb.PosteriorProbability));
                        DoDProperties.Add(new DoDPropertyRaster("Conditional Raster", dodProb.ConditionalRaster));
                        DoDProperties.Add(new DoDPropertyRaster("Surface Lowering Count", dodProb.SpatialCoherenceErosion));
                        DoDProperties.Add(new DoDPropertyRaster("Surface Raising Count", dodProb.SpatialCoherenceDeposition));
                    }
                }
            }
            DoDProperties.Add(new DoDPropertyRaster("Effective Threshold Surface", dod.ThrErr.Raster));

            // Values from the thresholded DoD raster stats are optional
            try
            {
                DoDProperties.Add(new DoDProperty("Output Raster Statistics"));
                string vUnits = UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit);
                dod.ThrDoD.Raster.ComputeStatistics();
                Dictionary<string, decimal> stats = dod.ThrDoD.Raster.GetStatistics();
                DoDProperties.Add(new DoDProperty("Thresholded DoD maximum raster value", stats["max"].ToString("n2") + vUnits));
                DoDProperties.Add(new DoDProperty("Thresholded DoD minimum raster value", stats["min"].ToString("n2") + vUnits));
                DoDProperties.Add(new DoDProperty("Thresholded DoD mean raster value", stats["mean"].ToString("n2") + vUnits));
                DoDProperties.Add(new DoDProperty("Thresholded DoD standard deviation of raster values", stats["stddev"].ToString("n2") + vUnits));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error calculating statistics from {0}, {1}", dod.ThrDoD.Raster.GISFileInfo.FullName, ex.Message));
            }
        }

        private class DoDProperty
        {
            public string Property { get; protected set; }
            public string Value { get; protected set; }
            public readonly bool Header;

            // Non-header cells are indented
            public int LeftPadding { get { return Header ? 0 : 16; } }

            /// <summary>
            /// Default constructor required for grid binding
            /// </summary>
            public DoDProperty()
            {

            }

            /// <summary>
            /// Constructor for regular NON-HEADER items
            /// </summary>
            /// <param name="prop"></param>
            /// <param name="value"></param>
            public DoDProperty(string prop, string value)
            {
                Property = prop;
                Value = value;
                Header = false;
            }

            /// <summary>
            /// Constructor for headers only
            /// </summary>
            /// <param name="header"></param>
            public DoDProperty(string header)
            {
                Property = header;
                Value = string.Empty;
                Header = true;
            }
        }

        private class DoDPropertyProjectItem : DoDProperty
        {
            public readonly GCDProjectItem ProjectItem;

            public DoDPropertyProjectItem(string prop, string value, GCDProjectItem item)
                : base(prop, value)
            {
                ProjectItem = item;
            }

            public DoDPropertyProjectItem(string prop, GCDProjectItem item)
                : base(prop, string.Empty)
            {
                if (item is GCDProjectRasterItem)
                    Value = ProjectManager.Project.GetRelativePath(((GCDProjectRasterItem)item).Raster.GISFileInfo);
                else if (item is GCDProjectVectorItem)
                    Value = ProjectManager.Project.GetRelativePath(((GCDProjectVectorItem)item).Vector.GISFileInfo);
            }
        }

        private class DoDPropertyRaster : DoDProperty
        {
            public readonly GCDConsoleLib.Raster Raster;

            public DoDPropertyRaster(string prop, string value, GCDConsoleLib.Raster raster)
                : base(prop, value)
            {
                Raster = raster;
            }

            public DoDPropertyRaster(string prop, GCDConsoleLib.Raster raster)
                : base(prop, string.Empty)
            {
                Raster = raster;
            }
        }

        private void addToMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count < 1)
                return;

            if (grdData.SelectedRows[0].DataBoundItem is DoDPropertyProjectItem)
            {
                DoDPropertyProjectItem propItem = (DoDPropertyProjectItem)grdData.SelectedRows[0].DataBoundItem;
                ProjectManager.AddNewProjectItemToMap(propItem.ProjectItem);
            }
        }

        /// <summary>
        /// Select the row that the user just right clicked on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
                grdData.Rows[e.RowIndex].Selected = true;

        }
    }
}
