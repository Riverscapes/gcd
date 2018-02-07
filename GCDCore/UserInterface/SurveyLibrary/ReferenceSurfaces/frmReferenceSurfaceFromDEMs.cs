using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public partial class frmReferenceSurfaceFromDEMs : Form
    {
        public naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey> DEMSurveys;

        public frmReferenceSurfaceFromDEMs()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromDEMs_Load(object sender, EventArgs e)
        {
            // Add all the project DEM surveys to the list and then bind to checked listbox
            DEMSurveys = new naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey>(GCDCore.Project.ProjectManager.Project.DEMSurveys.Values.ToList<GCDCore.Project.DEMSurvey>());
            lstDEMs.DataSource = DEMSurveys;

            // ensure all DEMs are checked by default
            for (int i = 0; i < lstDEMs.Items.Count; i++)
                lstDEMs.SetItemChecked(i, true);
        }
    }
}
