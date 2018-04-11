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
    public class BudgetSegGroup : TreeNodeGroup
    {
        public readonly GCDCore.Project.BudgetSegregation BudgetSeg;

        public BudgetSegGroup(TreeNodeCollection parentNodes, GCDCore.Project.BudgetSegregation bs, IContainer container)
            : base(parentNodes, bs.Name, "Budget Segregation", "Budget Segregations", bs.Folder, container, true, 9)
        {
            BudgetSeg = bs;

            ContextMenuStrip.Items[0].Text = "Add Morphological Analysis";
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Budget Segregation Results", Properties.Resources.GCD, OnViewResults));
            ContextMenuStrip.Items.Insert(3, new ToolStripMenuItem("Delete Budget Segregation", Properties.Resources.Delete, OnDelete));
            ContextMenuStrip.Items.Insert(4, new ToolStripSeparator());

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            BudgetSeg.MorphologicalAnalyses.Values.ToList().ForEach(x => Nodes.Add(new MorphologicalItem(x, ContextMenuStrip.Container)));

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            if (!BudgetSeg.IsMaskDirectional)
            {
                MessageBox.Show("You can only perform morphological approach sediment analyses on budget segregations that were" +
           " generated using directional mask. The selected budget segregation was generated using a regular mask.", "Invalid Budget Segregation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BudgetSegregation.Morphological.frmMorpProperties frm = new BudgetSegregation.Morphological.frmMorpProperties(BudgetSeg);
            if (EditTreeItem(frm) == DialogResult.OK)
            {
                BudgetSegregation.Morphological.frmMorphResults frmResults = new BudgetSegregation.Morphological.frmMorphResults(frm.Analysis);
                frmResults.ShowDialog();
            }
        }

        public void OnViewResults(object sender, EventArgs e)
        {
            try
            {
                BudgetSegregation.frmBudgetSegResults frm = new BudgetSegregation.frmBudgetSegResults(BudgetSeg);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        public void OnDelete(object sender, EventArgs e)
        {
            if (BudgetSeg.IsItemInUse)
            {
                MessageBox.Show(string.Format("The {0} {1} is currently in use and cannot be deleted. Before you can delete this {1}," +
                    " you must delete all GCD project items that refer to this {1} before it can be deleted.", BudgetSeg.Name, BudgetSeg.Noun),
                    string.Format("{0} In Use", BudgetSeg.Noun), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(string.Format("Are you sure that you want to delete the {0} {1}? The {0} {1} and all its underlying data will be deleted permanently.", BudgetSeg.Name, BudgetSeg.Noun),
                Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            try
            {
                BudgetSeg.Delete();
                Remove();
            }
            catch (IOException ex)
            {
                if (ex.Data.Contains("Locks"))
                {
                    MessageBox.Show(string.Format("The following files are being used by other process." +
                        " Close all applications that are using these files and try to delete this {0} again.\n\n{1}", NounSingle.ToLower(), ex.Data["Locks"]), "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.Data.Contains("Processes"))
                {
                    string processes = string.Format(" ({0})", ex.Data["Processes"]);
                    MessageBox.Show(string.Format("One or more files belonging to this {0} are being used by another process{1}." +
                        " Close all applications that are using these files and try to delete this {0} again.", NounSingle.ToLower(), processes), "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }
    }
}
