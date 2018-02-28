using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.UserInterface
{
    interface IProjectItemForm
    {
        GCDCore.Project.GCDProjectItem GCDProjectItem { get; }
    }
}
