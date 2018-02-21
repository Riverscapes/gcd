using System.Xml;

namespace GCDCore.Project
{
    public abstract class GCDProjectItem
    {
        public string Name { get; set; }

        // Inherited classes need to implement their own logic to determine if 
        // the item can be deleted
        public abstract bool IsItemInUse { get; }

        public override string ToString()
        {
            return Name;
        }

        public GCDProjectItem(string name)
        {
            Name = name;
        }

        public GCDProjectItem (XmlNode nodItem)
        {
            Name = nodItem.SelectSingleNode("Name").InnerText;
        }
    }
}