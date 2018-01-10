using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDCore.UserInterface.ChangeDetection;
using GCDCore.UserInterface.ChangeDetection.MultiEpoch;

namespace GCDCore.Engines.DoD
{
    public class ChangeDetectionMultiEpoch
    {
        public readonly ThresholdProps Thresholds;
        public readonly List<Epoch> Epochs;

        private readonly Dictionary<ThresholdProps.ThresholdMethods, ChangeDetectionEngineBase> DoDEngines;

        public ChangeDetectionMultiEpoch(List<Epoch> lEpochs, ThresholdProps tProps)
        {
            Epochs = lEpochs;
            Thresholds = tProps;

            DoDEngines = new Dictionary<ThresholdProps.ThresholdMethods, ChangeDetectionEngineBase>();
        }

        public void Run(BackgroundWorker bgWorker)
        {

            foreach (Epoch currentEpoch in Epochs)
            {
                PerformDoD(currentEpoch, Thresholds);
            }

            ProjectManager.Project.Save();
        }

        private void PerformDoD(Epoch DoDEpoch, ThresholdProps tProps)
        {
            DEMSurvey NewDEM = DoDEpoch.NewDEM;
            DEMSurvey OldDEM = DoDEpoch.OldDEM;

            string dodName = frmDoDProperties.GetUniqueAnalysisName(NewDEM.Name, OldDEM.Name, tProps.ThresholdString);
            System.IO.DirectoryInfo dFolder = ProjectManager.OutputManager.GetDoDOutputFolder(dodName);

            ChangeDetectionEngineBase cdEngine = null;
            switch (tProps.Method)
            {
                case ThresholdProps.ThresholdMethods.MinLoD:
                    cdEngine = new ChangeDetectionEngineMinLoD(NewDEM, OldDEM, tProps.Threshold);
                    break;

                    /*
                case ThresholdProps.ThresholdMethods.Propagated:
                    cdEngine = new ChangeDetectionEnginePropProb(NewDEM, OldDEM, NewError, OldError);
                    break;

                case ThresholdProps.ThresholdMethods.Probabilistic:
                    cdEngine = new ChangeDetectionEngineProbabilistic(NewDEM, OldDEM, NewError, OldError, tProps.Threshold, tProps.SpatialCoherenceProps);
                    break;
                    */
            }

            DoDBase dod = cdEngine.Calculate(dodName, dFolder, true, ProjectManager.Project.Units);
            ProjectManager.Project.DoDs[dodName] = dod;
        }
    }
}
