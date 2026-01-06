using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.KnowledgeGraph;
using GCDViewer.ProjectTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace GCDViewer
{
    internal class ProjectExplorerDockpaneViewModel : DockPane, INotifyPropertyChanged
    {
        private const string _dockPaneID = "GCDViewer_ProjectExplorerDockpane";

        /// <summary>
        /// User's RAVE AppData Folder
        /// </summary>
        /// <remarks>
        /// C:\Users\USERNAME\AppData\Roaming\RAVE</remarks>
        //public static DirectoryInfo AppDataFolder { get { return new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppDataFolder)); } }

        /// <summary>
        /// Software deployment folder
        /// </summary>
        public static DirectoryInfo DeployFolder { get { return new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "RiverscapesXML")); } }

        public ICommand AddToMap { get; }
        public ICommand AddToMapScaled { get; }

        public ICommand AddDoDDataRange { get; }
        public ICommand AddDoD2m { get; }
        public ICommand AddDoD5m { get; }
        public ICommand AddDoDAllRasters { get; }
  
        public ICommand BrowseFolder { get; }
        public ICommand AddAllLayersToMap { get; }
        public ICommand DataExchange { get; }
        public ICommand OpenFile { get; }
        public ICommand Refresh { get; }
        public ICommand Close { get; }

        private ObservableCollection<TreeViewItemModel> treeViewItems;
        public ObservableCollection<TreeViewItemModel> TreeViewItems
        {
            get => treeViewItems;
            set
            {
                SetProperty(ref treeViewItems, value);
            }
        }

        public ProjectExplorerDockpaneViewModel()
        {
            TreeViewItems = new ObservableCollection<TreeViewItemModel>();

            AddToMap = new ContextMenuCommand(ExecuteAddToMap, CanExecuteDefault);
            AddToMapScaled = new ContextMenuCommand(ExecuteAddToMapScaled, CanExecuteAddToMapScaled);
            AddDoDDataRange = new ContextMenuCommand(ExecuteAddDoDDataRange, CanExecuteDefault);
            AddDoD2m = new ContextMenuCommand(ExecuteAddDoD2m, CanExecuteDefault);
            AddDoD5m = new ContextMenuCommand(ExecuteAddDoD5m, CanExecuteDefault);
            AddDoDAllRasters = new ContextMenuCommand(ExecuteAddDoDAllRasters, CanExecuteDefault);
            BrowseFolder = new ContextMenuCommand(ExecuteBrowseFolder, CanExecuteBrowseFolder);
            AddAllLayersToMap = new ContextMenuCommand(ExecuteAddAllLayersToMap, CanExecuteAddAllLayersToMap);
            OpenFile = new ContextMenuCommand(ExecuteOpenFile, CanExecuteOpenFile);
            DataExchange = new ContextMenuCommand(ExecuteDataExchange, CanExecuteDataExchange);
            Refresh = new ContextMenuCommand(ExecuteRefresh, CanExecuteRefresh);
            Close = new ContextMenuCommand(ExecuteClose, CanExecuteClose);
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "GCD";
        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        internal static void VisitDataExchange()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            ProjectExplorerDockpaneViewModel pevm = (ProjectExplorerDockpaneViewModel)pane;

            // Attempt to get GCD project at root of tree. If it fails then pass null
            // and this should navigate to the root of the exchange.
            TreeViewItemModel nodRoot = pevm.treeViewItems.FirstOrDefault<TreeViewItemModel>();
            pevm.ExecuteDataExchange(nodRoot);
        }

        internal static void LoadProject(string filePath = null)
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            ProjectExplorerDockpaneViewModel pevm = (ProjectExplorerDockpaneViewModel)pane;

            // If no file is supplied then looking for existing project to reload
            TreeViewItemModel nodRoot = pevm.treeViewItems.FirstOrDefault<TreeViewItemModel>();
            if (filePath == null)
            {
                if (nodRoot == null)
                {
                    return;
                }
                else
                {
                    // Get the existing project to reload
                    filePath = ((GCDProject)nodRoot.Item).ProjectFile.FullName;
                }
            }

            // Clear ready for reload
            pevm.treeViewItems.Clear();

            GCDProject newProject = new GCDProject(filePath);
            newProject.Load();

            TreeViewItemModel projectItem = new TreeViewItemModel(newProject, null);
            projectItem.IsExpanded = true;
            try
            {
                newProject.BuildProjectTree(projectItem);
                pevm.treeViewItems.Insert(0, projectItem);
            }
            catch (Exception ex)
            {
                ex.Data["Project Name"] = newProject.Name;
                ex.Data["Project Path"] = newProject.ProjectFile.FullName;
                throw;
            }
        }

        public static void CloseAllProjects()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            ProjectExplorerDockpaneViewModel pevm = (ProjectExplorerDockpaneViewModel)pane;
            var deleteList = pevm.treeViewItems.Where(x => x.Item is GCDProject).ToList();
            deleteList.ForEach(x => pevm.ExecuteClose(x));
        }

        /// <summary>
        /// Get a unique name for a project suitable for use in project tree
        /// </summary>
        /// <param name="originalName">The name of the project from the XML</param>
        /// <returns>If a project with the same name exists in the project tree
        /// already then this method will return the original name plus a unique suffix</returns>
        private string GetUniqueProjectName(GCDProject proj)
        {
            int occurences = 0;
            foreach (TreeViewItemModel nod in TreeViewItems)
            {
                if (nod.Item is GCDProject && nod.Item != proj)
                {
                    if (nod.Name.StartsWith(proj.Name))
                        occurences++;
                }
            }

            if (occurences > 0)
                return string.Format("{0} Copy {1}", proj.OriginalName, occurences);
            else
                return proj.OriginalName;
        }

        private void CollapseChildren(object parameter)
        {
            // Your action logic here
            // For example: System.Windows.MessageBox.Show("Action 1 executed");
            System.Windows.MessageBox.Show("Action 1 executed");
        }

        private async Task AddLayerToMap(TreeViewItemModel node, bool recursive)
        {
            if (node.Item is IGISLayer)
            {
                var gis = new GISUtilities();
                int index = node.Parent.Children.IndexOf(node);

                System.Diagnostics.Debug.Print(string.Format("Adding to map: {0}", node.Name));

                if (node.Item is DoDRaster)
                    await gis.AddToMapDoDAsync(node, index);
                else
                    await gis.AddToMapAsync(node, index);
            }

            if (recursive && node.Children != null && node.Children.Count > 0)
            {
                // Use a foreach loop to await each recursive call
                foreach (var child in node.Children)
                {
                    await AddLayerToMap(child, recursive);
                }
            }
        }


        #region Context Menu Commands

        public void ExecuteAddToMap(object parameter)
        {
            try
            {
                // Explicit "fire and forget" asynchronous call
                _ = AddLayerToMap(parameter as TreeViewItemModel, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }

        private bool CanExecuteDefault(object parameter)
        {
            return true;
        }

        private bool CanExecuteAddToMapScaled(object parameter)
        {
            if (parameter is null)
                return false;

            // Your logic to determine if the command can execute
            // For example, always return true for now
            var node = parameter as TreeViewItemModel;
            if (node.Item is Surface)
            {
                return node.Item is DEMSurvey;
            }

            return true;
        }

        public void ExecuteAddToMapScaled(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node.Item is DEMSurvey)
                {
                    var gis = new GISUtilities();
                    int index = node.Parent.Children.IndexOf(node);
                    _ = gis.AddToMapScaledDEMAsync(node, index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }

        public void ExecuteAddDoDDataRange(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node.Item is DoDRaster)
                {
                    var gis = new GISUtilities();
                    int index = node.Parent.Children.IndexOf(node);
                    _ = gis.AddToMapDoDAsync(node, index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }
        public void ExecuteAddDoD2m(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node.Item is DoDRaster)
                {
                    var gis = new GISUtilities();
                    int index = node.Parent.Children.IndexOf(node);
                    _ = gis.AddToMapAsync(node, index, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }

        public void ExecuteAddDoD5m(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node.Item is DoDRaster)
                {
                    var gis = new GISUtilities();
                    int index = node.Parent.Children.IndexOf(node);
                    _ = gis.AddToMapAsync(node, index, 5);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }

        public void ExecuteAddDoDAllRasters(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node.Item is DoDRaster)
                {
                    var gis = new GISUtilities();
                    int index = node.Parent.Children.IndexOf(node);
                    _ = gis.AddToMapScaledDoDAsync(node, index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding a Layer to the Map");
            }
        }

        private void ExecuteBrowseFolder(object parameter)
        {
            try
            {
                var node = parameter as TreeViewItemModel;
                if (node != null)
                {
                    DirectoryInfo dir = null;
                    if (node.Item is FileSystemDataset)
                    {
                        dir = ((FileSystemDataset)node.Item).WorkspacePath;
                    }
                    else if (node.Item is GCDProject)
                    {
                        dir = ((GCDProject)node.Item).Folder;
                    }

                    if (dir != null && dir.Exists)
                    {
                        Process.Start(new ProcessStartInfo(dir.FullName) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Browsing to Folder");
            }
        }

        private bool CanExecuteBrowseFolder(object parameter)
        {
            var node = parameter as TreeViewItemModel;
            if (node != null)
            {
                DirectoryInfo dir = null;
                if (node.Item is FileSystemDataset)
                {
                    dir = ((FileSystemDataset)node.Item).WorkspacePath;
                }
                else if (node.Item is GCDProject)
                {
                    dir = ((GCDProject)node.Item).Folder;
                }
                else
                {
                    return false;
                }

                return dir.Exists;
            }

            return false;
        }

        private void ExecuteAddAllLayersToMap(object parameter)
        {
            try
            {
                AddLayerToMap(parameter as TreeViewItemModel, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding All Layers to Map");
            }
        }

        private bool CanExecuteAddAllLayersToMap(object parameter)
        {
            if (parameter is TreeViewItemModel)
            {
                var node = parameter as TreeViewItemModel;
                return node.Children != null && node.Children.Count > 0;
            }

            return false;
        }
        public void ExecuteOpenFile(object parameter)
        {
            var node = parameter as TreeViewItemModel;
            if (node != null && node.Item is FileSystemDataset)
            {
                var dataset = (FileSystemDataset)node.Item;
                try
                {
                    if (dataset.Exists)
                        Process.Start(new ProcessStartInfo(dataset.Path.FullName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    ex.Data["File Path"] = dataset.Path.FullName;
                    MessageBox.Show(ex.Message, "Error Opening File");
                }
            }
        }
        private bool CanExecuteOpenFile(object parameter)
        {
            var node = parameter as TreeViewItemModel;
            if (node != null && node.Item is FileSystemDataset)
            {
                var dataset = (FileSystemDataset)node.Item;
                return dataset.Exists;
            }

            return false;
        }

        private void ExecuteDataExchange(object parameter)
        {
            var node = parameter as TreeViewItemModel;
            if (node != null && node.Item is GCDProject)
            {
                var project = (GCDProject)node.Item;
                try
                {
                    if (!string.IsNullOrEmpty(project.WarehouseId))
                        Process.Start(new ProcessStartInfo(project.DataExchangeUri.ToString()) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    ex.Data["Project Name"] = project.Name;
                    ex.Data["Project Path"] = project.ProjectFile.FullName;
                    MessageBox.Show(ex.Message, "Error Opening Data Exchange");
                }
            }

            // No project open. Simply navigate  to the data exhcnage.
            Process.Start(new ProcessStartInfo(Properties.Resources.DataExchangeURL) { UseShellExecute = true });
        }

        private bool CanExecuteDataExchange(object parameter)
        {
            var node = parameter as TreeViewItemModel;
            if (node != null && node.Item is GCDProject)
            {
                var project = (GCDProject)node.Item;
                return !string.IsNullOrEmpty(project.WarehouseId);
            }

            return false;
        }

        internal void ExecuteRefresh(object parameter)
        {
            if (parameter is TreeViewItemModel)
            {
                TreeViewItemModel projectNode = (TreeViewItemModel)parameter;
                GCDProject project = projectNode.Item as GCDProject;
                if (project is GCDProject)
                {
                    string filePath = project.ProjectFile.FullName;
                    try
                    {
                        TreeViewItems.Remove(projectNode);
                        LoadProject(filePath);
                    }
                    catch (Exception ex)
                    {
                        ex.Data["Project Name"] = project.Name;
                        ex.Data["Project Path"] = filePath;
                        MessageBox.Show(ex.Message, "Error Refreshing Project Tree");
                    }
                }
            }
        }

        private bool CanExecuteRefresh(object parameter)
        {
            return parameter is TreeViewItemModel && ((TreeViewItemModel)parameter).Item is GCDProject;
        }

        private void ExecuteClose(object parameter)
        {
            if (parameter is TreeViewItemModel)
            {
                TreeViewItemModel projectNode = (TreeViewItemModel)parameter;
                GCDProject project = projectNode.Item as GCDProject;
                if (project is GCDProject)
                {
                    try
                    {
                        var gis = new GISUtilities();
                        gis.RemoveGroupLayer(projectNode, null);
                    }
                    catch (Exception ex)
                    {
                        // Proceed even though there will be lingering items in map ToC
                        Console.WriteLine("Failed to remove project group layer from map.");
                    }

                    TreeViewItems.Remove(projectNode);
                }
            }
        }
        private bool CanExecuteClose(object parameter)
        {
            return parameter is TreeViewItemModel && ((TreeViewItemModel)parameter).Item is GCDProject;
        }

        #endregion
    }
}
