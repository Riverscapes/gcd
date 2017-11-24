using GCDCore.Project;
using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.UtilityForms
{
    public class ucRasterInput : naru.ui.ucInput
    {
        public string Noun { get; set; }

        public event BrowseRasterEventHandler BrowseRaster;
        public delegate void BrowseRasterEventHandler(System.Windows.Forms.TextBox txtPath, naru.ui.PathEventArgs e);
        public event SelectRasterFromArcMapEventHandler SelectRasterFromArcMap;
        public delegate void SelectRasterFromArcMapEventHandler(System.Windows.Forms.TextBox txtPath, naru.ui.PathEventArgs e);

        public new void Initialize(string sNoun, System.IO.FileInfo fiPath, bool bRequiredInput)
        {
            base.Initialize(fiPath, bRequiredInput);
            Noun = sNoun;
        }

        //Public Event RasterSelected(e As naru)

        public GCDConsoleLib.Raster SelectedItem
        {
            get
            {
                if (Path is System.IO.FileInfo)
                {
                    return new GCDConsoleLib.Raster(Path);
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

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error browsing to raster");
            }

        }


        public void cmdSelectRaster_Click(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                if (SelectRasterFromArcMap != null)
                {
                    SelectRasterFromArcMap((TextBox)sender, e);
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error selecting raster from ArcMap");
            }

        }
        public ucRasterInput()
        {
            SelectLayer += cmdSelectRaster_Click;
            BrowseFile += cmdBrowseRaster_Click;
        }

        //Public Overrides Function Validate() As Boolean

        //    If Not TypeOf SelectedItem Is GCDConsoleLib.Raster Then
        //        System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " to continue."), GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        //        Return False
        //    End If

        //    Return True

        //End Function

    }

}

