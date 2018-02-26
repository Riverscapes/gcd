using System;
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
        public LinearExtractionGroup(TreeNodeCollection parentNodes, string name, string nounSingle, string nounPlural, IContainer container)
            : base(parentNodes, name, nounSingle, nounPlural, container, ProjectManager.Project.LinearExtractions.Values.OfType<T>().Count<T>() > 0)
        {
            foreach (T le in ProjectManager.Project.LinearExtractions.Values.OfType<T>())
            {
                Nodes.Add(new TreeNodeItem(le as GCDCore.Project.LinearExtraction.LinearExtraction, 16, container));
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
