using System;
using System.IO;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class ProjectHistogram
    {
        public readonly FileInfo Path;
        private Histogram _Data;

        public Histogram Data
        {
            get
            {
                if (_Data == null)
                    _Data = new Histogram(Path);

                return _Data;
            }
        }

        public ProjectHistogram(FileInfo histogramPath)
        {
            Path = histogramPath;
        }

        public ProjectHistogram(FileInfo histogramPath, Histogram data)
        {
            Path = histogramPath;
            _Data = data;
        }
    }
}
