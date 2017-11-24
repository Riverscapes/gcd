using System;
using System.Windows.Forms;
using GCDCore.Project;
using System.Diagnostics;

namespace GCDCore.UserInterface.About
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(System.Object sender, System.EventArgs e)
        {
            Panel2.Controls.Add(new ucAcknowledgements());

            string sProduct = "Standalone";
            if (ProjectManager.IsArcMap)
            {
                sProduct = "Addin";
            }

            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            lblVersion.Text = string.Format("{0} {1}", sProduct, ass.GetName().Version);

            try
            {
                //lblRMVersion.Text = RasterManager.GetFileVersion().FileVersion.ToString()
                //lblGCDCoreVersion.Text = GCDCore.GetFileVersion().FileVersion.ToString()
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void LinkLabel8_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.essa.com/team/index.html#fp");
        }

        private void LinkLabel9_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.essa.com/team/index.html#no");
        }

        private void LinkLabel6_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://northarrowresearch.com/north-arrow-research/people/");
        }

        private void LinkLabel4_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.PeopleJoeWheatonURL);
        }
    }
}