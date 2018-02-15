using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project.Masks;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmDirectionalMaskProps : Form
    {
        public DirectionalMask Mask { get; internal set; }

        public frmDirectionalMaskProps(DirectionalMask mask = null)
        {
            InitializeComponent();
            Mask = mask;
        }

        private void frmDirectionalMaskProps_Load(object sender, EventArgs e)
        {
            if (Mask != null)
            {
                txtName.Text = Mask.Name;

                ucPolygon.Initialize("Directional Mask", Mask._ShapeFile, true);
            }

            ucPolygon.ReadOnly = Mask != null;

            // subscribe to the even when the user changes the input ShapeFile
            ucPolygon.PathChanged += InputShapeFileChanged;

            UpdateControls(sender, e);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }



        }

        private bool ValidateForm()
        {



            return true;
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            cboLabel.Enabled = chkLabel.Checked;
            cboDistance.Enabled = chkDistance.Checked;
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
            cboLabel.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            cboDirection.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.IntField)).ToList<GCDConsoleLib.VectorField>();
            cboDistance.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.RealField)).ToList<GCDConsoleLib.VectorField>();
            Cursor = Cursors.Default;
        }
    }
}
