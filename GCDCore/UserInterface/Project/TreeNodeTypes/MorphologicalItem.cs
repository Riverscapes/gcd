using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class MorphologicalItem : TreeNodeItem
    {

        public MorphologicalItem(GCDProjectItem item, IContainer container)
            : base(item, 11, container)
        {
            ContextMenuStrip.Items.RemoveAt(0);
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Morphological Analysis Results", Properties.Resources.GCD, OnViewResults));
        }

        public void OnViewResults(object sender, EventArgs e)
        {
            BudgetSegregation.Morphological.frmMorphResults frm = new BudgetSegregation.Morphological.frmMorphResults(Item as GCDCore.Project.Morphological.MorphologicalAnalysis);
            frm.ShowDialog();
        }
    }
}
