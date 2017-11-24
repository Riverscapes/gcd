using OSGeo.OGR;
using System;

namespace GCDConsoleLib
{
    /// <summary>
    /// This is a simple abstraction on GDAL's Feature class
    /// so that you can use Vector without needing to load GDAL
    /// </summary>
    public class VectorFeature
    {
        public Feature Feat;

        public VectorFeature(Feature featDef)
        {
            Feat = featDef;
        }

        public string GetFieldAsString(string fieldName)
        {
            return Feat.GetFieldAsString(fieldName);
        }
    }
}
