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

            // Validate that hte user actually chose a POLYGON ShapeFile
            if (shp.GeometryType.SimpleType != GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon)
            {
                MessageBox.Show(string.Format("The selected ShapeFile appears to be of {0} geometry type. Only polygon ShapeFiles can be used as masks.", shp.GeometryType.SimpleType), "Invalid Geometry Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucInput.Select();
                return false;
            }

            if (!GCDCore.UserInterface.SurveyLibrary.GISDatasetValidation.ValidateVector(shp))
            {
                ucInput.Select();
                return false;
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
