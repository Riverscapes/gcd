using System.IO;
using System.Collections.Generic;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class DoD : GCDProjectItem
    {
        public enum ThresholdingMethods
        {
            MinLoD,
            Propagated,
            Probabilistic
        }

        public readonly DirectoryInfo Folder;

        public readonly DEMSurvey NewDEM;
        public readonly DEMSurvey OldDEM;

        public readonly ErrorSurface NewErrorSurface;
        public readonly ErrorSurface OldErrorSurface;

        public ProjectRaster RawDoD { get; set; }
        public ProjectRaster ThrDoD { get; set; }

        public FileInfo RawHistogram { get; set; }
        public FileInfo ThrHistogram { get; set; }
        public FileInfo SummaryXML { get; set; }

        public readonly double? Threshold;
        public readonly ThresholdingMethods ThresholdingMethod;

        public readonly DoDStats Statistics;
        public readonly ChangeDetection.CoherenceProperties SpatCoProperties;

        public Dictionary<string, BudgetSegregation> BudgetSegregations { get; internal set; }

        public DoD(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM,
            ErrorSurface newError, ErrorSurface oldError, double? threshold,
            ThresholdingMethods method, DoDStats stats, ChangeDetection.CoherenceProperties props)
            : base(name)
        {
            Folder = folder;
            NewDEM = newDEM;
            OldDEM = oldDEM;
            NewErrorSurface = newError;
            OldErrorSurface = oldError;
            Threshold = threshold;
            ThresholdingMethod = method;
            Statistics = stats;
            SpatCoProperties = props;

            BudgetSegregations = new Dictionary<string, BudgetSegregation>();
        }
    }
}
