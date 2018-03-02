using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GCDCore.Project;
using System.Windows.Forms;
using System.ComponentModel;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class LinearExtractionGrp : TreeNodeGroup
    {
        public GCDProjectItem Surface { get; internal set; } // could also be DoD

        public LinearExtractionGrp(TreeNodeCollection nodParents, GCDProjectItem parentSurface, DirectoryInfo dir, IContainer container)
            : base(nodParents, "Linear Extractions", "Linear Extraction", "Linear Extractions", dir, container)
        {
            Surface = parentSurface;

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            List<GCDCore.Project.LinearExtraction.LinearExtraction> LEs = new List<GCDCore.Project.LinearExtraction.LinearExtraction>();
            if (Surface is Surface)
            {
                ((Surface)Surface).LinearExtractions.Values.ToList();
            }
            else if (Surface is DoDBase)
            {
                ((DoDBase)Surface).LinearExtractions.Values.ToList();
            }

            LEs.ForEach(x => Nodes.Add(new TreeNodeItem(x, 16, ContextMenuStrip.Container)));
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            if (ProjectManager.Project.ProfileRoutes.Count<1)
            {
                MessageBox.Show("You must create at least one profile route before you can perform a linear extraction.", "Insufficient Profile Routes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(Surface);
            EditTreeItem(frm);
        }
    }
}
