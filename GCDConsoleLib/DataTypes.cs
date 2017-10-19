using System;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib
{
    /// <summary>
    /// Gdal Data Types are the type that is used by raster values
    /// </summary>
    public class GdalDataType
    {
        // We don't expose the actual type because it's just an enumerator
        internal DataType _origType;
        internal DataType GDalSubsetType { get { return Conversion.DataTypeToSubset(_origType); } }

        // Convertor Properties
        public string TypeName { get { return Enum.GetName(typeof(DataType), _origType); } }
        public string SimpleTypeName { get { return Enum.GetName(typeof(DataType), GDalSubsetType); } }
        public Type CSType { get { return Conversion.DataTypeToType(_origType); } }

        // Constructors to and from various types
        public GdalDataType(DataType origType) { _origType = origType; }
        public GdalDataType(GdalDataType origType) { _origType = origType._origType; }
        public GdalDataType(ref Raster rRaster) { _origType = rRaster.Datatype._origType; }
        public GdalDataType(Type csType) { _origType = Conversion.TypeToDatatype(csType); }

        // Equivalence Operators
        public bool Equals(GdalDataType otherType) { return _origType == otherType._origType; }
        public bool Equivalent(GdalDataType otherType) { return GDalSubsetType == otherType.GDalSubsetType; }

    }

    /// <summary>
    /// Gdal Field types are the types you use for Vector fields
    /// </summary>
    public class GDalFieldType
    {
        // We don't expose the actual type because it's just an enumerator
        internal FieldType _origType;

        // Convertor Properties
        public string TypeName { get { return Enum.GetName(typeof(FieldType), _origType); } }
        public Type CSType { get { return Conversion.FieldTypeToCSharpType(_origType); } }
        public new string ToString { get { return Enum.GetName(typeof(FieldType), _origType); } }

        // Constructors to and from various types
        public GDalFieldType(FieldType origFType) { _origType = origFType; }
        public GDalFieldType(GDalFieldType origFType) { _origType = origFType._origType; }
        public GDalFieldType(Type csType) { _origType = Conversion.CSharpTypeToFieldType(csType); }

        // Equivalence operators
        public bool Equals(GDalFieldType otherType) { return _origType == otherType._origType; }
    }

    /// <summary>
    /// Geometry Types is a helper class for the complex nuances of the WKBType
    /// </summary>
    public class GDalGeometryType
    {
        // We don't expose the actual type because it's just an enumerator
        internal wkbGeometryType _origType;

        // Constructors to and from various types
        public GDalGeometryType(wkbGeometryType origGType) { _origType = origGType; }
        public GDalGeometryType(GDalGeometryType origGType) { _origType = origGType._origType; }

        // Convertor Properties
        public string TypeName { get { return Enum.GetName(typeof(wkbGeometryType), _origType); } }

        // 
        public bool isMulti { get { return TypeName.Contains("Multi"); } }
        public bool has25D { get { return TypeName.EndsWith("25D"); } }
        public bool hasM { get { return hasZM || TypeName.EndsWith("M"); } }
        public bool hasZM { get { return TypeName.EndsWith("ZM");} }
        public bool hasZ { get { return hasZM || TypeName.EndsWith("Z"); } }

        // Simple enumerators to help us get a sense of the different types
        public enum SimpleTypes { Unknown, Point, LineString, Polygon, TIN, Curve, Other }

        public SimpleTypes SimpleType { get
            {
                SimpleTypes theType = SimpleTypes.Unknown;
                if (TypeName.Contains("Point"))  {  theType = SimpleTypes.Point; }
                else if (TypeName.Contains("LineString")) { theType = SimpleTypes.LineString;  }
                else if (TypeName.Contains("Polygon")) { theType = SimpleTypes.Polygon;  }
                else if (TypeName.Contains("TIN")) {  theType = SimpleTypes.TIN;  }
                else if (TypeName.Contains("Curve"))  {  theType = SimpleTypes.Curve; }
                return theType;
            } }
    }

}