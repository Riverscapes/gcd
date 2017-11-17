using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GCDCore.Project
{
    public class BudgetSegregation : GCDProjectItem
    {
        public readonly DoD DoD;
        public readonly DirectoryInfo Folder;

        public Dictionary<string, BudgetSegregationClass> Classes { get; internal set; }

        public BudgetSegregation(string name, DirectoryInfo folder, DoD dod)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            Classes = new Dictionary<string, BudgetSegregationClass>();
        }
    }
}
