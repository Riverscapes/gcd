using System.IO;

namespace GCDViewer.ProjectTree
{
    public class ProjectDataset
    {
        public readonly GCDProject Project;
        public readonly string Name;
        public readonly FileSystemInfo FilePath; // TINs

        public ProjectDataset(GCDProject project, FileSystemInfo filePath, string name)
        {
            Project = project;
            FilePath = filePath;
            Name = name;
        }
    }
}
