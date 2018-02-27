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
            ContextMenuStrip.Items.Add("Refresh Project Tree", Properties.Resources.refresh, OnRefresh);

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            TreeNodeGroup nodInpt = new GenericNodeGroup(Nodes, "Inputs", "Input", "Inputs", ProjectManager.Project.InputsFolder, ContextMenuStrip.Container, true);
            TreeNodeGroup nodSurv = new DEMSurveysGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodRefs = new ReferenceSurfaceGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodMask = new MasksGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodRout = new ProfileRouteGroup(nodInpt.Nodes, ContextMenuStrip.Container);
            TreeNodeGroup nodAnal = new GenericNodeGroup(Nodes, "Analyses", "Analysis", "Analyses", ProjectManager.Project.AnalysesFolder, ContextMenuStrip.Container, true);
            TreeNodeGroup nodChng = new GenericNodeGroup(nodAnal.Nodes, "Change Detections", "Change Detection", "Change Detection Analyses", ProjectManager.Project.ChangeDetectionFolder, ContextMenuStrip.Container, true);
            TreeNodeGroup nodIntr = new InterComparisonGroup(nodAnal.Nodes, ContextMenuStrip.Container);

            // NodInputs has no right click menu items
            nodInpt.ContextMenuStrip.Items.Clear();

            nodInpt.Expand();
            nodAnal.Expand();
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
