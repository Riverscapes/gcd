

namespace GCDViewer.ProjectTree
{
    public class GroupLayer: ITreeItem
    {
        public string Name { get; }
        public readonly bool Expanded;
        public readonly string Id;

        public string ContextMenu => "GroupLayer";

        public string ImagePath => "folder16.png";

        public GroupLayer(string label, bool collapse, string id)
        {
            Name = label;
            Expanded = !collapse;
            Id = id;
        }
    }
}
