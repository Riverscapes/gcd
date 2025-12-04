using System;


namespace GCDViewer.ProjectTree
{
    /// <summary>
    /// The interface that all items that appear in the tree must expose.
    /// This is the interface that TreeViewItemModel uses to keep track of
    /// the items in the tree.
    /// </summary>
    public interface ITreeItem
    {
        public string Name { get; }
        public string ImagePath { get; }

    }
}
