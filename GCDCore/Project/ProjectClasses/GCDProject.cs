using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    class GCDProject : GCDProjectItem
    {
        public string Description { get; set; }
        public readonly System.IO.FileInfo ProjectFile;
        public readonly DateTime DateTimeCreated;
        public readonly string GCDVersion;
        public readonly int Precision;
        public GCDConsoleLib.GCD.UnitGroup Units { get; set; }

        public Dictionary<string, DEMSurvey> DEMSurveys { get; internal set; }
        public Dictionary<string, DoD> DoDs { get; internal set; }

        public GCDProject(string name, string description, System.IO.FileInfo projectFile,
            DateTime dtCreated, string gcdVersion, int nPrecision, GCDConsoleLib.GCD.UnitGroup units)
            : base(name)
        {
            Description = description;
            ProjectFile = projectFile;
            DateTimeCreated = dtCreated;
            GCDVersion = gcdVersion;
            Precision = nPrecision;
            Units = units;

            DEMSurveys = new Dictionary<string, DEMSurvey>();
            DoDs = new Dictionary<string, DoD>();
        }
    }
}
