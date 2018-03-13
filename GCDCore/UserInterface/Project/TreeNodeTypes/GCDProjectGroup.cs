using System;
using System.Collections.Generic;
using System.Linq;
using GCDCore.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class GCDProjectGroup : TreeNodeGroup
    {
        public GCDProjectGroup(TreeView treeView, IContainer container)
            : base(treeView.Nodes, ProjectManager.Project.Name, string.Empty, string.Empty, ProjectManager.Project.Folder, container, true, 1)
        {
            ContextMenuStrip.Items.Clear();
            ContextMenuStrip.Items.Add("Edit Project Properties", Properties.Resources.Options, OnEditProperties);
            ContextMenuStrip.Items.Add("Explore Project Folder", Properties.Resources.BrowseFolder, OnExplore);
            ContextMenuStrip.Items.Add("Export Project to Cross Section Viewer", Properties.Resources.import, OnCrossSectionViewer);
            ContextMenuStrip.Items.Add("Refresh Project Tree", Properties.Resources.refresh, OnRefresh);

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            TreeNodeGroup nodInpt = new GenericNodeGroup(Nodes, "Inputs", "Input", "Inputs", ProjectManager.Project.InputsFolder, ContextMenuStrip.Container, true);
            TreeNodeGroup nodSurv = new DEMSurveysGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodRefs = new ReferenceSurfaceGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodMask = new MasksGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodRout = new ProfileRouteGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodAnal = new GenericNodeGroup(Nodes, "Analyses", "Analysis", "Analyses", ProjectManager.Project.AnalysesFolder, ContextMenuStrip.Container, true);
            TreeNodeGroup nodChng = new ChangeDetectionGroup(nodAnal.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodIntr = new InterComparisonGroup(nodAnal.Nodes, ContextMenuStrip.Container);

            // Inputs and analyses nodes have no right click menu items
            nodInpt.ContextMenuStrip.Items.Clear();
            nodAnal.ContextMenuStrip.Items.Clear();

            nodInpt.Expand();
            nodAnal.Expand();
            Expand();
        }

        public void OnEditProperties(object sender, EventArgs e)
        {
            frmProjectProperties frm = new frmProjectProperties();
            EditTreeItem(frm);
        }

        new public void OnExplore(object sender, EventArgs e)
        {
            if (ProjectManager.Project.ProjectFile.Directory.Exists)
            {
                System.Diagnostics.Process.Start(ProjectManager.Project.ProjectFile.Directory.FullName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// https://robindotnet.wordpress.com/2010/03/21/how-to-pass-arguments-to-an-offline-clickonce-application/
        /// </remarks>
        private void OnCrossSectionViewer(object sender, EventArgs e)
        {
            if (ProjectManager.Project.ProjectFile.Directory.Exists)
            {
                string publisher_name = "North Arrow Research";
                string product_name = "Cross Section Viewer";

                try
                {
                    string shortcut = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "\\", publisher_name, "\\", product_name, ".appref-ms");
                    System.Diagnostics.Process.Start(shortcut, ProjectManager.Project.ProjectFile.FullName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("system cannot find the file specified"))
                    {
                        MessageBox.Show(string.Format("Unable to find the product called {0} by the publisher {1}." +
                            " Ensure that you have the product installed and then try again.", product_name, publisher_name), "Missing Application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        naru.error.ExceptionUI.HandleException(ex);
                    }
                }
            }
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            try
            {
                LoadChildNodes();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Editing GCD Project Item");
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
