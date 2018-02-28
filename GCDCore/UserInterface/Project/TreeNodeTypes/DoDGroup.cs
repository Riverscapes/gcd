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
    public class DoDGroup : TreeNodeGroup
    {
        DoDBase DoD;

        public DoDGroup(TreeNodeCollection parentNodes, DoDBase dod, IContainer container)
            : base(parentNodes, dod.Name, string.Empty , "Change Detection", dod.Folder, container, true, 7)
        {
            DoD = dod;

            ContextMenuStrip.Items[0].Text = "Add Budget Segregation";
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Change Detection Results", Properties.Resources.GCD, OnViewResults));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

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
            catch(Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error viewing change detection results");
            }
        }
    }
}
