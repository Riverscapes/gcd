using OSGeo.OGR;

namespace GCDConsoleLib
{
    public class VectorFeature
    {
        public Feature _feat;
        public VectorFeature(ref Feature featDef)
        {
            _feat = featDef;
        }
    }
}
