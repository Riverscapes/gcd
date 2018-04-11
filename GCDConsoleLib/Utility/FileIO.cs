using System;
using System.IO;
using OSGeo.GDAL;
namespace GCDConsoleLib.Utility
{
    public struct FileHelpers
    {
        /// <summary>
        /// Check if the file is locked. Return false if it is. 
        /// Does not throw an exception
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Access"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string filepath, FileAccess myAccess = FileAccess.Read)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(filepath);
            try
            {
                stream = file.Open(FileMode.Open, myAccess, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Just a simple wrapper to use the GDAL file access enum
        /// </summary>
        /// <param name="file"></param>
        /// <param name="GdalAccess"></param>
        /// <returns></returns>
        public static bool IsGDALFileLocked(string filepath, Access GdalAccess = Access.GA_ReadOnly)
        {
            FileAccess myAccess = FileAccess.Read;
            if (GdalAccess == Access.GA_Update)
                myAccess = FileAccess.Write;

            return IsFileLocked(filepath, myAccess);
        }

    }
}
