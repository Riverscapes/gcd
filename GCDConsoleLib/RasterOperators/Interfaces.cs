using System;
using System.Collections.Generic;
using System.Reflection;
using GCDConsoleLib.Internal.Operators;
namespace GCDConsoleLib
{
    struct RasterOperators
    {
        /// <summary>
        /// Generic function to get a generic type
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object MakeGenericType(Type generic, Type innerType,
            params object[] args)
        {
            Type specificType = generic.MakeGenericType(new Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }

        /// <summary>
        /// Generic function to get a generic method
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object MakeGenericMethod(object obj, string methodName,
            Type innerType, params object[] args)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static);
            MethodInfo generic = method.MakeGenericMethod(innerType);
            return generic.Invoke(obj, args);
        }
        public static object GenericRunner(Type generic, Type innerType, params object[] args)
        {
            object myGenericClass = MakeGenericType(generic, innerType);
            return MakeGenericMethod(myGenericClass, "Run", innerType);
        }

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            return (Raster)GenericRunner(typeof(RasterCopy<>), rInput.Datatype.CSType, new object[] { rInput, sOutputRaster });
        }
    }

}




//public static Raster ExtendedCopy(Raster rInput, string sOutputRaster)
//{
//    Raster rOutputRaster = new Raster(sOutputRaster);
//    RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, rInput.Extent);
//    return myCopy.Run();
//}

//public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
//{
//    Raster rOutputRaster = new Raster(sOutputRaster);
//    RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect);
//    myCopy.SetOpExtent(newRect);
//    return myCopy.Run();
//}

///// This one's mainly for testing purposes
//public static Raster ExtendedCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect)
//{
//    RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect);
//    myCopy.SetOpExtent(newRect);
//    return myCopy.Run();
//}

/// <summary>
/// HEre are our public functions
/// </summary>
/// <param name="rInput"></param>
/// <param name="dOperand"></param>
/// <param name="sOutputRaster"></param>
/// <returns></returns>
//public static Raster Add(ref Raster rInput, double dOperand, string sOutputRaster)
//{
//    Raster rOutput = new Raster(sOutputRaster);
//    RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInput, dOperand, ref rOutput);
//    return mathOp.Run();
//}
//public static Raster Add(ref Raster rInputA, ref Raster rInputB, string sOutputRaster)
//{
//    Raster rOutput = new Raster(sOutputRaster);
//    RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInputA, ref rInputB, ref rOutput);
//    return mathOp.Run();
//}
