using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;

namespace GCDCore.UserInterface.LinearExtraction
{
    public partial class frmLinearExtractionProperties : Form
    {
        public readonly GCDProjectItem ElevationSurface;
        public GCDCore.Project.LinearExtraction.LinearExtraction LinearExtraction;

        public frmLinearExtractionProperties(GCDProjectItem surface)
        {
            InitializeComponent();
            ElevationSurface = surface;
        }

        private void frmLinearExtractionProperties_Load(object sender, EventArgs e)
        {
            if (ElevationSurface is Surface)
            {
                Surface surf = ElevationSurface as Surface;

                // Select default error surface
                cboError.DataSource = surf.ErrorSurfaces;
                if (cboError.Items.Count == 1)
                {
                    cboError.SelectedIndex = 0;
                }
                else if (cboError.Items.Count > 1 && surf.ErrorSurfaces.Any(x => x.IsDefault))
                {
                    cboError.SelectedItem = surf.ErrorSurfaces.First(x => x.IsDefault);
                }
            }
            else
            {
                lblErrorSurface.Visible = false;
                cboError.Visible = false;
                Height -= cboError.Bottom - txtElevationSurface.Bottom;
            }

            txtElevationSurface.Text = ElevationSurface.Name;

            cboRoute.DataSource = ProjectManager.Project.ProfileRoutes.Values.ToList();
            cboRoute.SelectedIndex = cboRoute.Items.Count > 0 ? 0 : -1;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            DirectoryInfo diParent = null;
            if (ElevationSurface is Surface)
            {
                diParent = ((Surface)ElevationSurface).Raster.GISFileInfo.Directory;
            }
            else if (ElevationSurface is DoDBase)
            {
                diParent = ((DoDBase)ElevationSurface).Folder;
            }

            txtPath.Text = diParent == null ? string.Empty : txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetLinearExtractionFolder(diParent).FullName);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                GCDCore.Project.ProfileRoutes.ProfileRoute route = cboRoute.SelectedItem as GCDCore.Project.ProfileRoutes.ProfileRoute;

                ErrorSurface errSurf = null;
                List<GCDConsoleLib.Raster> rasters = new List<Raster>();
                if (ElevationSurface is Surface)
                {
                    rasters.Add(((Surface)ElevationSurface).Raster);
                    if (cboError.SelectedItem is ErrorSurface)
                    {
                        errSurf = cboError.SelectedItem as ErrorSurface;
                        rasters.Add(errSurf.Raster);
                    }
                }
                else
                {
                    DoDBase dod = ElevationSurface as DoDBase;
                    rasters.Add(dod.ThrDoD.Raster);
                    if (dod is DoDPropagated)
                    {
                        rasters.Add(((DoDPropagated)dod).PropagatedError);
                    }
                }

                FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(Path.Combine(txtPath.Text, "extraction.csv"));
                fiOutput.Directory.Create();

                GCDConsoleLib.RasterOperators.LinearExtractor(route.FeatureClass, rasters, fiOutput);

                GCDCore.Project.LinearExtraction.LinearExtraction le;
                if (ElevationSurface is DEMSurvey)
                {
                    le = new GCDCore.Project.LinearExtraction.LinearExtractionFromDEM(txtName.Text, route, fiOutput, ElevationSurface as DEMSurvey, errSurf);
                }
                else if (ElevationSurface is Surface)
                {
                    le = new GCDCore.Project.LinearExtraction.LinearExtractionFromSurface(txtName.Text, route, fiOutput, ElevationSurface as Surface, errSurf);
                }
                else
                    le = new GCDCore.Project.LinearExtraction.LinearExtractionFromDoD(txtName.Text, route, fiOutput, ElevationSurface as DoDBase);

                ProjectManager.Project.LinearExtractions[le.Name] = le;
                ProjectManager.Project.Save();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error creating regular mask.");
            }
        }

        private bool ValidateForm()
        {
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the linear extraction.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboRoute.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a profile route on which you want to base this linear extraction.", "Missing Profile Route", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
    }
}
