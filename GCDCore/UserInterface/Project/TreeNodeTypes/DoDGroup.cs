using System;
using GCDCore.Project;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class DoDGroup : TreeNodeGroup
    {
        DoDBase DoD;

        public DoDGroup(TreeNodeCollection parentNodes, DoDBase dod, IContainer container)
            : base(parentNodes, dod.Name, string.Empty, "Change Detection", dod.Folder, container, true, 7)
        {
            DoD = dod;

            ContextMenuStrip.Items[0].Text = "Add Budget Segregation";
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Change Detection Results", Properties.Resources.GCD, OnViewResults));
            ContextMenuStrip.Items.Insert(3, new ToolStripMenuItem("Delete Change Detection", Properties.Resources.Delete, OnDelete));
            ContextMenuStrip.Items.Insert(4, new ToolStripSeparator());
            ContextMenuStrip.Items.Remove(ContextMenuStrip.Items.Find("AddToMap", false)[0]);
            ContextMenuStrip.Items.Insert(ContextMenuStrip.Items.Count - 2, new ToolStripMenuItem("Add Thresholded DoD Raster To Map", Properties.Resources.AddToMap, OnAddThrDoDToMap));
            ContextMenuStrip.Items.Insert(ContextMenuStrip.Items.Count - 2, new ToolStripMenuItem("Add Raw DoD Raster To Map", Properties.Resources.AddToMap, OnAddRawDoDToMap));

            ContextMenuStrip.Items.Insert(2, new ToolStripMenuItem("Add Linear Extraction From Profile Route", Properties.Resources.Add, OnLinear));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();
            
            if (DoD.LinearExtractions.Count > 0)
            {
                TreeNodeGroup nodLinea = new LinearExtractionGrp(Nodes, DoD, DoD.Folder, ContextMenuStrip.Container);
            }

            foreach (GCDCore.Project.BudgetSegregation bs in DoD.BudgetSegregations.Values)
            {
                BudgetSegGroup bsGroup = new BudgetSegGroup(Nodes, bs, ContextMenuStrip.Container);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            BudgetSegregation.frmBudgetSegProperties frm = new BudgetSegregation.frmBudgetSegProperties(DoD);
            if (EditTreeItem(frm) == DialogResult.OK)
            {
                BudgetSegregation.frmBudgetSegResults frmResults = new BudgetSegregation.frmBudgetSegResults(frm.BudgetSeg);
                frmResults.ShowDialog();
            }
        }

        public void OnViewResults(object sender, EventArgs e)
        {
            ChangeDetection.frmDoDResults frm = new ChangeDetection.frmDoDResults(DoD);
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error viewing change detection results");
            }
        }

        public void OnAddThrDoDToMap(object sender, EventArgs e)
        {
            ProjectManager.OnAddRasterToMap(DoD.ThrDoD);
        }

        public void OnAddRawDoDToMap(object sender, EventArgs e)
        {
            ProjectManager.OnAddRasterToMap(DoD.RawDoD);
        }

        private void OnLinear(object sender, EventArgs e)
        {
            ToolStripDropDownItem ctrl = sender as ToolStripDropDownItem;
            ContextMenuStrip cms = ctrl.Owner as ContextMenuStrip;
            TreeView tre = cms.SourceControl as TreeView;
            DoDGroup nodDoD = tre.SelectedNode as DoDGroup;

            LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(nodDoD.DoD as GCDProjectItem);
            EditTreeItem(frm);
        }

        public void OnDelete(object sender, EventArgs e)
        {
            if (DoD.IsItemInUse)
            {
                MessageBox.Show(string.Format("The {0} {1} is currently in use and cannot be deleted. Before you can delete this {1}," +
                    " you must delete all GCD project items that refer to this {1} before it can be deleted.", DoD.Name, DoD.Noun),
                    string.Format("{0} In Use", DoD.Noun), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(string.Format("Are you sure that you want to delete the {0} {1}? The {0} {1} and all its underlying data will be deleted permanently.", DoD.Name, DoD.Noun),
                Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            try
            {
                DoD.Delete();
                Remove();
            }
            catch (IOException ex)
            {
                string processes = string.Empty;
                if (ex.Data.Contains("Processes"))
                {
                    processes = string.Format(" ({0})", ex.Data["Processes"]);
                }

                MessageBox.Show(string.Format("One or more files belonging to this {0} are being used by another process{1}." +
                    " Close all applications that are using these files and try to delete this {0} again.", NounSingle.ToLower(), processes), "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}
