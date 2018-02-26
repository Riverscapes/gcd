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
            Item = item;

            ContextMenuStrip = new ContextMenuStrip(container);
            ContextMenuStrip.Items.Add(string.Format("Edit {0} Properties", NounSingle), Properties.Resources.Add, OnEdit);
            ContextMenuStrip.Items.Add(string.Format("Add {0} to the Map", NounSingle), Properties.Resources.AddToMap, OnAddToMap);
            ContextMenuStrip.Items.Add(string.Format("Delete {0}", NounSingle), Properties.Resources.Delete, OnDelete);
        }

        public void OnEdit(object sender, EventArgs e)
        {
            TreeNodeItem node = sender as TreeNodeItem;
            Form frm = null;

            if (Item is Surface)
            {
                frm = new SurveyLibrary.frmSurfaceProperties(Item as Surface);
            }
            else
            {
                throw new NotImplementedException("Unhandled editing of project type");
            }

            if (frm is Form)
            {
                try
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTree();
                    }
                }
                catch(Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex, "Error editing GCD project item.");
                }
            }
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
            if (Item.IsItemInUse)
            {
                MessageBox.Show(string.Format("The {0} {1} is currently in use and cannot be deleted. Before you can delete this {1}," +
                    " you must delete all GCD project items that refer to this {1} before it can be deleted.", Item.Name, Item.Noun),
                    string.Format("{0} In Use", Item.Noun), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(string.Format("Are you sure that you want to delete the {0} {1}? The {0} {1} and all its underlying data will be deleted permanently.", Item.Name, Item.Noun),
                Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            try
            {
                Item.Delete();
                LoadTree();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}
