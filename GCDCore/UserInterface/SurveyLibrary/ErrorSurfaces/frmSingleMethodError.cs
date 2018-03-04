using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    public partial class frmSingleMethodError : Form, IProjectItemForm
    {
        public const string m_sEntireDEMExtent = "Entire DEM Extent";

        public readonly DEMSurvey DEM;
        public ErrorSurface ErrorSurface { get; internal set; }
        public GCDProjectItem GCDProjectItem { get { return ErrorSurface as GCDProjectItem; } }

         public frmSingleMethodError(DEMSurvey parentDEM)
        {
            InitializeComponent();
            DEM = parentDEM;

            ucErrProps.InitializeNew(m_sEntireDEMExtent, DEM.AssocSurfaces.ToList());
            ucName.InitializeNewRaster("Error Surface", DEM.ErrorSurfaces.Select(x => x.Name).ToList<string>(), DEM.Raster.GISFileInfo.Directory, "Err");
        }

        public frmSingleMethodError(ErrorSurface errSurface)
        {
            InitializeComponent();
            DEM = errSurface.Surf as DEMSurvey;
            ErrorSurface = errSurface;

            // Need to exclude the current item from this list
            List<string> existingNames = DEM.ErrorSurfaces.Where(x => !x.Equals(errSurface)).Select(x => x.Name).ToList();

            ucName.InitializeExisting("Error Surface", existingNames, ErrorSurface.Name, ErrorSurface.Raster.GISFileInfo);
        }

        protected void frmSingleMethodError_Load(object sender, EventArgs e)
        {
            chkDefault.Checked = (ErrorSurface != null && ErrorSurface.IsDefault) || DEM.ErrorSurfaces.Count == 0;
            chkDefault.Enabled = DEM.ErrorSurfaces.Count > 1;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ucName.ValidateForm() || !ucErrProps.ValidateForm())
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
                    RasterOperators.CreateErrorRaster(DEM.Raster, ucErrProps.ErrSurfProperty.GCDErrSurfPropery, ucName.AbsolutePath);
                    ErrorSurface = new ErrorSurface(ucName.ItemName, ucName.AbsolutePath, DEM, chkDefault.Checked, ucErrProps.ErrSurfProperty);
                    DEM.ErrorSurfaces.Add(ErrorSurface);
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
    }
}
