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

        public string MethodString
        {
            get
            {
                switch (Method)
                {
                    case ThresholdMethods.MinLoD: return "Minimum Level of Detection";
                    case ThresholdMethods.Propagated: return "Propagated Error";
                    case ThresholdMethods.Probabilistic: return string.Format("Probabilistic{0}", BayesianUpdating ? " with Bayesian updating" : string.Empty);
                    default: return string.Empty;
                }
            }
        }

        public string ThresholdString
        {
            get
            {
                switch (Method)
                {
                    case ThresholdMethods.MinLoD: return string.Format("{0:0.00}{1}", Threshold, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
                    case ThresholdMethods.Probabilistic: return string.Format("{0:0.00}% Confidence Level", Threshold);
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
