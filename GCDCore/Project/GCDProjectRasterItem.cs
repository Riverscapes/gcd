using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public  void Delete()
        {
            // Get the folder
            DirectoryInfo dir = Raster.GISFileInfo.Directory;

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
    }
}
