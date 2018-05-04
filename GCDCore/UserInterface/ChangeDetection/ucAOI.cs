using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using GCDCore.Project.Masks;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucAOI : UserControl
    {
        public event EventHandler AOIMask_Changed;

        public AOIMask AOIMask
        {
            get
            {
                if (cboAOI.SelectedItem is AOIMask)
                {
                    return cboAOI.SelectedItem as AOIMask;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the name of the currently selected AOI or empty string if none
        /// </summary>
        public string AOIName
        {
            get
            {
                if (cboAOI.SelectedItem is AOIMask)
                {
                    return ((AOIMask)cboAOI.SelectedItem).Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public ucAOI()
        {
            InitializeComponent();
        }

        private void ucAOI_Load(object sender, EventArgs e)
        {
            if (ProjectManager.Project == null)
                return;

            tTip.SetToolTip(cboAOI, "The area of interest used for the change detection. Choosing the intersection of the surfaces applies no area of interest.");

            // Add all the AOIs to the dropdown
            cboAOI.Items.Add(AOIMask.SurfaceDataExtentIntersection);
            ProjectManager.Project.Masks.Where(x => x is AOIMask).ToList<Mask>().ForEach(x => cboAOI.Items.Add(x));
            cboAOI.SelectedIndex = 0;
        }

        private void cboAOI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AOIMask_Changed != null)
            {
                AOIMask_Changed(sender, e);
            }
        }
    }
}
