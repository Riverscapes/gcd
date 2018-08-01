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
        private naru.ui.SortableBindingList<GridViewPropertyValueItem> DoDProperties;

        public ucDoDPropertiesGrid()
        {
            InitializeComponent();
            DoDProperties = new naru.ui.SortableBindingList<GridViewPropertyValueItem>();
        }

        private void ucDoDPropertiesGrid_Load(object sender, EventArgs e)
        {
            grdData.AutoGenerateColumns = false;
            grdData.DataSource = DoDProperties;

            addToMapToolStripMenuItem.Visible = ProjectManager.IsArcMap;
        }

        public void AddDoDProperty(string property, string value)
        {
            DoDProperties.Add(new GridViewPropertyValueItem(property, value));
            DoDProperties.ResetBindings();
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                GridViewPropertyValueItem prop = (GridViewPropertyValueItem)grdData.Rows[e.RowIndex].DataBoundItem;

                if (prop.Header)
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Font = new Font(grdData.Font, FontStyle.Bold);
                }
                else
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Padding = new Padding(prop.LeftPadding, 0, 0, 0);
                }

                if (prop is GridViewGCDProjectItem || prop is GridViewRasterItem)
                    grdData.Rows[e.RowIndex].ContextMenuStrip = contextMenuStrip1;
            }
        }

        public void Initialize(DoDBase dod)
        {
            DoDProperties.Add(new GridViewPropertyValueItem("Input Datasets"));
            DoDProperties.Add(new GridViewGCDProjectItem("New Surface", dod.NewSurface.Name, dod.NewSurface));
            DoDProperties.Add(new GridViewGCDProjectItem("Old Surface", dod.OldSurface.Name, dod.OldSurface));
            if (dod is DoDPropagated)
            {
                DoDPropagated dodProp = dod as DoDPropagated;
                DoDProperties.Add(new GridViewGCDProjectItem("New Error Surface", dodProp.NewError.Name, dodProp.NewError));
                DoDProperties.Add(new GridViewGCDProjectItem("Old Error Surface", dodProp.OldError.Name, dodProp.OldError));
            }

            if (dod.AOIMask == null)
                DoDProperties.Add(new GridViewPropertyValueItem("Area Of Interest", dod.AOILabel));
            else
                DoDProperties.Add(new GridViewGCDProjectItem("Area Of Interest", dod.AOILabel, dod.AOIMask));

            DoDProperties.Add(new GridViewPropertyValueItem("Input Parameters"));
            DoDProperties.Add(new GridViewPropertyValueItem("Uncertainty Analysis", dod.UncertaintyAnalysisLabel));

            DoDProperties.Add(new GridViewPropertyValueItem("Output Datasets"));
            DoDProperties.Add(new GridViewPropertyValueItem("DoD Analysis Folder", ProjectManager.Project.GetRelativePath(dod.Folder.FullName)));
            DoDProperties.Add(new GridViewGCDProjectItem(dod.ThrDoD));
            DoDProperties.Add(new GridViewGCDProjectItem(dod.RawDoD));

            DoDProperties.Add(new GridViewPropertyValueItem("Intermediate Datasets"));
            if (dod is DoDPropagated)
            {
                DoDPropagated dodProp = dod as DoDPropagated;
                DoDProperties.Add(new GridViewRasterItem("Propagated Error Surface", dodProp.PropagatedError));

                if (dod is DoDProbabilistic)
                {
                    DoDProbabilistic dodProb = dod as DoDProbabilistic;

                    if (dodProb.SpatialCoherence == null)
                    {
                        DoDProperties.Add(new GridViewRasterItem("Probability Raster", dodProb.PriorProbability));
                    }
                    else
                    {
                        DoDProperties.Add(new GridViewPropertyValueItem("Spatial Coherence", string.Format("Bayesian updating with filter size of {0} X {0} cells", ((DoDProbabilistic)dod).SpatialCoherence.BufferSize)));
                        DoDProperties.Add(new GridViewGCDProjectItem(new DoDIntermediateRaster("Probability Raster", dodProb.PriorProbability)));
                        DoDProperties.Add(new GridViewGCDProjectItem(new DoDIntermediateRaster("Posterior Raster", dodProb.PosteriorProbability)));
                        DoDProperties.Add(new GridViewGCDProjectItem(new DoDIntermediateRaster("Conditional Raster", dodProb.ConditionalRaster)));
                        DoDProperties.Add(new GridViewGCDProjectItem(new DoDIntermediateRaster("Surface Lowering Count", dodProb.SpatialCoherenceErosion)));
                        DoDProperties.Add(new GridViewGCDProjectItem(new DoDIntermediateRaster("Surface Raising Count", dodProb.SpatialCoherenceDeposition)));
                    }
                }
            }
            DoDProperties.Add(new GridViewRasterItem("Effective Threshold Surface", dod.ThrErr.Raster));

            //// Values from the thresholded DoD raster stats are optional
            //try
            //{
            //    DoDProperties.Add(new GridViewPropertyValueItem("Output Raster Statistics"));
            //    string vUnits = UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit);
            //    dod.ThrDoD.Raster.ComputeStatistics();
            //    Dictionary<string, decimal> stats = dod.ThrDoD.Raster.GetStatistics();
            //    DoDProperties.Add(new DoDIntermediateRaster( GridViewPropertyValueItem("Thresholded DoD maximum raster value", stats["max"].ToString("n2") + vUnits));
            //    DoDProperties.Add(new GridViewPropertyValueItem("Thresholded DoD minimum raster value", stats["min"].ToString("n2") + vUnits));
            //    DoDProperties.Add(new GridViewPropertyValueItem("Thresholded DoD mean raster value", stats["mean"].ToString("n2") + vUnits));
            //    DoDProperties.Add(new GridViewPropertyValueItem("Thresholded DoD standard deviation of raster values", stats["stddev"].ToString("n2") + vUnits));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(string.Format("Error calculating statistics from {0}, {1}", dod.ThrDoD.Raster.GISFileInfo.FullName, ex.Message));
            //}
        }

        private void addToMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count < 1)
                return;

            if (grdData.SelectedRows[0].DataBoundItem is GridViewGCDProjectItem)
            {
                GridViewGCDProjectItem propItem = (GridViewGCDProjectItem)grdData.SelectedRows[0].DataBoundItem;
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

        private void rasterPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count < 1)
                return;

            if (grdData.SelectedRows[0].DataBoundItem is GridViewGCDProjectItem)
            {
                GridViewGCDProjectItem propItem = (GridViewGCDProjectItem)grdData.SelectedRows[0].DataBoundItem;
                SurveyLibrary.frmSurfaceProperties frm = new SurveyLibrary.frmSurfaceProperties((GCDProjectRasterItem)propItem.ProjectItem, false);
                frm.ShowDialog();
            }
        }
    }
}
