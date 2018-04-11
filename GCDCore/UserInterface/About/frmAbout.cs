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

        private void AboutForm_Load(Object sender, EventArgs e)
        {
            Text = string.Format("About {0}", Properties.Resources.ApplicationNameLong);

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
                GCDException.HandleException(ex);
            }
        }

        private void lnkFrankPoulsen(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.essa.com/team/index.html#fp");
        }

        private void lnkPhilipBailey(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://northarrowresearch.com/north-arrow-research/people/");
        }

        private void lnkJoeWheaton(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Properties.Resources.PeopleJoeWheatonURL);
        }
    }
}