using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDDEMSelection : UserControl
    {

        // Event so that parent form can be alerted when DEM names change
        public EventHandler SelectedDEMsChanged;

        // Initial DEM Surveys to select. (User right clicked on a pair of DEMs
        // in the project explorer and chose to add a new DoD for the same DEMs.
        public DEMSurvey NewDEM
        {
            get
            {
                return (DEMSurvey)cboNewDEM.SelectedItem;
            }
            set
            {
                cboNewDEM.SelectedItem = value;
            }
        }

        public DEMSurvey OldDEM
        {
            get
            {
                return (DEMSurvey)cboOldDEM.SelectedItem;
            }
            set
            {
                cboOldDEM.SelectedItem = value;
            }
        }

        public ErrorSurface NewError
        {
            get
            {
                return (ErrorSurface)cboNewError.SelectedItem;
            }
        }

        public ErrorSurface OldError
        {
            get
            {
                return (ErrorSurface)cboOldError.SelectedItem;
            }
        }

        public ucDoDDEMSelection()
        {
            InitializeComponent();

            if (ProjectManager.Project != null)
            {
                cboNewDEM.DataSource = ProjectManager.Project.DEMsSortByName(false);
                cboOldDEM.DataSource = ProjectManager.Project.DEMsSortByName(false);
            }
        }

        private void ucDoDDEMSelection_Load(object sender, EventArgs e)
        {
            if (cboNewDEM.Items.Count > 0)
            {
                if (NewDEM is DEMSurvey)
                    cboNewDEM.SelectedItem = NewDEM;
                else
                    cboNewDEM.SelectedIndex = 0;
            }

            if (cboOldDEM.Items.Count > 0)
            {
                if (OldDEM is DEMSurvey)
                    cboOldDEM.SelectedItem = OldDEM;
                else
                {
                    if (cboOldDEM.Items.Count > 1)
                        cboOldDEM.SelectedIndex = 1;
                    else
                        cboOldDEM.SelectedIndex = 0;
                }
            }
        }

        public void EnableErrorSurfaces(bool bEnabled)
        {
            cboNewError.Enabled = bEnabled;
            cboOldError.Enabled = bEnabled;
            lblNewError.Enabled = bEnabled;
            lblOldError.Enabled = bEnabled;

            if (bEnabled)
            {
                if (cboNewError.Items.Count > 0 && cboNewError.SelectedItem == null)
                    cboNewError.SelectedIndex = 0;

                if (cboOldError.Items.Count > 0 && cboOldError.SelectedItem == null)
                    cboOldError.SelectedIndex = 0;
            }
            else
            {
                cboNewError.SelectedIndex = -1;
                cboOldError.SelectedIndex = -1;
            }
        }

        private void cboNewDEM_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboNewDEM.SelectedItem is DEMSurvey)
            {
                DEMSurvey dem = ((DEMSurvey)cboNewDEM.SelectedItem);
                cboNewError.DataSource = ((DEMSurvey)cboNewDEM.SelectedItem).ErrorSurfaces;

                if (cboNewError.Items.Count == 1)
                {
                    cboNewError.SelectedIndex = 0;
                }
                else if (cboOldError.Items.Count > 1 && dem.ErrorSurfaces.Count<ErrorSurface>(x => x.IsDefault) > 0)
                {
                    cboNewError.SelectedItem = ((DEMSurvey)cboNewDEM.SelectedItem).ErrorSurfaces.First<ErrorSurface>(x => x.IsDefault);
                }
            }

            if (SelectedDEMsChanged != null)
                SelectedDEMsChanged(sender, e);
        }

        private void cboOldDEM_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboOldDEM.SelectedItem is DEMSurvey)
            {
                DEMSurvey dem = ((DEMSurvey)cboOldDEM.SelectedItem);
                cboOldError.DataSource = dem.ErrorSurfaces;

                if (cboOldError.Items.Count == 1)
                {
                    cboOldError.SelectedIndex = 0;
                }
                else if (cboOldError.Items.Count > 1 && dem.ErrorSurfaces.Count<ErrorSurface>(x => x.IsDefault) > 0)
                {
                    cboOldError.SelectedItem = ((DEMSurvey)cboOldDEM.SelectedItem).ErrorSurfaces.First<ErrorSurface>(x => x.IsDefault);
                }
            }

            if (SelectedDEMsChanged != null)
                SelectedDEMsChanged(sender, e);
        }

        public bool ValidateForm()
        {
            if (cboNewDEM.SelectedItem is DEMSurvey)
            {
                if (cboOldDEM.SelectedItem is DEMSurvey)
                {
                    if (object.ReferenceEquals(cboNewDEM.SelectedItem, cboOldDEM.SelectedItem))
                    {
                        MessageBox.Show("Please choose two different DEM Surveys.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    DEMSurvey newDEM =(DEMSurvey) cboNewDEM.SelectedItem;
                    DEMSurvey oldDEM = (DEMSurvey)cboOldDEM.SelectedItem;
                        
                    if (!newDEM.Raster.Extent.HasOverlap(oldDEM.Raster.Extent))
                    {
                        MessageBox.Show("The two DEM surveys do not overlap.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Please select an Old DEM Survey.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboOldDEM.Select();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please select a New DEM Survey.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNewDEM.Select();
                return false;
            }

            if (cboNewError.Enabled)
            {
                if (!(cboNewError.SelectedItem is ErrorSurface))
                {
                    MessageBox.Show("Please select a new error surface.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboNewError.Select();
                    return false;
                }

                if (!(cboOldError.SelectedItem is ErrorSurface))
                {
                    MessageBox.Show("Please select an old error surface.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboOldError.Select();
                    return false;
                }
            }

            return true;
        }
    }
}
