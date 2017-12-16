using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection.MultiEpoch
{
    public partial class frmMultiEpoch : Form
    {
        public readonly BindingList<DEMSurveyItem> DEMs;
        public readonly BindingList<Epoch> Epochs;

        public frmMultiEpoch()
        {
            InitializeComponent();

            // List of project DEMs that the user cannot add to or remove from, but they can change the order of this list.
            DEMs = new BindingList<DEMSurveyItem>(ProjectManager.Project.DEMSurveys.Values.Select(x => new DEMSurveyItem(x, x.DefaultErrorSurface)).ToList<DEMSurveyItem>());

            // Empty list of epochs that will have items added to and removed form as user interacts with form controls
            Epochs = new BindingList<Epoch>();

        }

        private void frmInterComp_Load(object sender, EventArgs e)
        {
            grdEpochs.AutoGenerateColumns = false;

            // Bind the two grids to the corresponding lists
            grdDEMs.DataSource = DEMs;
            grdEpochs.DataSource = Epochs;
     
            // Update the status of the move up and down buttons
            grdDEMs_SelectionChanged(sender, e);
      }

        private void cmdMoveUp_Click(object sender, EventArgs e)
        {
            DEMSurveyItem selectedDEM = (DEMSurveyItem)grdDEMs.SelectedRows[0].DataBoundItem;
            int index = DEMs.IndexOf(selectedDEM);
            if (index > 0)
            {
                DEMs.Remove(selectedDEM);
                DEMs.Insert(index - 1, selectedDEM);
                grdDEMs.Rows[index - 1].Selected = true;
            }
        }

        private void cmdMoveDown_Click(object sender, EventArgs e)
        {
            DEMSurveyItem selectedDEM = (DEMSurveyItem)grdDEMs.SelectedRows[0].DataBoundItem;
            int index = DEMs.IndexOf(selectedDEM);
            if (index < DEMs.Count - 1)
            {
                DEMs.Remove(selectedDEM);
                DEMs.Insert(index + 1, selectedDEM);
                grdDEMs.Rows[index + 1].Selected = true;
            }
        }

        private void grdDEMs_SelectionChanged(object sender, EventArgs e)
        {
            if (grdDEMs.SelectedRows.Count > 0)
            {
                cmdMoveUp.Enabled = grdDEMs.SelectedRows[0].Index > 0;
                cmdMoveDown.Enabled = grdDEMs.SelectedRows[0].Index < DEMs.Count - 1;
            }
        }
    }
}
