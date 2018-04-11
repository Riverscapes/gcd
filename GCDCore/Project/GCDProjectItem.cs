using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GCDCore.Project
{
    public abstract class GCDProjectItem : IComparable<GCDProjectItem>
    {
        public string Name { get; set; }

        // This is the type of thing being represented that must be implemented by all inherited types
        public abstract string Noun { get; }

        // Inherited classes need to implement their own logic to determine if the item can be deleted
        public abstract bool IsItemInUse { get; }

        // GCD project items need to implement their own method of deleting all files on disk
        public abstract void Delete();

        public override string ToString()
        {
            return Name;
        }

        public GCDProjectItem(string name)
        {
            Name = name;
        }

        public GCDProjectItem(XmlNode nodItem)
        {
            Name = nodItem.SelectSingleNode("Name").InnerText;
        }

        public int CompareTo(GCDProjectItem item)
        {
            return string.Compare(Name, item.Name);
        }

        protected void CheckFilesInUse(DirectoryInfo dir)
        {
            List<string> result = new List<string>();
            foreach (FileInfo aFile in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                foreach (System.Diagnostics.Process proc in naru.os.FileUtil.WhoIsLocking(aFile.FullName))
                {
                    if (GCDConsoleLib.Utility.FileHelpers.IsFileLocked(aFile.FullName, FileAccess.ReadWrite))
                    {
                        if (string.Compare(proc.ProcessName.ToLower(), "ArcMap", true) != 0)
                        {
                            result.Add(string.Format("{0} ({1})", ProjectManager.Project.GetRelativePath(aFile), proc.ProcessName));
                        }
                    }
                }
            }

            if (result.Count > 0)
            {
                IOException ex = new IOException("File Locks");
                ex.Data["Locks"] = string.Join(Environment.NewLine, result.ToArray());
                throw ex;
            }
        }
    }
}