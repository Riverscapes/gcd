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
        private Envelope _bounds;

        public VectorFeature(Feature featDef)
        {
            Feat = featDef;
            _bounds = null;
        }

        public Envelope Bounds
        {
            get {
                if (_bounds == null)
                {
                    Envelope tmpEnv = new Envelope();
                    Feat.GetGeometryRef().GetEnvelope(tmpEnv);
                    _bounds = tmpEnv;                    
                }
                return _bounds;
            }
        }

        public string GetFieldAsString(string fieldName)
        {
            return Feat.GetFieldAsString(fieldName);
        }
    }
}
