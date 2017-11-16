using System;
using OSGeo.OSR;
using GCDConsoleLib.Utility;
using UnitsNet;
using UnitsNet.Units;
using System.Diagnostics;

namespace GCDConsoleLib
{
    public class Projection
    {
        public enum ImportType : byte { WKT, USGS, EPSG }
        public string OriginalString;
        public SpatialReference mSRef;
        public LengthUnit HorizontalUnit;
        public decimal LinearUnitConversionToM;
        public string LinearUnits;

        /// <summary>
        /// Build our projection from the usual WKT string
        /// </summary>
        /// <param name="sWkt"></param>
        public Projection(string sWkt)
        {
            GdalConfiguration.ConfigureGdal();
            OriginalString = sWkt;
            mSRef = new SpatialReference(sWkt);
            _Init();
        }


        /// <summary>
        /// Build your projection from an EPSG number
        /// </summary>
        /// <param name="iEPSG"></param>
        public Projection(int iEPSG)
        {
            GdalConfiguration.ConfigureGdal();
            OriginalString = "";
            mSRef = new SpatialReference("");
            try
            {
                mSRef.ImportFromEPSG(iEPSG);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new IndexOutOfRangeException(string.Format("Could not find the EPSG:{0} in the lookup table.", iEPSG));
            }
            _Init();
        }

        private void _Init()
        {
            HorizontalUnit = LengthUnit.Meter;
            LinearUnitConversionToM = 0;
            try
            {
                HorizontalUnit =  Length.ParseUnit(mSRef.GetLinearUnitsName()); ;
                mSRef.AutoIdentifyEPSG();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Spelling differences might mean projections are not the same.
        /// We need something better.
        /// </summary>
        /// <param name="otherProj"></param>
        /// <returns></returns>
        public bool IsSame(Projection otherProj)
        {
            bool bSame = false;
            try
            {
                bSame = Convert.ToBoolean(mSRef.IsSame(otherProj.mSRef));
            }
            catch(Exception e) {
                Debug.WriteLine(e.Message);
            }
            return bSame;
        }


        /// <summary>
        /// Simple property for getting the Well known string back
        /// </summary>
        public string Wkt
        {
            get
            {
                string output = OriginalString;
                mSRef.ExportToWkt(out output);
                return output;
            }
        }

        /// <summary>
        /// Simple property for getting the Well known string back
        /// </summary>
        public string PrettyWkt
        {
            get
            {
                string output = OriginalString;
                mSRef.ExportToPrettyWkt(out output, 0);
                return output;
            }
        }
        /// <summary>
        /// This was a bit tricky to find. It's basically trying to pull the EPSG code out of the WKT
        /// </summary>
        public int EPSG
        {
            get
            {
                int iEPSG = 0;
                try {
                    iEPSG = Convert.ToInt32(mSRef.GetAttrValue("PROJCS|GEOGCS|AUTHORITY", 1));
                }
                catch(Exception e) {
                    Debug.WriteLine(e.Message);
                }
                return iEPSG;
            }
        }

    }
}
