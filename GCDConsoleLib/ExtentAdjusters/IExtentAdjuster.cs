using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.ExtentAdjusters
{
    public interface IExtentAdjuster
    {
        ExtentRectangle SrcExtent { get; }
        ExtentRectangle OutExtent { get; }
        ushort Precision { get; }
        bool RequiresResampling { get; }

        IExtentAdjuster AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left);
        IExtentAdjuster AdjustCellSize(decimal cellSize);
        IExtentAdjuster AdjustPrecision(ushort precision);
    }
}
