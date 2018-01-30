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
            List<DEMSurvey> SortedSurveys = GetSortedDEMSurveyList();
            DEMs = new BindingList<DEMSurveyItem>(SortedSurveys.Select(x => new DEMSurveyItem(x, x.DefaultErrorSurface)).ToList<DEMSurveyItem>());

            // Empty list of epochs that will have items added to and removed form as user interacts with form controls
            Epochs = new BindingList<Epoch>();

        }

        #endregion

        #region "Events"

        private void frmInterComp_Load(object sender, EventArgs e)
        {
            grdDEMs.AutoGenerateColumns = false;
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

            UpdateEpochQueue();

            for (int i = 0; i < grdDEMs.Rows.Count; i++)
            {
                DataGridViewComboBoxCell comboCell = grdDEMs[2, i] as DataGridViewComboBoxCell;

                comboCell.DataSource = new BindingSource(DEMs[i].ErrorSurfaces, null);
                comboCell.DisplayMember = "NameWithDefault"; //4.29 - lets try adding display member. It works but dropdown closes immediately and error name column is still there.

                //select first item
                if(DEMs[i].ErrorSurfaces.Count > 0) //check if any error surfaces are available
                {
                    comboCell.Value = DEMs[i].ErrorSurfaces[0].NameWithDefault;
                }

            }

            grdDEMs.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(
        dataGridView1_EditingControlShowing);
        }


        /// <summary>
        /// https://stackoverflow.com/questions/11141872/event-that-fires-during-datagridviewcomboboxcolumn-selectedindexchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_EditingControlShowing(object sender,
    DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = e.Control as ComboBox;
            if (combo != null)
            {
                // Remove an existing event-handler, if present, to avoid 
                // adding multiple handlers when the editing control is reused.
                combo.SelectedIndexChanged -=
                    new EventHandler(ComboBox_SelectedIndexChanged);

                // Add the event handler. 
                combo.SelectedIndexChanged +=
                    new EventHandler(ComboBox_SelectedIndexChanged);
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ComboBox)sender).BackColor = Color.AliceBlue;
            DEMSurveyItem selectedDEM = (DEMSurveyItem)grdDEMs.SelectedRows[0].DataBoundItem;

            ComboBox cbo = (ComboBox)sender;
            
            ErrorSurface selectedDEMErrorSurface = (ErrorSurface) cbo.SelectedItem;
            selectedDEM.SelectedErrorSurface = selectedDEMErrorSurface;

            UpdateEpochQueue();
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
                BatchEngine = new ChangeDetectionMultiEpoch(Epochs.ToList(), threshold);
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

        private void chkNewest_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }

        private void chkEarliest_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }

        private void chkPrevious_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEpochQueue();
        }
        #endregion

        #region "Methods"
        private void UpdateEpochQueue()
        {
            //clear list
            Epochs.Clear();

            if(chkNewest.Checked)
            {
                List<Epoch> NewestDEMMinusAllOtherDEMs = PopulateNewestDEMMinusAllOtherDEMs();
                AddEpochsWithoutDuplicates(NewestDEMMinusAllOtherDEMs);
            }

            if(chkEarliest.Checked)
            {
                List<Epoch> AllDEMsMinusTheEarliestDEMs = PopulateAllDEMsMinusTheEarliestDEMs();
                AddEpochsWithoutDuplicates(AllDEMsMinusTheEarliestDEMs);
            }

            if (chkPrevious.Checked)
            {
                List<Epoch> AllDEMsMinusThePreviousDEM = PopulateAllDEMsMinusThePreviousDEM();
                AddEpochsWithoutDuplicates(AllDEMsMinusThePreviousDEM);
            }
        }

        private void AddEpochsWithoutDuplicates(List<Epoch> NewEpochs)
        {
            foreach(Epoch currentEpoch in NewEpochs)
            {
                if(!EpochIsCurrentInList(currentEpoch))
                {
                    Epochs.Add(currentEpoch);
                }
            }
        }

        private Boolean EpochIsCurrentInList(Epoch newEpoch)
        {
            foreach(Epoch CurrentEpoch in Epochs)
            {
                if(CurrentEpoch.NewDEMName == newEpoch.NewDEMName & CurrentEpoch.OldDEMName == newEpoch.OldDEMName) //each DEM must have a unique name so we can compare DEM names
                {
                    return (true);
                }
            }
            return (false);
        }

        private List<Epoch> PopulateNewestDEMMinusAllOtherDEMs()
        {
            List<Epoch> NewestDEMMinusAllOtherDEMs = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs();
            if(ActiveDEMs.Count < 2)
            {
                return (NewestDEMMinusAllOtherDEMs);
            }

            DEMSurveyItem NewestDEM = ActiveDEMs[0];
            for (int i = 1; i< ActiveDEMs.Count; i++)
            {
                DEMSurveyItem OlderDEM = ActiveDEMs[i];
                Epoch FirstEpoch = new Epoch(NewestDEM.DEM, NewestDEM.SelectedErrorSurface, OlderDEM.DEM, OlderDEM.SelectedErrorSurface);
                NewestDEMMinusAllOtherDEMs.Add(FirstEpoch);
            }

            return (NewestDEMMinusAllOtherDEMs);
        }

        private List<Epoch> PopulateAllDEMsMinusTheEarliestDEMs()
        {
            List<Epoch> AllDEMsMinusTheEarliestDEMs = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs();

            if (ActiveDEMs.Count < 2)
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

        private List<Epoch> PopulateAllDEMsMinusThePreviousDEM()
        {
            List<Epoch> AllDEMsMinusThePreviousDEM = new List<Epoch>();

            List<DEMSurveyItem> ActiveDEMs = GetActiveDEMs();

            if (ActiveDEMs.Count < 2)
            {
                return (AllDEMsMinusThePreviousDEM);
            }

            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurveyItem newDEM = ActiveDEMs[i];
                DEMSurveyItem oldDEM = ActiveDEMs[i+1];
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

            for(int i=0; i<DEMs.Count; i++) //loop through all surveys
            {
                DEMSurveyItem CurrentDEM = DEMs[i];
                //if active, add to list of active DEMs
                if(CurrentDEM.IsActive)
                {
                    ActiveDEMs.Add(CurrentDEM);
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


        private List<DEMSurvey> GetSortedDEMSurveyList()
        {
            List<DEMSurvey> SortedSurveys = new List<DEMSurvey>();

            List<DEMSurvey> AllSurveys = ProjectManager.Project.DEMSurveys.Values.ToList();

            List<DEMSurvey> SurveysWithChronologicalOrder = AllSurveys.Where(survey => survey.ChronologicalOrder.HasValue).OrderBy(survey =>survey.ChronologicalOrder).ToList();
            List<DEMSurvey> SurveysWithSurveyDate= AllSurveys.Where(survey => !survey.ChronologicalOrder.HasValue && survey.SurveyDate != null).OrderBy(survey => survey.SurveyDate).ToList();
            List<DEMSurvey> RemainingSurveys= AllSurveys.Where(survey => !survey.ChronologicalOrder.HasValue && survey.SurveyDate == null).OrderBy(survey => survey.Name).ToList();

            SortedSurveys.AddRange(SurveysWithChronologicalOrder);
            SortedSurveys.AddRange(SurveysWithSurveyDate);
            SortedSurveys.AddRange(RemainingSurveys);

            return (SortedSurveys);
        }

        #endregion

        #region "Background worker support"

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

        #endregion


        //Implementing dropbox opening on initial click
        //see https://stackoverflow.com/questions/13005112/how-to-activate-combobox-on-first-click-datagridview
        private void grdDEMs_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            DataGridView datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }
    }

}
