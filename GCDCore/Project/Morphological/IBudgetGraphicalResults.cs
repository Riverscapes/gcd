using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace GCDCore.Project.Morphological
{
    public interface IBudgetGraphicalResults
    {
        string Name { get; }

        Volume VolErosion { get; }
        Volume VolErosionErr { get; }

        Volume VolDeposition { get; }
        Volume VolDepositionErr { get; }
        
        /// <summary>
        /// This is cumulative volume change for budget seg
        /// and VolOut for morphological
        /// </summary>
        Volume SecondGraphValue { get; }
    }
}
