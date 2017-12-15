using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
namespace GCDCore.UserInterface.ChangeDetection.Intercomparison
{
    public class Epoch
    {
        public DEMSurvey NewDEM;
        public DEMSurvey OldDEM;

        public string NewDEMName { get { return NewDEM.Name; } }
        public string OldDEMName { get { return OldDEM.Name; } }

        public Epoch(DEMSurvey newDEM, DEMSurvey oldDEM)
        {
            NewDEM = newDEM;
            OldDEM = oldDEM;
        }
    }
}
