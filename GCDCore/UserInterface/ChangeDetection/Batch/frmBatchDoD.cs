using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    public partial class frmBatchDoD : Form
    {
        public readonly naru.ui.SortableBindingList<ThresholdProps> Thresholds;

        public frmBatchDoD()
        {
            InitializeComponent();
            Thresholds = new naru.ui.SortableBindingList<ThresholdProps>();
        }

        private void frmBatchDoD_Load(object sender, EventArgs e)
        {
            UpdateControls(sender, e);
            grdMethods.AutoGenerateColumns = false;
            grdMethods.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdMethods.SelectionChanged += UpdateControls;
            grdMethods.DataSource = Thresholds;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            // If the user clicks OK the child form will
            // append the appropriate ThresholdProps to the binding list
            frmBatchDoDProperties frm = new frmBatchDoDProperties(Thresholds);
            frm.ShowDialog();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdMethods.SelectedRows)
                Thresholds.Remove((ThresholdProps)row.DataBoundItem);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            cmdDelete.Enabled = grdMethods.SelectedRows.Count > 0;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private bool ValidateForm()
        {
            if (!ucDoDDEMs.ValidateForm())
                return false;

            if (grdMethods.Rows.Count < 1)
            {
                MessageBox.Show("You must specify one or more uncertainty analysis methods to continue.", "No Uncertainty Analysis Methods", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdAdd.Select();
                return false;
            }

            return true;
        }
    }
}
