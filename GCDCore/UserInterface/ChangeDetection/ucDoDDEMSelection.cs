using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using GCDCore.Project.Masks;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDDEMSelection : UserControl
    {
        // Event so that parent form can be alerted when DEM names change
        public EventHandler SelectedSurfacesChanged;

        private BindingList<Surface> NewSurfaces;
        private BindingList<Surface> OldSurfaces;

        // Initial DEM Surveys to select. (User right clicked on a pair of DEMs
        // in the project explorer and chose to add a new DoD for the same DEMs.
        public Surface NewSurface
        {
            get
            {
                return (Surface)cboNewSurface.SelectedItem;
            }
            set
            {
                cboNewSurface.SelectedItem = value;
            }
        }

        public Surface OldSurface
        {
            get
            {
                return (Surface)cboOldSurface.SelectedItem;
            }
            set
            {
                cboOldSurface.SelectedItem = value;
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

        public AOIMask AOIMask
        {
            get
            {
                return ucAOI1.AOIMask;
            }
        }

        public ucDoDDEMSelection()
        {
            InitializeComponent();

            if (ProjectManager.Project == null)
                return;

            NewSurfaces = new BindingList<Surface>();
            OldSurfaces = new BindingList<Surface>();

            // Add all DEM Surveys
            ProjectManager.Project.DEMSurveys.ForEach(x => NewSurfaces.Add(x));
            ProjectManager.Project.DEMSurveys.ForEach(x => OldSurfaces.Add(x));

            // Add all reference surfaces
            ProjectManager.Project.ReferenceSurfaces.ForEach(x => NewSurfaces.Add(x));
            ProjectManager.Project.ReferenceSurfaces.ForEach(x => OldSurfaces.Add(x));

            cboNewSurface.DataSource = NewSurfaces;
            cboOldSurface.DataSource = OldSurfaces;
        }

        private void ucDoDDEMSelection_Load(object sender, EventArgs e)
        {
            if (cboNewSurface.Items.Count > 0)
            {
                if (NewSurface is Surface)
                    cboNewSurface.SelectedItem = NewSurface;
                else
                    cboNewSurface.SelectedIndex = 0;
            }

            if (cboOldSurface.Items.Count > 0)
            {
                if (OldSurface is Surface)
                    cboOldSurface.SelectedItem = OldSurface;
                else
                {
                    if (cboOldSurface.Items.Count > 1)
                        cboOldSurface.SelectedIndex = 1;
                    else
                        cboOldSurface.SelectedIndex = 0;
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
                {
                    SelectDefaultErrorSurface(cboNewSurface, cboNewError);
                }

                if (cboOldError.Items.Count > 0 && cboOldError.SelectedItem == null)
                {
                    SelectDefaultErrorSurface(cboOldSurface, cboOldError);
                }
            }
            else
            {
                cboNewError.SelectedIndex = -1;
                cboOldError.SelectedIndex = -1;
            }
        }

        private void SelectDefaultErrorSurface(ComboBox cboSurface, ComboBox cboError)
        {
            Surface surf = cboSurface.SelectedItem as Surface;
            cboError.DataSource = surf.ErrorSurfaces;

            if (cboError.Items.Count == 1)
            {
                cboError.SelectedIndex = 0;
            }
            else if (cboError.Items.Count > 1 && surf.ErrorSurfaces.Any(x => x.IsDefault))
            {
                cboError.SelectedItem = ((Surface)cboSurface.SelectedItem).ErrorSurfaces.First(x => x.IsDefault);
            }
        }

        private void SurfaceComboSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cboSurface = sender as ComboBox;
            ComboBox cboError = cboSurface.Name.ToLower().Contains("new") ? cboNewError : cboOldError;

            if (cboSurface.SelectedItem is Surface)
            {
                SelectDefaultErrorSurface(cboSurface, cboError);
            }

            if (SelectedSurfacesChanged != null)
                SelectedSurfacesChanged(sender, e);
        }

        public bool ValidateForm()
        {
            if (cboNewSurface.SelectedItem is Surface)
            {
                if (cboOldSurface.SelectedItem is Surface)
                {
                    if (object.ReferenceEquals(cboNewSurface.SelectedItem, cboOldSurface.SelectedItem))
                    {
                        MessageBox.Show("Please choose two different surfaces.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cboNewSurface.Select();
                        return false;
                    }

                    Surface newSurface = (Surface)cboNewSurface.SelectedItem;
                    Surface oldSurface = (Surface)cboOldSurface.SelectedItem;

                    if (!newSurface.Raster.Extent.HasOverlap(oldSurface.Raster.Extent))
                    {
                        MessageBox.Show("The two surfaces do not overlap.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Please select an old surface.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboOldSurface.Select();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please select a new surface.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNewSurface.Select();
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
