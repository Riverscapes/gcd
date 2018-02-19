using System;
using System.Collections.Generic;
using OSGeo.GDAL;
using System.Linq;
using System.Diagnostics;
using GCDConsoleLib.Internal.Operators;

namespace GCDConsoleLib
{
    public class VectorRaster : Raster
    {
        public Dictionary<int, string> FieldValues { get; private set; }


        /// <summary>
        /// Given a Template Raster, a vector and a field name, create a usable rasterized poly mask
        /// </summary>
        /// <param name="Template">Template will be used for Shape and projection only</param>
        /// <param name="input"></param>
        /// <param name="FieldName"></param>
        public VectorRaster(Raster Template, Vector vectorInput) : base(Template)
        {
            // This won't get populated 
            FieldValues = new Dictionary<int, string> { };
            Datatype = new GdalDataType(typeof(int));

            SetNoData(-1.0);

            // Do GDaL's rasterize first to get the rough boolean shape.
            Rasterize(vectorInput, this);

        }

        /// <summary>
        /// Given a Template Raster, a vector and a field name, create a usable rasterized poly mask
        /// </summary>
        /// <param name="Template">Template will be used for Shape and projection only</param>
        /// <param name="input"></param>
        /// <param name="FieldName"></param>
        public VectorRaster(Raster Template, Vector vectorInput, string FieldName) : base(Template)
        {
            FieldValues = new Dictionary<int, string> { };
            Datatype = new GdalDataType(typeof(int));

            SetNoData(-1.0);
            // Do GDaL's rasterize first to get the rough boolean shape.
            Rasterize(vectorInput, this);

            int fieldIndex = vectorInput.Features.First().Value.Feat.GetFieldIndex(FieldName);
            if (fieldIndex == -1) throw new IndexOutOfRangeException(String.Format("Could not find field: `{0}`", FieldName));

            int GDALMASKidx = vectorInput.Features.First().Value.Feat.GetFieldIndex(Vector.CGDMASKFIELD);
            if (GDALMASKidx == -1) throw new IndexOutOfRangeException(String.Format("Could not find MANDATORY field: `{0}`", FieldName));

            // Now make an equivalence between the GCDFID field and the FieldName Values
            foreach (KeyValuePair<long, VectorFeature> kvp in vectorInput.Features)
            {
                int maskid = kvp.Value.Feat.GetFieldAsInteger(GDALMASKidx);
                string val = kvp.Value.Feat.GetFieldAsString(fieldIndex);
                if (!FieldValues.ContainsKey(maskid))
                    FieldValues.Add(maskid, val);
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
            outputRaster.SetNoData(-1.0);
            Dataset _ds = Gdal.Open(outputRaster.GISFileInfo.FullName, Access.GA_Update);

            vectorinput.Open();

            int[] bandlist = new int[] { 1 };
            OSGeo.OGR.Layer layer = vectorinput._ds.GetLayerByIndex(0);

            double[] burnValues = new double[] { 1.0 };
            string[] rasterizeOptions = new string[] {String.Format("ATTRIBUTE={0}", Vector.CGDMASKFIELD) }; // String.Format("ATTRIBUTE={0}", fieldname) 

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
                Console.Write(" Message:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message));

            if (data != IntPtr.Zero)
                Console.Write(" Data:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(data));

            Console.WriteLine("");
            return 1;
        }
    }
}
