using ArcGIS.Core.Internal.CIM;
using System.IO;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class FileSystemDataset : BaseDataset
    {
        public override string Noun => throw new System.NotImplementedException();
        public enum GISDataStorageTypes
        {
            RasterFile,
            ShapeFile,
            FileGeodatase,
            CAD,
            PersonalGeodatabase,
            TIN,
            GeoPackage
        };

        public readonly GCDProject Project;
        public readonly FileSystemInfo Path;

        public override string ContextMenu => "FileSystemDataset";

        public FileSystemDataset(GCDProject project, string name, FileSystemInfo fsInfo, string image_Exists, string image_Missing, string id)
            : base(name, image_Exists, image_Missing, id)
        {
            Project = project;
            Path = fsInfo;
        }

        public FileSystemDataset(GCDProject project, XmlNode nodItem, string image_Exists, string image_Missing)
             : base(nodItem, image_Exists, image_Missing)
        {
            Path = project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText);
            Project = project;
        }

        public override bool Exists
        {
            get
            {
                if (Path.FullName.ToLower().Contains(".gpkg"))
                {
                    int index5 = Path.FullName.ToLower().LastIndexOf(".gpkg");
                    string sWorkspacePath = Path.FullName.Substring(0, index5 + 5);
                    return File.Exists(sWorkspacePath);
                }
                else
                {
                    return Path.Exists;
                }
            }
        }

        /// <summary>
        /// Derives the file system path of a workspace given any path
        /// </summary>
        /// <param name="sPath">Any path. Can be a folder (e.g. file geodatabase) or absolute path to a file.</param>
        /// <returns>The workspace path (ending with .gdb for file geodatabases) or the folder for file based data.</returns>
        /// <remarks>PGB 9 Sep 2011.</remarks>
        public DirectoryInfo WorkspacePath
        {
            get
            {
                string sWorkspacePath = string.Empty;

                switch (WorkspaceType)
                {
                    case GISDataStorageTypes.FileGeodatase:
                        int index = Path.FullName.ToLower().LastIndexOf(".gdb");
                        sWorkspacePath = Path.FullName.Substring(0, index + 4);
                        break;
                    case GISDataStorageTypes.GeoPackage:
                        int index5 = Path.FullName.ToLower().LastIndexOf(".gpkg");
                        sWorkspacePath = Path.FullName.Substring(0, index5 + 5);
                        break;
                    case GISDataStorageTypes.CAD:
                        index = Path.FullName.ToLower().LastIndexOf(".dxf");
                        sWorkspacePath = System.IO.Path.GetDirectoryName(Path.FullName.Substring(0, index));
                        break;
                    default:
                        sWorkspacePath = System.IO.Path.GetDirectoryName(Path.FullName);
                        break;
                }
                return new System.IO.DirectoryInfo(sWorkspacePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        /// <remarks>Note that the path that comes in may or may not have a dataset name on the end. So it
        /// may be the path to a directory, or end with .gdb if a file geodatabase or may have a slash and
        /// then the dataset name on the end.</remarks>
        public GISDataStorageTypes WorkspaceType
        {
            get
            {
                if (Path.FullName.ToLower().Contains(".gdb"))
                {
                    return GISDataStorageTypes.FileGeodatase;
                }
                else if (Path.FullName.ToLower().Contains(".gpkg"))
                {
                    return GISDataStorageTypes.GeoPackage;
                }
                else
                {
                    if (System.IO.Directory.Exists(Path.FullName))
                    {
                        return GISDataStorageTypes.TIN; // ESRI GRID (folder)
                    }
                    else
                    {
                        if (Path.FullName.ToLower().Contains(".dxf"))
                        {
                            return GISDataStorageTypes.CAD;
                        }
                        else if (Path.FullName.ToLower().Contains(".tif"))
                        {
                            return GISDataStorageTypes.RasterFile;
                        }
                        else if (Path.FullName.ToLower().Contains(".img"))
                        {
                            return GISDataStorageTypes.RasterFile;
                        }
                        else
                        {
                            return GISDataStorageTypes.ShapeFile;
                        }
                    }
                }
            }
        }
    }
}
