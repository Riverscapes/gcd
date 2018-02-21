namespace GCDCore.Project
{
    public class GCDProjectItem
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public GCDProjectItem(string name)
        {
            Name = name;
        }
    }
}