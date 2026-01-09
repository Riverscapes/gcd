using ArcGIS.Desktop.Framework.Threading.Tasks;
using GCDViewer.ProjectTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace GCDViewer
{
    /// <summary>
    /// Interaction logic for ProjectExplorerDockpaneView.xaml
    /// </summary>
    public partial class ProjectExplorerDockpaneView : UserControl
    {
        public ProjectExplorerDockpaneView()
        {
            InitializeComponent();
            this.Name = "test";
        }

        private void treProject_DoubleClick(object sender, EventArgs e)
        {
            if (treProject.SelectedItem is TreeViewItemModel)
            {
                TreeViewItemModel selNode = treProject.SelectedItem as TreeViewItemModel;
                try
                {
                    var model = this.DataContext as ProjectExplorerDockpaneViewModel;

                    if (selNode.Item is IGISLayer)
                    {
                        model.ExecuteAddToMap(selNode);
                    }
                    else if (selNode.Item is FileSystemDataset)
                    {
                        model.ExecuteOpenFile(selNode);
                    }
                    //else if (selNode.Item is ProjectView)
                    //{
                    //    model.ExecuteAddViewToMap(selNode);
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Double Clicking Tree Item");
                }
            }
        }

        //private void AddChildrenToMap(TreeViewItemModel e)
        //{
        //    e.Children.OfType<TreeViewItemModel>().ToList().ForEach(x => AddChildrenToMap(x));

        //    GISDataset ds = null;

        //    if (e.Item is GISDataset)
        //    {
        //        ds = e.Item as GISDataset;
        //    }
        //    else if (e.Item is ProjectView)
        //    {
        //        ((ProjectView)e.Item).Layers.ForEach(x => AddChildrenToMap(x.LayerNode));
        //    }

        //    if (ds is GISDataset)
        //    {
        //        // TODO: GIS
        //        //GISDataset layer = (GISDataset)e.Tag;
        //        //IGroupLayer parentGrpLyr = BuildArcMapGroupLayers(e);
        //        //FileInfo symbology = GetSymbology(layer);

        //        //string def_query = ds is Vector ? ((Vector)ds).DefinitionQuery : string.Empty;

        //        //ArcMapUtilities.AddToMap(layer, layer.Name, parentGrpLyr, GetPrecedingLayers(e), symbology, transparency: layer.Transparency, definition_query: def_query);
        //        //Cursor.Current = Cursors.Default;
        //    }
        //}
    }
}
