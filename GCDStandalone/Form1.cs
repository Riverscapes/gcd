using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDStandalone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sFullPath = @"D:\GISData\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img";
            double m_fCellHeight = 0;
            double m_fCellWidth = 0;
            double m_fLeft = 0;
            double m_fTop = 0;
            Int32 m_nRows = 0;
            Int32 m_nColumns = 0;
            double m_fNoData = 0;
            Int32 nNoData = 0;
            Int32 nDataType = 0;

            System.Text.StringBuilder sUnits = new StringBuilder(1024);
            System.Text.StringBuilder sSpatialReference = new StringBuilder(1024);
            StringBuilder theError = new StringBuilder(1024);

            //System.Diagnostics.Debug.Assert(System.IO.File.Exists(sFullPath));

            //string sPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "RasterManagerGCD.dll");
            //System.Diagnostics.Debug.Assert(System.IO.File.Exists(sPath));

            //RegisterGDAL();
            //GetRasterProperties(sFullPath, ref m_fCellHeight, ref m_fCellWidth,
            //                                  ref m_fLeft, ref m_fTop, ref m_nRows, ref m_nColumns, ref m_fNoData, ref nNoData, ref nDataType,
            //                                  sUnits, sSpatialReference,
            //                                  theError);

            GCDConsoleLib.Raster r = new GCDConsoleLib.Raster(sFullPath);

            System.Diagnostics.Debug.Print("hi");


        }

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern void RegisterGDAL();

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern int GetRasterProperties(string sFullPath, ref double fCellHeight, ref double fCellWidth, ref double fLeft, ref double fTop, ref int nRows, ref int nCols, ref double fNoData, ref int nHasNoData, ref int nDataType,
        //System.Text.StringBuilder sUnits, System.Text.StringBuilder sSpatialReference, System.Text.StringBuilder sError);
    }
}
