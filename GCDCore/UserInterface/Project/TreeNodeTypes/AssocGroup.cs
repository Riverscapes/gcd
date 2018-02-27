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
    public class AssocGroup : TreeNodeGroup
    {
        public readonly DEMSurvey DEM;

        public AssocGroup(TreeNodeCollection parentNodes, IContainer container, DEMSurvey dem)
            : base(parentNodes, "Associatd Surfaces", "Associated Surface", "Associated Surfaces", dem.AssocSurfacesFolder, container)
        {
            DEM = dem;

            // Associated surface uses the same form for browsing to existing as well as calculating new
            ContextMenuStrip.Items[0].Text = "Add Associated Surface";

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();
            DEM.AssocSurfaces.ToList().ForEach(x => Nodes.Add(new TreeNodeItem(x, 3, ContextMenuStrip.Container)));

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            Form frm = new SurveyLibrary.frmAssocSurfaceProperties(DEM, null);
            EditTreeItem(frm);
        }
    }
}
