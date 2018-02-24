using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class DEMSurveysGroup : TreeNodeGroup
    {
        public DEMSurveysGroup(IContainer container)
            : base("DEM Surveys", "DEM Survey", "DEM Surveys", container, ProjectManager.Project.DEMSurveys.Count > 0)
        {
            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                TreeNodeItem nodDEM = new TreeNodeTypes.TreeNodeItem(dem, 2, container);
                Nodes.Add(nodDEM);

                TreeNodeGroup nodAssoc = new TreeNodeGroup("Associated Surfaces", "Associated Surface", "Associated Surfaces", container, dem.AssocSurfaces.Count > 0);
                nodDEM.Nodes.Add(nodAssoc);
                dem.AssocSurfaces.ToList().ForEach(x => nodAssoc.Nodes.Add(new TreeNodeItem(x, 3, container)));

                TreeNodeGroup nodError = new TreeNodeGroup("Error Surfaces", "Error Surface", "Error Surfaces", container, dem.ErrorSurfaces.Count > 0);
                nodDEM.Nodes.Add(nodError);
                dem.ErrorSurfaces.ToList().ForEach(x => nodError.Nodes.Add(new TreeNodeItem(x, 4, container)));
            }
        }
    }
}
