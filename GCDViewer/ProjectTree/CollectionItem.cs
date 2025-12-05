using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDViewer.ProjectTree
{
    public class CollectionItem : BaseDataset
    {
        public override bool Exists { get { return true; } }
        public override string Noun { get { return Name; } }
        public CollectionItem(string name) :
            base(name, "folder16.png", "folder16.png", "0")
        {
        }
    }
}
