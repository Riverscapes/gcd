using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public class GISDatasetValidation
    {

        /// <summary>
        /// Standardize how we get the reference projection
        /// </summary>
        /// <returns></returns>
        public static Projection GetProjectProjection()
        {
            Projection referenceProjection = null;
            if (ProjectManager.Project != null)
                referenceProjection = ProjectManager.Project.ReferenceProjection;
            return referenceProjection;
        }

        /// <summary>
        /// Validate that the dataset has a spatial reference
        /// </summary>
        /// <param name="gisDS"></param>
        /// <param name="sTypeSingle"></param>
        /// <param name="sTypePlural"></param>
        /// <returns></returns>
        public static bool DSHasSpatialRef(GISDataset gisDS, string sTypeSingle, string sTypePlural)
        {
            if (gisDS.Proj.PrettyWkt.ToLower().Contains("unknown"))
            {
                string msg = string.Format("The selected {0} appears to be missing a spatial reference. All GCD {0} must possess a spatial reference and it must be the same spatial reference for all GIS datasets in a GCD project.", sTypeSingle);

                Projection referenceProjection = GetProjectProjection();

                if (referenceProjection != null)
                {
                    msg += string.Format(" If the selected {0} exists in the GCD project coordinate system ({1}), but the coordinate system has not yet been defined for the {0}, then" +
                                " use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected {0} by defining the coordinate system as:" +
                                "{2}{2}{1}{2}{2}Then try importing it into the GCD again.", sTypeSingle, referenceProjection.PrettyWkt, Environment.NewLine);
                }

                MessageBox.Show(msg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate that the dataset spatial reference matches that of the project
        /// </summary>
        /// <param name="gisDS"></param>
        /// <returns></returns>
        public static bool DSSpatialRefMatchesProject(GISDataset gisDS)
        {
            Projection referenceProjection = GetProjectProjection();

            if (referenceProjection != null && !referenceProjection.IsSame(gisDS.Proj))
                return false;

            return true;
        }

        /// <summary>
        /// This is the non-optional projection check. This does the function above but then only gives a warning, not a yes/no chooser
        /// 
        /// IN THEORY THIS SHOULD NEVER TRIGGER SINCE WE'RE SO CAREFUL NOT TO IMPORT ANY BAD PROJECTIONS
        /// </summary>
        /// <param name="gisDS"></param>
        /// <returns></returns>
        public static bool DSSpatialRefMatchesProjectWithMsgbox(GISDataset gisDS, string sTypeSingle, string sTypePlural)
        {

            if (DSSpatialRefMatchesProject(gisDS))
            {
                string msg = string.Format(
                    "{0}{1}{0} Projections must match exactly.",
                    Environment.NewLine, GISDatasetValidation.SpatialRefNoMatchString(gisDS, sTypeSingle, sTypePlural));

                MessageBox.Show(msg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            return true;
        }

        /// <summary>
        /// Just a helper to build us a string for when we ask the user if we want to override the projection (force or reproject)
        /// </summary>
        /// <param name="gisDS"></param>
        /// <param name="sTypeSingle"></param>
        /// <param name="sTypePlural"></param>
        /// <returns></returns>
        public static string SpatialRefNoMatchString(GISDataset gisDS, string sTypeSingle, string sTypePlural)
        {
            Projection referenceProjection = GetProjectProjection();

            string msg = string.Format("The coordinate system of the selected {1}:{0}{0}{2}{0}{0} does not match that of the GCD project:{0}{0}{3}{0}{0}" +
               "All {4} within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause" +
               "the two coordinate systems to appear different.",
                Environment.NewLine, sTypeSingle, gisDS.Proj.PrettyWkt, referenceProjection.PrettyWkt, sTypePlural);
            return msg;
        }

        /// <summary>
        /// Verify that the horizontal units match those of the project.
        /// </summary>
        /// <param name="gisDS"></param>
        /// <param name="sTypeSingle"></param>
        /// <param name="sTypePlural"></param>
        /// <returns></returns>
        public static bool DSHorizUnitsMatchProject(GISDataset gisDS, string sTypeSingle, string sTypePlural)
        {
            if (ProjectManager.Project.Units.HorizUnit != gisDS.Proj.HorizontalUnit)
            {
                string msg = string.Format("The horizontal units of the {0} ({1}) do not match those of the GCD project ({2}).", sTypeSingle, gisDS.Proj.HorizontalUnit.ToString(), ProjectManager.Project.Units.HorizUnit.ToString());
                if (ProjectManager.Project.DEMSurveys.Count < 1)
                {
                    msg += " You can change the GCD project horizontal units by canceling this form and opening the GCD project properties form.";
                }
                msg += " Choose \"Yes\" to continue anyway. Otherwise choose \"No\" to abort.";
                DialogResult result = MessageBox.Show(msg, "HorizontalUnits Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                    return false;

            }
            return true;
        }

        public static bool DSGeometryTypeMatches(Vector vectorDS, GDalGeometryType.SimpleTypes eGeometryType)
        {
            if (vectorDS.GeometryType.SimpleType != eGeometryType)
            {
                string requiredTypeName = string.Empty;
                switch(eGeometryType)
                {
                    case GDalGeometryType.SimpleTypes.Point: requiredTypeName = "point"; break;
                    case GDalGeometryType.SimpleTypes.LineString: requiredTypeName = "line";break;
                    case GDalGeometryType.SimpleTypes.Polygon: requiredTypeName = "polygon"; break;
                    default:
                        throw new Exception("Unhandled geometry type");
                }

                MessageBox.Show(string.Format("The selected feature class does not contain {0} geometry features. Please choose a {0} ShapeFile.", requiredTypeName),
                    "Geometry Type Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            return true;
        }
    }
}
