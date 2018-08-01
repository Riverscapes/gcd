using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GCDCore.UserInterface
{
    public partial class ucRasterProperties : UserControl
    {
        private readonly naru.ui.SortableBindingList<GridViewPropertyValueItem> ItemProperties;

        public ucRasterProperties()
        {
            InitializeComponent();
            ItemProperties = new naru.ui.SortableBindingList<GridViewPropertyValueItem>();
        }

        private void ucRasterProperties_Load(object sender, EventArgs e)
        {
            grdData.AutoGenerateColumns = false;
            grdData.ColumnHeadersVisible = false;
            grdData.DataSource = ItemProperties;
            grdData.Select();
        }

        public void Initialize(string noun, GCDConsoleLib.Raster raster)
        {
            grpProperties.Text = string.Format("{0} {1}Properties", noun, noun.ToLower().EndsWith("raster") ? string.Empty : "Raster ");

            ItemProperties.Add(new GridViewPropertyValueItem("Raster Properties"));
            ItemProperties.Add(new GridViewPropertyValueItem("Cell size", UnitsNet.Length.From((double)raster.Extent.CellWidth, raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Rows", raster.Extent.Rows.ToString("N0")));
            ItemProperties.Add(new GridViewPropertyValueItem("Columns", raster.Extent.Cols.ToString("N0")));
            ItemProperties.Add(new GridViewPropertyValueItem("Height", UnitsNet.Length.From((double)raster.Extent.Height, raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Width", UnitsNet.Length.From((double)raster.Extent.Width, raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Spatial Reference", raster.Proj.Wkt));

            try
            {
                raster.ComputeStatistics();
                Dictionary<string, decimal> stats = raster.GetStatistics();
                ItemProperties.Add(new GridViewPropertyValueItem("Raster Statistics"));
                ItemProperties.Add(new GridViewPropertyValueItem("Maximum raster value", stats["max"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Minimum raster value", stats["min"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Mean raster value", stats["mean"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Standard deviation of raster values", stats["stddev"].ToString("n2")));
            }
            catch (Exception ex)
            {
                ItemProperties.Add(new GridViewPropertyValueItem("Raster Statistics", "Failed to compute statistics"));
            }
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
            }
        }
    }
}
