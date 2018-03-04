﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    public partial class frmRegionErrorProperty : Form
    {
        public ErrorSurfaceProperty ErrorSurProp { get { return ucErrProp.ErrSurfProperty; } }

        public frmRegionErrorProperty(string region, ErrorSurfaceProperty errProp, List<AssocSurface> assocs)
        {
            InitializeComponent();
            txtRegion.Text = region;

            ucErrProp.InitializeExisting(errProp, assocs);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ucErrProp.ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
