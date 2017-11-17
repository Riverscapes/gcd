using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
  public   class GCDProjectItem
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public GCDProjectItem(string name)
        {
            Name = name;
        }
    }
}
