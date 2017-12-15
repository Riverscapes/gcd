using GCDCore.Project;
using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDProperties
    {
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
        }

        public void Initialize(DoDBase dod)
        {
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
                // Get the new or old survey name
                if (cms.SourceControl.Name.ToLower().Contains("new"))
                {
                    sItemName = txtNewDEM.Text;
                }
                else
                {
                    sItemName = txtOldDEM.Text;
                }

                throw new NotImplementedException("add to map functionality commented out");
                //For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                //    If String.Compare(sItemName, aDEMSurvey.Name, True) = 0 Then
                //        '  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(aDEMSurvey)
                //        Exit Sub
                //    End If
                //Next


            }
            else if (cms.SourceControl.Name.ToLower().Contains("error"))
            {
                // Get the new or old error surface name
                if (cms.SourceControl.Name.ToLower().Contains("old"))
                {
                    sItemName = txtNewError.Text;
                }
                else
                {
                    sItemName = txtOldError.Text;
                }

                throw new NotImplementedException("Add to map functionaility commented out");
                //For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                //    For Each anErrorSurface In aDEMSurvey.GetErrorSurfaceRows
                //        If String.Compare(anErrorSurface.Name, sItemName, True) = 0 Then
                //            'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(anErrorSurface)
                //            Exit Sub
                //        End If
                //    Next
                //Next
            }
            else
            {
                string sPath = cms.SourceControl.Text;
                if (!string.IsNullOrEmpty(sPath))
                {
                    if (System.IO.File.Exists(sPath))
                    {
                        //GISCode.ArcMap.AddToMap(My.ThisApplication, sPath, IO.Path.GetFileNameWithoutExtension(sPath), GISDataStructures.BasicGISTypes.Raster)
                        string sFileName = System.IO.Path.GetFileNameWithoutExtension(sPath);

                        switch (sFileName)
                        {

                            case "PropErr":
                                // TODO 
                                throw new Exception("not implemented");
                            // Core.GCDProject.ProjectManagerUI.ArcMapManager.AddPropErr(m_rDoD)

                            case "priorProb":
                                // TODO 
                                throw new Exception("not implemented");
                            //  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.ProbabilityRaster)

                            case "nbrErosion":
                                // TODO 
                                throw new Exception("not implemented");
                            // Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.SpatialCoErosionRaster)

                            case "nbrDeposition":
                                // TODO 
                                throw new Exception("not implemented");
                                //  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.SpatialCoDepositionRaster)

                        }
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
