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

            ToolStripMenuItem tsmiBatch = new ToolStripMenuItem("Batch Change Detection");
            tsmiBatch.DropDownItems.Add("Multi-Epoch Change Detection", Properties.Resources.Add, OnMultiEpoch);
            tsmiBatch.DropDownItems.Add("Multi-Uncertainty Change Detection", Properties.Resources.Add, OnMultiUncertainty);
            ContextMenuStrip.Items.Insert(1, tsmiBatch);

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            Dictionary<string, DoDPairGroup> dDoD = new Dictionary<string, DoDPairGroup>();
            foreach (DoDBase rDoD in ProjectManager.Project.DoDs.Values)
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
            ChangeDetection.frmDoDProperties frm = new ChangeDetection.frmDoDProperties();
            EditTreeItem(frm);
        }

        public void OnMultiEpoch(object sender, EventArgs e)
        {
            ChangeDetection.MultiEpoch.frmMultiEpoch frm = new ChangeDetection.MultiEpoch.frmMultiEpoch();
            EditTreeItem(frm);
        }

        public void OnMultiUncertainty(object sender, EventArgs e)
        {
            ChangeDetection.Batch.frmBatchDoD frm = new ChangeDetection.Batch.frmBatchDoD();
            EditTreeItem(frm);
        }
    }
}
