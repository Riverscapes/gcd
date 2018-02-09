using System;
using System.Collections.Generic;
using OSGeo.GDAL;
using System.Diagnostics;
using GCDConsoleLib.Internal.Operators;

namespace GCDConsoleLib
{
    public class VectorRaster : Raster
    {
        public List<string> FieldValues {  get; private set;  }

        /// <summary>
        /// Given a Template Raster, a vector and a field name, create a usable rasterized poly mask
        /// </summary>
        /// <param name="Template">Template will be used for Shape and projection only</param>
        /// <param name="input"></param>
        /// <param name="FieldName"></param>
        public VectorRaster(Raster Template, Vector vectorInput, string FieldName) : base(Template)
        {
            FieldValues = new List<string> { };
            Datatype = new GdalDataType(typeof(int));

            using (Raster tmp = new Raster(Template))
            {
                // Make sure our nodatavals line up
                SetNoData(0.0);
                tmp.SetNoData(0.0);
                // Do GDaL's rasterize first to get the rough boolean shape.
                Rasterize(vectorInput, tmp);

                // Now we call our Rasterize with an empty List that will be filled with field values.
                RasterizeVector op = new RasterizeVector(tmp, vectorInput, this, FieldName, FieldValues);
                op.RunWithOutput();
                // This is a little sneaky because we're kind of writing back to ourselves here.
            }
        }

        /// <summary>
        /// This is GDAL's rasterization method. It's static becuase it makes a bit of a mess
        /// </summary>
        /// <param name="outputRaster"></param>
        public static void Rasterize(Vector vectorinput, Raster outputRaster)
        {
            //http://www.gisremotesensing.com/2015/09/vector-to-raster-conversion-using-gdal-c.html
            // Register GDal and get the driver objects
            GdalConfiguration.ConfigureOgr();

            outputRaster.Create();
            outputRaster.SetNoData(0.0);
            Dataset _ds = Gdal.Open(outputRaster.GISFileInfo.FullName, Access.GA_Update);

            vectorinput.Open();

            int[] bandlist = new int[] { 1 };
            OSGeo.OGR.Layer layer = vectorinput._ds.GetLayerByIndex(0);

            double[] burnValues = new double[] { 1.0 };
            string[] rasterizeOptions = new string[] {}; // String.Format("ATTRIBUTE={0}", fieldname) 

            Gdal.RasterizeLayer(_ds, 1, bandlist, layer, IntPtr.Zero, IntPtr.Zero,
                burnValues.Length, burnValues, rasterizeOptions,
                new Gdal.GDALProgressFuncDelegate(ProgressFunc), "Raster conversion");

            vectorinput.UnloadDS();
            _ds.Dispose();
        }


        private static int ProgressFunc(double complete, IntPtr message, IntPtr data)
        {
            Debug.Write("Processing ... " + complete * 100 + "% Completed.");
            if (message != IntPtr.Zero)
            {
                Console.Write(" Message:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message));
            }
            if (data != IntPtr.Zero)
            {
                Console.Write(" Data:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(data));
            }
            Console.WriteLine("");
            return 1;
        }
    }
}
