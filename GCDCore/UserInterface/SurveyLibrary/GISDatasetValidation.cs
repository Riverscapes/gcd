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
        public static bool ValidateRaster(Raster raster)
        {
            if (!ValidateSpatialReference(raster, "raster", "rasters"))
                return false;


            return true;
        }

        public static bool ValidateVector(Vector vector)
        {
            if (!ValidateSpatialReference(vector, "feature class", "feature classes"))
                return false;

            if (vector.Features.Count < 1)
            {
                MessageBox.Show("The feature class does not contain any features. You must choose a feature class with one or more feature.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private static bool ValidateSpatialReference(GISDataset gisDS, string sTypeSingle, string sTypePlural)
        {
            // Get the reference projection for the project (if the project exists and has at least one GIS dataset)
            Projection referenceProjection = null;
            if (ProjectManager.Project != null)
                referenceProjection = ProjectManager.Project.ReferenceProjection;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Validate that the dataset has a spatial reference

            if (gisDS.Proj.PrettyWkt.ToLower().Contains("unknown"))
            {
                string msg = string.Format("The selected {0} appears to be missing a spatial reference. All GCD {0} must possess a spatial reference and it must be the same spatial reference for all GIS datasets in a GCD project.", sTypeSingle);

                if (referenceProjection != null)
                {
                    msg += string.Format(" If the selected {0} exists in the GCD project coordinate system ({1}), but the coordinate system has not yet been defined for the {0}, then" +
                                " use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected {0} by defining the coordinate system as:" +
                                "{2}{2}{1}{2}{2}Then try importing it into the GCD again.", sTypeSingle, referenceProjection.PrettyWkt, Environment.NewLine);
                }

                MessageBox.Show(msg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Validate that the dataset spatial reference matches that of the project

            if (referenceProjection != null && !referenceProjection.IsSame(gisDS.Proj))
            {
                string msg = string.Format("The coordinate system of the selected {1}:{0}{0}{2}{0}{0} does not match that of the GCD project:{0}{0}{3}{0}{0}" +
                   "All {4} within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " +
                    //"If you believe that the selected {1} does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " +
                    //"'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected {1} using the GCD project coordinate system specified above.",
                    "If you believe that these projections are the same (or equivalent) choose \"Yes\" to continue anyway. Otherwise choose \"No\" to abort.",
                Environment.NewLine, sTypeSingle, gisDS.Proj.PrettyWkt, referenceProjection.PrettyWkt, sTypePlural);

                DialogResult result = MessageBox.Show(msg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                    return false;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Verify that the horizontal units match those of the project.
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
    }
}
