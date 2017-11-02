using System;
using System.IO;
using OSGeo.GDAL;

namespace GCDConsoleLib
{

    public abstract class GISDataset : IDisposable
    {
        public FileInfo GISFileInfo { get; internal set; }
        public Projection Proj;

        public static bool FileExists(FileInfo fullPath)
        {
            return !String.IsNullOrEmpty(fullPath.FullName) && fullPath.Exists;
        }
        public bool FileExists()
        {
            return GISFileInfo !=null && GISFileInfo.Exists;
        }

        /// <summary>
        /// Each type is responsible for opening its own dataset methods. This is because creating a DS requires knowledge of that type. 
        /// </summary>
        public abstract void Create(bool leaveopen);
        public abstract void Open(bool write = false);
        public abstract void Copy(FileInfo destPath);
        public abstract void Delete();
        public abstract void Dispose();

        public void RefreshFileInfo()
        {
            GISFileInfo.Refresh();
        }

        public GISDataset(){}

        /// <summary>
        /// Load a dataset from a filepath
        /// </summary>
        /// <param name="sFilepath"></param>
        public GISDataset(FileInfo sFilepath)
        {
            GISFileInfo = sFilepath;
        }

    }
}
