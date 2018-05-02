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

        public ucAOI()
        {
            InitializeComponent();
        }

        private void ucAOI_Load(object sender, EventArgs e)
        {
            if (ProjectManager.Project == null)
                return;

            // Add all the AOIs to the dropdown
            cboAOI.Items.Add(AOIMask.SurfaceDataExtentIntersection);
            ProjectManager.Project.Masks.Where(x => x is AOIMask).ToList<Mask>().ForEach(x => cboAOI.Items.Add(x));
            cboAOI.SelectedIndex = 0;
        }
    }
}
