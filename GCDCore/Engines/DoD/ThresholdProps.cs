using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.Engines.DoD
{
    public class ThresholdProps
    {
        public enum ThresholdMethods
        {
            MinLoD,
            Propagated,
            Probabilistic
        }

        public ThresholdMethods Method { get; set; }
        public decimal Threshold { get; set; }
        public bool BayesianUpdating { get; set; }
        public readonly GCDCore.Project.CoherenceProperties SpatialCoherenceProps;

        public override string ToString()
        {
            return ThresholdString;
        }

        public string ThresholdString
        {
            get
            {
                switch (Method)
                {
                    case ThresholdMethods.Propagated: return "Propagated";
                    case ThresholdMethods.MinLoD: return string.Format("MinLoD at {0:0.00}{1}", Threshold, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
                    case ThresholdMethods.Probabilistic: return string.Format("Probabilistic at {0:0.00} Confidence Level{1}", Threshold, SpatialCoherenceProps == null ? "" : " Spat Co.");
                    default: return string.Empty;
                }

            }
        }

        public ThresholdProps(decimal minLoD)
        {
            Threshold = minLoD;
            Method = ThresholdMethods.MinLoD;
        }

        // Default constructor needed for UI binding
        public ThresholdProps()
        {
            Method = ThresholdMethods.Propagated;
        }

        public ThresholdProps(decimal ConfidenceLevel, GCDCore.Project.CoherenceProperties spatCoProps)
        {
            Method = ThresholdMethods.Probabilistic;
            Threshold = ConfidenceLevel;
            BayesianUpdating = spatCoProps != null;
            SpatialCoherenceProps = spatCoProps;
        }
    }
}
