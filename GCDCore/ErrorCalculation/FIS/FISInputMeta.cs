using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISInputMeta
    {
        public string Name { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }

        public FISInputMeta(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Default constructor required for binding
        /// </summary>
        public FISInputMeta()
        {
        }
    }
}
