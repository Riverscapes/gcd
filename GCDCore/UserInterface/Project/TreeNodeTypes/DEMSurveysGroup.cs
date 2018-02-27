﻿using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class DEMSurveysGroup : TreeNodeGroup
    {
        public DEMSurveysGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "DEM Surveys", "DEM Survey", "DEM Surveys", ProjectManager.Project.SurveysFolder, container, ProjectManager.Project.DEMSurveys.Count > 0)
        {
            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                TreeNodeItem nodDEM = new TreeNodeTypes.TreeNodeItem(dem, 2, ContextMenuStrip.Container);
                Nodes.Add(nodDEM);

                TreeNodeGroup nodAssoc = new AssocGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);
                TreeNodeGroup nodError = new ErrorSurfaceGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);

                //Nodes.Add(new LinearExtractionGroup<GCDCore.Project.LinearExtraction.LinearExtractionFromDEM>("Linear Extractions From DEM Surveys", "Linear Extraction", "Linear Extractions", container));
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            DEMSurvey referenceDEM = null;
            SurveyLibrary.ExtentImporter.Purposes ePurpose = SurveyLibrary.ExtentImporter.Purposes.FirstDEM;

            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                referenceDEM = ProjectManager.Project.DEMSurveys.Values.First();
                ePurpose = SurveyLibrary.ExtentImporter.Purposes.SubsequentDEM;
            }

            try
            {
                SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(referenceDEM, ePurpose, "DEM Survey");
                if (EditTreeItem(frm, false) == DialogResult.OK)
                {
                    frm.ProcessRaster();
                    DEMSurvey dem = new DEMSurvey(frm.txtName.Text, null, ProjectManager.Project.GetAbsolutePath(frm.txtRasterPath.Text));
                    ProjectManager.Project.DEMSurveys[dem.Name] = dem;
                    ProjectManager.Project.Save();
                    LoadChildNodes();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Importing DEM Survey");
            }
        }
    }
}
