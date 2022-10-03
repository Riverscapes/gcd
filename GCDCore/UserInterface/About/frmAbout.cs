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
            webBrowser1.Url = new Uri("http://gcd.riverscapes.net/dotnetack.html");            
            lblName.Text = string.Format("{0} {1}", Properties.Resources.ApplicationNameShort, ProjectManager.IsArcMap ? "AddIn" : "Standalone");
            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void lnkJoeWheaton(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Properties.Resources.PeopleJoeWheatonURL);
        }

        private void lnkWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkWebSite.Text);
        }

        private void lnkOnlineHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkOnlineHelp.Text);
        }

        private void lnkIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkIssues.Text);
        }
    }
}