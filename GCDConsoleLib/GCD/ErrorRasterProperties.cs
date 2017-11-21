using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.GCD
{
    public class ErrorRasterProperties
    {
        public readonly double? UniformValue;
        public readonly Raster AssociatedSurface;

        public readonly FileInfo FISRuleFile;
        public readonly Dictionary<string, Raster> FISInputs;

        public ErrorRasterProperties(double uniformValue)
        {
            UniformValue = new double?(uniformValue);
        }

        public ErrorRasterProperties(Raster assoc)
        {
            AssociatedSurface = assoc;
        }

        public ErrorRasterProperties(FileInfo fisRuleFile, Dictionary<string, Raster> fisInputs)
        {
            FISRuleFile = fisRuleFile;
            FISInputs = fisInputs;
        }
    }
}
