using System.Xml;

namespace GCDCore.Project
{
    public abstract class GCDProjectItem
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
    }
}