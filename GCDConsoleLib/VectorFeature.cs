using OSGeo.OGR;

namespace GCDConsoleLib
{
    public class VectorFeature
    {
        public Feature _feat;
        public VectorFeature(Feature featDef)
        {
            _feat = featDef;
        }
    }
}
