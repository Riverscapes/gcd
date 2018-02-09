using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmMaskProperties : Form
    {
        public readonly naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem> MaskItems;

        public frmMaskProperties()
        {
            InitializeComponent();
            MaskItems = new naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem>();
        }

        private void frmMaskProperties_Load(object sender, EventArgs e)
        {
            // subscribe to the even when the user changes the input ShapeFile
            ucPolygon.PathChanged += InputShapeFileChanged;

            grdData.DataSource = MaskItems;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = GCDCore.Project.ProjectManager.Project.GetRelativePath(GCDCore.Project.ProjectManager.OutputManager.GetMaskShapeFilePath(txtName.Text));
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
                txtName.Text = naru.os.File.RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName));
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
                if (!MaskItems.Any<GCDCore.Project.Masks.MaskItem>(x => string.Compare(x.FieldValue, fieldValue, true) == 0))
                {
                    MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, fieldValue, fieldValue));
                }
            }
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
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the mask.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (GCDCore.Project.ProjectManager.Project.is)




            return true;
        }


    }
}
