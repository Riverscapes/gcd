using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmErrorSurfaceProperties : Form
    {
        public readonly DEMSurvey DEM;
        public ErrorSurface ErrorSurf { get; internal set; }

        public frmErrorSurfaceProperties(DEMSurvey dem, ErrorSurface errorSurf)
        {
            InitializeComponent();
            DEM = dem;
            ErrorSurf = errorSurf;
        }
    }
}
