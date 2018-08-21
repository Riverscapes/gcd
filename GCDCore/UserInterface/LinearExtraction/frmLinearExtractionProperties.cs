using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;

namespace GCDCore.UserInterface.LinearExtraction
{
    public partial class frmLinearExtractionProperties : Form, IProjectItemForm
    {
        public readonly GCDProjectItem ElevationSurface;
        public GCDCore.Project.LinearExtraction.LinearExtraction LinearExtraction;

        public GCDProjectItem GCDProjectItem { get { return LinearExtraction; } }

        public frmLinearExtractionProperties(GCDProjectItem surface)
        {
            InitializeComponent();
            ElevationSurface = surface;
        }

        public frmLinearExtractionProperties(GCDCore.Project.LinearExtraction.LinearExtraction linExt = null)
        {
            InitializeComponent();
            LinearExtraction = linExt;
        }

        private void frmLinearExtractionProperties_Load(object sender, EventArgs e)
        {
            cboRoute.DataSource = ProjectManager.Project.ProfileRoutes;


            if (LinearExtraction == null)
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

                cmdOK.Text = Properties.Resources.CreateButtonText;
                txtElevationSurface.Text = ElevationSurface.Name;
                cboRoute.SelectedIndex = cboRoute.Items.Count > 0 ? 0 : -1;
            }
            else
            {
                cmdOK.Text = Properties.Resources.UpdateButtonText;
                txtName.Text = LinearExtraction.Name;
                txtPath.Text = ProjectManager.Project.GetRelativePath(LinearExtraction.Database.FullName);

                txtElevationSurface.Visible = false;

                valSampleDistance.Value = LinearExtraction.SampleDistance;
                valSampleDistance.Enabled = false;
                cboError.Enabled = false;
                cboRoute.Enabled = false;
                txtPath.Enabled = false;

            }

            lblSampleDistance.Text = lblSampleDistance.Text.Replace("(", string.Format("({0}", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.HorizUnit)));

            tTip.SetToolTip(txtName, "The name for this linear extraction. It cannot be empty and must be unique among all linear extractions for the parent DEM survey, reference surface or DoD.");
            tTip.SetToolTip(txtPath, "The relative path where the linear extraction output will be generated within this GCD project.");
            tTip.SetToolTip(cboRoute, "The ShapeFile containing the polyline features that will be traversed to extract values from the underlying raster.");
            tTip.SetToolTip(valSampleDistance, "The distance interval along each polyline feature at which values will be sampled from the underlying raster.");
            tTip.SetToolTip(txtElevationSurface, "The name of the underlying DEM survey, reference surface or DoD raster that will be sampled.");
            tTip.SetToolTip(cboError, "The (optional) error surface raster that will also be sampled at the sample locations as the elevation surface and also written to the linear extraction output.");
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

            if (diParent != null)
            {
                diParent = new DirectoryInfo(Path.Combine(diParent.FullName, "LinearExt"));
                txtPath.Text = diParent == null ? string.Empty : txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.GetProjectItemPath(diParent, "LE", txtName.Text, "csv"));
            }
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

                if (LinearExtraction == null)
                {
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
                        if (dod.ThrErr != null)
                        {
                            rasters.Add(dod.ThrErr.Raster);
                        }
                    }

                    FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiOutput.Directory.Create();

                    RasterOperators.LinearExtractor(route.Vector, rasters, fiOutput, valSampleDistance.Value, route.DistanceField, ProjectManager.OnProgressChange);

                    GCDCore.Project.LinearExtraction.LinearExtraction le;
                    if (ElevationSurface is DEMSurvey)
                    {
                        le = new GCDCore.Project.LinearExtraction.LinearExtractionFromDEM(txtName.Text, route, fiOutput, valSampleDistance.Value, ElevationSurface as DEMSurvey, errSurf);
                    }
                    else if (ElevationSurface is Surface)
                    {
                        le = new GCDCore.Project.LinearExtraction.LinearExtractionFromSurface(txtName.Text, route, fiOutput, valSampleDistance.Value, ElevationSurface as Surface, errSurf);
                    }
                    else
                        le = new GCDCore.Project.LinearExtraction.LinearExtractionFromDoD(txtName.Text, route, fiOutput, valSampleDistance.Value, ElevationSurface as DoDBase);

                    if (ElevationSurface is Surface)
                    {
                        ((Surface)ElevationSurface).LinearExtractions.Add(le);
                    }
                    else
                    {
                        ((DoDBase)ElevationSurface).LinearExtractions.Add(le);
                    }


                }
                else
                {
                    LinearExtraction.Name = txtName.Text;
                }


                ProjectManager.Project.Save();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error performing linear extraction.");
                DialogResult = DialogResult.None;
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

        private void valSampleDistance_Enter(object sender, EventArgs e)
        {
            valSampleDistance.Select(0, valSampleDistance.Text.Length);
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
