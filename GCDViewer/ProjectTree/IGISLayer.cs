
namespace GCDViewer.ProjectTree
{
    interface IGISLayer
    {
        string Name { get; }
        string GISPath { get; }
        string SymbologyKey { get; }
    }
}
