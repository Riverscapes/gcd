using System;
using System.IO;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class GCDProjectRasterItem : GCDProjectItem
    {
        public readonly Raster Raster;
  
        public GCDProjectRasterItem(string name, FileInfo rasterPath)
            : base(name)
        {
            Raster = new Raster(rasterPath);
        }

        public void Delete()
        {
            // Get the folder
            DirectoryInfo dir = Raster.GISFileInfo.Directory;

            // Remove the raster from the ArcGIS map
            try
            {
                DeleteRaster(Raster);
            }
            catch (Exception ex)
            {
                Console.Write("Error attempting to remove raster from ArcGIS " + Raster.GISFileInfo.FullName, ex);
            }

            try
            {
                // Delete the raster itself
                Raster.Delete();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error deleting associated surface raster.", ex);
                ex2.Data["Associated Surface Name"] = Name;
                ex2.Data["Raster Path"] = Raster.GISFileInfo.FullName;
                throw ex2;
            }

            try
            {
                // Delete the directory 
                dir.Delete();
            }
            catch (Exception ex)
            {
                Console.Write("Error attempting to delete associated surface directory " + dir.FullName, ex);
            }
        }

        public static void DeleteRaster(Raster raster)
        {
            try
            {
                // Raise the event to say that a GIS layer is about to be deleted.
                // This should bubble to ArcGIS so that the layer is removed from the ArcMap ToC
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(raster.GISFileInfo));

                // Delete the actual raster
                raster.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error attempting to delete DoD raster " + raster.GISFileInfo.FullName);
                Console.WriteLine("Raster Path: ", raster.GISFileInfo.FullName);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
