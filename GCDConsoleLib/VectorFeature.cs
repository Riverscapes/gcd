using OSGeo.OGR;
using System;

namespace GCDConsoleLib
{
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
