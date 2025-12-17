using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using ArcGIS.Desktop.Core;
using System.Globalization;
using GCDViewer.ProjectTree.Masks;
using GCDViewer.ProjectTree.ProfileRoutes;
using System.Windows.Navigation;

namespace GCDViewer.ProjectTree
{
    public class GCDProject : ITreeItem, IMetadata
    {
        public readonly FileInfo ProjectFile;
        public DirectoryInfo Folder { get { return ProjectFile.Directory; } }

        // Determines whether uses V1 or V2 XSD and business logic
        public readonly string GCDVersion;

        public readonly string OriginalName;
        public string Name { get; set; }

        public string ContextMenu => "GCDProject";
        public string ImagePath => "GCD_16.png";

        public override string ToString()
        {
            return Name;
        }

        public string WarehouseId { get; internal set; }

        public readonly List<Mask> Masks;
        public readonly List<ProfileRoute> ProfileRoutes;
        public readonly List<DEMSurvey> DEMSurveys;
        public readonly List<Surface> ReferenceSurfaces;
        public readonly List<DoDBase> DoDs;

        public Dictionary<string, string> Metadata { get; internal set; }

        public GCDProject(string projectFile)
        {
            ProjectFile = new FileInfo(projectFile);

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ProjectFile.FullName);

                //var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                //nsmgr.AddNamespace("p", "http://tempuri.org/ProjectDS.xsd");

                XmlNode nodProject = xmlDoc.SelectSingleNode("Project");
                Name = nodProject.SelectSingleNode("Name")?.InnerText;
                GCDVersion = nodProject.SelectSingleNode("GCDVersion")?.InnerText;

                Masks = new List<Mask>();
                ProfileRoutes = new List<ProfileRoute>();
                DEMSurveys = new List<DEMSurvey>();
                ReferenceSurfaces = new List<Surface>();
                DoDs = new List<DoDBase>();

                // See if there's a WarehouseID in the adjacent Riverscapes project file
                WarehouseId = string.Empty;
                try
                {
                    var rs_project = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(projectFile), "project.rs.xml");
                    if (System.IO.File.Exists(rs_project))
                    {
                        XmlDocument xmlRSProject = new XmlDocument();
                        xmlRSProject.Load(rs_project);
                        var nodWarehouse = xmlRSProject.SelectSingleNode("Project/Warehouse");
                        if (nodWarehouse is XmlNode)
                        {
                            var attId = nodWarehouse.Attributes["id"];
                            if (attId is XmlAttribute && !string.IsNullOrEmpty(attId.Value))
                            {
                                WarehouseId = attId.Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Do nothing. Warehouse ID is nice to have.
                }
            }
            catch (Exception ex)
            {
                ex.Data["Project File"] = ProjectFile.FullName;
                throw;
            }
        }

        public void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ProjectFile.FullName);

            //var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            //nsmgr.AddNamespace("p", "http://tempuri.org/ProjectDS.xsd");

            XmlNode nodProject = xmlDoc.SelectSingleNode("Project");
            Name = nodProject.SelectSingleNode("Name").InnerText;
            string desc = nodProject.SelectSingleNode("Description").InnerText;
            string gvn = nodProject.SelectSingleNode("GCDVersion").InnerText;
            DateTime dtCreated = DateTime.Parse(nodProject.SelectSingleNode("DateTimeCreated").InnerText);

            // Load masks before DEMs. DEMs will load error surfaces that refer to masks
            foreach (XmlNode nodMask in nodProject.SelectNodes("Masks/Mask"))
            {
                if (nodMask.SelectSingleNode("Field") is XmlNode)
                {
                    // Regular or directional mask
                    if (nodMask.SelectSingleNode("Items") is XmlNode)
                    {
                        Masks.Add(new RegularMask(this, nodMask));
                    }
                    else
                    {
                        Masks.Add(new DirectionalMask(this, nodMask));
                    }
                }
                else
                {
                    // Area of interest
                    Masks.Add(new AOIMask(this, nodMask));
                }
            }

