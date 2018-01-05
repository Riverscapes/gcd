using GCDCore.Project;
using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDProperties
    {
        public DoDBase DoD { get; internal set; }

        private void DodPropertiesUC_Load(object sender, System.EventArgs e)
        {
            // Make all textboxes have invisible border. Easier to keep border turned on
            // for working in the designer.
            foreach (Control aControl in this.Controls)
            {
                if (aControl is TextBox)
                {
                    ((TextBox)aControl).BorderStyle = System.Windows.Forms.BorderStyle.None;
                }
            }

            if (ProjectManager.IsArcMap)
            {
                txtNewDEM.ContextMenuStrip = cmsBasicRaster;
                txtOldDEM.ContextMenuStrip = cmsBasicRaster;
                txtNewError.ContextMenuStrip = cmsBasicRaster;
                txtOldError.ContextMenuStrip = cmsBasicRaster;

                txtPropErr.ContextMenuStrip = cmsBasicRaster;

                txtProbabilityRaster.ContextMenuStrip = cmsBasicRaster;
                txtErosionalSpatialCoherenceRaster.ContextMenuStrip = cmsBasicRaster;
                txtDepositionSpatialCoherenceRaster.ContextMenuStrip = cmsBasicRaster;
                txtPosteriorRaster.ContextMenuStrip = cmsBasicRaster;
                txtConditionalRaster.ContextMenuStrip = cmsBasicRaster;
            }
        }

        public void Initialize(DoDBase dod)
        {
            DoD = dod;
            txtNewDEM.Text = dod.NewDEM.Name;
            txtOldDEM.Text = dod.OldDEM.Name;

            if (dod is DoDMinLoD)
            {
                txtType.Text = "Minimum Level of Detection (MinLoD)";
                var _with1 = (DoDMinLoD)dod;
                txtThreshold.Text = string.Format("{0:0.00}{1}", _with1.Threshold, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
                lblNewError.Visible = false;
                lblOldError.Visible = false;
                txtNewError.Visible = false;
                txtOldError.Visible = false;
                grpPropagated.Visible = false;
            }
            else
            {
                txtType.Text = "Propagated Error";
                grpPropagated.Visible = true;

                txtNewError.Text = ((DoDPropagated)dod).NewError.Name;
                txtOldError.Text = ((DoDPropagated)dod).OldError.Name;
                txtPropErr.Text = ProjectManager.Project.GetRelativePath(((DoDPropagated)dod).PropagatedError.GISFileInfo);

                if (dod is DoDProbabilistic)
                {
                    txtType.Text = "Probabilistic";
                    grpProbabilistic.Visible = true;
                    grpPropagated.Visible = true;

                    var _with3 = (DoDProbabilistic)dod;
                    txtConfidence.Text = (100 * ((DoDProbabilistic)dod).ConfidenceLevel).ToString("0") + "%";
                    txtProbabilityRaster.Text = ProjectManager.Project.GetRelativePath(((DoDProbabilistic)dod).PriorProbability.GISFileInfo);
                    txtBayesian.Text = "None";

                    if (_with3.SpatialCoherence is GCDCore.Project.CoherenceProperties)
                    {
                        txtPosteriorRaster.Text = ProjectManager.Project.GetRelativePath(((DoDProbabilistic)dod).PosteriorProbability.GISFileInfo);
                        txtConditionalRaster.Text = ProjectManager.Project.GetRelativePath(((DoDProbabilistic)dod).ConditionalRaster.GISFileInfo);
                        txtErosionalSpatialCoherenceRaster.Text = ProjectManager.Project.GetRelativePath(((DoDProbabilistic)dod).SpatialCoherenceErosion.GISFileInfo);
                        txtDepositionSpatialCoherenceRaster.Text = ProjectManager.Project.GetRelativePath(((DoDProbabilistic)dod).SpatialCoherenceDeposition.GISFileInfo);
                        txtBayesian.Text = string.Format("Bayesian updating with filter size of {0} X {0} cells", ((DoDProbabilistic)dod).SpatialCoherence.BufferSize);
                    }
                }
            }
        }


        private void AddToMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            ToolStripMenuItem myItem = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)myItem.Owner;

            string sItemName = string.Empty;

            if (cms.SourceControl.Name.ToLower().Contains("dem"))
            {
                // Get the new or old DEM
                DEMSurvey dem = cms.SourceControl.Name.ToLower().Contains("new") ? DoD.NewDEM : DoD.OldDEM;
                ProjectManager.OnAddToMap(dem);
            }
            else if (cms.SourceControl.Name.ToLower().Contains("error"))
            {
                if (DoD is DoDPropagated)
                {
                    ErrorSurface err = cms.SourceControl.Name.ToLower().Contains("new") ? ((DoDPropagated)DoD).NewError : ((DoDPropagated)DoD).OldError;
                    ProjectManager.OnAddToMap(err);
                }
            }
            else
            {
                string sPath = cms.SourceControl.Text;
                if (!string.IsNullOrEmpty(sPath))
                {
                    if (System.IO.File.Exists(sPath))
                    {
                        string rasterFileName = System.IO.Path.GetFileNameWithoutExtension(sPath);
                        string rasterDisplayName = string.Empty;

                        switch (rasterFileName)
                        {
                            case "PropErr": rasterDisplayName = "Propagated Error"; break;
                            case "priorProb": rasterDisplayName = "Prior Probability"; break;
                            case "nbrErosion": rasterDisplayName = "Erosion Spatial Coherence"; break;
                            case "nbrDeposition": rasterDisplayName = "Deposition Spatial Coherence"; break;
                        }

                        System.IO.FileInfo rasterPath = ProjectManager.Project.GetAbsolutePath(sPath);
                        GCDProjectRasterItem raster = new GCDProjectRasterItem("Propagated Error", rasterPath);
                        ProjectManager.OnAddToMap(raster);
                    }
                }
            }
        }

        private void PropertiesToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("Properties not yet implemented", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public ucDoDProperties()
        {
            Load += DodPropertiesUC_Load;
            InitializeComponent();
        }
    }
}
