using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDViewer.ProjectTree
{
    /// <summary>
    /// Both projects and layers can have MetaData
    /// </summary>
    public interface IMetadata
    {
        public Dictionary<string, string> Metadata { get; }
    }
}
