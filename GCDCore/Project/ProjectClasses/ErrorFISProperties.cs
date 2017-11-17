using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class ErrorFISProperties : GCDProjectItem
    {
        public readonly AssocSurface AssocSurface;
        public readonly string FISInputName;
        public readonly System.IO.FileInfo FISInputFile;
        
        public ErrorFISProperties(string name, AssocSurface assoc, string fisInputName,
            System.IO.FileInfo fisInputFile)
            : base(name)
        {
            AssocSurface = assoc;
            FISInputName = fisInputName;
            FISInputFile = fisInputFile;
        }
    }
}
