using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class LinearExtractionGroup<T> : TreeNodeGroup
    {
        public LinearExtractionGroup(TreeNodeCollection parentNodes, string name, string nounSingle, string nounPlural, DirectoryInfo folder, IContainer container)
            : base(parentNodes, name, nounSingle, nounPlural, folder, container, ProjectManager.Project.LinearExtractions.Values.OfType<T>().Count<T>() > 0)
        {
            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (T le in ProjectManager.Project.LinearExtractions.Values.OfType<T>())
            {
                Nodes.Add(new TreeNodeItem(le as GCDCore.Project.LinearExtraction.LinearExtraction, 16, ContextMenuStrip.Container));
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
