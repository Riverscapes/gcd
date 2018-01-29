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

            //4.28 - need to set the datasource for each cell instead of at the column level because the error surfaces are different for each DEM Survey
            for (int i = 0; i < grdDEMs.Rows.Count; i++)
            {
                DataGridViewComboBoxCell comboCell = grdDEMs[2, i] as DataGridViewComboBoxCell;
                //int[] data = { 1 * i, 2 * i, 3 * i }; This works, but is not the data we want
                //comboCell.DataSource = new BindingSource(data, null);

                comboCell.DataSource = new BindingSource(DEMs[i].ErrorSurfaces, null);
                comboCell.DisplayMember = "NameWithDefault"; //4.29 - lets try adding display member. It works but dropdown closes immediately and error name column is still there.

                //select first item
                comboCell.Value = DEMs[i].ErrorSurfaces[0].NameWithDefault;

                //8.08 - set selected error surface
                //8.09 - the cell doesnt have any datapropertyname, so I will try setting that on the column
                //8.19 - I think maybe we need to set the property on the DEMSurveyItwm using an event.

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
            if (e.ColumnIndex == 1 && e.RowIndex > -1)

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

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();
            if(ActiveDEMs.Count < 2)
            {
                return (NewestDEMMinusAllOtherDEMs);
            }


            
            DEMSurvey NewestDEM = ActiveDEMs[0];
            for(int i = 1; i< ActiveDEMs.Count; i++)
            {
                DEMSurvey OlderDEM = ActiveDEMs[i];
                Epoch FirstEpoch = new Epoch(NewestDEM, OlderDEM);
                NewestDEMMinusAllOtherDEMs.Add(FirstEpoch);
            }

            return (NewestDEMMinusAllOtherDEMs);
        }

        private List<Epoch> PopulateAllDEMsMinusTheEarliestDEMs()
        {
            List<Epoch> AllDEMsMinusTheEarliestDEMs = new List<Epoch>();

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();

            if (ActiveDEMs.Count < 2)
            {
                return (AllDEMsMinusTheEarliestDEMs);
            }


            DEMSurvey EarliestDEM = ActiveDEMs[ActiveDEMs.Count - 1];
            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurvey OtherDEM = ActiveDEMs[i];
                Epoch NewEpoch = new Epoch(OtherDEM, EarliestDEM);
                AllDEMsMinusTheEarliestDEMs.Add(NewEpoch);
            }

            return (AllDEMsMinusTheEarliestDEMs);
        }

        private List<Epoch> PopulateAllDEMsMinusThePreviousDEM()
        {
            List<Epoch> AllDEMsMinusThePreviousDEM = new List<Epoch>();

            List<DEMSurvey> ActiveDEMs = GetActiveDEMs();

            if (ActiveDEMs.Count < 2)
            {
                return (AllDEMsMinusThePreviousDEM);
            }

            for (int i = 0; i < ActiveDEMs.Count - 1; i++)
            {
                DEMSurvey newDEM = ActiveDEMs[i];
                DEMSurvey oldDEM = ActiveDEMs[i+1];
                Epoch NewEpoch = new Epoch(newDEM, oldDEM);
                AllDEMsMinusThePreviousDEM.Add(NewEpoch);
            }

            return (AllDEMsMinusThePreviousDEM);
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
