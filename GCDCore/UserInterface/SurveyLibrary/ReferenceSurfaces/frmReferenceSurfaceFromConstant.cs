using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public partial class frmReferenceSurfaceFromConstant : Form
    {
        public naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey> DEMSurveys;

        public frmReferenceSurfaceFromConstant()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromConstant_Load(object sender, EventArgs e)
        {
            // Add all the project DEM surveys to the list and then bind to checked listbox
            DEMSurveys = new naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey>(GCDCore.Project.ProjectManager.Project.DEMSurveys.Values.ToList<GCDCore.Project.DEMSurvey>());
            cboDEMSurvey.DataSource = DEMSurveys;
            cboDEMSurvey.SelectedIndex = 0;

            string sUnits = UnitsNet.Length.GetAbbreviation(GCDCore.Project.ProjectManager.Project.Units.VertUnit);
            lblSingle.Text = lblSingle.Text.Replace(")", sUnits + ")");
            lblLower.Text = lblLower.Text.Replace(")", sUnits + ")");
            lblUpper.Text = lblUpper.Text.Replace(")", sUnits + ")");
            lblIncrement.Text = lblIncrement.Text.Replace(")", sUnits + ")");

            UpdateControls(null, null);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            lblSingle.Enabled = rdoSingle.Checked;
            valSingle.Enabled = rdoSingle.Checked;

            lblUpper.Enabled = rdoMultiple.Checked;
            valUpper.Enabled = rdoMultiple.Checked;
            lblLower.Enabled = rdoMultiple.Checked;
            valLower.Enabled = rdoMultiple.Checked;
            lblIncrement.Enabled = rdoMultiple.Checked;
            valIncrement.Enabled = rdoMultiple.Checked;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
