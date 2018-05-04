using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    public partial class frmRegionErrorProperty : Form
    {
        public ErrorSurfaceProperty ErrorSurProp { get { return ucErrProp.ErrSurfProperty; } }

        public frmRegionErrorProperty(string region, ErrorSurfaceProperty errProp, List<AssocSurface> assocs)
        {
            InitializeComponent();

            txtRegion.Text = region;
            ucErrProp.InitializeExisting(errProp, assocs);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ucErrProp.ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }
        }

        private void frmRegionErrorProperty_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.UpdateButtonText;

            tTip.SetToolTip(txtRegion, "The name of the mask region where this error configuration will be applied.");
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
