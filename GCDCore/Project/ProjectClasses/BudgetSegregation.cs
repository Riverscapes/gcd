using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class BudgetSegregation : GCDProjectItem
    {
        public readonly DoD DoD;
        public readonly System.IO.DirectoryInfo Folder;

        public Dictionary<string, GCDConsoleLib.GCD.DoDStats> Statistics { get; internal set; }

        public BudgetSegregation(string name,System.IO.DirectoryInfo folder, DoD dod)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            Statistics = new Dictionary<string, GCDConsoleLib.GCD.DoDStats>();
        }
    }
}
