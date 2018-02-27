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
        private const string m_sMasks = "Masks";
        private const string m_sReferenceSurfaces = "Reference Surfaces";
        private const string m_sMorphological = "Morphological Analyses";
        private const string m_sProfileRoutes = "Profile Routes";
        private const string m_sLinearExtractionDEM = "Linear DEM Profile Results";
        private const string m_sLinearExtractionSurf = "Linear Reference Surface Profile Results";
        private const string m_sLinearExtractionDoD = "Linear Change Detection Results";

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
            //LoadTree(null);

            // AddToMapToolStripMenuItem_Click
            this.treProject.MouseDown += treProject_MouseDown;
            this.treProject.DoubleClick += treProject_DoubleClick;
            this.treProject.MouseDown += treProject_MouseDown;
            this.treProject.AfterSelect += treProject_AfterSelect;
            this.EditGCDProjectPropertiesToolStripMenuItem.Click += EditGCDProjectPropertiesToolStripMenuItem_Click;
            this.cmsAddProjectToMap.Click += AddProjectToMap_Click;
            this.ExploreGCDProjectFolderToolStripMenuItem.Click += ExploreGCDProjectFolderToolStripMenuItem_Click;
            this.refreshProjectTreeToolStripMenuItem.Click += LoadTree;
            this.EditDEMSurveyProperatieToolStripMenuItem.Click += EditSurface_Click;
            this.AddToMapToolStripMenuItem.Click += AddToMapToolStripMenuItem_Click;
            this.DeleteDEMSurveyToolStripMenuItem.Click += btnDelete_Click;
            this.AddAssociatedSurfaceToolStripMenuItem.Click += AddAssociatedSurfaceToolStripMenuItem_Click;
            this.AddToMapToolStripMenuItem1.Click += AddToMapToolStripMenuItem_Click;
            this.AddErrorSurfaceToolStripMenuItem.Click += SpecifyErrorSurfaceToolStripMenuItem_Click;
            this.DeriveErrorSurfaceToolStripMenuItem1.Click += DeriveErrorSurfaceToolStripMenuItem_Click;
            this.EditPropertiesToolStripMenuItem.Click += EditPropertiesToolStripMenuItem_Click;
            this.DeleteAssociatedSurfaceToolStripMenuItem.Click += btnDelete_Click;
            this.AddAssociatedSurfaceToolStripMenuItem1.Click += AddAssociatedSurfaceToolStripMenuItem1_Click;
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Click += AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click;
            this.AddDEMSurveyToolStripMenuItem.Click += ToolStripMenuItem1_Click;
            this.AddErrorSurfaceToolStripMenuItem1.Click += SpecifyErrorSurfaceToolStripMenuItem_Click;
            this.DeriveErrorSurfaceToolStripMenuItem.Click += DeriveErrorSurfaceToolStripMenuItem_Click;
            this.AddErrorSurfaceToMapToolStripMenuItem.Click += AddErrorSurfaceToMapToolStripMenuItem_Click;
            this.AddErrorSurfaceToMapToolStripMenuItem1.Click += AddToMapToolStripMenuItem_Click;
            this.EditErrorSurfacePropertiesToolStripMenuItem.Click += EditErrorSurfacePropertiesToolStripMenuItem_Click;
            this.DeleteErrorSurfaceToolStripMenuItem.Click += btnDelete_Click;
            this.AddChangeDetectionToolStripMenuItem.Click += AddChangeDetectionToolStripMenuItem_Click;
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Click += AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click;
            this.ViewChangeDetectionResultsToolStripMenuItem.Click += ViewChangeDetectionResultsToolStripMenuItem_Click;
            this.AddChangeDetectionToTheMapToolStripMenuItem.Click += AddChangeDetectionToTheMapToolStripMenuItem_Click;
            this.AddRawChangeDetectionToTheMapToolStripMenuItem.Click += AddRawChangeDetectionToTheMapToolStripMenuItem_Click;
            this.ExploreChangeDetectionFolderToolStripMenuItem.Click += ExploreChangeDetectionFolderToolStripMenuItem_Click;
            this.DeleteChangeDetectionToolStripMenuItem.Click += btnDelete_Click;
            this.AddBudgetSegregationToolStripMenuItem.Click += AddBudgetSegregationToolStripMenuItem1_Click;
            this.AddDEMSurveyToolStripMenuItem1.Click += AddDEMSurveyToolStripMenuItem1_Click;
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Click += AddAllDEMSurveysToTheMapToolStripMenuItem1_Click;
            this.AddAllDEMSurveysToTheMapToolStripMenuItem.Click += AddAllDEMSurveysToTheMapToolStripMenuItem1_Click;
            this.NameAscendingToolStripMenuItem.Click += SortTOC_Click;
            this.NameDescendingToolStripMenuItem.Click += SortTOC_Click;
            this.SurveyDateAscendingToolStripMenuItem.Click += SortTOC_Click;
            this.SurveyDateDescendingToolStripMenuItem.Click += SortTOC_Click;
            this.DateAddedAscendingToolStripMenuItem.Click += SortTOC_Click;
            this.DateAddedDescendingToolStripMenuItem.Click += SortTOC_Click;
            this.AddChangeDetectionToolStripMenuItem1.Click += AddChangeDetectionToolStripMenuItem1_Click;
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Click += AddAllChangeDetectionsToTheMapToolStripMenuItem_Click;
            this.AddBudgetSegregationToolStripMenuItem1.Click += AddBudgetSegregationToolStripMenuItem1_Click;
            this.BudgetSegregationPropertiesToolStripMenuItem.Click += BudgetSegregationPropertiesToolStripMenuItem_Click;
            this.BrowseBudgetSegregationFolderToolStripMenuItem.Click += BrowseBudgetSegregationFolderToolStripMenuItem_Click;
            this.AddBudgetSegregationToolStripMenuItem2.Click += AddBudgetSegregationToolStripMenuItem1_Click;

            this.deriveReferenceSurfaceFromDEMSurveysToolStripMenuItem.Click += addReferenceSurfaceFromDEMs;
            this.deriveConstantReferenceSurfaceToolStripMenuItem.Click += addReferenceSurfaceFromContant;
            this.deriveErrorSurfaceToolStripMenuItem2.Click += DeriveErrorSurfaceToolStripMenuItem_Click;

            this.specifyErrorSurfaceToolStripMenuItem.Click += SpecifyErrorSurfaceToolStripMenuItem_Click;

            this.specifyReferenceSurfaceToolStripMenuItem.Click += specifyReferenceSurface;
            this.addChangeDetectionIntercomparisonToolStripMenuItem.Click += addChangeDetectionIntercomparisonToolStripMenuItem_Click;
            this.addChangeDetectionInterComparisonToolStripMenuItem1.Click += addChangeDetectionIntercomparisonToolStripMenuItem_Click;
            this.openInterComparisonFolderToolStripMenuItem.Click += openInterComparisonFolderToolStripMenuItem_Click;
            this.addReferenceSurfaceToMapToolStripMenuItem.Click += AddToMapToolStripMenuItem_Click;
            this.deleteToolStripMenuItem.Click += btnDelete_Click;
            this.editReferenceSurfacePropertiesToolStripMenuItem.Click += EditSurface_Click;
            this.addAllReferenceSurfacesToTheMapToolStripMenuItem.Click += AddAllReferenceSurfacesToMap_Click;

            this.collapseChildrenInGCDProjectTreeToolStripMenuItem.Click += CollapseChildren_Click;
            this.collapseChildrenInGCDProjectTreeToolStripMenuItem1.Click += CollapseChildren_Click;
            this.editMaskPropertiesToolStripMenuItem.Click += EditMaskProperties_Click;
            this.addMaskToMapToolStripMenuItem.Click += AddMaskToMap_Click;
            this.addAllMasksToTheMapToolStripMenuItem.Click += AllAllMasksToMap_Click;
            this.deleteMaskToolStripMenuItem.Click += btnDelete_Click;
            this.viewMorphologicalAnalysisToolStripMenuItem.Click += EditMorphological_Click;

            this.addAreaOfInterestAOIMaskToolStripMenuItem.Click += AddAOI_Click;

            this.addProfileGroupToolStripMenuItem.Click += AddEditProfile_Click;
            this.editProfileRoutePropertiesToolStripMenuItem.Click += AddEditProfile_Click;

            this.deriveProfileFromDEMSurveyToolStripMenuItem.Click += AddLinearExtraction_Click;

        }

        public void LoadTree(object sender, EventArgs e)
        {
            ProjectManager.RefreshProject();
            LoadTree(null, SortSurveyBy.NameAsc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSelectedNodeTag">If provided, the code will make this the selected node</param>
        /// <remarks>Grouping nodes are added to the tree with the enumeration above as their key. i.e. Project node has key "1".
        /// Items that have database IDs are added with the key as type_id. So DEM Survey with ID 4 would have key "3_4"</remarks>
        public void LoadTree(ProjectTreeNode selectItem, SortSurveyBy eSortSurveyBy = SortSurveyBy.SurveyDateDsc)
        {
            treProject.Nodes.Clear();

            if (ProjectManager.Project is GCDProject)
            {
                //LoadTree(treProject, false, selectItem, eSortSurveyBy);
                LoadTree(selectItem);
            }
        }

        private TreeNode AddTreeNode(TreeNode nodParent, GCDNodeTypes eType, string displayText, object projectItem, ProjectTreeNode selItem)
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

            newNode.ImageIndex = 0;
            switch (eType)
            {
                case GCDNodeTypes.Project: newNode.ImageIndex = 1; break;
                case GCDNodeTypes.DEMSurvey: newNode.ImageIndex = 2; break;
                case GCDNodeTypes.AssociatedSurface: newNode.ImageIndex = 3; break;
                case GCDNodeTypes.ErrorSurface: newNode.ImageIndex = 4; break;
                case GCDNodeTypes.ReferenceSurface: newNode.ImageIndex = 5; break;
                case GCDNodeTypes.BudgetSegregationMask: newNode.ImageIndex = 8; break;
                case GCDNodeTypes.BudgetSegregation: newNode.ImageIndex = 9; break;
                case GCDNodeTypes.InterComparison: newNode.ImageIndex = 10; break;
                case GCDNodeTypes.MorphologicalAnalysis: newNode.ImageIndex = 11; break;
                case GCDNodeTypes.AOI: newNode.ImageIndex = 14; break;
                case GCDNodeTypes.ProfileRoute: newNode.ImageIndex = 15; break;
                case GCDNodeTypes.LinearExtraction: newNode.ImageIndex = 16; break;
                case GCDNodeTypes.DoD:
                    DoDBase dod = (GCDCore.Project.DoDBase)projectItem;
                    if (dod.NewSurface is DEMSurvey && dod.OldSurface is DEMSurvey)
                        newNode.ImageIndex = 7;
                    else
                        newNode.ImageIndex = 12;
                    break;

                case GCDNodeTypes.Mask:
                    if (projectItem is GCDCore.Project.Masks.RegularMask)
                        newNode.ImageIndex = 6;
                    else
                        newNode.ImageIndex = 13;
                    break;
            }

            newNode.SelectedImageIndex = newNode.ImageIndex;
            newNode.Tag = new ProjectTreeNode(eType, projectItem);

            if (selItem is ProjectTreeNode && selItem.Equals((ProjectTreeNode)newNode.Tag))
            {
                newNode.TreeView.SelectedNode = newNode;
                newNode.Parent.Expand();
            }

            return newNode;
        }

        public void LoadTree(ProjectTreeNode selectItem)
        {
            TreeNodeTypes.TreeNodeGroup nodProj = new TreeNodeTypes.GCDProjectGroup(treProject, components);
            //TreeNodeTypes.TreeNodeGroup nodInpt = new TreeNodeTypes.GenericNodeGroup(nodProj.Nodes, m_sGroupInputs, "Input", "Inputs", components, true);
            //TreeNodeTypes.TreeNodeGroup nodSurv = new TreeNodeTypes.DEMSurveysGroup(nodInpt.Nodes, components);
            //TreeNodeTypes.TreeNodeGroup nodRefs = new TreeNodeTypes.ReferenceSurfaceGroup(nodInpt.Nodes, components);
            //TreeNodeTypes.TreeNodeGroup nodMask = new TreeNodeTypes.MasksGroup(nodInpt.Nodes, components);
            //TreeNodeTypes.TreeNodeGroup nodRout = new TreeNodeTypes.ProfileRouteGroup(nodInpt.Nodes, components);
            //TreeNodeTypes.TreeNodeGroup nodAnal = new TreeNodeTypes.GenericNodeGroup(nodProj.Nodes, "Analyses", "Analysis", "Analyses", components, true);
            //TreeNodeTypes.TreeNodeGroup nodChng = new TreeNodeTypes.GenericNodeGroup(nodAnal.Nodes, "Change Detections", "Change Detection", "Change Detection Analyses", components, true);
            //TreeNodeTypes.TreeNodeGroup nodIntr = new TreeNodeTypes.InterComparisonGroup(nodAnal.Nodes, components);

            //// NodInputs has no right click menu items
            //nodInpt.ContextMenuStrip.Items.Clear();

            nodProj.Expand();
           
        }

        public void LoadTree(TreeView tre, bool bCheckboxes, ProjectTreeNode selectItem, SortSurveyBy eSortSurveyBy = SortSurveyBy.SurveyDateDsc)
        {
            try
            {
                TreeNode nodProject = AddTreeNode(null, GCDNodeTypes.Project, ProjectManager.Project.Name, ProjectManager.Project, selectItem);
                TreeNode nodInputs = AddTreeNode(nodProject, GCDNodeTypes.InputsGroup, m_sGroupInputs, null, selectItem);
                TreeNode nodSurveys = AddTreeNode(nodInputs, GCDNodeTypes.SurveysGroup, "DEM Surveys", null, selectItem);

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
                    TreeNode nodSurvey = AddTreeNode(nodSurveys, GCDNodeTypes.DEMSurvey, dem.Name, dem, selectItem);
                    bool bExpandSurveyNode = false;

                    // Associated surfaces
                    TreeNode nodAssocGroup = AddTreeNode(nodSurvey, GCDNodeTypes.AssocGroup, m_sAssocSurfaces, null, selectItem);
                    foreach (AssocSurface assoc in dem.AssocSurfaces)
                    {
                        AddTreeNode(nodAssocGroup, GCDNodeTypes.AssociatedSurface, assoc.Name, assoc, selectItem);
                        bExpandSurveyNode = true;
                    }

                    // Error surfaces
                    TreeNode nodErrorGroup = AddTreeNode(nodSurvey, GCDNodeTypes.ErrorSurfaceGroup, m_sErrorSurfaces, null, selectItem);
                    foreach (ErrorSurface errSurf in dem.ErrorSurfaces)
                    {
                        AddTreeNode(nodErrorGroup, GCDNodeTypes.ErrorSurface, errSurf.NameWithDefault, errSurf, selectItem);
                        bExpandSurveyNode = true;
                    }

                    if (bExpandSurveyNode)
                        nodSurvey.Expand();
                }

                if (ProjectManager.Project.LinearExtractions.Values.Any(x => x is GCDCore.Project.LinearExtraction.LinearExtractionFromDEM))
                {
                    TreeNode nodLinear = AddTreeNode(nodSurveys, GCDNodeTypes.LinearExtractionGroup, m_sLinearExtractionDEM, null, selectItem);
                    foreach (GCDCore.Project.LinearExtraction.LinearExtraction le in ProjectManager.Project.LinearExtractions.Values.Where(x => x is GCDCore.Project.LinearExtraction.LinearExtractionFromDEM))
                    {
                        AddTreeNode(nodLinear, GCDNodeTypes.LinearExtraction, le.Name, le, selectItem);
                    }
                    nodLinear.Expand();
                }

                // Reference Surfaces
                TreeNode nodReferenceSurfaces = AddTreeNode(nodInputs, GCDNodeTypes.ReferenceSurfaceGroup, m_sReferenceSurfaces, null, selectItem);
                foreach (Surface surf in ProjectManager.Project.ReferenceSurfaces.Values)
                {
                    bool bExpandSurfNode = false;
                    TreeNode nodSurface = AddTreeNode(nodReferenceSurfaces, GCDNodeTypes.ReferenceSurface, surf.Name, surf, selectItem);

                    // Error surfaces
                    TreeNode nodErrorGroup = AddTreeNode(nodSurface, GCDNodeTypes.ErrorSurfaceGroup, m_sErrorSurfaces, null, selectItem);
                    foreach (ErrorSurface errSurf in surf.ErrorSurfaces)
                    {
                        AddTreeNode(nodErrorGroup, GCDNodeTypes.ErrorSurface, errSurf.NameWithDefault, errSurf, selectItem);
                        bExpandSurfNode = true;
                    }

                    if (bExpandSurfNode)
                        nodSurface.Expand();
                }

                if (ProjectManager.Project.LinearExtractions.Values.Any(x => x.GCDProjectItem is Surface && !(x.GCDProjectItem is DEMSurvey)))
                {
                    TreeNode nodLinear = AddTreeNode(nodReferenceSurfaces, GCDNodeTypes.LinearExtractionGroup, m_sLinearExtractionSurf, null, selectItem);
                    foreach (GCDCore.Project.LinearExtraction.LinearExtraction le in ProjectManager.Project.LinearExtractions.Values.Where(x => x.GCDProjectItem is Surface && !(x.GCDProjectItem is DEMSurvey)))
                    {
                        AddTreeNode(nodLinear, GCDNodeTypes.LinearExtraction, le.Name, le, selectItem);
                    }
                    nodLinear.Expand();
                }

                nodReferenceSurfaces.Expand();

                TreeNode nodMaskGroup = AddTreeNode(nodInputs, GCDNodeTypes.MasksGroup, m_sMasks, null, selectItem);
                if (ProjectManager.Project.Masks.Count > 0)
                {
                    foreach (GCDCore.Project.Masks.Mask aMask in ProjectManager.Project.Masks.Values)
                    {
                        AddTreeNode(nodMaskGroup, aMask is GCDCore.Project.Masks.AOIMask ? GCDNodeTypes.AOI : GCDNodeTypes.Mask, aMask.Name, aMask, selectItem);
                    }
                    nodMaskGroup.Expand();
                }

                TreeNode nodRoutes = AddTreeNode(nodInputs, GCDNodeTypes.ProfileRoutesGroup, m_sProfileRoutes, null, selectItem);
                if (ProjectManager.Project.ProfileRoutes.Count > 0)
                {
                    ProjectManager.Project.ProfileRoutes.Values.ToList().ForEach(x => AddTreeNode(nodRoutes, GCDNodeTypes.ProfileRoute, x.Name, x, selectItem));
                    nodRoutes.Expand();
                }

                nodInputs.Expand();
                nodSurveys.Expand();

                TreeNode AnalNode = AddTreeNode(nodProject, GCDNodeTypes.AnalysesGroup, "Analyses", null, selectItem);
                TreeNode ChDtNode = AddTreeNode(AnalNode, GCDNodeTypes.ChangeDetectionGroup, "Change Detection", null, selectItem);

                Dictionary<string, TreeNode> dDoD = new Dictionary<string, TreeNode>();
                foreach (DoDBase rDoD in ProjectManager.Project.DoDs.Values)
                {
                    ChDtNode.Expand();

                    string sDEMPair = rDoD.NewSurface.Name + " - " + rDoD.OldSurface.Name;
                    TreeNode theParent = null;
                    if (dDoD.ContainsKey(sDEMPair))
                    {
                        // This pair of DEMs already exists in the tree
                        theParent = dDoD[sDEMPair];
                    }
                    else
                    {
                        // Create a new parent of DEM surveys for this DoD
                        theParent = AddTreeNode(ChDtNode, GCDNodeTypes.ChangeDetectionDEMPair, sDEMPair, null, selectItem);
                        dDoD[sDEMPair] = theParent;
                    }

                    // Now create the actual DoD node under the node for the pair of DEMs
                    TreeNode nodDoD = AddTreeNode(theParent, GCDNodeTypes.DoD, rDoD.Name, rDoD, selectItem);
                    theParent.Expand();

                    // Budget Segregation Group Node
                    TreeNode nodBSGroup = null;
                    Dictionary<string, string> sMaskDict = new Dictionary<string, string>();
                    foreach (GCDCore.Project.BudgetSegregation rBS in rDoD.BudgetSegregations.Values)
                    {
                        nodDoD.Expand();

                        // Loop through and find all the unique polygon masks used
                        sMaskDict[rBS.Mask.Name] = System.IO.Path.GetFileNameWithoutExtension(rBS.Mask._ShapeFile.FullName);
                    }

                    // Now loop through all the BS and add them under the appropriate mask polygon node
                    foreach (string sPolygonMask in sMaskDict.Keys)
                    {
                        TreeNode nodMask = null;

                        foreach (GCDCore.Project.BudgetSegregation rBS in rDoD.BudgetSegregations.Values)
                        {
                            if (string.Compare(sPolygonMask, rBS.Mask.Name, true) == 0)
                            {
                                if (!(nodBSGroup is TreeNode))
                                {
                                    nodBSGroup = AddTreeNode(nodDoD, GCDNodeTypes.BudgetSegregationGroup, m_sBudgetSegs, null, selectItem);
                                    nodBSGroup.Expand();
                                }

                                if (nodMask == null)
                                {
                                    nodMask = AddTreeNode(nodBSGroup, GCDNodeTypes.BudgetSegregationMask, sMaskDict[sPolygonMask], null, selectItem);
                                }

                                // Budget Segregation
                                TreeNode nodBS = AddTreeNode(nodMask, GCDNodeTypes.BudgetSegregation, rBS.Name, rBS, selectItem);

                                if (rBS.MorphologicalAnalyses.Count > 0)
                                {
                                    TreeNode nodMAGroup = AddTreeNode(nodBS, GCDNodeTypes.MorphologicalAnalysisGroup, m_sMorphological, null, selectItem);
                                    rBS.MorphologicalAnalyses.Values.ToList<GCDCore.Project.Morphological.MorphologicalAnalysis>().ForEach(x =>
                                        AddTreeNode(nodMAGroup, GCDNodeTypes.MorphologicalAnalysis, x.Name, x, selectItem));
                                }
                            }
                        }
                    }
;
                    nodDoD.ExpandAll();
                }

                if (ProjectManager.Project.LinearExtractions.Values.Any(x => x is GCDCore.Project.LinearExtraction.LinearExtractionFromDoD))
                {
                    TreeNode nodLinear = AddTreeNode(nodReferenceSurfaces, GCDNodeTypes.LinearExtractionGroup, m_sLinearExtractionDoD, null, selectItem);
                    foreach (GCDCore.Project.LinearExtraction.LinearExtraction le in ProjectManager.Project.LinearExtractions.Values.Where(x => x is GCDCore.Project.LinearExtraction.LinearExtractionFromDoD))
                    {
                        AddTreeNode(nodLinear, GCDNodeTypes.LinearExtraction, le.Name, le, selectItem);
                    }
                    nodLinear.Expand();
                }

                TreeNode nodInter = AddTreeNode(AnalNode, GCDNodeTypes.InterComparisonGroup, "Inter-Comparisons", null, selectItem);
                ProjectManager.Project.InterComparisons.Values.ToList<InterComparison>().ForEach(x => AddTreeNode(nodInter, GCDNodeTypes.InterComparison, x.Name, x, selectItem));
                if (nodInter.Nodes.Count > 0)
                    nodInter.Expand();

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
                            frm = new frmSurfaceProperties((DEMSurvey)tag.Item);
                            break;

                        case GCDNodeTypes.AssociatedSurface:
                            AssocSurface assoc = (AssocSurface)tag.Item;
                            frm = new frmAssocSurfaceProperties(assoc.DEM, assoc);
                            break;

                        case GCDNodeTypes.ErrorSurface:
                            ErrorSurface err = (ErrorSurface)tag.Item;
                            frm = new frmErrorSurfaceProperties((DEMSurvey)err.Surf, err);
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
                            LoadTree(tag);
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
            int nIndex = ((ProjectTreeNode)aNode.Tag).Name.ToString().IndexOf("_");
            if (nIndex > 0)
            {
                int.TryParse(aNode.Tag.ToString().Substring(nIndex + 1), out nID);
            }

            return nID;
        }

        private void treProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            return;

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
                case GCDNodeTypes.ReferenceSurfaceGroup: cms = cmsRefSurfaceGroup; break;
                case GCDNodeTypes.ReferenceSurface: cms = cmsRefSurface; break;
                case GCDNodeTypes.MasksGroup: cms = cmsMasks; break;
                case GCDNodeTypes.AOI:
                case GCDNodeTypes.Mask: cms = cmsMask; break;
                case GCDNodeTypes.ChangeDetectionGroup: cms = cmsChangeDetectionGroup; break;
                case GCDNodeTypes.DoD: cms = cmsChangeDetection; break;
                case GCDNodeTypes.ChangeDetectionDEMPair: cms = cmsDEMSurveyPair; break;
                case GCDNodeTypes.BudgetSegregationGroup: cms = cmsBSGroup; break;
                case GCDNodeTypes.BudgetSegregation: cms = cmsBS; break;
                case GCDNodeTypes.BudgetSegregationMask: cms = cmsBSGroup; break;
                case GCDNodeTypes.MorphologicalAnalysis: cms = cmsMorphological; break;
                case GCDNodeTypes.InterComparisonGroup: cms = cmsInterComparison; break;
                case GCDNodeTypes.ProfileRoutesGroup: cms = cmsProfileRouteGroup; break;
                case GCDNodeTypes.ProfileRoute: cms = cmsProfileRoute; break;
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
                this.Cursor = Cursors.WaitCursor;

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

                    LoadTree(new ProjectTreeNode(GCDNodeTypes.DEMSurvey, dem));
                    ProjectManager.OnAddRasterToMap(dem);
                }
            }

            this.Cursor = Cursors.Default;

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
                            ProjectManager.OnAddRasterToMap(frm.m_Assoc);
                            LoadTree(new ProjectTreeNode(GCDNodeTypes.AssociatedSurface, frm.m_Assoc));
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
                            ProjectManager.OnAddRasterToMap(assoc);
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
                            LoadTree((ProjectTreeNode)selNode.Tag);
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

                    Surface parent = ((ProjectTreeNode)nodDEM.Tag).Item as Surface;
                    ErrorSurface err = null;

                    if (parent is DEMSurvey)
                    {
                        frmErrorSurfaceProperties frm = new frmErrorSurfaceProperties(parent as DEMSurvey, null);
                        if (frm.ShowDialog() == DialogResult.OK)
                            err = frm.ErrorSurf;
                    }
                    else
                    {
                        SurveyLibrary.ReferenceSurfaces.frmRefErrorSurface frm = new SurveyLibrary.ReferenceSurfaces.frmRefErrorSurface(parent);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            err = frm.ErrorSurface;
                        }
                    }

                    if (err is ErrorSurface)
                    {
                        ProjectManager.OnAddRasterToMap(err);
                        LoadTree(new ProjectTreeNode(GCDNodeTypes.ErrorSurface, err));
                    }
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

                    Surface parent = ((ProjectTreeNode)nodDEM.Tag).Item as Surface;
                    ErrorSurface err = null;

                    if (parent is DEMSurvey)
                    {
                        frmErrorSurfaceProperties frm = new frmErrorSurfaceProperties(parent as DEMSurvey, null);
                        if (frm.ShowDialog() == DialogResult.OK)
                            err = frm.ErrorSurf;
                    }
                    else
                    {
                        frmImportRaster frm = new frmImportRaster(parent, ExtentImporter.Purposes.ReferenceErrorSurface, "Reference Error Surface");
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            GCDConsoleLib.Raster rError = frm.ProcessRaster();
                            err = new ErrorSurface(frm.txtName.Text, rError.GISFileInfo, parent);
                            parent.ErrorSurfaces.Add(err);
                            ProjectManager.Project.Save();
                        }
                    }

                    if (err is ErrorSurface)
                    {
                        ProjectManager.OnAddRasterToMap(err);
                        LoadTree(new ProjectTreeNode(GCDNodeTypes.ErrorSurface, err));
                    }
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
                            ProjectManager.OnAddRasterToMap(errSurf);
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
                        Form frm = null;
                        if (errSurf.Surf is DEMSurvey)
                        {
                            frm = new frmErrorSurfaceProperties((DEMSurvey)errSurf.Surf, errSurf);
                        }
                        else
                        {
                            frm = new frmSurfaceProperties(errSurf);
                        }

                        if (frm.ShowDialog() == DialogResult.OK)
                            LoadTree((ProjectTreeNode)selNode.Tag);
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

        //private void DeleteErrorSurfaceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        TreeNode selNode = treProject.SelectedNode;
        //        if (selNode is TreeNode)
        //        {
        //            GCDNodeTypes eType = GetNodeType(selNode);
        //            if (eType == GCDNodeTypes.ErrorSurface)
        //            {
        //                ErrorSurface errSurf = (ErrorSurface)((ProjectTreeNode)selNode.Tag).Item;
        //                errSurf.DEM.DeleteErrorSurface(errSurf);
        //                LoadTree();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        naru.error.ExceptionUI.HandleException(ex);
        //    }
        //}

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

        private void AddProjectToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (ProjectManager.Project == null)
                    return;

                foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
                {
                    ProjectManager.OnAddRasterToMap(dem);

                    foreach (AssocSurface assoc in dem.AssocSurfaces)
                        ProjectManager.OnAddRasterToMap(assoc);

                    foreach (ErrorSurface err in dem.ErrorSurfaces)
                        ProjectManager.OnAddRasterToMap(err);
                }

                foreach (DoDBase dod in ProjectManager.Project.DoDs.Values)
                    ProjectManager.OnAddRasterToMap(dod.ThrDoD);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void ToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                DEMSurvey dem = AddDEMSurvey();
                if (dem is DEMSurvey)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.DEMSurvey, dem));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion

        #region "DEM Survey Menu Items"

        private void EditSurface_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                TreeNode selNode = treProject.SelectedNode;
                if (selNode is TreeNode)
                {
                    GCDNodeTypes eType = GetNodeType(selNode);

                    if (((ProjectTreeNode)selNode.Tag).Item is Surface)
                    {
                        Surface surf = (Surface)((ProjectTreeNode)selNode.Tag).Item;
                        frmSurfaceProperties frm = new frmSurfaceProperties(surf);

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            LoadTree((ProjectTreeNode)selNode.Tag);
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
                    if (selNode.Tag is ProjectTreeNode)
                    {
                        if (((ProjectTreeNode)selNode.Tag).Item is GCDProjectRasterItem)
                        {
                            ProjectManager.OnAddRasterToMap((GCDProjectRasterItem)((ProjectTreeNode)selNode.Tag).Item);
                        }
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
                            LoadTree(new ProjectTreeNode(GCDNodeTypes.AssociatedSurface, frm.m_Assoc));
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
                        LoadTree(new ProjectTreeNode(GCDNodeTypes.DoD, frmDoDCalculation.DoD));

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
                            ProjectManager.OnAddRasterToMap(rDoD.ThrDoD);
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
                        ProjectManager.OnAddRasterToMap(dod.ThrDoD);
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
                        ProjectManager.OnAddRasterToMap(dod.RawDoD);
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
            ProjectTreeNode ptn = (ProjectTreeNode)nodSelected.Tag;
            GCDProjectItem item = (GCDProjectItem)ptn.Item;

            if (item.IsItemInUse)
            {
                MessageBox.Show(string.Format("The {0} {1} is currently in use and cannot be deleted. Before you can delete this {1}," +
                    " you must delete all GCD project items that refer to this {1} before it can be deleted.", item.Name, item.Noun),
                    string.Format("{0} In Use", item.Noun), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(string.Format("Are you sure that you want to delete the {0} {1}? The {0} {1} and all its underlying data will be deleted permanently.", item.Name, item.Noun),
                Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            try
            {
                item.Delete();
                ProjectManager.Project.Save();
                LoadTree((ProjectTreeNode)nodSelected.Parent.Tag);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

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
                ChangeDetection.frmDoDProperties frm = new ChangeDetection.frmDoDProperties(dod.NewSurface, dod.OldSurface);
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
                try
                {
                    ProjectManager.OnAddRasterToMap(dem);
                }
                catch (Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex);
                }
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
                    GCDCore.Project.BudgetSegregation bs = (GCDCore.Project.BudgetSegregation)((ProjectTreeNode)nodSelected.Tag).Item;
                    BudgetSegregation.frmBudgetSegResults frm = new BudgetSegregation.frmBudgetSegResults(bs);
                    frm.ShowDialog();
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
                            // TODO: nod selection needed
                            LoadTree(null);
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
                        LoadTree(new ProjectTreeNode(GCDNodeTypes.BudgetSegregation, frm.BudgetSeg));
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

        private void multiEpochChangeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeDetection.MultiEpoch.frmMultiEpoch frm = new ChangeDetection.MultiEpoch.frmMultiEpoch();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.ChangeDetectionGroup, null));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        private void multiUncertaintyAnalysisChangeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeDetection.Batch.frmBatchDoD frm = new ChangeDetection.Batch.frmBatchDoD();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.DoD, null));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        private void addMorphologicalAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            GCDCore.Project.BudgetSegregation bs = ((ProjectTreeNode)nodSelected.Tag).Item as GCDCore.Project.BudgetSegregation;

            if (!(bs.Mask is GCDCore.Project.Masks.DirectionalMask))
            {
                MessageBox.Show("You can only perform morphological approach sediment analyses on budget segregations that were" +
                    " generated using directional mask. The selected budget segregation was generated using a regular mask.", "Invalid Budget Segregation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UserInterface.BudgetSegregation.Morphological.frmMorpProperties frm1 = new BudgetSegregation.Morphological.frmMorpProperties(bs);
            if (frm1.ShowDialog() == DialogResult.OK)
            {
                UserInterface.BudgetSegregation.Morphological.frmMorphResults frm2 = new BudgetSegregation.Morphological.frmMorphResults(frm1.Analysis);
                if (frm2.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.MorphologicalAnalysis, frm2.Analysis));
                }
            }
        }

        private void addRegularMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Masks.frmMaskProperties frm = new Masks.frmMaskProperties();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadTree(new ProjectTreeNode(GCDNodeTypes.Mask, frm.Mask));
            }
        }

        private void addDirectionalMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Masks.frmDirectionalMaskProps frm = new Masks.frmDirectionalMaskProps();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadTree(new ProjectTreeNode(GCDNodeTypes.Mask, frm.Mask));
            }
        }

        private void addReferenceSurfaceFromDEMs(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 2)
            {
                MessageBox.Show("You must have at least two DEM surveys in your GCD project before you can generate a reference surface from DEM surveys.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UserInterface.SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadTree(new ProjectTreeNode(GCDNodeTypes.ReferenceSurface, frm.ReferenceSurface));
            }
        }

        private void addReferenceSurfaceFromContant(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 1)
            {
                MessageBox.Show("You must have at least one DEM survey in your GCD project before you can generate a constant reference surface.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UserInterface.SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadTree(new ProjectTreeNode(GCDNodeTypes.ReferenceSurface, frm.ReferenceSurface));
            }
        }

        private void specifyReferenceSurface(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 1)
            {
                MessageBox.Show("You must have at least one DEM survey in your GCD project before you can generate a constant reference surface.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmImportRaster frm = new frmImportRaster(ProjectManager.Project.DEMSurveys.Values.First<DEMSurvey>(), ExtentImporter.Purposes.ReferenceSurface, "Reference Surface");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GCDConsoleLib.Raster rOutput = frm.ProcessRaster();
                GCDCore.Project.Surface refSurf = new Surface(frm.txtName.Text, rOutput);
                ProjectManager.Project.ReferenceSurfaces[refSurf.Name] = refSurf;
                ProjectManager.Project.Save();
                LoadTree(new ProjectTreeNode(GCDNodeTypes.ReferenceSurface, refSurf));
            }
        }

        private void addChangeDetectionIntercomparisonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DoDs.Count < 2)
            {
                MessageBox.Show("Your project must contain at least two change detection analyses before you can perform an inter-comparison.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChangeDetection.Intercomparison.frmInterComparisonProperties frm = new ChangeDetection.Intercomparison.frmInterComparisonProperties();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadTree(new ProjectTreeNode(GCDNodeTypes.InterComparisonGroup, null));
            }
        }

        private void openInterComparisonFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = ProjectManager.OutputManager.GetInterComparisonPath("junk").Directory;
            if (dir.Exists)
                Process.Start(dir.FullName);
            else
                MessageBox.Show("This project does not contain any inter-comparisons.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CollapseChildren_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                foreach (TreeNode nodChild in nodSelected.Nodes)
                    nodChild.Collapse();
            }
        }

        public void EditMaskProperties_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                ProjectTreeNode ptn = nodSelected.Tag as ProjectTreeNode;
                GCDCore.Project.Masks.Mask mask = ptn.Item as GCDCore.Project.Masks.Mask;
                Form frm = null;

                if (mask is GCDCore.Project.Masks.RegularMask)
                {
                    frm = new Masks.frmMaskProperties((GCDCore.Project.Masks.RegularMask)mask);
                }
                else if (mask is GCDCore.Project.Masks.DirectionalMask)
                {
                    frm = new Masks.frmDirectionalMaskProps(mask as GCDCore.Project.Masks.DirectionalMask);
                }
                else if (mask is GCDCore.Project.Masks.AOIMask)
                {
                    frm = new Masks.frmAOIProperties(mask as GCDCore.Project.Masks.AOIMask);
                }

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(ptn.NodeType, mask));
                }
            }
        }

        public void EditMorphological_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                GCDCore.Project.Morphological.MorphologicalAnalysis ma = ((ProjectTreeNode)nodSelected.Tag).Item as GCDCore.Project.Morphological.MorphologicalAnalysis;
                UserInterface.BudgetSegregation.Morphological.frmMorphResults frm = new BudgetSegregation.Morphological.frmMorphResults(ma);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.MorphologicalAnalysis, ma));
                }
            }
        }

        public void AddMaskToMap_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                GCDCore.Project.Masks.Mask mask = ((ProjectTreeNode)nodSelected.Tag).Item as GCDCore.Project.Masks.Mask;
                ProjectManager.OnAddVectorToMap(mask);
            }
        }

        public void AllAllMasksToMap_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                ProjectManager.Project.Masks.Values.ToList<GCDCore.Project.Masks.Mask>().ForEach(x => ProjectManager.OnAddVectorToMap(x));
            }
        }

        public void AddAllReferenceSurfacesToMap_Click(object sender, EventArgs e)
        {
            TreeNode nodSelected = treProject.SelectedNode;
            if (nodSelected is TreeNode)
            {
                ProjectManager.Project.ReferenceSurfaces.Values.ToList<GCDCore.Project.Surface>().ForEach(x => ProjectManager.OnAddRasterToMap(x));
            }
        }

        public void AddAOI_Click(object sender, EventArgs e)
        {
            try
            {
                Masks.frmAOIProperties frm = new Masks.frmAOIProperties(null);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.AOI, frm.AOIMask));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        public void AddEditProfile_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectTreeNode ptn = treProject.SelectedNode.Tag as ProjectTreeNode;
                GCDCore.Project.ProfileRoutes.ProfileRoute route = null;
                if (ptn.Item is GCDCore.Project.ProfileRoutes.ProfileRoute)
                    route = ptn.Item as GCDCore.Project.ProfileRoutes.ProfileRoute;

                ProfileRoutes.frmProfileRouteProperties frm = new ProfileRoutes.frmProfileRouteProperties(route);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(new ProjectTreeNode(GCDNodeTypes.ProfileRoute, frm.ProfileRoute));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        public void AddLinearExtraction_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectTreeNode ptn = treProject.SelectedNode.Tag as ProjectTreeNode;
                if (ptn.Item is GCDProjectItem)
                {
                    LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(ptn.Item as GCDProjectItem);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTree(new ProjectTreeNode(GCDNodeTypes.LinearExtraction, frm.LinearExtraction));
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}