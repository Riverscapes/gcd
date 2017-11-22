using System;
using System.IO;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class HistogramPair
    {
        public readonly HistogramDefinition Raw;
        public readonly HistogramDefinition Thr;

        /// <summary>
        /// Constructor for DoD engines when both the files and data are already loaded
        /// </summary>
        /// <param name="rawHist"></param>
        /// <param name="rawHistPath"></param>
        /// <param name="thrHist"></param>
        /// <param name="thrHistPath"></param>
        public HistogramPair(Histogram rawHist, FileInfo rawHistPath, Histogram thrHist, FileInfo thrHistPath)
        {
            Raw = new HistogramDefinition(rawHistPath, rawHist);
            Thr = new HistogramDefinition(thrHistPath, thrHist);
        }

        /// <summary>
        /// Constructor for XML deserialization when only the file paths are known
        /// </summary>
        /// <param name="rawHistPath"></param>
        /// <param name="thrHistPath"></param>
        public HistogramPair(FileInfo rawHistPath, FileInfo thrHistPath)
        {
            Raw = new HistogramDefinition(rawHistPath);
            Thr = new HistogramDefinition(thrHistPath);
        }

        public class HistogramDefinition
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

            public HistogramDefinition(FileInfo histogramPath)
            {
                Path = histogramPath;
            }

            public HistogramDefinition(FileInfo histogramPath, Histogram data)
            {
                Path = histogramPath;
                _Data = data;
            }
        }
    }
}
