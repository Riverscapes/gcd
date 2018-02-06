using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GCDConsoleTest.Utility
{
    interface ITempFile : IDisposable
    {
        FileInfo fInfo
        {
            get;
        }
    }

    class TempGeoJSONFile : ITempFile
    {
        public FileInfo fInfo
        {
            get;
            private set;
        }
        private bool dispose;

        public TempGeoJSONFile(string geojson, string filepath = null)
        {
            if (String.IsNullOrEmpty(geojson))
                throw new ArgumentException("geojson");

            string path;
            if (String.IsNullOrEmpty(filepath))
            {
                path = Path.GetTempFileName();
                dispose = true;
            }
            else
            {
                path = filepath;
                dispose = false;
            }

            fInfo = new FileInfo(path);
            using (StreamWriter sw = new StreamWriter(fInfo.FullName))
                sw.WriteLine(geojson.Replace("'","\""));
        }


        ~TempGeoJSONFile() { Dispose(false); }
        public void Dispose() { Dispose(true); }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            if (dispose && fInfo != null)
            {
                try { File.Delete(fInfo.FullName); }
                catch { } // best effort
                fInfo = null;
            }
        }
    }
}
