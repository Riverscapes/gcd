﻿using System;
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

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            BudgetSeg.MorphologicalAnalyses.Values.ToList().ForEach(x => new TreeNodeItem(x, 11, ContextMenuStrip.Container));

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
            EditTreeItem(frm);
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
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}