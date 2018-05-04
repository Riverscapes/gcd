using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmMaskProperties : Form, IProjectItemForm
    {
        public readonly naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem> MaskItems;
        public GCDCore.Project.Masks.RegularMask Mask { get; internal set; }
        public GCDProjectItem GCDProjectItem { get { return Mask; } }

        public frmMaskProperties(GCDCore.Project.Masks.RegularMask mask = null)
        {
            InitializeComponent();
            MaskItems = new naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem>();
            Mask = mask;
        }

        private void frmMaskProperties_Load(object sender, EventArgs e)
        {
            if (Mask is GCDCore.Project.Masks.RegularMask)
            {
                cmdOK.Text = Properties.Resources.UpdateButtonText;

                txtName.Text = Mask.Name;

                lblPath.Visible = false;
                txtPath.Visible = false;

                grpFieldValues.Top -= grpFeatureClass.Top - txtPath.Top;
                grpFeatureClass.Top -= grpFeatureClass.Top - txtPath.Top;

                ucPolygon.InitializeExisting("Mask Polygon ShapeFile", Mask.Vector);
                ucPolygon.AddToMap += cmdAddToMap_Click;

                cboField.DataSource = new BindingList<string>() { Mask._Field };
                cboField.SelectedIndex = 0;
                cboField.Enabled = false;

                Mask._Items.ForEach(x => MaskItems.Add(x));
                cmdOK.Text = "Update";

                grdData.Select();
            }
            else
            {
                cmdOK.Text = Properties.Resources.CreateButtonText;
                ucPolygon.InitializeBrowseNew("Directional Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
            }

            // subscribe to the even when the user changes the input ShapeFile
            ucPolygon.PathChanged += InputShapeFileChanged;

            // Subscribe to name changing so that path can update
            // do this after any setting of name for existing mask
            txtName.TextChanged += txtName_TextChanged;

            // Subscribe to the field changing.
            // do this after any setting of shapefile for existing mask
            cboField.SelectedIndexChanged += cboField_SelectedIndexChanged;

            grdData.DataSource = MaskItems;

            tTip.SetToolTip(txtName, "The name used to refer to this mask within the GCD user interface. It cannot be empty and must be unique among all masks within this GCD project.");
            tTip.SetToolTip(txtPath, "The relative file path within this GCD project where the mask ShapeFile will get copied.");
            tTip.SetToolTip(cboField, "The string attribute field that distinguishes mask polygons.");
            tTip.SetToolTip(grdData, "List of unique values from the mask field. Check the box beside each item to toggle its inclusion in the mask. Provide an optional label to override the display text used for each mask item.");
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.MaskPath(txtName.Text));
        }

        private void InputShapeFileChanged(object sender, naru.ui.PathEventArgs e)
        {
            cboField.DataSource = null;

            if (ucPolygon.SelectedItem == null)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            GCDConsoleLib.Vector shapeFile = ucPolygon.SelectedItem;

            // Use the ShapeFile file name if the user hasn't specified one yet
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName);
            }

            cboField.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            Cursor = Cursors.Default;
        }

        private void cboField_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaskItems.Clear();
            if (string.IsNullOrEmpty(cboField.Text))
                return;

            Cursor = Cursors.WaitCursor;

            foreach (KeyValuePair<long, GCDConsoleLib.VectorFeature> vFeature in ucPolygon.SelectedItem.Features)
            {
                string fieldValue = vFeature.Value.GetFieldAsString(cboField.Text);
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    if (!MaskItems.Any<GCDCore.Project.Masks.MaskItem>(x => string.Compare(x.FieldValue, fieldValue, true) == 0))
                    {
                        MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, fieldValue, fieldValue));
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool bIncude = ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all");
                MaskItems.ToList<GCDCore.Project.Masks.MaskItem>().ForEach(x => x.Include = bIncude);
                MaskItems.ResetBindings();
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }

        private bool ValidateForm()
        {
            if (!MaskValidation.ValidateMaskName(txtName, Mask))
                return false;

            if (!MaskValidation.ValidateShapeFile(ucPolygon) || !MaskValidation.ValidateShapefileProjection(ucPolygon))
                return false;

            if (!MaskValidation.ValidateField(cboField))
                return false;

            if (MaskItems.Count < 1)
            {
                MessageBox.Show(string.Format("The ShapeFile selected does not contain any valid string values in the {0} field. Please choose a different field or ShapeFile.", cboField.Text), "No Field Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboField.Select();
                return false;
            }

            if (MaskItems.Count(x => x.Include) < 1)
            {
                MessageBox.Show("You must check the box beside at least one field value.", "No Field Values Included", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdData.Select();
                return false;
            }

            return true;
        }

        private void grdData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                MessageBox.Show("You must provide a non-empty label for every field value.", "Empty Label", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }

            string msg = "Labels must be unique and cannot be reused for multiple field values. Also, labels cannot duplicate any field value other than" +
                " the field value that they represent.";

            // Ensure that the new label text does not match any other field values
            string fieldValue = grdData.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (MaskItems.Any(x => string.Compare(x.FieldValue, fieldValue, true) != 0 && string.Compare(x.FieldValue, e.FormattedValue.ToString(), true) == 0))
            {
                MessageBox.Show(string.Format("The label matches another field value in the {0} attribute field of the ShapeFile. {1}", cboField.Text, msg), "Invalid Label", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else if (MaskItems.Any(x => string.Compare(x.FieldValue, fieldValue, true) != 0 && string.Compare(x.Label, e.FormattedValue.ToString(), true) == 0))
            {
                MessageBox.Show(string.Format("The label matches another label value. {0}", msg), "Invalid Label", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                if (Mask == null)
                {
                    FileInfo fiMask = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiMask.Directory.Create();

                    ucPolygon.SelectedItem.Copy(fiMask);

                    Mask = new GCDCore.Project.Masks.RegularMask(txtName.Text, fiMask, cboField.Text, MaskItems.ToList<GCDCore.Project.Masks.MaskItem>());
                    ProjectManager.Project.Masks.Add(Mask);
                    ProjectManager.AddNewProjectItemToMap(Mask);
                }
                else
                {
                    Mask.Name = txtName.Text;
                }

                ProjectManager.Project.Save();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error creating regular mask.");
            }
        }

        private void cmdAddToMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddVectorToMap(Mask);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error adding directional mask to the map.");
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
