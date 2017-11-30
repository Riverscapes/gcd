using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using static GCDCore.UserInterface.Project.ProjectTreeNode;
using GCDCore.UserInterface.SurveyLibrary;

namespace GCDCore.UserInterface.Project
{
    public partial class ucProjectExplorer
    {
        public event ProjectTreeNodeSelectionChangeEventHandler ProjectTreeNodeSelectionChange;
        public delegate void ProjectTreeNodeSelectionChangeEventHandler(object sender, EventArgs e);

        private const string m_sGroupInputs = "Inputs";
        private const string m_sAssocSurfaces = "Associated Surfaces";
        private const string m_sErrorSurfaces = "Error Surfaces";
        private const string m_sBudgetSegs = "Budget Segregations";

        private static SortSurveyBy m_eSortBy = SortSurveyBy.SurveyDateDsc;


        public enum SortSurveyBy
        {
            NameAsc,
            NameDsc,
            SurveyDateAsc,
            SurveyDateDsc
        }

        private void ProjectExplorerUC_Load(object sender, System.EventArgs e)
        {
            LoadTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSelectedNodeTag">If provided, the code will make this the selected node</param>
        /// <remarks>Grouping nodes are added to the tree with the enumeration above as their key. i.e. Project node has key "1".
        /// Items that have database IDs are added with the key as type_id. So DEM Survey with ID 4 would have key "3_4"</remarks>

        private void LoadTree(string sSelectedNodeTag = "", SortSurveyBy eSortSurveyBy = SortSurveyBy.SurveyDateDsc)
        {
            treProject.Nodes.Clear();

            if (ProjectManager.Project is GCDProject)
            {
                LoadTree(treProject, false, sSelectedNodeTag, eSortSurveyBy);
            }

        }

        private TreeNode AddTreeNode(TreeNode nodParent, GCDNodeTypes eType, string displayText, object projectItem)
        {
            TreeNode newNode = null;
            if (nodParent is TreeNode)
            {
                newNode = nodParent.Nodes.Add(displayText);
            }
            else
            {
                newNode = treProject.Nodes.Add(displayText);
            }

            newNode.ImageIndex = (int)eType;
            newNode.SelectedImageIndex = (int)eType;
            newNode.Tag = new ProjectTreeNode(eType, projectItem);
            return newNode;
        }

        public void LoadTree(TreeView tre, bool bCheckboxes, string sSelectedNodeTag = "", SortSurveyBy eSortSurveyBy = SortSurveyBy.SurveyDateDsc)
        {
            try
            {
                TreeNode nodProject = AddTreeNode(null, GCDNodeTypes.Project, ProjectManager.Project.Name, ProjectManager.Project);
                TreeNode nodInputs = AddTreeNode(nodProject, GCDNodeTypes.InputsGroup, m_sGroupInputs, null);
                TreeNode nodSurveys = AddTreeNode(nodInputs, GCDNodeTypes.SurveysGroup, "DEM Surveys", null);

                // DEM Surveys
                IEnumerable<DEMSurvey> orderedSurveys = new List<DEMSurvey>();
                switch (eSortSurveyBy)
                {
                    case SortSurveyBy.NameAsc:
                        orderedSurveys = ProjectManager.Project.DEMsSortByName(true);
                        break;
                    case SortSurveyBy.NameDsc:
                        orderedSurveys = ProjectManager.Project.DEMsSortByName(false);
                        break;
                    case SortSurveyBy.SurveyDateAsc:
                        orderedSurveys = ProjectManager.Project.DEMsSortByDate(true);
                        break;
                    case SortSurveyBy.SurveyDateDsc:
                        orderedSurveys = ProjectManager.Project.DEMsSortByName(false);
                        break;
                }

                foreach (DEMSurvey dem in orderedSurveys)
                {
                    TreeNode nodSurvey = AddTreeNode(nodSurveys, GCDNodeTypes.DEMSurvey, dem.Name, dem);
                    bool bExpandSurveyNode = false;

                    // Associated surfaces
                    TreeNode nodAssocGroup = AddTreeNode(nodSurvey, GCDNodeTypes.AssocGroup, m_sAssocSurfaces, null);
                    foreach (AssocSurface assoc in dem.AssocSurfaces)
                    {
                        AddTreeNode(nodAssocGroup, GCDNodeTypes.AssociatedSurface, assoc.Name, assoc);
                        bExpandSurveyNode = true;
                    }

                    // Error surfaces
                    TreeNode nodErrorGroup = AddTreeNode(nodSurvey, GCDNodeTypes.ErrorSurfaceGroup, m_sErrorSurfaces, null);
                    foreach (ErrorSurface errSurf in dem.ErrorSurfaces)
                    {
                        AddTreeNode(nodErrorGroup, GCDNodeTypes.ErrorSurface, errSurf.Name, errSurf);
                        bExpandSurveyNode = true;
                    }

                    if (bExpandSurveyNode)
                        nodSurvey.Expand();
                }

                nodInputs.Expand();
                nodSurveys.Expand();

                TreeNode AnalNode = AddTreeNode(nodProject, GCDNodeTypes.AnalysesGroup, "Analyses", null);
                TreeNode ChDtNode = AddTreeNode(AnalNode, GCDNodeTypes.ChangeDetectionGroup, "Change Detection", null);

                Dictionary<string, TreeNode> dDoD = new Dictionary<string, TreeNode>();
                foreach (DoDBase rDoD in ProjectManager.Project.DoDs.Values)
                {
                    ChDtNode.Expand();

                    string sDEMPair = rDoD.NewDEM.Name + " - " + rDoD.OldDEM.Name;
                    TreeNode theParent = null;
                    if (dDoD.ContainsKey(sDEMPair))
                    {
                        // This pair of DEMs already exists in the tree
                        theParent = dDoD[sDEMPair];
                    }
                    else
                    {
                        // Create a new parent of DEM surveys for this DoD
                        theParent = AddTreeNode(ChDtNode, GCDNodeTypes.ChangeDetectionDEMPair, sDEMPair, null);
                        dDoD[sDEMPair] = theParent;
                    }

                    // Now create the actual DoD node under the node for the pair of DEMs
                    TreeNode nodDoD = AddTreeNode(theParent, GCDNodeTypes.DoD, rDoD.Name, rDoD);
                    theParent.Expand();

                    // Budget Segregation Group Node
                    TreeNode nodBSGroup = null;
                    Dictionary<string, string> sMaskDict = new Dictionary<string, string>();
                    foreach (GCDCore.Project.BudgetSegregation rBS in rDoD.BudgetSegregations.Values)
                    {
                        nodDoD.Expand();

                        // Loop through and find all the unique polygon masks used
                        sMaskDict[rBS.PolygonMask.FullName] = System.IO.Path.GetFileNameWithoutExtension(rBS.PolygonMask.FullName);
                    }

                    // Now loop through all the BS and add them under the appropriate mask polygon node
                    foreach (string sPolygonMask in sMaskDict.Keys)
                    {
                        TreeNode nodMask = null;

                        foreach (GCDCore.Project.BudgetSegregation rBS in rDoD.BudgetSegregations.Values)
                        {
                            if (string.Compare(sPolygonMask, rBS.PolygonMask.FullName, true) == 0)
                            {
                                if (!(nodBSGroup is TreeNode))
                                {
                                    nodBSGroup = AddTreeNode(nodDoD, GCDNodeTypes.BudgetSegregationGroup, m_sBudgetSegs, null);
                                    nodBSGroup.Expand();
                                }

                                if (nodMask == null)
                                {
                                    nodMask = AddTreeNode(nodBSGroup, GCDNodeTypes.BudgetSegregationMask, sMaskDict[sPolygonMask], null);
                                }

                                // Budget Segregation
                                AddTreeNode(nodMask, GCDNodeTypes.BudgetSegregation, rBS.Name, rBS);
                            }
                        }
                    }
;
                    nodDoD.ExpandAll();
                }

                AnalNode.Expand();

                if (ProjectManager.Project.DoDs.Count > 0)
                    ChDtNode.Expand();

                //nodProject.ExpandAll()
                nodProject.Expand();

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void treProject_DoubleClick(object sender, System.EventArgs e)
        {
            if (treProject.SelectedNode == null)
            {
                return;
            }

            try
            {
                if (treProject.SelectedNode.Tag is ProjectTreeNode)
                {
                    ProjectTreeNode tag = (ProjectTreeNode)treProject.SelectedNode.Tag;
                    Form frm = null;

                    switch (tag.NodeType)
                    {
                        case GCDNodeTypes.DEMSurvey:
                            frm = new frmDEMSurveyProperties((DEMSurvey)tag.Item);
                            break;

                        case GCDNodeTypes.AssociatedSurface:
                            AssocSurface assoc = (AssocSurface)tag.Item;
                            frm = new frmAssocSurfaceProperties(assoc.DEM, assoc);
                            break;

                        case GCDNodeTypes.ErrorSurface:
                            ErrorSurface err = (ErrorSurface)tag.Item;
                            frm = new frmErrorSurfaceProperties(err.DEM, err);
                            break;

                        case GCDNodeTypes.DoD:
                            DoDBase dod = (DoDBase)tag.Item;
                            frm = new ChangeDetection.frmDoDResults(dod);

                            break;
                    }

                    if (frm is Form)
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private GCDNodeTypes GetNodeType(TreeNode aNode)
        {

            if (aNode.Tag is ProjectTreeNode)
            {
                return ((ProjectTreeNode)aNode.Tag).NodeType;
            }

            string sType = string.Empty;
            if (aNode.Tag.ToString().Contains("_"))
            {
                int nIndexOfSeparator = aNode.Tag.ToString().IndexOf("_");
                sType = aNode.Tag.ToString().Substring(0, nIndexOfSeparator);
            }
            else
            {
                sType = aNode.Tag.ToString();
            }

            GCDNodeTypes eType = default(GCDNodeTypes);
            switch (eType)
            {
                case GCDNodeTypes.Project: eType = GCDNodeTypes.Project; break;
                case GCDNodeTypes.InputsGroup: eType = GCDNodeTypes.InputsGroup; break;
                case GCDNodeTypes.SurveysGroup: eType = GCDNodeTypes.SurveysGroup; break;
                case GCDNodeTypes.DEMSurvey: eType = GCDNodeTypes.DEMSurvey; break;
                case GCDNodeTypes.AssocGroup: eType = GCDNodeTypes.AssocGroup; break;
                case GCDNodeTypes.AssociatedSurface: eType = GCDNodeTypes.AssociatedSurface; break;
                case GCDNodeTypes.ErrorSurfaceGroup: eType = GCDNodeTypes.ErrorSurfaceGroup; break;
                case GCDNodeTypes.ErrorSurface: eType = GCDNodeTypes.ErrorSurface; break;
                case GCDNodeTypes.AnalysesGroup: eType = GCDNodeTypes.AnalysesGroup; break;
                case GCDNodeTypes.ChangeDetectionGroup: eType = GCDNodeTypes.ChangeDetectionGroup; break;
                case GCDNodeTypes.ChangeDetectionDEMPair: eType = GCDNodeTypes.ChangeDetectionDEMPair; break;
                case GCDNodeTypes.DoD: eType = GCDNodeTypes.DoD; break;
                case GCDNodeTypes.BudgetSegregationGroup: eType = GCDNodeTypes.BudgetSegregationGroup; break;
                case GCDNodeTypes.BudgetSegregationMask: eType = GCDNodeTypes.BudgetSegregationMask; break;
                case GCDNodeTypes.BudgetSegregation: eType = GCDNodeTypes.BudgetSegregation; break;
                case GCDNodeTypes.ReservoirGroup: eType = GCDNodeTypes.ReservoirGroup; break;
                case GCDNodeTypes.Reservoir: eType = GCDNodeTypes.Reservoir; break;
                case GCDNodeTypes.AOIGroup: eType = GCDNodeTypes.AOIGroup; break;
                case GCDNodeTypes.AOI: eType = GCDNodeTypes.AOI; break;
                default:
                    throw new Exception("Unhandled tree node type");
            }

            return eType;
        }

        private int GetNodeID(TreeNode aNode)
        {

            int nID = -1;
            int nIndex = aNode.Tag.ToString().IndexOf("_");
            if (nIndex > 0)
            {
                int.TryParse(aNode.Tag.ToString().Substring(nIndex + 1), out nID);
            }

            return nID;
        }

        private void treProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeNode theNode = treProject.GetNodeAt(e.X, e.Y);
            if (!(theNode is TreeNode))
                return;


            treProject.SelectedNode = theNode;
            ContextMenuStrip cms = null;
            switch (((ProjectTreeNode)theNode.Tag).NodeType)
            {
                case GCDNodeTypes.Project: cms = cmsProject; break;
                case GCDNodeTypes.InputsGroup: cms = cmsInputsGroup; break;
                case GCDNodeTypes.SurveysGroup: cms = cmsSurveysGroup; break;
                case GCDNodeTypes.DEMSurvey: cms = cmsDEMSurvey; break;
                case GCDNodeTypes.AssocGroup: cms = cmsAssociatedSurfaceGroup; break;
                case GCDNodeTypes.AssociatedSurface: cms = cmsAssociatedSurface; break;
                case GCDNodeTypes.ErrorSurfaceGroup: cms = cmsErrorSurfacesGroup; break;
                case GCDNodeTypes.ErrorSurface: cms = cmsErrorSurface; break;
                case GCDNodeTypes.ChangeDetectionGroup: cms = cmsChangeDetectionGroup; break;
                case GCDNodeTypes.DoD: cms = cmsChangeDetection; break;
                case GCDNodeTypes.AOIGroup: cms = cmsAOIGroup; break;
                case GCDNodeTypes.ChangeDetectionDEMPair: cms = cmsDEMSurveyPair; break;
                case GCDNodeTypes.BudgetSegregationGroup: cms = cmsBSGroup; break;
                case GCDNodeTypes.BudgetSegregation: cms = cmsBS; break;
                case GCDNodeTypes.BudgetSegregationMask: cms = cmsBSGroup; break;
            }

            if (cms is ContextMenuStrip)
            {
                // Hide any GIS related menu items in standalone mode
                if (!ProjectManager.IsArcMap)
                {
                    foreach (ToolStripItem item in cms.Items)
                    {
                        item.Visible = !item.Text.ToLower().Contains("map");
                    }
                }

                cms.Show(treProject, new Point(e.X, e.Y));
            }
        }

        public DEMSurvey AddDEMSurvey()
        {
            DEMSurvey referenceDEM = null;
            ExtentImporter.Purposes ePurpose = ExtentImporter.Purposes.FirstDEM;
            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                referenceDEM = ProjectManager.Project.DEMSurveys.First().Value;
                ePurpose = ExtentImporter.Purposes.SubsequentDEM;
            }

            frmImportRaster frmImport = new frmImportRaster(referenceDEM, ePurpose, "DEM Survey");
            DEMSurvey demResult = null;
            if (frmImport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GCDConsoleLib.Raster gRaster = frmImport.ProcessRaster();

                if (gRaster is GCDConsoleLib.Raster)
                {
                    DEMSurvey dem = new DEMSurvey(frmImport.txtName.Text, null, ProjectManager.Project.GetAbsolutePath(frmImport.txtRasterPath.Text));

                    // If this is the first DEM in the project then store the cell area on the project
                    if (ProjectManager.Project.DEMSurveys.Count == 0)
                    {
                        ProjectManager.Project.CellArea = gRaster.Extent.CellArea(ProjectManager.Project.Units);
                    }

                    ProjectManager.Project.DEMSurveys.Add(dem.Name, dem);
                    ProjectManager.Project.Save();

                    LoadTree();

                    frmDEMSurveyProperties frm = new frmDEMSurveyProperties(dem);
                    frm.ShowDialog();

                    // Load the tree again because the use may have added Assoc or error surfaces
                    // while the form was open (and since the tree was last loaded)
                }

                LoadTree();
            }

            return demResult;
        }

        #region "Associated Surface Group Menu Items"


        private void AddAssociatedSurfaceToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.AssocGroup)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Parent.Tag).Item;
                        frmAssocSurfaceProperties frm = new frmAssocSurfaceProperties(dem, null);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree(selNode.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.AssocGroup)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Parent.Tag).Item;
                        foreach (AssocSurface assoc in dem.AssocSurfaces)
                        {
                            throw new Exception("add all associated surfaes to map ");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        #endregion

        #region "Associated Surface Menu Items"


        private void EditPropertiesToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.AssociatedSurface)
                    {
                        AssocSurface assoc = (AssocSurface)((ProjectTreeNode)selNode.Tag).Item;
                        frmAssocSurfaceProperties frm = new frmAssocSurfaceProperties(assoc.DEM, assoc);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree(selNode.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void AddToMapToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.AssociatedSurface)
                    {
                        AssocSurface assoc = (AssocSurface)((ProjectTreeNode)selNode.Tag).Item;
                        throw new Exception("Add associated surface to map not implemented");
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "Error Surface Group Menu Items"


        private void DeriveErrorSurfaceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);

                    TreeNode nodDEM = selNode;
                    if (eType == GCDNodeTypes.ErrorSurfaceGroup)
                    {
                        nodDEM = selNode.Parent;
                    }

                    DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)nodDEM.Tag).Item;
                    if (frmDEMSurveyProperties.CalculateErrorSurface(dem) is ErrorSurface)
                        LoadTree();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void SpecifyErrorSurfaceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    TreeNode nodDEM = selNode;
                    if (GetNodeType(selNode) == GCDNodeTypes.ErrorSurfaceGroup)
                    {
                        nodDEM = selNode.Parent;
                    }

