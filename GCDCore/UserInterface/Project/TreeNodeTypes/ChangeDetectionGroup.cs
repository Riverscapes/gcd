using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.Windows.Forms;
using System.ComponentModel;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class ChangeDetectionGroup : TreeNodeGroup
    {
        public ChangeDetectionGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "Change Detection", "Change Detection", "Change Detections", ProjectManager.Project.ChangeDetectionFolder, container)
        {
            ContextMenuStrip.Items[0].Text = "Add Change Detection";

            int totalSurfaces = ProjectManager.Project.DEMSurveys.Count + ProjectManager.Project.ReferenceSurfaces.Count;

            ToolStripMenuItem tsmiBatch = new ToolStripMenuItem("Batch Change Detection");
            tsmiBatch.DropDownItems.Add("Multiple Epoch", Properties.Resources.Add, OnMultiEpoch);
            tsmiBatch.DropDownItems.Add("Multiple Uncertainty Analysis", Properties.Resources.Add, OnMultiUncertainty);
            ContextMenuStrip.Items.Insert(1, tsmiBatch);

            // Override default handling of adding to map
            ContextMenuStrip.Items[ContextMenuStrip.Items.Count - 2].Click += OnAddAllDoDsToMap;

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            Dictionary<string, DoDPairGroup> dDoD = new Dictionary<string, DoDPairGroup>();
            foreach (DoDBase rDoD in ProjectManager.Project.DoDs)
            {
                string sDEMPair = rDoD.NewSurface.Name + " - " + rDoD.OldSurface.Name;

                if (!dDoD.ContainsKey(sDEMPair))
                {
                    // Create a new parent of DEM surveys for this DoD
                    DoDPairGroup nodPair = new DoDPairGroup(Nodes, rDoD.NewSurface, rDoD.OldSurface, sDEMPair, ContextMenuStrip.Container);
                    dDoD[sDEMPair] = nodPair;
                }
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count + ProjectManager.Project.ReferenceSurfaces.Count < 2)
            {
                MessageBox.Show("You must have at least two DEM surveys in your GCD project before you can perform a change detection analysis." +
                                        " These could be two DEM surveys or two reference surfaces or a combination of both.", "Insufficient Surfaces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChangeDetection.frmDoDProperties frm = new ChangeDetection.frmDoDProperties();
            if (EditTreeItem(frm) == DialogResult.OK)
            {
                ChangeDetection.frmDoDResults frmResults = new ChangeDetection.frmDoDResults(frm.DoD);
                frmResults.ShowDialog();
            }
        }

        public void OnMultiEpoch(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 2)
            {
                MessageBox.Show("You must have at least DEM surveys in your GCD project before you can perform a change detection analysis.", "Insufficient DEM Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChangeDetection.MultiEpoch.frmMultiEpoch frm = new ChangeDetection.MultiEpoch.frmMultiEpoch();
            EditTreeItem(frm);
            LoadChildNodes();
        }

        public void OnMultiUncertainty(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 2)
            {
                MessageBox.Show("You must have at least two surface in your GCD project before you can perform a change detection analysis." +
                         " These could be two DEM surveys or two reference surfaces or a combination of both.", "Insufficient Surfaces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChangeDetection.Batch.frmBatchDoD frm = new ChangeDetection.Batch.frmBatchDoD();
            EditTreeItem(frm);
            LoadChildNodes();
        }

        public void OnAddAllDoDsToMap(object sender, EventArgs e)
        {
            ProjectManager.Project.DoDs.ForEach(x => ProjectManager.OnAddRasterToMap(x.ThrDoD as GCDProjectRasterItem));
        }
    }
}
