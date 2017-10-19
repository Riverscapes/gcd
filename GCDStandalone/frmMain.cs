using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDStandalone
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }  

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = GCDLib.My.Resources.Resources.ApplicationNameLong;
        }

        private void newGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.Core.WorkspaceManager.Initialize();

                GCDLib.UI.Project.frmProjectProperties frm = new GCDLib.UI.Project.frmProjectProperties(GCDLib.UI.Project.frmProjectProperties.DisplayModes.Create);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                   ucProjectExplorer1.cmdRefresh_Click(sender, e);
                }
            }
            catch(Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void openGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void browseGCDProjectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern void RegisterGDAL();

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern int GetRasterProperties(string sFullPath, ref double fCellHeight, ref double fCellWidth, ref double fLeft, ref double fTop, ref int nRows, ref int nCols, ref double fNoData, ref int nHasNoData, ref int nDataType,
        //System.Text.StringBuilder sUnits, System.Text.StringBuilder sSpatialReference, System.Text.StringBuilder sError);
    }
}
