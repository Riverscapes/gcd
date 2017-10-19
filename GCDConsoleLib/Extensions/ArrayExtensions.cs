namespace GCDConsoleLib.Common.Extensons
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Slice a segment out of a 1D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>s
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public static T[] Slice<T>(this T[] source, int fromId, int toId)
        {
            T[] ret = new T[toId - fromId + 1];
            for (int srcId = fromId, dstId = 0; srcId <= toId; srcId++)
            {
                ret[dstId++] = source[srcId];
            }
            return ret;
        }

        /// <summary>
        ///  Slice a segment out of a 2D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fromR0"></param>
        /// <param name="toR0"></param>
        /// <param name="fromR1"></param>
        /// <param name="toR1"></param>
        /// <returns></returns>
        public static T[,] Slice<T>(this T[,] source, int fromR0, int toR0, int fromR1, int toR1)
        {
            T[,] ret = new T[toR0 - fromR0 + 1, toR1 - fromR1 + 1];

            for (int srcIdR0 = fromR0, dstIdR0 = 0; srcIdR0 <= toR0; srcIdR0++, dstIdR0++)
            {
                for (int srcIdR1 = fromR1, dstIdR1 = 0; srcIdR1 <= toR1; srcIdR1++, dstIdR1++)
                {
                    ret[dstIdR0, dstIdR1] = source[srcIdR0, srcIdR1];
                }
            }
            return ret;
        }


        /// <summary>
        /// Place or "plunk" smaller data into a bigger 1D
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="srcData"></param>
        /// <param name="offset"></param>
        public static void Plunk<T>(this T[] dstData, ref T[] srcData, int offset)
        {
            for (int srcId = 0; srcId < srcData.Length; srcId++)
            {
                dstData[srcId + offset] = srcData[srcId];
            }
        }

        /// <summary>
        /// Place or "plunk" smaller data into a bigger 2D Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dstData"></param>
        /// <param name="srcData"></param>
        /// <param name="OffsetR0"></param>
        /// <param name="OffsetR1"></param>
        public static void Plunk<T>(this T[,] dstData, ref T[,] srcData, int OffsetR0, int OffsetR1)
        {
            for (int srcIdR0 = 0; srcIdR0 < srcData.GetLength(0); srcIdR0++)
            {
                for (int srcIdR1 = 0; srcIdR1 < srcData.GetLength(1); srcIdR1++)
                {
                    dstData[srcIdR0 + OffsetR0, srcIdR1 + OffsetR1] = srcData[srcIdR0, srcIdR1];
                }
            }
        }

        /// <summary>
        /// plunk 2D data into a bigger 2D structure even though both are 1D arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dstData"></param>
        /// <param name="srcData"></param>
        /// <param name="dstSizeR0"></param>
        /// <param name="dstSizeR1"></param>
        /// <param name="srcSizeR0"></param>
        /// <param name="srcSizeR1"></param>
        /// <param name="offsetR0"></param>
        /// <param name="offsetR1"></param>
        public static void Plunk<T>(this T[] dstData, ref T[] srcData, 
            int dstSizeR0, int dstSizeR1, 
            int srcSizeR0, int srcSizeR1, 
            int offsetR0, int offsetR1)
        {
            for (int srcIdR0 = 0; srcIdR0 < srcSizeR0; srcIdR0++)
            {
                for (int srcIdR1 = 0; srcIdR1 < srcSizeR1; srcIdR1++)
                {
                    dstData[(srcIdR0+offsetR0)*(srcSizeR1) + (srcIdR1+offsetR1)] = srcData[srcIdR0 * srcSizeR1 + srcIdR1];
                }
            }
        }


        /// <summary>
        /// Make a 2D array from a 1D array 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="sizeR0"></param>
        /// <param name="sizeR1"></param>
        /// <returns></returns>
        public static T[,] Make2DArray<T>(this T[] input, int sizeR0, int sizeR1)
        {
            T[,] output = new T[sizeR0, sizeR1];
            for (int idR0 = 0; idR0 < sizeR0; idR0++)
            {
                for (int idR1 = 0; idR1 < sizeR1; idR1++)
                {
                    output[idR0, idR1] = input[idR0 * sizeR1 + idR1];
                }
            }
            return output;
        }

        /// <summary>
        /// Turn a 2D arrau into a 1D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T[] Make1DArray<T>(this T[,] input)
        {
            int sizeR0 = input.GetLength(0);
            int sizeR1 = input.GetLength(1);

            T[] output = new T[sizeR0 * sizeR1];

            for (int idR0 = 0; idR0 < sizeR0; idR0++)
            {
                for (int idR1 = 0; idR1 < sizeR1; idR1++)
                {
                    output[idR0 * sizeR1 + idR1] = input[idR0, idR1];
                }
            }
            return output;
        }

        /// <summary>
        /// Fill a 1D array with a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalArray"></param>
        /// <param name="with"></param>
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        /// <summary>
        ///  Fill a 2D array with a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalArray"></param>
        /// <param name="with"></param>
        public static void Fill<T>(this T[,] originalArray, T with)
        {
            for (int idR1 = 0; idR1 < originalArray.GetLength(1); idR1++)
            {
                for (int idR0 = 0; idR0 < originalArray.GetLength(0); idR0++)
                {
                    originalArray[idR0, idR1] = with;
                }
            }
        }

    }
}



