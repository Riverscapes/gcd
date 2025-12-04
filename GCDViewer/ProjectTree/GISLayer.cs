using System.IO;

namespace GCDViewer.ProjectTree
{
    public class GISLayer : ProjectDataset
    {
        public readonly string SymbologyKey;

        public GISLayer(GCDProject project, FileInfo filePath, string name, string symbologyKey)
            : base(project, filePath, name)
        {
            SymbologyKey = symbologyKey;
        }
    }
}
