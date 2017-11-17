
namespace GCDCore.Project
{
    public class ErrorUniformProperties : GCDProjectItem
    {
        public readonly double ErrorValue;
        public readonly AssocSurface AssocSurface;

        public ErrorUniformProperties(string name, double errorValue, AssocSurface assoc)
            : base(name)
        {
            ErrorValue = errorValue;
            AssocSurface = assoc;
        }
    }
}
