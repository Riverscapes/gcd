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
using GCDCore.Engines.DoD;

namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    public partial class frmBatchDoD : Form
    {
        public enum ThresholdTypes
        {
            MinLoDSingle,
            MinLoDMulti,
            Propagated,
            ProbSingle,
            ProbMulti
        }

        public readonly naru.ui.SortableBindingList<ThresholdProps> Thresholds;
        private GCDCore.Engines.DoD.ChangeDetetctionBatch BatchEngine;

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

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripItem tsi = new ToolStripMenuItem("Single Minimum Level of Detection", null, cmdAdd_Click);
            tsi.Tag = ThresholdTypes.MinLoDSingle;
            cms.Items.Add(tsi);

            ToolStripItem tsi2 = new ToolStripMenuItem("Multiple Minimum Level of Detections", null, cmdAdd_Click);
            tsi2.Tag = ThresholdTypes.MinLoDMulti;
            cms.Items.Add(tsi2);

            ToolStripItem tsi3 = new ToolStripMenuItem("Propagated Error", null, cmdAdd_Click);
            tsi3.Tag = ThresholdTypes.Propagated;
            cms.Items.Add(tsi3);

            ToolStripItem tsi4 = new ToolStripMenuItem("Single Probabilistic", null, cmdAdd_Click);
            tsi4.Tag = ThresholdTypes.ProbSingle;
            cms.Items.Add(tsi4);

            ToolStripItem tsi5 = new ToolStripMenuItem("Multiple Probabilistic", null, cmdAdd_Click);
            tsi5.Tag = ThresholdTypes.ProbMulti;
            cms.Items.Add(tsi5);

            // Prescribed is distinguished by not having a tag
            ToolStripItem tsi6 = new ToolStripMenuItem("Prescribed Probabilistic Thresholds", null, cmdAdd_Click);
            cms.Items.Add(tsi6);

            cmdAdd.Menu = cms;

            Thresholds.ListChanged += Thresholds_ListChanged;
        }

        /// <summary>
        /// Need to update where the error surface combo boxes should be enabled or disabled
        /// </summary>
        /// <remarks>
        /// Call this method after changing the </remarks>
        private void Thresholds_ListChanged(object sender, ListChangedEventArgs e)
        {
            ucDEMs.EnableErrorSurfaces(Thresholds.Any<ThresholdProps>(x => x.Method != ThresholdProps.ThresholdMethods.MinLoD));
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            if (tsmi.Tag is ThresholdTypes)
            {
                if (((ThresholdTypes)tsmi.Tag) == ThresholdTypes.Propagated)
                {
                    // Simply add a new propagated
                    Thresholds.Add(new ThresholdProps());
                }
                else
                {
                    // If the user clicks OK the child form will
                    // append the appropriate ThresholdProps to the binding list
                    frmBatchDoDProperties frm = new frmBatchDoDProperties(Thresholds, tsmi.Text, (ThresholdTypes)tsmi.Tag);
                    frm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Prescribed thresholding not implemented");
            }
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

            try
            {
                //Change state of UI
                this.UseWaitCursor = true;

                cmdOK.Enabled = false;
                cmdCancel.DialogResult = DialogResult.None;
                BatchEngine = new ChangeDetetctionBatch(ucDEMs.NewSurface, ucDEMs.OldSurface, ucDEMs.AOIMask, ucDEMs.NewError, ucDEMs.OldError, Thresholds.ToList<ThresholdProps>());
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ValidateForm()
        {
            if (grdMethods.Rows.Count < 1)
            {
                MessageBox.Show("You must specify one or more uncertainty analysis methods to continue.", "No Uncertainty Analysis Methods", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdAdd.Select();
                return false;
            }

            if (!ucDEMs.ValidateForm())
                return false;

            return true;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BatchEngine.Run(bgWorker);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdCancel.DialogResult = DialogResult.OK;
            cmdCancel.Text = "Close";
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Batch Change Detection Complete.", "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
