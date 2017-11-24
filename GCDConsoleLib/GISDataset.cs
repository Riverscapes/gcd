﻿using System;
using System.IO;
using OSGeo.GDAL;

namespace GCDConsoleLib
{
    /// <summary>
    /// The GISDataset class is the base below Raster and Vector
    /// </summary>
    public abstract class GISDataset : IDisposable
    {
        public FileInfo GISFileInfo { get; internal set; }
        public Projection Proj;

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

        /// <summary>
        /// FileInfo does not refresh until you tell it to. Use this when you create or destroy a file
        /// </summary>
        public void RefreshFileInfo(){  GISFileInfo.Refresh();  }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public GISDataset(){}

        /// <summary>
        /// Load a dataset from a filepath
        /// </summary>
        /// <param name="sFilepath"></param>
        public GISDataset(FileInfo sFilepath)  { GISFileInfo = sFilepath;  }

    }
}