                    DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)nodDEM.Tag).Item;
                    if (frmDEMSurveyProperties.SpecifyErrorSurface(dem) is ErrorSurface)
                        LoadTree();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void AddErrorSurfaceToMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.ErrorSurfaceGroup)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Parent.Tag).Item;
                        foreach (ErrorSurface errSurf in dem.ErrorSurfaces)
                        {
                            throw new Exception("Add error surface to map not implemented");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "Error Surface Menu Items"


        private void EditErrorSurfacePropertiesToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.ErrorSurface)
                    {
                        ErrorSurface errSurf = (ErrorSurface)((ProjectTreeNode)selNode.Tag).Item;
                        frmDEMSurveyProperties.ViewErrorSurfaceProperties(errSurf);
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

       }


        private void AddErrorSurfaceToMapToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.ErrorSurface)
                    {
                        ErrorSurface errSurf = (ErrorSurface)((ProjectTreeNode)selNode.Tag).Item;
                        throw new Exception("Adding error surface to map not implemented");
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void DeleteErrorSurfaceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.ErrorSurface)
                    {
                        ErrorSurface errSurf = (ErrorSurface)((ProjectTreeNode)selNode.Tag).Item;
                        errSurf.DEM.DeleteErrorSurface(errSurf);
                        LoadTree();
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "GCD Project Menu Items"


        private void EditGCDProjectPropertiesToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            GCDCore.UserInterface.Project.frmProjectProperties frm = new GCDCore.UserInterface.Project.frmProjectProperties(false);
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void ToolStripMenuItem2_Click(System.Object sender, System.EventArgs e)
        {
            //TODO entire function contents commented out
            throw new Exception("not implemented");
            //Try
            //    Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManagerBase.CurrentProject

            //    'TODO: Insert the GetSortedSurveyRowsMethod
            //    If TypeOf rProject Is ProjectDS.ProjectRow Then

            //        'Store DEM Survey Rows in an Ienumerable then loop over
            //        Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)
            //        For Each rDEM As ProjectDS.DEMSurveyRow In sortedSurveys.Reverse()
            //            GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEM)

            //            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows
            //                GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            //            Next

            //            For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
            //                GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            //            Next
            //        Next

            //        For Each rAOI As ProjectDS.AOIsRow In rProject.GetAOIsRows
            //            GCDProject.ProjectManagerUI.ArcMapManager.AddAOI(rAOI)
            //        Next

            //        For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
            //            GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(rDoD)
            //        Next
            //    End If

            //Catch ex As Exception
            //    naru.error.ExceptionUI.HandleException(ex)
            //End Try

        }


        private void ToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                DEMSurvey dem = AddDEMSurvey();
                if (dem is DEMSurvey)
                {
                    LoadTree();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "DEM Survey Menu Items"


        private void EditDEMSurveyProperatieToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DEMSurvey)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Tag).Item;
                        frmDEMSurveyProperties frm = new frmDEMSurveyProperties(dem);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree(selNode.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void AddToMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DEMSurvey)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Tag).Item;
                        throw new Exception("Adding DEM to map not implemented");
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void AddAssociatedSurfaceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DEMSurvey)
                    {
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)selNode.Tag).Item;
                        frmAssocSurfaceProperties frm = new frmAssocSurfaceProperties(dem, null);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree(selNode.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        //Private Sub AddErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToolStripMenuItem.Click

        //    Try
        //        Dim selNode As TreeNode = treProject.SelectedNode
        //        If TypeOf selNode Is TreeNode Then
        //            Dim eType As GCDNodeTypes = GetNodeType(selNode)
        //            If eType = GCDNodeTypes.DEMSurvey Then
        //                Dim nID As Integer = GetNodeID(selNode)
        //                Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nID)
        //                If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
        //                    Dim frm As New ErrorCalculationForm(My.ThisApplication, rDEMSurvey)
        //                    If frm.ShowDialog() = DialogResult.OK Then
        //                        LoadTree(selNode.Tag)
        //                    End If
        //                End If
        //            End If
        //        End If
        //    Catch ex As Exception
        //        ExceptionUI.HandleException(ex)
        //    End Try
        //End Sub

        #endregion

        #region "Inputs Group Menu Items"

        #endregion

        #region "Change Detection Group Menu Items"


        private void AddChangeDetectionToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            AddDoDChangeDetection();

            //Try
            //    Dim frmDoDCalculation As New DoDPropertiesForm(My.ThisApplication)
            //    DoChangeDetection(frmDoDCalculation)

            //Catch ex As Exception
            //    ExceptionUI.HandleException(ex)
            //End Try
        }

        /// <summary>
        /// Allows for a change detection to be added 
        /// </summary>
        /// <remarks></remarks>

        public void AddDoDChangeDetection()
        {
            try
            {
                ChangeDetection.frmDoDProperties frmDoDCalculation = new ChangeDetection.frmDoDProperties();
                DoChangeDetection(ref frmDoDCalculation);

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void DoChangeDetection(ref ChangeDetection.frmDoDProperties frmDoDCalculation)
        {
            try
            {
                if (frmDoDCalculation.ShowDialog() == DialogResult.OK)
                {
                    string sTag = string.Empty;
                    if (frmDoDCalculation.DoD is GCDCore.Project.DoDBase)
                    {
                        LoadTree();

                        // Now show the results form for this new DoD Calculation
                        ChangeDetection.frmDoDResults frmResults = new ChangeDetection.frmDoDResults(frmDoDCalculation.DoD);
                        frmResults.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }


        private void AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.ChangeDetectionGroup)
                    {
                        foreach (DoDBase rDoD in ProjectManager.Project.DoDs.Values)
                        {
                            throw new Exception("add all dods to the map not implemented");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "Change Detection Menu Items"


        private void AddChangeDetectionToTheMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DoD)
                    {
                        DoDBase dod = (DoDBase)((ProjectTreeNode)selNode.Tag).Item;
                        throw new Exception("Add thresholded DoD raster to map not implemented");
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        private void AddRawChangeDetectionToTheMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {

            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DoD)
                    {
                        DoDBase dod = (DoDBase)((ProjectTreeNode)selNode.Tag).Item;
                        throw new Exception("Add raw DoD raster to map not implemented");
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void ViewChangeDetectionResultsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DoD)
                    {
                        DoDBase dod = (DoDBase)((ProjectTreeNode)selNode.Tag).Item;
                        ChangeDetection.frmDoDResults frm = new ChangeDetection.frmDoDResults(dod);
                        frm.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        #endregion

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected == null)
            {
                return;
            }

            GCDNodeTypes eType = GetNodeType(nodSelected);

            try
            {
                throw new NotImplementedException("Delete click");
                LoadTree();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        #region "Properties Button"


        private void btnProperties_Click(object sender, System.EventArgs e)
        {
            try
            {
                TreeNode nodSelected = treProject.SelectedNode;
                if (nodSelected == null)
                {
                    return;
                }

                GCDNodeTypes eType = GetNodeType(nodSelected);
                int nID = GetNodeID(nodSelected);
                Form frm = null;
                switch (eType)
                {

                    case GCDNodeTypes.Project:
                        frm = new GCDCore.UserInterface.Project.frmProjectProperties(true);

                        break;
                    case GCDNodeTypes.DEMSurvey:
                        DEMSurvey dem = (DEMSurvey)((ProjectTreeNode)nodSelected.Tag).Item;
                        frm = new frmDEMSurveyProperties(dem);

                        break;
                    case GCDNodeTypes.AssociatedSurface:
                        AssocSurface assoc = (AssocSurface)((ProjectTreeNode)nodSelected.Tag).Item;
                        frm = new frmAssocSurfaceProperties(assoc.DEM, assoc);

                        break;
                    case GCDNodeTypes.ErrorSurface:
                        throw new NotImplementedException("error surface properties");
                        //    Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                        //    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
                        //    For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
                        //        If rError.ErrorSurfaceID = nID Then
                        //            Dim frm As Newe
                        //        End If
                        //    Next

                }

                if (frm is Form)
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTree();
                    }
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        public void cmdRefresh_Click(System.Object sender, System.EventArgs e)
        {
            //Dim sSortBy As String = sender.ToString

            //If String.Compare(sSortBy, "Name") = 0 Then
            //    m_eSortBy = SortSurveyBy.Name
            //ElseIf String.Compare(sSortBy, "Survey date") = 0 Then
            //    m_eSortBy = SortSurveyBy.SurveyDate
            //Else
            //    m_eSortBy = SortSurveyBy.DEMSurveyID
            //End If

            LoadTree(null, m_eSortBy);
        }

        private void SortTOC_Click(System.Object sender, System.EventArgs e)
        {
            //TODO entire function contents commented out
            throw new Exception("not implemented");

            //'Get the name of the menu item that was clicked
            //Dim pMenuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            //Dim pParentMenuItem As ToolStripMenuItem = CType(pMenuItem.OwnerItem, ToolStripMenuItem)


            //'Change the image of the selected tag to a Check mark
            //pMenuItem.Image = My.Resources.Check
            //pParentMenuItem.Image = My.Resources.Check

            //'Set other menu item images to nothing
            //For Each pTempMenuItem As ToolStripMenuItem In pMenuItem.Owner.Items
            //    If Not String.Compare(pTempMenuItem.Text, pMenuItem.Text) = 0 Then
            //        pTempMenuItem.Image = Nothing
            //    End If
            //Next

            //RefreshMenuStripImages(pParentMenuItem)


            //'Assign the proper enumeration value to the member sort by member variable
            //If String.Compare(pParentMenuItem.Text, "Name") = 0 Then
            //    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            //        m_eSortBy = SortSurveyBy.NameAsc
            //    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            //        m_eSortBy = SortSurveyBy.NameDsc
            //    End If
            //ElseIf String.Compare(pParentMenuItem.Text, "Survey date") = 0 Then
            //    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            //        m_eSortBy = SortSurveyBy.SurveyDateAsc
            //    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            //        m_eSortBy = SortSurveyBy.SurveyDateDsc
            //    End If
            //ElseIf String.Compare(pParentMenuItem.Text, "Date added") = 0 Then
            //    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            //        m_eSortBy = SortSurveyBy.DEMSurveyID_Asc
            //    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            //        m_eSortBy = SortSurveyBy.DEMSurveyID_Dsc
            //    End If
            //Else
            //    Throw New Exception("Unsupported sorting order selected.")
            //End If

            //'Load the tree using the sort by variable
            //LoadTree(Nothing, m_eSortBy)

            //Try
            //    Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManagerBase.CurrentProject
            //    If TypeOf rProject Is ProjectDS.ProjectRow Then

            //        'Loop over the survey rows in the order provided by the m_eSortBy
            //        'For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderByDescending(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Item(m_eSortBy.ToString()))
            //        'Store DEM Survey Rows in an Ienumerable then loop over
            //        Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)


            //        ' DEM survyes
            //        'For Each rSurveys As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderBy(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Item(eSortSurveyBy.ToString))
            //        If sortedSurveys Is Nothing Then
            //            Exit Sub
            //        End If

            //        For Each rDEM In sortedSurveys.Reverse()

            //            'Test to see if the group layer for the survey is in the map
            //            Dim pTest As ESRI.ArcGIS.Carto.ILayer = ArcMap.GetLayerByName(rDEM.Name, My.ThisApplication, ArcMap.eEsriLayerType.Esri_GroupLayer)

            //            'If it is in map then we will apply methods to add it in the order that the ProjectExplorerUC is now ordered in
            //            If pTest IsNot Nothing Then

            //                'The survey group layer is in the map so get the group variable
            //                Dim pSurveyGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, rDEM.Name, False)

            //                'Check to see if associated surfaces and/or error surfaces are in the map for that survey
            //                Dim bAddAssociatedSurfaces = ArcMap.SubGroupLayerExists(My.ThisApplication, "Associated Surfaces", pSurveyGroupLayer)
            //                Dim bAddErrorSurfaces = ArcMap.SubGroupLayerExists(My.ThisApplication, "Error Surfaces", pSurveyGroupLayer)

            //                'Create empty layer variable to be used and empty list of associated surface rows
            //                Dim pLayer As ESRI.ArcGIS.Carto.ILayer = Nothing
            //                Dim lAssocLayers As List(Of ProjectDS.AssociatedSurfaceRow) = New List(Of ProjectDS.AssociatedSurfaceRow)

            //                'Associated surfaces were in the map for this survey
            //                If bAddAssociatedSurfaces Then
            //                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows

            //                        'Get path of associated surface source path, get group layer, and check for presence of associated surface in associated surface sub layer of survey layer
            //                        Dim sPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rAssoc.Source)
            //                        Dim pAssocGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, "Associated Surfaces", pSurveyGroupLayer)
            //                        pLayer = GCDProject.ProjectManagerUI.ArcMapManager.IsLayerInGroupLayer(sPath, pAssocGroupLayer)
            //                        If pLayer IsNot Nothing Then
            //                            'Add associated surface row to list to be used later
            //                            lAssocLayers.Add(rAssoc)
            //                        End If
            //                    Next
            //                End If

            //                'Create  empty list of error surface rows
            //                Dim lErrorLayers As List(Of ProjectDS.ErrorSurfaceRow) = New List(Of ProjectDS.ErrorSurfaceRow)

            //                'Error surfaces were in the map for this survey
            //                If bAddErrorSurfaces Then
            //                    For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows

            //                        'Get path of error surface source path, get group layer, and check for presence of error surface in error surface sub layer of survey layer
            //                        Dim sPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rError.Source)
            //                        Dim pErrorGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, "Error Surfaces", pSurveyGroupLayer)
            //                        pLayer = GCDProject.ProjectManagerUI.ArcMapManager.IsLayerInGroupLayer(sPath, pErrorGroupLayer)
            //                        If pLayer IsNot Nothing Then
            //                            'Add error surface row to list to be used later
            //                            lErrorLayers.Add(rError)
            //                        End If
            //                    Next
            //                End If

            //                'Remove group layer as it will be placed in new sort order
            //                ArcMap.RemoveGroupLayer(My.ThisApplication, pSurveyGroupLayer.Name)

            //                'Add DEM in the order new order that is created by loop with OrderByDescending
            //                GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEM)

            //                'Add all associated surfaces to group layer that were previously in map
            //                If lAssocLayers.Count > 0 Then
            //                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In lAssocLayers
            //                        GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            //                    Next
            //                End If

            //                'Add all error surfaces to group layer that were previously in map
            //                If lErrorLayers.Count > 0 Then
            //                    For Each rError As ProjectDS.ErrorSurfaceRow In lErrorLayers
            //                        GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            //                    Next
            //                End If
            //            End If
            //        Next

            //    End If
            //Catch ex As Exception
            //    naru.error.ExceptionUI.HandleException(ex)
            //End Try

        }

        private void RefreshMenuStripImages(ref ToolStripMenuItem pParentMenuItem)
        {
            //Set other parent menu items images to nothing
            foreach (ToolStripMenuItem pTempMenuItem in pParentMenuItem.Owner.Items)
            {
                if (!(string.Compare(pTempMenuItem.Text, pParentMenuItem.Text) == 0))
                {
                    pTempMenuItem.Image = null;
                    if (pTempMenuItem.HasDropDownItems)
                    {
                        foreach (ToolStripMenuItem pItem in pTempMenuItem.DropDownItems)
                        {
                            pItem.Image = null;
                        }
                    }
                }
            }

        }

        private void ExploreChangeDetectionFolderToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);
                    if (eType == GCDNodeTypes.DoD)
                    {
                        DoDBase dod = (DoDBase)((ProjectTreeNode)selNode.Tag).Item;
                        if (dod.Folder.Exists)
                        {
                            Process.Start("explorer.exe", dod.Folder.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void ExploreGCDProjectFolderToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(ProjectManager.Project.ProjectFile.FullName));
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void AddChangeDetectionToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode nodSelected = treProject.SelectedNode;
                Debug.Assert(nodSelected.Nodes.Count > 0, "a pair of DEMs should only be in tree if there are child DoDs");

                ProjectTreeNode dodTag = (ProjectTreeNode)nodSelected.Nodes[0].Tag;
                DoDBase dod = (DoDBase)dodTag.Item;
                ChangeDetection.frmDoDProperties frm = new ChangeDetection.frmDoDProperties(dod.NewDEM, dod.OldDEM);
                DoChangeDetection(ref frm);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void AddAllChangeDetectionsToTheMapToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                throw new NotImplementedException("Add all DoDs to the map");

                //Dim nodSelected As TreeNode = treProject.SelectedNode
                //If TypeOf nodSelected Is TreeNode Then
                //    If GetNodeType(nodSelected) = GCDNodeTypes.ChangeDetectionDEMPair Then
                //        Dim n1stUnderscore As Integer = nodSelected.Tag.ToString.IndexOf("_")
                //        Dim n2ndUnderscore As Integer = nodSelected.Tag.ToString.LastIndexOf("_")
                //        Dim nNewDEMID, nOldDEMID As Integer
                //        If Integer.TryParse(nodSelected.Tag.ToString.Substring(n1stUnderscore + 1, n2ndUnderscore - n1stUnderscore - 1), nNewDEMID) Then
                //            If Integer.TryParse(nodSelected.Tag.ToString.Substring(n2ndUnderscore + 1, nodSelected.Tag.ToString.Length - n2ndUnderscore - 1), nOldDEMID) Then
                //                Dim sNewDEMName As String = String.Empty
                //                Dim sOldDEMName As String = String.Empty
                //                For Each aDEMRow As ProjectDS.DEMSurveyRow In ProjectManager.ds.DEMSurvey
                //                    If aDEMRow.DEMSurveyID = nNewDEMID Then
                //                        sNewDEMName = aDEMRow.Name
                //                    ElseIf aDEMRow.DEMSurveyID = nOldDEMID Then
                //                        sOldDEMName = aDEMRow.Name
                //                    End If
                //                Next

                //                If Not String.IsNullOrEmpty(sNewDEMName) Then
                //                    If Not String.IsNullOrEmpty(sOldDEMName) Then
                //                        For Each aDoDRow As ProjectDS.DoDsRow In ProjectManager.ds.DoDs
                //                            If String.Compare(aDoDRow.NewSurveyName, sNewDEMName, True) = 0 Then
                //                                If String.Compare(aDoDRow.OldSurveyName, sOldDEMName, True) = 0 Then
                //                                    ' TODO 
                //                                    Throw New Exception("not implemented")
                //                                    '  GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(aDoDRow)
                //                                End If
                //                            End If
                //                        Next
                //                    End If
                //                End If
                //            End If
                //        End If
                //    End If
                //End If
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer");
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void AddAllDEMSurveysToTheMapToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                throw new Exception("Add all DEMs to the map");
            }
        }

        private void AddDEMSurveyToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                AddDEMSurvey();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        private void BudgetSegregationPropertiesToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode nodSelected = treProject.SelectedNode;
                if (nodSelected is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(nodSelected);
                    int nID = GetNodeID(nodSelected);
                    if (eType == GCDNodeTypes.BudgetSegregation && nID > 0)
                    {
                        //Dim frm As New BudgetSegregation.frmBudgetSegResults(nID)
                        //frm.ShowDialog()
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void BrowseBudgetSegregationFolderToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (treProject.SelectedNode is TreeNode)
                {
                    if (treProject.SelectedNode.Tag is ProjectTreeNode)
                    {
                        ProjectTreeNode tag = (ProjectTreeNode)treProject.SelectedNode.Tag;
                        if (tag.Item is GCDCore.Project.BudgetSegregation)
                        {
                            Process.Start("explorer.exe", ((GCDCore.Project.BudgetSegregation)tag.Item).Folder.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                TreeNode nodSelected = treProject.SelectedNode;
                if (nodSelected is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(nodSelected);
                    int nID = GetNodeID(nodSelected);
                    Form frm = null;

                    switch (eType)
                    {
                        case GCDNodeTypes.InputsGroup:
                        case GCDNodeTypes.DEMSurvey:
                            AddDEMSurvey();
                            break;

                        case GCDNodeTypes.SurveysGroup:
                            AddDEMSurvey();

                            break;
                        case GCDNodeTypes.AssocGroup:
                            DEMSurvey DEM = (DEMSurvey)((ProjectTreeNode)nodSelected.Parent.Tag).Item;
                            frm = new frmAssocSurfaceProperties(DEM, null);

                            break;
                        case GCDNodeTypes.AssociatedSurface:
                            DEMSurvey DEM2 = (DEMSurvey)((ProjectTreeNode)nodSelected.Parent.Parent.Tag).Item;
                            frm = new frmAssocSurfaceProperties(DEM2, null);

                            break;
                        case GCDNodeTypes.ErrorSurfaceGroup:
                            DEMSurvey DEM3 = (DEMSurvey)((ProjectTreeNode)nodSelected.Parent.Tag).Item;
                            frm = new frmErrorSurfaceProperties(DEM3, null);
                            break;

                        case GCDNodeTypes.ErrorSurface:
                            DEMSurvey DEM4 = (DEMSurvey)((ProjectTreeNode)nodSelected.Parent.Parent.Tag).Item;
                            frm = new frmErrorSurfaceProperties(DEM4, null);
                            break;

                        case GCDNodeTypes.BudgetSegregationGroup:
                        case GCDNodeTypes.BudgetSegregation:
                            DoDBase DoD = (DoDBase)((ProjectTreeNode)nodSelected.Parent.Parent.Tag).Item;
                            frm = new GCDCore.UserInterface.BudgetSegregation.frmBudgetSegProperties(DoD);

                            break;
                        case GCDNodeTypes.DoD:
                        case GCDNodeTypes.ChangeDetectionGroup:
                        case GCDNodeTypes.ChangeDetectionDEMPair:
                        case GCDNodeTypes.AnalysesGroup:
                            frm = new ChangeDetection.frmDoDProperties();

                            break;
                    }

                    if (frm is Form)
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree(nodSelected.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void AddBudgetSegregationToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode nodSelected = treProject.SelectedNode;
                if (nodSelected is TreeNode)
                {
                    int nID = GetNodeID(nodSelected);
                    GCDNodeTypes eType = GetNodeType(nodSelected);
                    TreeNode nodDoD = null;
                    switch (eType)
                    {
                        case GCDNodeTypes.DoD:
                            nodDoD = nodSelected;

                            break;
                        case GCDNodeTypes.BudgetSegregationGroup:
                            nodDoD = nodSelected.Parent;

                            break;
                        case GCDNodeTypes.BudgetSegregationMask:
                            nodDoD = nodSelected.Parent.Parent;

                            break;
                        case GCDNodeTypes.BudgetSegregation:
                            nodDoD = nodSelected.Parent.Parent.Parent;
                            break;
                        default:
                            throw new Exception("Unhandled Node Type");
                    }

                    ProjectTreeNode treDod = (ProjectTreeNode)nodDoD.Tag;

                    BudgetSegregation.frmBudgetSegProperties frm = new BudgetSegregation.frmBudgetSegProperties((DoDBase)treDod.Item);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTree();
                        BudgetSegregation.frmBudgetSegResults frmResults = new BudgetSegregation.frmBudgetSegResults(frm.BudgetSeg);
                        frmResults.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void treProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ProjectTreeNodeSelectionChange != null)
            {
                ProjectTreeNodeSelectionChange(sender, e);
            }
        }
        public ucProjectExplorer()
        {
            Load += ProjectExplorerUC_Load;
            InitializeComponent();
        }
    }

}
