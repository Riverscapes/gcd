using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDCore.Project.Masks;
using GCDCore.UserInterface.ChangeDetection;
using GCDCore.UserInterface.ChangeDetection.MultiEpoch;

namespace GCDCore.Engines.DoD
{
    public class ChangeDetectionMultiEpoch
    {
        public readonly ThresholdProps Thresholds;
        public readonly List<Epoch> Epochs;
        public readonly AOIMask AOIMask;

        public Boolean Cancelled
        {
            get { return _Cancelled; }
        }
        private Boolean _Cancelled = false;



        private readonly Dictionary<ThresholdProps.ThresholdMethods, ChangeDetectionEngineBase> DoDEngines;

        public ChangeDetectionMultiEpoch(List<Epoch> lEpochs, Project.Masks.AOIMask aoi, ThresholdProps tProps)
        {
            Epochs = lEpochs;
            Thresholds = tProps;
            AOIMask = aoi;

            DoDEngines = new Dictionary<ThresholdProps.ThresholdMethods, ChangeDetectionEngineBase>();
        }

        public void Run(BackgroundWorker bgWorker)
        {

            foreach (Epoch currentEpoch in Epochs)
            {
                if (bgWorker.CancellationPending)
                {
                    _Cancelled = true;
                    break;
                }

                PerformDoD(currentEpoch, Thresholds);
            }

            ProjectManager.Project.Save();
        }

        private void PerformDoD(Epoch DoDEpoch, ThresholdProps tProps)
        {
            DEMSurvey NewDEM = DoDEpoch.NewDEM;
            DEMSurvey OldDEM = DoDEpoch.OldDEM;

            string dodName = frmDoDProperties.GetUniqueAnalysisName(NewDEM.Name, OldDEM.Name, tProps.ThresholdString);
            System.IO.DirectoryInfo dFolder = ProjectManager.Project.GetDoDFolder();

            ChangeDetectionEngineBase cdEngine = null;
            switch (tProps.Method)
            {
                case ThresholdProps.ThresholdMethods.MinLoD:
                    cdEngine = new ChangeDetectionEngineMinLoD(NewDEM, OldDEM, AOIMask, tProps.Threshold);
                    break;

                case ThresholdProps.ThresholdMethods.Propagated:
                    cdEngine = new ChangeDetectionEnginePropProb(NewDEM, OldDEM, DoDEpoch.NewDEMErrorSurface, DoDEpoch.OldDEMErrorSurface, AOIMask);
                    break;

                case ThresholdProps.ThresholdMethods.Probabilistic:
                    cdEngine = new ChangeDetectionEngineProbabilistic(NewDEM, OldDEM, AOIMask, DoDEpoch.NewDEMErrorSurface, DoDEpoch.OldDEMErrorSurface, tProps.Threshold, tProps.SpatialCoherenceProps);
                    break;
            }

            DoDBase dod = cdEngine.Calculate(dodName, dFolder, true, ProjectManager.Project.Units);
            ProjectManager.Project.DoDs[dodName] = dod;
        }
    }
}
