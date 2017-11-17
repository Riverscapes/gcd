using System.IO;

namespace GCDCore.Project
{
    public class ErrorFISProperties : GCDProjectItem
    {
        public readonly AssocSurface AssocSurface;
        public readonly string FISInputName;
        public readonly FileInfo FISInputFile;

        public ErrorFISProperties(string name, AssocSurface assoc, string fisInputName, FileInfo fisInputFile)
            : base(name)
        {
            AssocSurface = assoc;
            FISInputName = fisInputName;
            FISInputFile = fisInputFile;
        }
    }
}
