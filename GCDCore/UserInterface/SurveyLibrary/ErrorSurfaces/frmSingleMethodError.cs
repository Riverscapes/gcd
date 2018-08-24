using System;
using System.IO;
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
            ucName.InitializeNewRaster("Error Surface", DEM.ErrorSurfaces.Select(x => x.Name).ToList<string>(), DEM.ErrorSurfacesFolder, "Err");

            // Hide raster properties
            ucRasterProperties1.Visible = false;
            Height -= cmdOK.Top - ucRasterProperties1.Top;
        }

        public frmSingleMethodError(ErrorSurface errSurface)
        {
            InitializeComponent();
            DEM = errSurface.Surf as DEMSurvey;
            ErrorSurface = errSurface;

            // Need to exclude the current item from this list
            List<string> existingNames = DEM.ErrorSurfaces.Where(x => !x.Equals(errSurface)).Select(x => x.Name).ToList();

            ucErrProps.InitializeExisting(errSurface.ErrorProperties.Values.First(), DEM.AssocSurfaces.ToList(), errSurface == null);
            ucName.InitializeExisting("Error Surface", existingNames, ErrorSurface.Name, ErrorSurface.Raster.GISFileInfo);

            // Initialize raster properties
            ucRasterProperties1.Initialize(errSurface.Noun, errSurface.Raster);
        }

        protected void frmSingleMethodError_Load(object sender, EventArgs e)
        {
            cmdOK.Text = ErrorSurface == null ? Properties.Resources.CreateButtonText : Properties.Resources.UpdateButtonText;

            // Set up the IsDefault checkbox
            InitializeDefaultCheckBox(chkDefault, ErrorSurface, DEM);

            tTip.SetToolTip(chkDefault, "Specifies whether this is the default error surface that is used when the parent DEM survey is used within a change detection.");
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
                    // If this is a FIS error surface then copy the FIS file to the project and point the error surface property to
                    // this local file before saving the project. This will ensure that path to the FIS file is local to the project.
                    // MUST BE DONE BEFORE SAVING ERROR PROPERTIES TO THE PROJECT
                    ucErrProps.ErrSurfProperty.CloneToProject(ucName.ItemName, ucName.AbsolutePath.Directory);

                    // Create the raster then add it to the DEM survey
                    ucName.AbsolutePath.Directory.Create();
                    RasterOperators.CreateErrorRaster(DEM.Raster, ucErrProps.ErrSurfProperty.GCDErrSurfPropery, ucName.AbsolutePath, ProjectManager.OnProgressChange);
                    ErrorSurface = new ErrorSurface(ucName.ItemName, ucName.AbsolutePath, DEM, chkDefault.Checked, ucErrProps.ErrSurfProperty);
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
                GCDException.HandleException(ex, "Error editing single region error surface");
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        /// <summary>
        /// One place that both the single and multi-method error surface forms enable/disable and check the default box
        /// </summary>
        /// <param name="chkDefault">Is default checkbox control</param>
        /// <param name="errSurf">error surface being created (null) or edited</param>
        /// <param name="surface">parent surface</param>
        public static void InitializeDefaultCheckBox(CheckBox chkDefault, ErrorSurface errSurf, Surface surface)
        {
            if (errSurf == null)
            {
                chkDefault.Checked = surface.ErrorSurfaces.Count == 0;
                chkDefault.Enabled = surface.ErrorSurfaces.Count > 0;
            }
            else
            {
                chkDefault.Checked = errSurf.IsDefault;

                if (errSurf.IsDefault)
                    chkDefault.Enabled = false;
                else
                    chkDefault.Enabled = surface.ErrorSurfaces.Count > 1;
            }
        }
    }
}
