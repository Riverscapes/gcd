using System;
using System.IO;
using System.Collections.Generic;

namespace GCDConsoleLib.GCD
{
    public class ErrorRasterProperties
    {
        public enum ERPType { FIS, UNIFORM, ASSOC }

        public readonly ERPType TheType;
        public readonly decimal? UniformValue;
        public readonly Raster AssociatedSurface;

        public readonly FileInfo FISRuleFile;
        public readonly Dictionary<string, Raster> FISInputs;

        public ErrorRasterProperties(decimal uniformValue)
        {
            UniformValue = new decimal?(uniformValue);
            TheType = ERPType.UNIFORM;
        }

        public ErrorRasterProperties(Raster assoc)
        {
            AssociatedSurface = assoc;
            TheType = ERPType.ASSOC;
        }

        public ErrorRasterProperties(FileInfo fisRuleFile, Dictionary<string, Raster> fisInputs)
        {
            FISRuleFile = fisRuleFile;
            FISInputs = fisInputs;
            TheType = ERPType.FIS;
        }
    }
}
