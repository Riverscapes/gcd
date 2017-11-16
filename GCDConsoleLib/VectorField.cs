using System;
using OSGeo.OGR;

namespace GCDConsoleLib
{
    public class VectorField
    {
        public string Name { get { return _fieldDef.GetName(); } }
        public int Precision { get { return _fieldDef.GetPrecision(); } }
        public GDalFieldType Type;

        private FieldDefn _fieldDef;
        public VectorField(FieldDefn fieldDef)
        {
            _fieldDef = fieldDef;
            Type = new GDalFieldType(_fieldDef.GetFieldType());
        }
    }
}
