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
        #region "Members"
        public readonly BindingList<DEMSurveyItem> DEMs;
        public readonly BindingList<Epoch> Epochs;

        private BindingSource DEMSBindingSource = new BindingSource();
        private GCDCore.Engines.DoD.ChangeDetectionMultiEpoch BatchEngine;
        #endregion

        #region "Constructor"
        public frmMultiEpoch()
        {
            InitializeComponent();

            // List of project DEMs that the user cannot add to or remove from, but they can change the order of this list.
            List<DEMSurvey> SortedSurveys = GetSortedDEMSurveyList();
            DEMs = new BindingList<DEMSurveyItem>(SortedSurveys.Select(x => new DEMSurveyItem(x, x.DefaultErrorSurface)).ToList<DEMSurveyItem>());

            // Empty list of epochs that will have items added to and removed form as user interacts with form controls
            Epochs = new BindingList<Epoch>();

        }

        #endregion

        #region "Events"

        /// <summary>
        /// Initiates the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmInterComp_Load(object sender, EventArgs e)
        {
            //setup grids
            grdDEMs.AutoGenerateColumns = false;
            grdEpochs.AutoGenerateColumns = false;

            // Bind the two grids to the corresponding lists
            DEMSBindingSource.DataSource = DEMs;
            grdDEMs.DataSource = DEMSBindingSource;
            grdEpochs.DataSource = Epochs;

            // Update the status of the move up and down buttons
            grdDEMs_SelectionChanged(sender, e);

            //hook up handler for when DEM IsActive property is changed
            foreach (DEMSurveyItem SurveyItem in DEMs)
            {
                SurveyItem.PropertyChanged += DEMIsActive_Changed;
            }

            //Setup error surfaces for DEM grid
            for (int i = 0; i < grdDEMs.Rows.Count; i++)
            {
                DataGridViewComboBoxCell comboCell = grdDEMs[2, i] as DataGridViewComboBoxCell;

                comboCell.DataSource = new BindingSource(DEMs[i].ErrorSurfaces, null);
                comboCell.DisplayMember = "NameWithDefault";

                //select first item
                if (DEMs[i].ErrorSurfaces.Count > 0) //check if any error surfaces are available
                {
                    comboCell.Value = DEMs[i].ErrorSurfaces[0].NameWithDefault;
                }

            }

            //setup handler for when error surfaces changes
            grdDEMs.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);

            //update the list of epochs
            UpdateEpochQueue();

        }

        /// <summary>
        /// Handler for when Error Surfaces change.
        /// See https://stackoverflow.com/questions/11141872/event-that-fires-during-datagridviewcomboboxcolumn-selectedindexchanged for details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = e.Control as ComboBox;
            if (combo != null)
            {
                // Remove an existing event-handler, if present, to avoid 
                // adding multiple handlers when the editing control is reused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);

                // Add the event handler. 
                combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            }
        }

        /// <summary>
        /// Handler support for when error surfaces change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get selected error surface
            ComboBox cbo = (ComboBox)sender;
            ErrorSurface selectedDEMErrorSurface = (ErrorSurface)cbo.SelectedItem;

            //set selected error surface on relevant DEMSurveyItem
            DEMSurveyItem selectedDEM = (DEMSurveyItem)grdDEMs.SelectedRows[0].DataBoundItem;
            selectedDEM.SelectedErrorSurface = selectedDEMErrorSurface;

            //Update list of epochs (necessary because we need to make sure the epoch has the correct error surface
            UpdateEpochQueue();
        }

        /// <summary>
        /// Changes the order of the survey items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Update list of epochs
            UpdateEpochQueue();
        }

        /// <summary>
        /// Changes the order of the survey items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Update list of epochs
            UpdateEpochQueue();
        }

        /// <summary>
        /// Manages if up and down buttons are enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDEMs_SelectionChanged(object sender, EventArgs e)
        {
            if (grdDEMs.SelectedRows.Count > 0)
            {
                cmdMoveUp.Enabled = grdDEMs.SelectedRows[0].Index > 0;
                cmdMoveDown.Enabled = grdDEMs.SelectedRows[0].Index < DEMs.Count - 1;
            }
        }

        /// <summary>
        /// Updates epoch queue when survey items are made active or inactive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DEMIsActive_Changed(object sender, EventArgs e)
        {
            //Update list of epochs
            UpdateEpochQueue();
        }

        /// <summary>
        /// Makes sure changes to the active checkbox is propagated through to changing the databound Active property on DEMSurveyItem
        /// when the user check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// See link below for strategy 
        /// http://geekswithblogs.net/FrostRed/archive/2008/09/07/125001.aspx
        private void grdDEMs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get column name
            int colIndex = e.ColumnIndex;
            string colName = grdDEMs.Columns[colIndex].Name;

            //if its the Active checkbox column, end edit when mouse button is up to propate changes to databound item
            if (colName == "colActive" && e.RowIndex > -1)
            {
                grdDEMs.EndEdit();
            }
        }

        /// <summary>
        /// Handles clicking the OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //validate form
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
                DisableForm();

                //set chronological order
                for (int i = 0; i < DEMs.Count; i++)
                {
                    DEMSurveyItem currentSurveyItem = DEMs[i];
                    DEMSurvey currentDEMSurvey = currentSurveyItem.DEM;
                    currentDEMSurvey.ChronologicalOrder = i;
                }
                ProjectManager.Project.Save();

                //setup and run batch engine
                ThresholdProps threshold = ucThresholding1.ThresholdProperties;
                List<Epoch> ActiveEpochs = Epochs.Where(epoch => epoch.IsActive == true).ToList();
                BatchEngine = new ChangeDetectionMultiEpoch(ActiveEpochs, ucAOI1.AOIMask, threshold);
                bgWorker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        /// <summary>
        /// Update epoch queue when analysis changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNewest_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }

        /// <summary>
        /// Update epoch queue when analysis changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkEarliest_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }

        /// <summary>
        /// Update epoch queue when analysis changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPrevious_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }

        /// <summary>
        /// Handles dropdown for error surface opening on initial click
        /// See https://stackoverflow.com/questions/13005112/how-to-activate-combobox-on-first-click-datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDEMs_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            DataGridView datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                //begin editing and show downdown control
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        /// <summary>
        /// Send signal to background worker to cancel when Cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Disables UI Controls on form
        /// Called when analysis is running
        /// </summary>
        private void DisableForm()
        {
            grdDEMs.Enabled = false;
            cmdMoveDown.Enabled = false;
            cmdMoveUp.Enabled = false;
            ucThresholding1.Enabled = false;
            chkEarliest.Enabled = false;
            chkNewest.Enabled = false;
            chkPrevious.Enabled = false;
            grdEpochs.Enabled = false;
        }

        /// <summary>
        /// Enables UI control on form
        /// Called when an analysis is cancelled
        /// </summary>
        private void EnableForm()
        {
            grdDEMs.Enabled = true;
            cmdMoveDown.Enabled = true;
            cmdMoveUp.Enabled = true;
            ucThresholding1.Enabled = true;
            chkEarliest.Enabled = true;
            chkNewest.Enabled = true;
            chkPrevious.Enabled = true;
            grdEpochs.Enabled = true;
            cmdOK.Enabled = true;
        }

        /// <summary>
        /// Updates epoch queu
        /// </summary>
        private void UpdateEpochQueue()
        {
            List<Epoch> InActiveEpochs = Epochs.Where(epoch => epoch.IsActive == false).ToList();

            //clear list
            Epochs.Clear();

            //if Newest DEM minus all other DEMs is selected, get all relevant epochs and add without duplicates
            if (chkNewest.Checked)
            {
                List<Epoch> NewestDEMMinusAllOtherDEMs = PopulateNewestDEMMinusAllOtherDEMs();
                AddEpochsWithoutDuplicates(NewestDEMMinusAllOtherDEMs, InActiveEpochs);
            }

            //if All DEMs minus the earliest DEM is selected, get all relevant epochs and add without duplicates
            if (chkEarliest.Checked)
            {
                List<Epoch> AllDEMsMinusTheEarliestDEMs = PopulateAllDEMsMinusTheEarliestDEMs();
                AddEpochsWithoutDuplicates(AllDEMsMinusTheEarliestDEMs, InActiveEpochs);
            }

            //if All DEMs minus the previous DEM is selected, get all relevant epochs and add without duplicates
            if (chkPrevious.Checked)
            {
                List<Epoch> AllDEMsMinusThePreviousDEM = PopulateAllDEMsMinusThePreviousDEM();
                AddEpochsWithoutDuplicates(AllDEMsMinusThePreviousDEM, InActiveEpochs);
            }
        }

        /// <summary>
        /// Adds epochs to epoch grid if not already in grid
        /// </summary>
        /// <param name="NewEpochs"></param>
        private void AddEpochsWithoutDuplicates(List<Epoch> NewEpochs, List<Epoch> InActiveEpochs)
        {
            foreach (Epoch currentEpoch in NewEpochs)
            {
                //if epoch is not already in grid, add it
                if (!EpochIsCurrentInList(currentEpoch))
                {
                    //check if its inactive
                    if (InActiveEpochs.Where(IAEpochs => IAEpochs.NewDEMName == currentEpoch.NewDEMName && IAEpochs.OldDEMName == currentEpoch.OldDEMName).Count() > 0)
                    {
                        currentEpoch.IsActive = false;
                    }

                    Epochs.Add(currentEpoch);
                }
            }
        }

        /// <summary>
        /// Checks if an epoch is already in the epoch grid
        /// </summary>
        /// <param name="newEpoch"></param>
        /// <returns></returns>
        private Boolean EpochIsCurrentInList(Epoch newEpoch)
        {
            foreach (Epoch CurrentEpoch in Epochs)
            {
                if (CurrentEpoch.NewDEMName == newEpoch.NewDEMName & CurrentEpoch.OldDEMName == newEpoch.OldDEMName) //each DEM must have a unique name so we can compare DEM names
                {
                    return (true);
                }
            }
            return (false);
        }

        /// <summary>
        /// Returns list of Newest DEM Minus All Other DEMs Epochs
        /// </summary>
        /// <returns></returns>
        private List<Epoch> PopulateNewestDEMMinusAllOtherDEMs()
        {
            List<Epoch> NewestDEMMinusAllOtherDEMs = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs(); //only use active survey items

            if (ActiveDEMs.Count < 2)//check there is more than one active survey item
            {
                return (NewestDEMMinusAllOtherDEMs);
            }

            DEMSurveyItem NewestDEM = ActiveDEMs[0];
            for (int i = 1; i < ActiveDEMs.Count; i++)
            {
                DEMSurveyItem OlderDEM = ActiveDEMs[i];
                Epoch FirstEpoch = new Epoch(NewestDEM.DEM, NewestDEM.SelectedErrorSurface, OlderDEM.DEM, OlderDEM.SelectedErrorSurface);
                NewestDEMMinusAllOtherDEMs.Add(FirstEpoch);
            }

            return (NewestDEMMinusAllOtherDEMs);
        }

        /// <summary>
        /// Returns list of All DEMs Minus The Earliest DEM epochs
        /// </summary>
        /// <returns></returns>
        private List<Epoch> PopulateAllDEMsMinusTheEarliestDEMs()
        {
            List<Epoch> AllDEMsMinusTheEarliestDEMs = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs();//only use active survey items

            if (ActiveDEMs.Count < 2) //check there is more than one active survey item
            {
                return (AllDEMsMinusTheEarliestDEMs);
            }

            DEMSurveyItem EarliestDEM = ActiveDEMs[ActiveDEMs.Count - 1];
            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurveyItem OtherDEM = ActiveDEMs[i];
                Epoch NewEpoch = new Epoch(OtherDEM.DEM, OtherDEM.SelectedErrorSurface, EarliestDEM.DEM, EarliestDEM.SelectedErrorSurface);
                AllDEMsMinusTheEarliestDEMs.Add(NewEpoch);
            }

            return (AllDEMsMinusTheEarliestDEMs);
        }

        /// <summary>
        /// Returns list of All DEMs Minus The Previous DEM Epochs
        /// </summary>
        /// <returns></returns>
        private List<Epoch> PopulateAllDEMsMinusThePreviousDEM()
        {
            List<Epoch> AllDEMsMinusThePreviousDEM = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs();//only use active survey items

            if (ActiveDEMs.Count < 2) //check there is more than one active survey item
            {
                return (AllDEMsMinusThePreviousDEM);
            }

            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurveyItem newDEM = ActiveDEMs[i];
                DEMSurveyItem oldDEM = ActiveDEMs[i + 1];
                Epoch NewEpoch = new Epoch(newDEM.DEM, newDEM.SelectedErrorSurface, oldDEM.DEM, oldDEM.SelectedErrorSurface);
                AllDEMsMinusThePreviousDEM.Add(NewEpoch);
            }

            return (AllDEMsMinusThePreviousDEM);
        }

        /// <summary>
        /// Returns a list of active DEMs
        /// </summary>
        /// <returns></returns>
        private List<DEMSurveyItem> GetActiveDEMs()
        {
            List<DEMSurveyItem> ActiveDEMs = new List<DEMSurveyItem>();

            for (int i = 0; i < DEMs.Count; i++) //loop through all surveys
            {
                DEMSurveyItem CurrentDEM = DEMs[i];
                //if active, add to list of active DEMs
                if (CurrentDEM.IsActive)
                {
                    ActiveDEMs.Add(CurrentDEM);
                }
            }

            return (ActiveDEMs);

        }

        /// <summary>
        /// Validates form
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            //Check there is at least one active epoch to analyse
            List<Epoch> ActiveEpochs = Epochs.Where(epoch => epoch.IsActive == true).ToList();
            if (ActiveEpochs.Count == 0)
            {
                MessageBox.Show("Please select at least one one active epoch for analysis .", "No Active Epochs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //check for surveys without error surfaces if uncertainty analysis method is propagated or probabilistic thresholding
            if (ucThresholding1.ThresholdProperties.Method != ThresholdProps.ThresholdMethods.MinLoD) //if method is not minimum level of detection
            {
                foreach (Epoch currentEpoch in Epochs)
                {
                    if (currentEpoch.NewDEMErrorSurface == null || currentEpoch.OldDEMErrorSurface == null)
                    {
                        MessageBox.Show("Please specify an error surface for each included survey if your uncertainty analysis method is not minimum level of detection.", "Missing Error Surface", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Returns list of sorted survey items
        /// </summary>
        /// <returns></returns>
        private List<DEMSurvey> GetSortedDEMSurveyList()
        {
            List<DEMSurvey> SortedSurveys = new List<DEMSurvey>();

            List<DEMSurvey> AllSurveys = ProjectManager.Project.DEMSurveys.Values.ToList();

            List<DEMSurvey> SurveysWithChronologicalOrder = AllSurveys.Where(survey => survey.ChronologicalOrder.HasValue).OrderBy(survey => survey.ChronologicalOrder).ToList();
            List<DEMSurvey> SurveysWithSurveyDate = AllSurveys.Where(survey => !survey.ChronologicalOrder.HasValue && survey.SurveyDate != null).OrderByDescending(survey => survey.SurveyDate).ToList();
            List<DEMSurvey> RemainingSurveys = AllSurveys.Where(survey => !survey.ChronologicalOrder.HasValue && survey.SurveyDate == null).OrderByDescending(survey => survey.Name).ToList();

            //First add sorted surveys with chronological orders (the user has specified the order)
            SortedSurveys.AddRange(SurveysWithChronologicalOrder);

            //Next, add surveys with survey date sorted (but without chronological order)
            SortedSurveys.AddRange(SurveysWithSurveyDate);

            //Finally, add all remaining surveys sorted alphabetically
            SortedSurveys.AddRange(RemainingSurveys);

            return (SortedSurveys);
        }

        #endregion

        #region "Background worker support"

        /// <summary>
        /// Runs background worker analysis and handles cancellation
        /// Called when OK button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BatchEngine.Run(bgWorker);
                if (BatchEngine.Cancelled) //if analysis is cancelled, set cancel flag on DoWorkEventArgs
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
            finally
            {
                this.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// Stub for handling progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// Handles when analysis is complete or cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if cancelled, let the user know and enable the form
            if (e.Cancelled)
            {
                MessageBox.Show("Batch Change Detection cancelled.");
                EnableForm();
            }
            else
            {
                ///Otherwise that the user know the analysis completed
                cmdCancel.DialogResult = DialogResult.OK;
                cmdCancel.Text = "Close";
                MessageBox.Show("Batch Change Detection Complete.");
            }

        }

        #endregion

    }

}
