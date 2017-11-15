using System;
using System.Collections;
using OSGeo.OGR;
using OSGeo.GDAL;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.Utility
{
    public struct Conversion
    {
        public static DataType DataTypeToSubset(DataType dType)
        {
            DataType theType = dType;
            switch (dType)
            {
                case DataType.GDT_Byte:
                    theType = DataType.GDT_Byte;
                    break;
                case DataType.GDT_UInt16:
                    theType = DataType.GDT_Int32;
                    break;
                case DataType.GDT_Int16:
                    theType = DataType.GDT_Int32;
                    break;
                case DataType.GDT_UInt32:
                    theType = DataType.GDT_Int32;
                    break;
                case DataType.GDT_Int32:
                    theType = DataType.GDT_Int32;
                    break;
                case DataType.GDT_Float32:
                    theType = DataType.GDT_Float32;
                    break;
                case DataType.GDT_Float64:
                    theType = DataType.GDT_Float64;
                    break;
                //We don't support complex types
                //case DataType.GDT_CInt16:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CInt32:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CFloat32:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CFloat64:
                //    theType = typeof(Byte);
                //    break;
                default:
                    throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dType));
            }
            return theType;
        }
        public static Type DataTypeToType(DataType dType)
        {
            Type theType = typeof(float);
            switch (dType)
            {
                case DataType.GDT_Byte:
                    theType = typeof(Byte);
                    break;
                case DataType.GDT_UInt16:
                    theType = typeof(Int32);
                    break;
                case DataType.GDT_Int16:
                    theType = typeof(Int32);
                    break;
                case DataType.GDT_UInt32:
                    theType = typeof(Int32);
                    break;
                case DataType.GDT_Int32:
                    theType = typeof(Int32);
                    break;
                case DataType.GDT_Float32:
                    theType = typeof(float);
                    break;
                case DataType.GDT_Float64:
                    theType = typeof(double);
                    break;
                // We don't support complex types
                //case DataType.GDT_CInt16:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CInt32:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CFloat32:
                //    theType = typeof(Byte);
                //    break;
                //case DataType.GDT_CFloat64:
                //    theType = typeof(Byte);
                //    break;
                default:
                    throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dType));
            }
            return theType;
        }
        public static DataType CSharpTypeToDataType(Type theType)
        {
            DataType dType = OSGeo.GDAL.DataType.GDT_Float32;
            if (theType == typeof(Byte))
            {
                dType = DataType.GDT_Byte;
            }
            else if (theType == typeof(Int32))
            {
                dType = DataType.GDT_Int32;
            }
            // Let's keep our ints to a minimum
            //else if (theType == typeof(int))
            //{
            //    dType = DataType.GDT_Int16;
            //}
            //else if (theType == typeof(int))
            //{
            //    dType = DataType.GDT_UInt32;
            //}
            //else if (theType == typeof(int))
            //{
            //    dType = DataType.GDT_Int32;
            //}
            else if (theType == typeof(float))
            {
                dType = DataType.GDT_Float32;
            }
            else if (theType == typeof(double))
            {
                dType = DataType.GDT_Float64;
            }
            else
            {
                throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dType));
            }
            return dType;
        }


        /// <summary>
        /// The rest of these methods are static so they can be useful in other places.
        /// </summary>

        public static Type FieldTypeToCSharpType(FieldType ftType)
        {
            Type theType = typeof(int);
            switch (ftType)
            {
                case FieldType.OFTInteger:
                    theType = typeof(int);
                    break;
                case FieldType.OFTInteger64:
                    theType = typeof(int);
                    break;
                case FieldType.OFTIntegerList:
                    theType = typeof(List<int>);
                    break;
                case FieldType.OFTInteger64List:
                    theType = typeof(List<int>);
                    break;
                case FieldType.OFTReal:
                    theType = typeof(double);
                    break;
                case FieldType.OFTRealList:
                    theType = typeof(List<double>);
                    break;
                case FieldType.OFTString:
                    theType = typeof(string);
                    break;
                case FieldType.OFTStringList:
                    theType = typeof(List<string>);
                    break;
                case FieldType.OFTWideString:
                    theType = typeof(string);
                    break;
                case FieldType.OFTWideStringList:
                    theType = typeof(List<string>);
                    break;
                case FieldType.OFTDate:
                    theType = typeof(DateTime);
                    break;
                case FieldType.OFTDateTime:
                    theType = typeof(DateTime);
                    break;
                case FieldType.OFTTime:
                    theType = typeof(DateTime);
                    break;
                default:
                    //OFTBinary = 8,
                    throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), theType));
            }
            return theType;


        }

        public static FieldType CSharpTypeToFieldType(Type theType)
        {
            FieldType dFType = FieldType.OFTReal;
            if (theType == typeof(Int64))
            {
                dFType = FieldType.OFTInteger64;
            }
            else if (theType == typeof(List<Int64>))
            {
                dFType = FieldType.OFTInteger64List;
            }
            else if (theType == typeof(Int32))
            {
                dFType = FieldType.OFTInteger;
            }
            else if (theType == typeof(List<Int32>))
            {
                dFType = FieldType.OFTIntegerList;
            }
            else if (theType == typeof(Int16))
            {
                dFType = FieldType.OFTInteger;
            }
            else if (theType == typeof(List<Int16>))
            {
                dFType = FieldType.OFTIntegerList;
            }
            else if (theType == typeof(DateTime))
            {
                dFType = FieldType.OFTDateTime;
            }
            else if (theType == typeof(string))
            {
                dFType = FieldType.OFTString;
            }
            else if (theType == typeof(List<string>))
            {
                dFType = FieldType.OFTStringList;
            }

            else if (theType == typeof(string))
            {
                dFType = FieldType.OFTString;
            }
            else if (theType == typeof(List<string>))
            {
                dFType = FieldType.OFTStringList;
            }

            else if (theType == typeof(double) || theType == typeof(float))
            {
                dFType = FieldType.OFTReal;
            }
            else if (theType == typeof(List<double>) || theType == typeof(List<float>))
            {
                dFType = FieldType.OFTRealList;
            }
            else
            {
                //OFTBinary = 8,
                throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dFType));
            }
            return dFType;

        }


        public static VolumeUnit AreaUnit2VolumeUnit(AreaUnit aUnit)
        {
            VolumeUnit retVal;
            switch (aUnit)
            {
                case (AreaUnit.SquareInch):
                    retVal = VolumeUnit.CubicInch;
                    break;
                case (AreaUnit.SquareFoot):
                    retVal = VolumeUnit.CubicFoot;
                    break;
                case (AreaUnit.SquareMile):
                    retVal = VolumeUnit.CubicMile;
                    break;
                case (AreaUnit.SquareDecimeter):
                    retVal = VolumeUnit.CubicDecimeter;
                    break;
                case (AreaUnit.SquareMicrometer):
                    retVal = VolumeUnit.CubicMicrometer;
                    break;
                case (AreaUnit.SquareMillimeter):
                    retVal = VolumeUnit.CubicMillimeter;
                    break;
                case (AreaUnit.SquareCentimeter):
                    retVal = VolumeUnit.CubicCentimeter;
                    break;
                case (AreaUnit.SquareMeter):
                    retVal = VolumeUnit.CubicMeter;
                    break;
                case (AreaUnit.SquareKilometer):
                    retVal = VolumeUnit.CubicKilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");                   
            }
            return retVal;
        }

        public static AreaUnit VolumeUnit2AreaUnit(VolumeUnit vUnit)
        {
            AreaUnit retVal;
            switch (vUnit)
            {
                case (VolumeUnit.CubicInch):
                    retVal = AreaUnit.SquareInch;
                    break;
                case (VolumeUnit.CubicFoot):
                    retVal = AreaUnit.SquareFoot;
                    break;
                case (VolumeUnit.CubicMile):
                    retVal = AreaUnit.SquareMile;
                    break;
                case (VolumeUnit.CubicDecimeter):
                    retVal = AreaUnit.SquareDecimeter;
                    break;
                case (VolumeUnit.CubicMicrometer):
                    retVal = AreaUnit.SquareMicrometer;
                    break;
                case (VolumeUnit.CubicMillimeter):
                    retVal = AreaUnit.SquareMillimeter;
                    break;
                case (VolumeUnit.CubicCentimeter):
                    retVal = AreaUnit.SquareCentimeter;
                    break;
                case (VolumeUnit.CubicMeter):
                    retVal = AreaUnit.SquareMeter;
                    break;
                case (VolumeUnit.CubicKilometer):
                    retVal = AreaUnit.SquareKilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");
            }
            return retVal;
        }

        public static LengthUnit AreaUnit2LengthUnit(AreaUnit lUnit)
        {
            LengthUnit retVal;
            switch (lUnit)
            {
                case (AreaUnit.SquareInch):
                    retVal = LengthUnit.Inch;
                    break;
                case (AreaUnit.SquareFoot):
                    retVal = LengthUnit.Foot;
                    break;
                case (AreaUnit.SquareMile):
                    retVal = LengthUnit.Mile;
                    break;
                case (AreaUnit.SquareDecimeter):
                    retVal = LengthUnit.Decimeter;
                    break;
                case (AreaUnit.SquareMicrometer):
                    retVal = LengthUnit.Micrometer;
                    break;
                case (AreaUnit.SquareMillimeter):
                    retVal = LengthUnit.Millimeter;
                    break;
                case (AreaUnit.SquareCentimeter):
                    retVal = LengthUnit.Centimeter;
                    break;
                case (AreaUnit.SquareMeter):
                    retVal = LengthUnit.Meter;
                    break;
                case (AreaUnit.SquareKilometer):
                    retVal = LengthUnit.Kilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");
            }
            return retVal;
        }

        public static AreaUnit LengthUnit2AreaUnit(LengthUnit aUnit)
        {
            AreaUnit retVal;
            switch (aUnit)
            {
                case (LengthUnit.Inch):
                    retVal = AreaUnit.SquareInch;
                    break;
                case (LengthUnit.Foot):
                    retVal = AreaUnit.SquareFoot;
                    break;
                case (LengthUnit.Mile):
                    retVal = AreaUnit.SquareMile;
                    break;
                case (LengthUnit.Decimeter):
                    retVal = AreaUnit.SquareDecimeter;
                    break;
                case (LengthUnit.Micrometer):
                    retVal = AreaUnit.SquareMicrometer;
                    break;
                case (LengthUnit.Millimeter):
                    retVal = AreaUnit.SquareMillimeter;
                    break;
                case (LengthUnit.Centimeter):
                    retVal = AreaUnit.SquareCentimeter;
                    break;
                case (LengthUnit.Meter):
                    retVal = AreaUnit.SquareMeter;
                    break;
                case (LengthUnit.Kilometer):
                    retVal = AreaUnit.SquareKilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");
            }
            return retVal;
        }

        public static VolumeUnit LengthUnit2VolumeUnit(LengthUnit lUnit)
        {
            VolumeUnit retVal;
            switch (lUnit)
            {
                case (LengthUnit.Inch):
                    retVal = VolumeUnit.CubicInch;
                    break;
                case (LengthUnit.Foot):
                    retVal = VolumeUnit.CubicFoot;
                    break;
                case (LengthUnit.Mile):
                    retVal = VolumeUnit.CubicMile;
                    break;
                case (LengthUnit.Decimeter):
                    retVal = VolumeUnit.CubicDecimeter;
                    break;
                case (LengthUnit.Micrometer):
                    retVal = VolumeUnit.CubicMicrometer;
                    break;
                case (LengthUnit.Millimeter):
                    retVal = VolumeUnit.CubicMillimeter;
                    break;
                case (LengthUnit.Centimeter):
                    retVal = VolumeUnit.CubicCentimeter;
                    break;
                case (LengthUnit.Meter):
                    retVal = VolumeUnit.CubicMeter;
                    break;
                case (LengthUnit.Kilometer):
                    retVal = VolumeUnit.CubicKilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");
            }
            return retVal;
        }

        public static LengthUnit LengthUnit2VolumeUnit(VolumeUnit vUnit)
        {
            LengthUnit retVal;
            switch (vUnit)
            {
                case (VolumeUnit.CubicInch):
                    retVal = LengthUnit.Inch;
                    break;
                case (VolumeUnit.CubicFoot):
                    retVal = LengthUnit.Foot;
                    break;
                case (VolumeUnit.CubicMile):
                    retVal = LengthUnit.Mile;
                    break;
                case (VolumeUnit.CubicDecimeter):
                    retVal = LengthUnit.Decimeter;
                    break;
                case (VolumeUnit.CubicMicrometer):
                    retVal = LengthUnit.Micrometer;
                    break;
                case (VolumeUnit.CubicMillimeter):
                    retVal = LengthUnit.Millimeter;
                    break;
                case (VolumeUnit.CubicCentimeter):
                    retVal = LengthUnit.Centimeter;
                    break;
                case (VolumeUnit.CubicMeter):
                    retVal = LengthUnit.Meter;
                    break;
                case (VolumeUnit.CubicKilometer):
                    retVal = LengthUnit.Kilometer;
                    break;
                default:
                    throw new Exception("that unit conersion is not specified");
            }
            return retVal;
        }

        public static double? MinValueDouble<T>()
        {
            T minVal = minValue<T>();
            return (double?)Convert.ChangeType(minVal, typeof(double));
        }
        
        public static T minValue<T>()
        {
            T retval;
            // No Nodatavalue. Choose something appropriate please
            if (typeof(T) == typeof(int))
                retval = (T)Convert.ChangeType(int.MinValue, typeof(T));
            else if (typeof(T) == typeof(double))
                // NOTE: SINGLE VALUE HERE IS NOT A TYPO.
                retval = (T)Convert.ChangeType(float.MinValue, typeof(T));
            else if (typeof(T) == typeof(float))
                retval = (T)Convert.ChangeType(float.MinValue, typeof(T));
            else if (typeof(T) == typeof(byte))
                retval = (T)Convert.ChangeType(byte.MinValue, typeof(T));
            else
                throw new NotSupportedException("Type conversion problem");
            return retval;
        }
    }
}
