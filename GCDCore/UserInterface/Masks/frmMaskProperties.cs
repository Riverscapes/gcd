using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmMaskProperties : Form
    {
        public readonly naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem> MaskItems;

        public frmMaskProperties()
        {
            InitializeComponent();
            MaskItems = new naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem>();
        }

        private void frmMaskProperties_Load(object sender, EventArgs e)
        {
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Point Bars", "Point Bars"));
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Riffle", "Riffle"));
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Riffle Type 2", "Riffle"));
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Pool", "Pool"));
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Beaver Dam", "Beaver Dam"));
            MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, "Glide", "Custom Glide"));
       

            grdData.DataSource = MaskItems;
        }
    }
}