            // Load profile routes before DEMs, Ref Surfs and DoDs that might refer to
            // routes in their linear extractions
            foreach (XmlNode nodRoute in nodProject.SelectNodes("ProfileRoutes/ProfileRoute"))
            {
                ProfileRoutes.Add(new ProfileRoute(this, nodRoute));
            }

            foreach (XmlNode nodDEM in nodProject.SelectNodes("DEMSurveys/DEM"))
            {
                DEMSurveys.Add(new DEMSurvey(this, nodDEM));
            }

            foreach (XmlNode nodRefSurf in nodProject.SelectNodes("ReferenceSurfaces/ReferenceSurface"))
            {
                ReferenceSurfaces.Add(new Surface(this, nodRefSurf, "Reservoir.png", "Reservoir.png"));
            }

            foreach (XmlNode nodDoD in nodProject.SelectNodes("DoDs/DoD"))
            {
                DoDBase dod = null;
                if (nodDoD.SelectSingleNode("Threshold") is XmlNode)
                    dod = new DoDBase(this, nodDoD);
                else if (nodDoD.SelectSingleNode("ConfidenceLevel") is XmlNode)
                    dod = new DoDBase(this, nodDoD);
                else
                    dod = new DoDBase(this, nodDoD);

                DoDs.Add(dod);
            }

            //foreach (XmlNode nodInter in nodProject.SelectNodes("InterComparisons/InterComparison"))
            //{
            //    InterComparison inter = new InterComparison(nodInter, ProjectManager.Project.DoDs);
            //    ProjectManager.Project.InterComparisons.Add(inter);
            //}

            //foreach (XmlNode nodItem in nodProject.SelectNodes("MetaData/Item"))
            //{
            //    ProjectManager.Project.MetaData[nodItem.SelectSingleNode("Key").InnerText] = nodItem.SelectSingleNode("Value").InnerText;
            //}

