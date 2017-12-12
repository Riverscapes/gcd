using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.About
{
    public partial class ucAcknowledgements : UserControl
    {
        public ucAcknowledgements()
        {
            InitializeComponent();
        }

        private void ucAcknowledgements_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                AddLink(lnkNSF, "Award #1226127", "http://www.nsf.gov/awardsearch/showAward?AWD_ID=1226127");
                AddLink(lnkNSF, "ZCloud Tools", "http://zcloudtools.boisestate.edu");

                lnkELR.Links.Clear();
                AddLink(lnkELR, "Eco Logical Research", "https://www.eco-logical-research.com/");

                lnkGCMRC.Links.Add(2, lnkGCMRC.Text.Length - 2, "http://www.gcmrc.gov/gcmrc.aspx");
                lnkUSACE.Links.Add(2, lnkUSACE.Text.Length - 2, "http://www.nwk.usace.army.mil/");

                lnkICRRR.Links.Clear();
                AddLink(lnkICRRR, "ICRRR", "https://www.cnr.usu.edu/icrrr/");

                AddLink(lnkjDevelopers, "Joe Wheaton", "http://gcd.joewheaton.org/");
                AddLink(lnkjDevelopers, "Utah State University Department of Watershed Sciences", "http://www.cnr.usu.edu/wats/");
                AddLink(lnkjDevelopers, "James Brasington", "http://www.geog.qmul.ac.uk/staff/brasingtonj.html");
                AddLink(lnkjDevelopers, "Queen Mary University", "http://www.geog.qmul.ac.uk/staff/brasingtonj.html");
                AddLink(lnkjDevelopers, "North Arrow Research", "http://northarrowresearch.com/");
                AddLink(lnkjDevelopers, "ET-AL", "http://etal.joewheaton.org/");
                AddLink(lnkjDevelopers, "Philip Bailey", "http://northarrowresearch.com/people/");
                AddLink(lnkjDevelopers, "James Hensleigh", "http://etal.joewheaton.org/people/researchers-technicians/james-hensleigh");
                AddLink(lnkjDevelopers, "Frank Poulsen", "http://essa.com/about-essa/our-team/frank-poulsen/");
                AddLink(lnkjDevelopers, "ESSA", "http://essa.com/");
                AddLink(lnkjDevelopers, "Chris Gerrard", "http://www.gis.usu.edu/~chrisg/");
                AddLink(lnkjDevelopers, "RSGIS Lab", "http://www.gis.usu.edu/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AddLink(LinkLabel lnk, string sText, string sURL)
        {
            int nStart = lnk.Text.IndexOf(sText);
            int nLength = sText.Length;
            //lnkUSACE.Links.Add(nStart, nLength, sURL)

            // do proper validation and add only proper links
            // this will help you to avoid the exception 
            if (!string.IsNullOrEmpty(sText) && nStart >= 0 && (nStart + nLength <= lnk.Text.Length))
            {
                LinkLabel.Link link = new LinkLabel.Link();
                //link.Description = textDescription.Text.ToString()
                link.LinkData = sURL;
                link.Name = sText;
                //link.Enabled = checkBoxEnabled.Checked
                //link.Visited = checkBoxVisited.Checked
                link.Start = nStart;
                link.Length = nLength;
                try
                {
                    lnk.Links.Add(link);
                }
                catch (InvalidOperationException exception)
                {
                    // links can't overlap 
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Displays the appropriate link based on the value of the LinkData property of the Link object. 
            if (e.Link.LinkData != null)
            {
                string target = e.Link.LinkData.ToString();
                if (!string.IsNullOrEmpty(target))
                    System.Diagnostics.Process.Start(target);
            }
        }
    }
}