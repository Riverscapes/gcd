using System;
using GCDCore.Project;
using System.Windows.Forms;

namespace GCDCore.UserInterface.UtilityForms
{
    public class ucVectorInput : naru.ui.ucInput
    {
        public event BrowseVectorEventHandler BrowseVector;
        public delegate void BrowseVectorEventHandler(TextBox txtPath, naru.ui.PathEventArgs e);

        private GCDConsoleLib.GDalGeometryType.SimpleTypes m_GeometryType;
        private string m_sNoun;
        public string Noun { get { return m_sNoun; } }

        public GCDConsoleLib.Vector SelectedItem
        {
            get
            {
                if (Path is System.IO.FileInfo)
                {
                    return new GCDConsoleLib.Vector(Path);
                }
                else
                {
                    return null;
                }
            }
        }

        private GCDConsoleLib.GDalGeometryType.SimpleTypes GeometryType
        {
            get { return m_GeometryType; }
            set { m_GeometryType = value; }
        }

        public ucVectorInput()
        {
            BrowseFile += cmdBrowse_Click;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sNoun"></param>
        /// <param name="eBrowseType"></param>
        /// <remarks></remarks>
        public new void Initialize(string sNoun, GCDConsoleLib.GDalGeometryType.SimpleTypes eBrowseType)
        {
            m_sNoun = sNoun;
            GeometryType = eBrowseType;
        }

        public void cmdBrowse_Click(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                if (ProjectManager.IsArcMap)
                {
                    if (BrowseVector != null)
                    {
                        BrowseVector((TextBox)sender, e);
                    }
                }
                else
                {
                    naru.ui.Textbox.BrowseOpenVector(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "ShapeFile"));
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error browsing to raster");
            }
        }
    }
}