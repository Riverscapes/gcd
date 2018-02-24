using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class LinearExtractionGroup<T> : TreeNodeGroup
    {
        public LinearExtractionGroup(string name, string nounSingle, string nounPlural, IContainer container)
            : base(name, nounSingle, nounPlural, container, ProjectManager.Project.LinearExtractions.Values.OfType<T>().Count<T>() > 0)
        {
            foreach (T le in ProjectManager.Project.LinearExtractions.Values.OfType<T>())
            {
                Nodes.Add(new TreeNodeItem(le as GCDCore.Project.LinearExtraction.LinearExtraction, 16, container));
            }
        }
    }
}
