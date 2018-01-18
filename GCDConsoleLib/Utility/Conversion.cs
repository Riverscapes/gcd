using System;
using OSGeo.OGR;
using OSGeo.GDAL;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;
using System.Linq;

namespace GCDConsoleLib.Utility
{
    public static class Conversion
    {
        /// <summary>
        /// Convert from a GDAL Raster Data Type to a simplified type
        /// This is a way to make sure all the Integer types use Int32 etc.
        /// </summary>
        /// <param name="dType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from a GDAL Raster Data type to a C# data type
        /// </summary>
        /// <param name="dType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from a C# data type to a GDAL Raster data Type
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static DataType CSharpTypeToDataType(Type theType)
        {
            DataType dType = OSGeo.GDAL.DataType.GDT_Float32;
            if (theType == typeof(Byte))
                dType = DataType.GDT_Byte;
            else if (theType == typeof(Int32))
                dType = DataType.GDT_Int32;
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
                dType = DataType.GDT_Float32;
            else if (theType == typeof(double))
                dType = DataType.GDT_Float64;
            else
                throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dType));
            return dType;
        }

        /// <summary>
        /// Convert from an OGR Field type to a C# data type
        /// </summary>
        /// <param name="ftType"></param>
        /// <returns></returns>
        public static Type FieldTypeToCSharpType(FieldType ftType)
        {
            Type theType = typeof(int);
            switch (ftType)
            {
                case FieldType.OFTInteger:
                    return typeof(int);
                case FieldType.OFTInteger64:
                    return typeof(int);
                case FieldType.OFTIntegerList:
                    return typeof(List<int>);
                case FieldType.OFTInteger64List:
                    return typeof(List<int>);
                case FieldType.OFTReal:
                    return typeof(double);
                case FieldType.OFTRealList:
                    return typeof(List<double>);
                case FieldType.OFTString:
                    return typeof(string);
                case FieldType.OFTStringList:
                    return typeof(List<string>);
                case FieldType.OFTWideString:
                    return typeof(string);
                case FieldType.OFTWideStringList:
                    return typeof(List<string>);
                case FieldType.OFTDate:
                    return typeof(DateTime);
                case FieldType.OFTDateTime:
                    return typeof(DateTime);
                case FieldType.OFTTime:
                    return typeof(DateTime);
                default:
                    //OFTBinary = 8,
                    throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), theType));
            }
        }

        /// <summary>
        /// Convert from a C# type to an OGR FieldType 
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static FieldType CSharpTypeToFieldType(Type theType)
        {
            FieldType dFType = FieldType.OFTReal;
            if (theType == typeof(Int64))
                dFType = FieldType.OFTInteger64;
            else if (theType == typeof(List<Int64>))
                dFType = FieldType.OFTInteger64List;
            else if (theType == typeof(Int32))
                dFType = FieldType.OFTInteger;
            else if (theType == typeof(List<Int32>))
                dFType = FieldType.OFTIntegerList;
            else if (theType == typeof(Int16))
                dFType = FieldType.OFTInteger;
            else if (theType == typeof(List<Int16>))
                dFType = FieldType.OFTIntegerList;
            else if (theType == typeof(DateTime))
                dFType = FieldType.OFTDateTime;
            else if (theType == typeof(string))
                dFType = FieldType.OFTString;
            else if (theType == typeof(List<string>))
                dFType = FieldType.OFTStringList;

            else if (theType == typeof(string))
                dFType = FieldType.OFTString;
            else if (theType == typeof(List<string>))
                dFType = FieldType.OFTStringList;

            else if (theType == typeof(double) || theType == typeof(float))
                dFType = FieldType.OFTReal;
            else if (theType == typeof(List<double>) || theType == typeof(List<float>))
                dFType = FieldType.OFTRealList;
            else
                throw new NotSupportedException("This datatype is not supported: " + Enum.GetName(typeof(DataType), dFType));

            return dFType;

        }

        /// <summary>
        /// Get the smallest possible value of whatever "T" is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static double? MinValueDouble<T>()
        {
            T minVal = minValue<T>();
            return (double?)Convert.ChangeType(minVal, typeof(double));
        }

        /// <summary>
        /// Return the smallest number for a given numeric type "T"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Here's your lookup table. Keep it all lowercase. Don't put the standard spelling in there because we already try that
        /// </summary>
        private static Dictionary<LengthUnit, string[]> LengthAliases = new Dictionary<LengthUnit, string[]>() {
            { LengthUnit.Meter, new string[] { "metre" } },
            { LengthUnit.Foot, new string[] { "feet", "ft-us" } },
        };

        /// <summary>
        /// Do a proper parse of all acronyms and weird spellings of units
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static LengthUnit ParseLengthUnit(string u)
        {
            // Meters is always going to be our fallback
            LengthUnit lUnit = LengthUnit.Meter;
            bool bFound = false;
            try
            {
                lUnit = Length.ParseUnit(u);
                bFound = true;
            }
            catch (Exception e)
            {
                // Didn't work. Do it the hard way
                foreach (LengthUnit iterUnit in Enum.GetValues(typeof(LengthUnit)))
                {
                    // If it's a direct lookup then just go with that.
                    string sIterUnit = Enum.GetName(typeof(LengthUnit), iterUnit);
                    if (String.Compare(sIterUnit, u, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        lUnit = iterUnit;
                        bFound = true;
                        break;
                    }
                    else if (LengthAliases.ContainsKey(iterUnit) && 
                        LengthAliases[iterUnit].Any(s => String.Compare(s, u, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        lUnit = iterUnit;
                        bFound = true;
                        break;
                    }
                }
                if (!bFound) throw new ArgumentException(String.Format("Could not parse unit string: {0}", u));
            }
            return lUnit;
        }
    }
}