            //return ProjectManager.Project;
        }

        /// <summary>
        /// Get the inner string of an XPath in the project XML
        /// </summary>
        /// <param name="xmlDoc">The project XML document</param>
        /// <param name="xPath">The absolute XPath of the desired node</param>
        /// <param name="mandatory">If true then throws an exception if the node either doesn't exist or contains no string</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetProjectXPath(XmlDocument xmlDoc, string xPath, bool mandatory)
        {
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xPath);
            if (xmlNode == null && mandatory)
                throw new Exception("Missing project XML node at " + xPath);

            if (string.IsNullOrEmpty(xmlNode.InnerText) && mandatory)
                throw new Exception(string.Format("The project node at XPath '{0}' contains no value. This XPath cannot be empty.", xPath));

            return xmlNode.InnerText;
        }

        public bool IsSame(string projectFile)
        {
            return string.Compare(ProjectFile.FullName, projectFile) == 0;
        }

        //private FileInfo AbsolutePath(string relativePath)
        //{
        //    return new FileInfo(Path.Combine(ProjectFile.DirectoryName, relativePath));
        //}

        /// <summary>
        /// Determine the location of the business lofic XML file for this project
        /// </summary>
        /// <remarks>
        /// The following locations will be searched in order for a 
        /// file with an XPath of /Project/ProjectType that matches (case insenstive)
        /// the ProjectType of this project object.
        /// 
        /// 1. ProjectFolder
        /// 2. %APPDATA%\RAVE\XML
        /// 3. SOFTWARE_DEPLOYMENT\XML
        /// 
        /// </remarks>
        //private XmlNode LoadBusinessLogicXML()
        //{
        //    string versionFolder = string.Format("V{0}", Version);

        //    List<string> SearchFolders = new List<string>()
        //    {
        //        ProjectFile.DirectoryName,
        //        Path.Combine(ProjectExplorerDockpaneViewModel.AppDataFolder.FullName, Properties.Resources.BusinessLogicXMLFolder, versionFolder)
        //    };

        //    foreach (string folder in SearchFolders)
        //    {
        //        string xmlPath = Path.ChangeExtension(Path.Combine(folder, ProjectType), "xml");
        //        if (File.Exists(xmlPath))
        //        {
        //            try
        //            {
        //                XmlDocument xmlDoc = new XmlDocument();
        //                xmlDoc.Load(xmlPath);
        //                System.Diagnostics.Debug.Print(string.Format("Using business logic at {0}", xmlPath));

        //                XmlNode nodBLRoot = xmlDoc.SelectSingleNode("Project/Node");
        //                if (nodBLRoot == null)
        //                {
        //                    throw new Exception("Business logic XML file does not contain 'Project/Node' XPath.");
        //                }

        //                // Success! Loaded correct business logic file and found the project node
        //                return nodBLRoot;
        //            }
        //            catch (Exception ex)
        //            {
        //                Exception ex2 = new FileLoadException(string.Format("Error Loading business logic from the following path." +
        //                    " Remove or rename this file to allow RAVE to continue searching for alternative {0} business logic files.\n\n{1}",
        //                    ProjectType, xmlPath), ex);
        //                ex2.Data["FilePath"] = xmlPath;
        //                throw ex2;
        //            }
        //        }
        //    }

        //    var ex3 = new Exception(string.Format("Failed to find business logic for project type {0} for project file {1}", ProjectType, ProjectFile));
        //    ex3.Data["ErrorCode"] = MISSING_BL_ERR_CODE;
        //    throw ex3;
        //}

        //public Dictionary<string, string> GetMetdata(XmlDocument xmlDoc)
        //{
        //    XmlNode nodProject = xmlDoc.SelectSingleNode("Project");
        //    if (nodProject == null)
        //        return null;

        //    return BaseDataset.LoadMetadata(nodProject);
        //}

        private string GetWarehouseId(XmlDocument xmlDoc)
        {
            //< Warehouse id = "e9bd505e-9158-41ed-9a26-7f616804aaac" apiUrl = "https://api.data.riverscapes.net" ref= "MTcxNTEyMjA2ODE3Mw==" />

            XmlNode nodWarehouse = xmlDoc.SelectSingleNode("Project/Warehouse");
            if (nodWarehouse is XmlNode)
            {
                XmlAttribute attId = nodWarehouse.Attributes["id"];
                if (attId is XmlAttribute && !string.IsNullOrEmpty(attId.InnerText))
                {
                    return attId.InnerText;
                }
            }

            return string.Empty;
        }

        public Uri DataExchangeUri
        {
            get
            {
                if (!string.IsNullOrEmpty(WarehouseId))
                {
                    Uri baseUri = new Uri(Properties.Resources.DataExchangeURL);
                    Uri projectUri = new Uri(baseUri, string.Format("/p/{0}", WarehouseId));
                    return projectUri;
                }

                return null;
            }
        }

        /// <summary>
        /// Load a project into the tree that doesn't already exist
        /// </summary>
        /// <param name="treProject">This is the project tree item. It already exists, but hasn't been added to the tree yet</param>
        /// <returns></returns>
        public void BuildProjectTree(TreeViewItemModel treProject)
        {
            // Remove all the existing child nodes (required if refreshing existing tree node)
            treProject.Children?.Clear();

            var col_dems = new GroupLayer("DEM Surveys", false, "0");
            var nod_dems = treProject.AddChild(col_dems);

            foreach (DEMSurvey dem in DEMSurveys)
            {
                var nod_dem = nod_dems.AddChild(dem);

                var col_assoc = new GroupLayer("Associated Surfaces", true, "0");
                var nod_assoc = nod_dem.AddChild(col_assoc);
                foreach (AssocSurface assoc in dem.AssocSurfaces)
                    nod_assoc.AddChild(assoc);

                var col_err = new GroupLayer("Error Surfaces", true, "0");
                var nod_err = nod_dem.AddChild(col_err);
                foreach (ErrorSurface err in dem.ErrorSurfaces)
                    nod_err.AddChild(err);
            }


            var col_ref = new GroupLayer("Reference Surfaces", true, "0");
            var nod_ref = treProject.AddChild(col_ref);

            var col_masks = new GroupLayer("Masks", true, "0");
            var nod_masks = treProject.AddChild(col_masks);

            var col_anal = new GroupLayer("Analyses", false, "0");
            var nod_anal = treProject.AddChild(col_anal);

            var col_dod = new GroupLayer("Change Detection", false, "0");
            var nod_dod = nod_anal.AddChild(col_dod);
            foreach (DoDBase dod in DoDs)
            {
                nod_dod.AddChild(dod);
            }

            // Load the business logic XML file and retrieve the root node. 
            //XmlNode nodBLRoot = LoadBusinessLogicXML();


            ////// Retrieve and apply the project name to the parent node
            ////tnProject.Header = GetLabel(nodBLRoot, projectXMLRoot);

            //// The parent node might specify the starting project node (e.g. Riverscapes Context and VBET)
            //XmlAttribute attXPath = nodBLRoot.Attributes["xpath"];
            //if (attXPath is XmlAttribute && !string.IsNullOrEmpty(attXPath.InnerText))
            //{
            //    XmlNode xmlProjectChild = projectXMLRoot.SelectSingleNode(attXPath.InnerText);
            //    if (xmlProjectChild is XmlNode)
            //        projectXMLRoot = xmlProjectChild;
            //}

            //// Loop over all child nodes of the business logic XML and load them to the tree
            //nodBLRoot.ChildNodes.OfType<XmlNode>().ToList().ForEach(x => LoadTreeNode(treProject, projectXMLRoot, x));

            //LoadProjectViews(treProject, nodBLRoot.ParentNode);
        }

        private static void ExpandAll(TreeViewItem item)
        {
            item.IsExpanded = true;
            item.UpdateLayout();

            foreach (object obj in item.Items)
            {
                if (obj is TreeViewItem)
                {
                    ExpandAll(obj as TreeViewItem);
                }
            }
        }

        private ITreeItem LoadProjectViews(TreeViewItemModel tnProject, XmlNode xmlBusiness)
        {
            XmlNode nodViews = xmlBusiness.SelectSingleNode("Views");
            if (nodViews == null)
                return null;

            XmlAttribute attDefault = nodViews.Attributes["default"];
            ITreeItem defaultView = null;
            string defaultViewName = string.Empty;
            if (attDefault is XmlAttribute)
            {
                defaultViewName = attDefault.InnerText;
            }

            TreeViewItemModel tnViews = null;

            foreach (XmlNode nodView in nodViews.SelectNodes("View"))
            {
                XmlAttribute attName = nodView.Attributes["name"];
                if (attName == null || string.IsNullOrEmpty(attName.InnerText))
                    continue;

                string viewId = string.Empty;
                XmlAttribute viewAttId = nodView.Attributes["id"];
                if (viewAttId is XmlAttribute && !string.IsNullOrEmpty(viewAttId.InnerText))
                    viewId = viewAttId.InnerText;

                bool IsDefaultView = !string.IsNullOrEmpty(defaultViewName) && string.Compare(defaultViewName, viewId) == 0;

                string viewName = nodView.Attributes["name"].InnerText;
                //ProjectView view = new ProjectView(viewId, viewName, IsDefaultView);

                foreach (XmlNode nodLayer in nodView.SelectNodes("Layers/Layer"))
                {
                    XmlAttribute attId = nodLayer.Attributes["id"];
                    if (attId == null || string.IsNullOrEmpty(attId.InnerText))
                        continue;

                    bool isVisible = true;
                    XmlAttribute attVisible = nodLayer.Attributes["visible"];
                    if (attVisible is XmlAttribute && !string.IsNullOrEmpty(attVisible.InnerText))
                    {
                        bool.TryParse(attVisible.InnerText, out isVisible);
                    }

                    TreeViewItemModel tnLayer = FindTreeNodeById(tnProject, attId.InnerText);
                    if (tnLayer is TreeViewItemModel)
                    {
                        //view.Layers.Add(new ProjectViewLayer(tnLayer, isVisible));
                    }
                }

                //if (view.Layers.Count > 0)
                //{
                //    // Create the project tree branch that will contain the views
                //    if (tnViews == null)
                //    {
                //        var grpLayer = new GroupLayer("Project Views", true, string.Empty);
                //        tnViews = tnProject.AddChild(grpLayer);
                //    }

                //    tnViews.AddChild(view);

                //    // Check if this is the default view
                //    if (view.IsDefaultView)
                //        defaultView = view;
                //}
            }

            return defaultView;
        }

        private TreeViewItemModel FindTreeNodeById(TreeViewItemModel tnNode, string id)
        {
            // Check the current node
            if (tnNode.Item is BaseDataset && string.Compare(((BaseDataset)tnNode.Item).Id, id, true) == 0)
                return tnNode;

            // Recursively check its children
            if (tnNode.Children != null)
            {
                foreach (TreeViewItemModel child in tnNode.Children)
                {
                    TreeViewItemModel match = FindTreeNodeById(child, id);
                    if (match is TreeViewItemModel)
                    {
                        return match;
                    }
                }
            }

            return null;
        }

        private void LoadTreeNode(TreeViewItemModel tnParent, XmlNode xmlProject, XmlNode xmlBusiness)
        {
            if (xmlBusiness.NodeType == XmlNodeType.Comment)
                return;

            if (string.Compare(xmlBusiness.Name, "Repeater", true) == 0)
            {
                // Add the repeater label
                XmlAttribute attLabel = xmlBusiness.Attributes["label"];
                if (attLabel is XmlAttribute && !string.IsNullOrEmpty(attLabel.InnerText))
                {
                    GroupLayer repeater = new GroupLayer(attLabel.InnerText, true, string.Empty);
                    TreeViewItemModel newNode = tnParent.AddChild(repeater);

                    // Repeat the business logic items inside the repeater for all items in the xPath
                    XmlAttribute attXPath = xmlBusiness.Attributes["xpath"];
                    if (attXPath is XmlAttribute && !string.IsNullOrEmpty(attXPath.InnerText))
                    {
                        foreach (XmlNode xmlProjectChild in xmlProject.SelectNodes(attXPath.InnerText))
                        {
                            foreach (XmlNode xmlBusinessChild in xmlBusiness.ChildNodes)
                            {
                                LoadTreeNode(newNode, xmlProjectChild, xmlBusinessChild);
                            }
                        }
                    }
                }
            }
            else if (string.Compare(xmlBusiness.Name, "Children", true) == 0)
            {
                foreach (XmlNode xmlBusinessNode in xmlBusiness.ChildNodes)
                {
                    LoadTreeNode(tnParent, xmlProject, xmlBusinessNode);
                }
            }
            else if (string.Compare(xmlBusiness.Name, "Node", true) == 0)
            {
                // First the new project node referred to by the XPath
                XmlAttribute attXPath = xmlBusiness.Attributes["xpath"];
                if (attXPath is XmlAttribute && !string.IsNullOrEmpty(attXPath.InnerText))
                {
                    xmlProject = xmlProject.SelectSingleNode(attXPath.InnerText);
                    if (xmlProject == null)
                        return;
                }

                // Now get the label. First Try the XPath, then the label
                string label = "No Label Provided";
                XmlAttribute attXPathLabel = xmlBusiness.Attributes["xpathlabel"];
                if (attXPathLabel is XmlAttribute && !string.IsNullOrEmpty(attXPathLabel.InnerText))
                {
                    XmlNode xmlLabel = xmlProject.SelectSingleNode(attXPathLabel.InnerText);
                    if (xmlLabel is XmlNode && !string.IsNullOrEmpty(xmlLabel.InnerText))
                    {
                        label = xmlLabel.InnerText;
                    }
                }
                else
                {
                    XmlAttribute attLabel = xmlBusiness.Attributes["label"];
                    if (attLabel is XmlAttribute && !string.IsNullOrEmpty(attLabel.InnerText))
                    {
                        label = attLabel.InnerText;
                    }
                }

                // Get the ID used for associated nodes with project views
                string id = string.Empty;
                XmlAttribute attId = xmlBusiness.Attributes["id"];
                if (attId is XmlAttribute && !string.IsNullOrEmpty(attId.InnerText))
                    id = attId.InnerText;

                XmlAttribute attType = xmlBusiness.Attributes["type"];
                if (attType is XmlAttribute)
                {
                    //This is a GIS Node!

                    // Retrieve symbology key from business logic
                    string symbology = string.Empty;
                    XmlAttribute attSym = xmlBusiness.Attributes["symbology"];
                    if (attSym is XmlAttribute && !String.IsNullOrEmpty(attSym.InnerText))
                        symbology = attSym.InnerText;

                    // Retrieve the transparency from the business logic
                    short transparency = 0;
                    XmlAttribute attTransparency = xmlBusiness.Attributes["transparency"];
                    if (attTransparency is XmlAttribute && !string.IsNullOrEmpty(attTransparency.InnerText))
                    {
                        if (!short.TryParse(attTransparency.InnerText, out transparency))
                            System.Diagnostics.Debug.Print(string.Format("Invalid layer transparency for {0}: {1}", label, transparency));
                    }

                    string def_query = string.Empty;
                    XmlAttribute attDefQuery = xmlBusiness.Attributes["filter"];
                    if (attDefQuery is XmlAttribute && !string.IsNullOrEmpty(attDefQuery.InnerText))
                        def_query = attDefQuery.InnerText;

                    AddGISNode(tnParent, attType.InnerText, xmlProject, symbology, label, transparency, id, def_query);
                }
                else
                {
                    // Static label node

                    // First check if there are children to this node and if so where collapsed is specified
                    bool collapsed = true;
                    XmlNode xmlChildren = xmlBusiness.SelectSingleNode("Children");
                    if (xmlChildren is XmlNode)
                    {
                        XmlAttribute attCollapsed = xmlChildren.Attributes["collapsed"];
                        if (attCollapsed is XmlAttribute && !string.IsNullOrEmpty(attCollapsed.InnerText))
                        {
                            bool.TryParse(attCollapsed.InnerText, out collapsed);
                        }
                    }

                    GroupLayer grpStatic = new GroupLayer(label, collapsed, id);
                    TreeViewItemModel newNode = tnParent.AddChild(grpStatic);
                    tnParent = newNode;
                }

                // Finally process all child nodes
                xmlBusiness.ChildNodes.OfType<XmlNode>().ToList().ForEach(x => LoadTreeNode(tnParent, xmlProject, x));
            }
        }

        private void AddGISNode(TreeViewItemModel tnParent, string type, XmlNode nodGISNode, string symbology, string label, short transparency, string id, string query_definition)
        {
            if (nodGISNode == null)
                return;

            //// If the project node has a ref attribute then lookup the redirect to the inputs
            //XmlAttribute attRef = nodGISNode.Attributes["ref"];
            //if (attRef is XmlAttribute)
            //{
            //    nodGISNode = nodGISNode.OwnerDocument.SelectSingleNode(string.Format("Project/Inputs/*[@id='{0}']", attRef.InnerText));
            //}

            //if (string.IsNullOrEmpty(label))
            //    label = nodGISNode.SelectSingleNode("Name").InnerText;

            //string path = String.Empty;
            //if (GCDVersion == 1)
            //{
            //    path = nodGISNode.SelectSingleNode("Path").InnerText;

            //    if (string.Compare(nodGISNode.ParentNode.Name, "layers", true) == 0)
            //    {
            //        XmlNode nodGeoPackage = nodGISNode.SelectSingleNode("../../Path");
            //        if (nodGeoPackage is XmlNode)
            //        {
            //            path = nodGeoPackage.InnerText + "/" + path;
            //        }
            //        else
            //        {
            //            throw new MissingMemberException("Unable to find GeoPackage file path");
            //        }
            //    }
            //}
            //else if (GCDVersion == 2)
            //{
            //    XmlNode nodPath = nodGISNode.SelectSingleNode("Path");
            //    if (nodPath is XmlNode)
            //    {
            //        path = nodPath.InnerText;
            //    }
            //    else
            //    {
            //        if (string.Compare(nodGISNode.ParentNode.Name, "layers", true) == 0)
            //        {
            //            XmlNode nodGeoPackage = nodGISNode.SelectSingleNode("../../Path");
            //            XmlAttribute attLayerName = nodGISNode.Attributes["lyrName"];

            //            if (nodGeoPackage is XmlNode && attLayerName is XmlAttribute)
            //            {
            //                path = nodGeoPackage.InnerText + "/" + attLayerName.InnerText;
            //            }
            //            else
            //            {
            //                throw new MissingMemberException("Unable to find GeoPackage file path");
            //            }
            //        }
            //    }
            //}

            //string absPath = Path.Combine(ProjectFile.DirectoryName, path);

            //// Load the layer metadata
            //Dictionary<string, string> metadata = BaseDataset.LoadMetadata(nodGISNode);

            //FileSystemDataset dataset = null;
            //switch (type.ToLower())
            //{
            //    case "file":
            //    case "report":
            //        {
            //            dataset = new FileSystemDataset(this, label, new FileInfo(absPath), "draft16.png", "draft16.png", id);
            //            break;
            //        }

            //    case "raster":
            //        {
            //            dataset = new Raster(this, label, absPath, symbology, transparency, id, metadata);
            //            break;
            //        }

            //    case "vector":
            //    case "line":
            //    case "point":
            //    case "polygon":
            //        {
            //            dataset = new Vector(this, label, absPath, symbology, transparency, id, metadata, query_definition);
            //            break;
            //        }

            //    //case "tin":
            //    //    {
            //    //        dataset = new TIN(this, label, absPath, transparency, id, metadata);
            //    //        break;
            //    //    }

            //    default:
            //        throw new Exception(string.Format("Unhandled Node type attribute string '{0}'", type));
            //}

            //tnParent.AddChild(dataset);
        }

        private static string GetXPath(XmlNode businessLogicNode, string xPath)
        {

            XmlAttribute attXPath = businessLogicNode.Attributes["xpath"];
            try
            {
                if (attXPath is XmlAttribute && !String.IsNullOrEmpty(attXPath.InnerText))
                {
                    if (!string.IsNullOrEmpty(xPath))
                        xPath += @"/";

                    xPath += attXPath.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error attempting get the XPath", ex);
            }

            return xPath;
        }

        public FileInfo GetAbsolutePath(string sRelativePath)
        {
            if (string.IsNullOrEmpty(sRelativePath))
                return null;
            else if (sRelativePath.Contains(":") || sRelativePath.StartsWith("\\\\"))
                return new FileInfo(sRelativePath);
            else
                return new FileInfo(Path.Combine(ProjectFile.DirectoryName, sRelativePath));
        }

        public DirectoryInfo GetAbsoluteDir(string sRelativeDir)
        {
            return new DirectoryInfo(Path.Combine(ProjectFile.DirectoryName, sRelativeDir));
        }

        private static string GetLabel(XmlNode businessLogicNode, XmlNode projectNode)
        {
            try
            {
                if (businessLogicNode.Attributes != null)
                {
                    // See if the business logic has a label attribute.
                    XmlAttribute attLabel = businessLogicNode.Attributes["label"];
                    if (attLabel is XmlAttribute && !string.IsNullOrEmpty(attLabel.InnerText))
                    {
                        return attLabel.InnerText;
                    }

                    // See if the project node has a child Name node with valid inner text.
                    if (projectNode is XmlNode)
                    {
                        XmlNode nodName = projectNode.SelectSingleNode("Name");
                        if (nodName is XmlNode && !string.IsNullOrEmpty(nodName.InnerText))
                            return nodName.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error attempting to get node label", ex);
            }

            return string.Empty;
        }

    }
}