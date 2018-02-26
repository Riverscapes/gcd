using GCDCore.Project;
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
            : base(parentNodes, "DEM Surveys", "DEM Survey", "DEM Surveys", container, ProjectManager.Project.DEMSurveys.Count > 0)
        {
            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                TreeNodeItem nodDEM = new TreeNodeTypes.TreeNodeItem(dem, 2, container);
                Nodes.Add(nodDEM);

                TreeNodeGroup nodAssoc = new TreeNodeGroup(Nodes, "Associated Surfaces", "Associated Surface", "Associated Surfaces", container, dem.AssocSurfaces.Count > 0);
                nodDEM.Nodes.Add(nodAssoc);
                dem.AssocSurfaces.ToList().ForEach(x => nodAssoc.Nodes.Add(new TreeNodeItem(x, 3, container)));

                TreeNodeGroup nodError = new TreeNodeGroup(Nodes, "Error Surfaces", "Error Surface", "Error Surfaces", container, dem.ErrorSurfaces.Count > 0);
                nodDEM.Nodes.Add(nodError);
                dem.ErrorSurfaces.ToList().ForEach(x => nodError.Nodes.Add(new TreeNodeItem(x, 4, container)));
            }

            //TreeNodeGroup nodLinear = new linear

            //Nodes.Add(new LinearExtractionGroup<GCDCore.Project.LinearExtraction.LinearExtractionFromDEM>("Linear Extractions From DEM Surveys", "Linear Extraction", "Linear Extractions", container));
            
            if (Nodes.Count > 0)
                Expand();
        }
    }
}
