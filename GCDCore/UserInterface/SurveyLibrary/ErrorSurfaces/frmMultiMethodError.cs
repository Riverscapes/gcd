using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib.GCD;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    public partial class frmMultiMethodError : Form, IProjectItemForm
    {
        DEMSurvey DEM;
        public ErrorSurface ErrorSurface { get; internal set; }
        public GCDProjectItem GCDProjectItem { get { return ErrorSurface as GCDProjectItem; } }
        private naru.ui.SortableBindingList<ErrorSurfaceProperty> ErrProps;

        public frmMultiMethodError(DEMSurvey dem)
        {
            InitializeComponent();
            DEM = dem;
            ErrProps = new naru.ui.SortableBindingList<ErrorSurfaceProperty>();
            ucName.InitializeNewRaster("Error Surface", dem.ErrorSurfaces.Select(x => x.Name).ToList<string>(), DEM.ErrorSurfacesFolder, "Err");
        }

        public frmMultiMethodError(ErrorSurface errSurf)
        {
            InitializeComponent();
            DEM = errSurf.Surf as DEMSurvey;
            ErrorSurface = errSurf;
            ErrProps = new naru.ui.SortableBindingList<ErrorSurfaceProperty>(errSurf.ErrorProperties.Values.ToList());

            // Need to exclude the current item from this list
            List<string> existingNames = DEM.ErrorSurfaces.Where(x => !x.Equals(errSurf)).Select(x => x.Name).ToList();
            ucName.InitializeExisting("Error Surface", existingNames, ErrorSurface.Name, ErrorSurface.Raster.GISFileInfo);
        }

        private void frmMultiMethodError_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;
            chkDefault.Checked = (ErrorSurface != null && ErrorSurface.IsDefault) || DEM.ErrorSurfaces.Count == 0;
            chkDefault.Enabled = DEM.ErrorSurfaces.Count > 0;

            if (!ProjectManager.IsArcMap)
            {
                cmdAddMaskToMap.Visible = false;
                cboMask.Width = cmdAddMaskToMap.Right - cboMask.Left;
            }

            cboMask.SelectedIndexChanged += cboMask_SelectedIndexChanged;
            cboMask.DataSource = ProjectManager.Project.Masks.Values.Where(x => x is GCDCore.Project.Masks.RegularMask).ToList();

            grdRegions.AutoGenerateColumns = false;
            grdRegions.DataSource = ErrProps;

            if (ErrorSurface != null)
            {
                cboMask.Enabled = false;
                grdRegions.Enabled = false;
            }

        }

        private void cboMask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ErrorSurface == null)
            {
                GCDCore.Project.Masks.RegularMask mask = cboMask.SelectedItem as GCDCore.Project.Masks.RegularMask;
                ErrProps.Clear();
                mask.ActiveFieldValues.Values.ToList().ForEach(x => ErrProps.Add(new ErrorSurfaceProperty(x)));
            }
            else
            {
                ErrProps = new naru.ui.SortableBindingList<ErrorSurfaceProperty>(ErrorSurface.ErrorProperties.Values.ToList());
            }
        }

        private void cmdAddMaskToMap_Click(object sender, EventArgs e)
        {
            ProjectManager.OnAddVectorToMap(((GCDCore.Project.Masks.RegularMask)cboMask.SelectedItem));
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ucName.ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                if (chkDefault.Checked && DEM.ErrorSurfaces.Count > 0)
                {
                    // Need to set all other error surfaces to not be the default
                    DEM.ErrorSurfaces.ToList().ForEach(x => x.IsDefault = false);
                }

                if (ErrorSurface == null)
                {
                    // Create the raster then add it to the DEM survey
                    ucName.AbsolutePath.Directory.Create();

                    GCDCore.Project.Masks.RegularMask mask = cboMask.SelectedItem as GCDCore.Project.Masks.RegularMask;

                    // Build dictionary of GCDConsole error properties
                    Dictionary<string, ErrorRasterProperties> gcdErrProps = new Dictionary<string, ErrorRasterProperties>();
                    ErrProps.ToList().ForEach(x => gcdErrProps.Add(x.Name, x.GCDErrSurfPropery));

                    // Build dictionary of GCD project error properties
                    Dictionary<string, ErrorSurfaceProperty> errProps = new Dictionary<string, ErrorSurfaceProperty>();
                    ErrProps.ToList().ForEach(x => errProps.Add(x.Name, x));

                    RasterOperators.CreateErrorRaster(DEM.Raster, mask.Vector, mask._Field, gcdErrProps, ucName.AbsolutePath);
                    ErrorSurface = new ErrorSurface(ucName.ItemName, ucName.AbsolutePath, DEM, chkDefault.Checked, errProps, mask);
                    DEM.ErrorSurfaces.Add(ErrorSurface);
                    ProjectManager.AddNewProjectItemToMap(ErrorSurface);
                }
                else
                {
                    ErrorSurface.Name = ucName.ItemName;
                    ErrorSurface.IsDefault = chkDefault.Checked;
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show("Error Surface Created Successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                naru.error.ExceptionUI.HandleException(ex, "Error editing single region error surface");
            }
        }

        private void grdRegions_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditErrorProperty(sender, e);
        }

        private void EditErrorProperty(object sender, EventArgs e)
        {
            ErrorSurfaceProperty errProp = grdRegions.SelectedRows[0].DataBoundItem as ErrorSurfaceProperty;

            frmRegionErrorProperty frm = new frmRegionErrorProperty(errProp.Name, errProp, DEM.AssocSurfaces.ToList());
            if (frm.ShowDialog() == DialogResult.OK)
                ErrProps.ResetBindings();
        }

        private void grdRegions_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
                grdRegions.Rows[e.RowIndex].Selected = true;
        }
    }
}
