using System;
using System.Collections;
using OSGeo.OGR;
using OSGeo.GDAL;
using System.Collections.Generic;

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
            Type theType = typeof(Single);
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
                    theType = typeof(Single);
                    break;
                case DataType.GDT_Float64:
                    theType = typeof(Double);
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
        public static DataType TypeToDatatype(Type theType)
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
            else if (theType == typeof(Single))
            {
                dType = DataType.GDT_Float32;
            }
            else if (theType == typeof(Double))
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

            else if (theType == typeof(double) || theType == typeof(Single))
            {
                dFType = FieldType.OFTReal;
            }
            else if (theType == typeof(List<double>) || theType == typeof(List<Single>))
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




    }
}
