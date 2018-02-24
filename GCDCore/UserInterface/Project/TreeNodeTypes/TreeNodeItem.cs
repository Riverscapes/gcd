using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class TreeNodeItem : TreeNodeBase
    {
        public readonly GCDProjectItem Item;

        public TreeNodeItem(GCDProjectItem item, int imageindex, IContainer container)
            : base(item.Name, item.Noun, item.Noun, imageindex)
        {

            ContextMenuStrip = new ContextMenuStrip(container);
            ContextMenuStrip.Items.Add(string.Format("Edit {0}", NounSingle), Properties.Resources.Add, OnEdit);
            ContextMenuStrip.Items.Add(string.Format("Add {0} to the Map", NounSingle), Properties.Resources.AddToMap, OnAddToMap);
            ContextMenuStrip.Items.Add(string.Format("Delete {0}", NounSingle), Properties.Resources.Delete, OnDelete);
        }

        public void OnEdit(object sender, EventArgs e)
        {

        }

        public void OnAddToMap(object sender, EventArgs e)
        {
            if (Tag is GCDProjectRasterItem)
            {
                ProjectManager.OnAddRasterToMap(Tag as GCDProjectRasterItem);
            }
        }

        public void OnDelete(object sender, EventArgs e)
        {
            Item.Delete();
        }
    }
}
