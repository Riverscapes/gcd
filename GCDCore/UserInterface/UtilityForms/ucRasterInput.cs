using GCDCore.Project;
using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.UtilityForms
{
    public class ucRasterInput : naru.ui.ucInput
    {
        public event BrowseRasterEventHandler BrowseRaster;
        public delegate void BrowseRasterEventHandler(System.Windows.Forms.TextBox txtPath, naru.ui.PathEventArgs e);

        new public void InitializeBrowseNew(string sNoun)
        {
            base.InitializeBrowseNew(sNoun);
            BrowseRaster += ProjectManager.OnBrowseRaster;
        }

        public void InitializeExisting(string sNoun, GCDConsoleLib.Raster raster)
        {
            base.InitializeExisting(sNoun, raster.GISFileInfo, ProjectManager.Project.GetRelativePath(raster.GISFileInfo));
        }

        public GCDConsoleLib.Raster SelectedItem
        {
            get
            {
                if (FullPath is System.IO.FileInfo)
                {
                    return new GCDConsoleLib.Raster(FullPath);
                }
                else
                {
                    return null;
                }
            }
        }

        public void cmdBrowseRaster_Click(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                if (ProjectManager.IsArcMap)
                {
                    if (BrowseRaster != null)
                    {
                        BrowseRaster((TextBox)sender, e);
                    }
                }
                else
                {
                    naru.ui.Textbox.BrowseOpenRaster(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"));
                }

                if (!string.IsNullOrEmpty(txtPath.Text) && System.IO.File.Exists(txtPath.Text))
                {
                    FullPath = new System.IO.FileInfo(txtPath.Text);
                }
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error browsing to raster");
            }
        }

        public ucRasterInput()
        {
            Browse += cmdBrowseRaster_Click;
        }
    }
}

