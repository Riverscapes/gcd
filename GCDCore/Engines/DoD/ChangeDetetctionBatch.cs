using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDCore.UserInterface.ChangeDetection;

namespace GCDCore.Engines.DoD
{
    public class ChangeDetetctionBatch
    {
        public readonly List<BatchProps> Batches;

        public ChangeDetetctionBatch(List<BatchProps> batches)
        {
            Batches = batches;
        }

        public void Run(BackgroundWorker bgWorker)
        {
            Batches.ForEach(x => PerformDoD(x));
            ProjectManager.Project.Save();
        }

        private void PerformDoD(BatchProps props)
        {
            string aoiName = props.AOIMask is Project.Masks.AOIMask ? props.AOIMask.Name : string.Empty;
            string dodName = frmDoDProperties.GetUniqueAnalysisName(props.NewSurface.Name, props.OldSurface.Name, props.ThresholdProps.ThresholdString, aoiName);
            System.IO.DirectoryInfo dFolder = ProjectManager.Project.GetDoDFolder();

            ChangeDetectionEngineBase cdEngine = null;
            switch (props.ThresholdProps.Method)
            {
                case ThresholdProps.ThresholdMethods.MinLoD:
                    cdEngine = new ChangeDetectionEngineMinLoD(props.NewSurface, props.OldSurface, props.AOIMask, props.ThresholdProps.Threshold);
                    break;

                case ThresholdProps.ThresholdMethods.Propagated:
                    cdEngine = new ChangeDetectionEnginePropProb(props.NewSurface, props.OldSurface, props.NewError, props.OldError, props.AOIMask);
                    break;

                case ThresholdProps.ThresholdMethods.Probabilistic:
                    cdEngine = new ChangeDetectionEngineProbabilistic(props.NewSurface, props.OldSurface, props.AOIMask, props.NewError, props.OldError, props.ThresholdProps.Threshold, props.ThresholdProps.SpatialCoherenceProps);
                    break;
            }

            DoDBase dod = cdEngine.Calculate(dodName, dFolder, true, ProjectManager.Project.Units);
            ProjectManager.Project.DoDs.Add(dod);
        }
    }
}
