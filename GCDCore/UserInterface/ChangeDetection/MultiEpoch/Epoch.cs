using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
namespace GCDCore.UserInterface.ChangeDetection.MultiEpoch
{
    public class Epoch
    {
        public bool IsActive { get; set; }
        public DEMSurvey NewDEM;
        public DEMSurvey OldDEM;
        public ErrorSurface NewDEMErrorSurface;
        public ErrorSurface OldDEMErrorSurface;

        public string NewDEMName { get { return NewDEM.Name; } }
        public string OldDEMName { get { return OldDEM.Name; } }

        public Epoch(DEMSurvey newDEM, ErrorSurface newDEMErrorSurface, DEMSurvey oldDEM, ErrorSurface oldDEMErrorSurface)
        {
            NewDEM = newDEM;
            NewDEMErrorSurface = newDEMErrorSurface;
            OldDEM = oldDEM;
            OldDEMErrorSurface = oldDEMErrorSurface;
            IsActive = true;
        }
    }
}
