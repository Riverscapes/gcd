using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Masks
{
    /// <summary>
    /// Shared validation methods for regular and directional masks
    /// </summary>
    public class MaskValidation
    {
        public static bool ValidateMaskName(TextBox txt, GCDCore.Project.Masks.Mask mask)
        {
            // Sanity check to prevent empty names
            txt.Text = txt.Text.Trim();

            if (string.IsNullOrEmpty(txt.Text))
            {
                MessageBox.Show("You must provide a name for the mask.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!ProjectManager.Project.IsMaskNameUnique(txt.Text, mask))
            {
                MessageBox.Show("This project already contains a mask with this name. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt.Select();
                return false;
            }

            return true;
        }

        public static bool ValidateShapeFile(GCDCore.UserInterface.UtilityForms.ucVectorInput ucInput)
        {
            if (!(ucInput.SelectedItem is GCDConsoleLib.Vector))
            {
                MessageBox.Show("You must choose a mask ShapeFile to continue.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucInput.Select();
                return false;
            }

            // Should be safe after Validate call above
            GCDConsoleLib.Vector shp = ucInput.SelectedItem;

            if (shp.Features.Count < 1)
            {
                MessageBox.Show("The ShapeFile does not contain any features. You must choose a polygon ShapeFile with one or more feature.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucInput.Select();
                return false;
            }

            // Validate that hte user actually chose a POLYGON ShapeFile
            if (shp.GeometryType.SimpleType != GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon)
            {
                MessageBox.Show(string.Format("The selected ShapeFile appears to be of {0} geometry type. Only polygon ShapeFiles can be used as masks.", shp.GeometryType.SimpleType), "Invalid Geometry Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucInput.Select();
                return false;
            }

            if (shp.Proj.PrettyWkt.ToLower().Contains("unknown"))
            {
                MessageBox.Show("The selected ShapeFile appears to be missing a spatial reference." +
                    " All GCD ShapeFiles must possess a spatial reference and it must be the same spatial reference for all rasters in a GCD project.", "Missing Spatial Reference", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucInput.Select();
                return false;
            }

            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                if (!ProjectManager.Project.DEMSurveys.Values.First<DEMSurvey>().Raster.Proj.IsSame(shp.Proj))
                {
                    string wkt = ProjectManager.Project.DEMSurveys.Values.First<DEMSurvey>().Raster.Proj.Wkt;

                    MessageBox.Show("The coordinate system of the selected ShapeFile:" + Environment.NewLine + Environment.NewLine + shp.Proj.PrettyWkt + Environment.NewLine + Environment.NewLine +
                       "does not match that of the GCD project:" + Environment.NewLine + Environment.NewLine + wkt + Environment.NewLine + Environment.NewLine +
                       "All ShapeFiles and rasters within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " +
                       "If you believe that the selected ShapeFile does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " +
                       "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:"
                       + Environment.NewLine + Environment.NewLine + wkt + Environment.NewLine + Environment.NewLine + "Then try importing it into the GCD again.",
                       Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateField(ComboBox cboField)
        {
            if (string.IsNullOrEmpty(cboField.Text))
            {
                MessageBox.Show("You must select the field in the ShapeFile that identifies the mask values.", "Missing Mask Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboField.Select();
                return false;
            }

            return true;
        }
    }
}
