﻿using System;
using System.Xml;
using System.IO;
using System.Linq;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public abstract class GCDProjectRasterItem : GCDProjectItem
    {
        public readonly Raster Raster;

        public GCDProjectRasterItem(string name, FileInfo rasterPath)
            : base(name)
        {
            Raster = new Raster(rasterPath);
        }

        public GCDProjectRasterItem(string name, Raster raster)
            : base(name)
        {
            Raster = raster;
        }

        public GCDProjectRasterItem(XmlNode nodItem)
            : base(nodItem)
        {
            Raster = new Raster(ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText));
        }

        public override void Delete()
        {
            // Get the folder
            DirectoryInfo dir = Raster.GISFileInfo.Directory;

            // Remove the raster from the ArcGIS map and then delete the dataset
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
                // Delete empty directory 
                if (dir.Exists && !Directory.EnumerateFileSystemEntries(dir.FullName).Any())
                {
                    dir.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error attempting to delete associated surface directory " + dir.FullName, ex);
            }
        }

        private void DeleteRaster(Raster raster)
        {
            try
            {
                // Raise the event to say that a GIS layer is about to be deleted.
                // This should bubble to ArcGIS so that the layer is removed from the ArcMap ToC
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(raster.GISFileInfo));

                // Delete the actual raster
                if (raster.GISFileInfo.Exists)
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
