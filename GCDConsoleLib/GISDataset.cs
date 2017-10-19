using System;
using System.IO;
using OSGeo.GDAL;

namespace GCDConsoleLib
{

    public abstract class GISDataset : IDisposable
    {
        public string FilePath { get; internal set; }
        public Projection Proj;

        public static bool FileExists(string fullPath)
        {
            return !String.IsNullOrEmpty(fullPath) && System.IO.File.Exists(fullPath);
        }
        public bool FileExists()
        {
            return !String.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath);
        }

        /// <summary>
        /// Each type is responsible for opening its own dataset methods. This is because creating a DS requires knowledge of that type. 
        /// </summary>
        public abstract void Create(bool leaveopen);
        public abstract void Open(bool write = false);
        public abstract void Copy(string destPath);
        public abstract void Delete();
        public abstract void Dispose();


        /// <summary>
        /// Load a dataset from a filepath
        /// </summary>
        /// <param name="sFilepath"></param>
        public GISDataset(string sFilepath)
        {
            FilePath = sFilepath;
        }

    }
}
