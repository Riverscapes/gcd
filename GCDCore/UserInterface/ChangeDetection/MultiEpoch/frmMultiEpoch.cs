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

namespace GCDCore.UserInterface.ChangeDetection.MultiEpoch
{
    public partial class frmMultiEpoch : Form
    {
        public readonly BindingList<DEMSurveyItem> DEMs;
        public readonly BindingList<Epoch> Epochs;
        private BindingSource DEMSBindingSource = new BindingSource();

        private GCDCore.Engines.DoD.ChangeDetectionMultiEpoch BatchEngine;


        #region "Constructor"
        public frmMultiEpoch()
        {
            InitializeComponent();

            // List of project DEMs that the user cannot add to or remove from, but they can change the order of this list.
            DEMs = new BindingList<DEMSurveyItem>(ProjectManager.Project.DEMSurveys.Values.Select(x => new DEMSurveyItem(x, x.DefaultErrorSurface)).ToList<DEMSurveyItem>());

            // Empty list of epochs that will have items added to and removed form as user interacts with form controls
            Epochs = new BindingList<Epoch>();

        }

        #endregion

        #region "Events"

        private void frmInterComp_Load(object sender, EventArgs e)
        {
            grdEpochs.AutoGenerateColumns = false;

            // Bind the two grids to the corresponding lists
            DEMSBindingSource.DataSource = DEMs;
            grdDEMs.DataSource = DEMSBindingSource;
            grdEpochs.DataSource = Epochs;
     
            // Update the status of the move up and down buttons
            grdDEMs_SelectionChanged(sender, e);

            //hook up handler for when DEM IsActive property is changed
            foreach(DEMSurveyItem SurveyItem in DEMs)
            {
                SurveyItem.PropertyChanged+= DEMIsActive_Changed;
            }
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
            UpdateEpochQueue();
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
            UpdateEpochQueue();
        }

        private void grdDEMs_SelectionChanged(object sender, EventArgs e)
        {
            if (grdDEMs.SelectedRows.Count > 0)
            {
                cmdMoveUp.Enabled = grdDEMs.SelectedRows[0].Index > 0;
                cmdMoveDown.Enabled = grdDEMs.SelectedRows[0].Index < DEMs.Count - 1;
            }
        }

        private void DEMIsActive_Changed(object sender, EventArgs e)
        {
            Console.WriteLine("DEM isactive changed");
            UpdateEpochQueue();
        }

        private void grdDEMs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("Cell Value CHanged");

        }

        //http://geekswithblogs.net/FrostRed/archive/2008/09/07/125001.aspx
        private void grdDEMs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)

            {

                grdDEMs.EndEdit();

            }
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
                Cursor.Current = Cursors.WaitCursor;
                cmdOK.Enabled = false;
                cmdCancel.DialogResult = DialogResult.None;

                //set chronological order
                for(int i=0; i<DEMs.Count; i++)
                {
                    DEMSurveyItem currentSurveyItem = DEMs[i];
                    DEMSurvey currentDEMSurvey = currentSurveyItem.DEM;
                    currentDEMSurvey.ChronologicalOrder = i;
                }
                ProjectManager.Project.Save();

                ThresholdProps threshold = ucThresholding1.ThresholdProperties;
                List < ThresholdProps > thresholds = new List<ThresholdProps>();
                thresholds.Add(threshold);
                BatchEngine = new ChangeDetectionMultiEpoch(DEMs[0].DEM, DEMs[1].DEM, DEMs[0].ErrorSurf, DEMs[1].ErrorSurf, thresholds);
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region "Methods"
        private void UpdateEpochQueue()
        {
            //PopulateNewestDEMMinusAllOtherDEMs();
            //PopulateAllDEMsMinusTheEarliestDEMs();
            PopulateAllDEMsMinusThePreviousDEM();
        }

        private void PopulateNewestDEMMinusAllOtherDEMs()
        {
            //clear list
            Epochs.Clear();

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();
            
            DEMSurvey NewestDEM = ActiveDEMs[ActiveDEMs.Count-1];
            for(int i = 0; i< ActiveDEMs.Count - 1; i++)
            {
                DEMSurvey OlderDEM = ActiveDEMs[i];
                Epoch FirstEpoch = new Epoch(NewestDEM, OlderDEM);
                Epochs.Add(FirstEpoch);
            }

        }

        private void PopulateAllDEMsMinusTheEarliestDEMs()
        {
            //clear list
            Epochs.Clear();

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();

            DEMSurvey EarliestDEM = ActiveDEMs[0];
            for (int i = 1; i < ActiveDEMs.Count; i++)
            {
                DEMSurvey OtherDEM = ActiveDEMs[i];
                Epoch NewEpoch = new Epoch(OtherDEM, EarliestDEM);
                Epochs.Add(NewEpoch);
            }

        }

        private void PopulateAllDEMsMinusThePreviousDEM()
        {
            //clear list
            Epochs.Clear();

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();

            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurvey oldDEM = ActiveDEMs[i];
                DEMSurvey newDEM = ActiveDEMs[i+1];
                Epoch NewEpoch = new Epoch(newDEM, oldDEM);
                Epochs.Add(NewEpoch);
            }

        }

        /// <summary>
        /// Returns a list of active DEMs
        /// </summary>
        /// <returns></returns>
        private List<DEMSurvey> GetActiveDEMs()
        {
            List<DEMSurvey> ActiveDEMs = new List<DEMSurvey>();

            for(int i=0; i<DEMs.Count; i++) //loop through all surveys
            {
                DEMSurveyItem CurrentDEM = DEMs[i];
                //if active, add to list of active DEMs
                if(CurrentDEM.IsActive)
                {
                    ActiveDEMs.Add(CurrentDEM.DEM);
                }
            }

            return (ActiveDEMs);

        }

        private bool ValidateForm()
        {
            /*
            if (!ucDEMs.ValidateForm())
                return false;

            if (grdMethods.Rows.Count < 1)
            {
                MessageBox.Show("You must specify one or more uncertainty analysis methods to continue.", "No Uncertainty Analysis Methods", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdAdd.Select();
                return false;
            }
            */
            return true;
        }

        #endregion

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BatchEngine.Run(bgWorker);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
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
            MessageBox.Show("Batch Change Detection Complete.");

        }
    }

}
