using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class FISInput
    {
        public string FISInputName { get; set; }
        public AssocSurface AssociatedSurface { get; set; }

        /// <summary>
        /// Default constructor needed for binding lists of these items to DataGridViews
        /// </summary>
        public FISInput()
        {

        }



        /// <summary>
        /// Constructor for UI when switching between FIS rule files and no assoc has been selected yet
        /// </summary>
        /// <param name="sFISInputName"></param>
        public FISInput(string sFISInputName)
        {
            FISInputName = sFISInputName;
        }

        /// <summary>
        /// Constructor for deserializing FIS items from project XML
        /// </summary>
        /// <param name="sFISInputName"></param>
        /// <param name="assoc"></param>
        public FISInput(string sFISInputName, AssocSurface assoc)
        {
            FISInputName = sFISInputName;
            AssociatedSurface = assoc;
        }
    }
}
