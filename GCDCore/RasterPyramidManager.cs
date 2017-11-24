using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GCDCore
{
    public class RasterPyramidManager
    {
        private const char LISTDELIMETER = ',';
        private const char KEYVALUEDELIMETER = '=';

        public enum PyramidRasterTypes
        {
            DEM,
            Hillshade,
            AssociatedSurfaces,
            ErrorSurfaces,
            DoDRaw,
            DoDThresholded,
            PropagatedError,
            ProbabilityRasters
        }

        private Dictionary<PyramidRasterTypes, bool> PyramidTypes { get; set; }

        /// <summary>
        /// Use this constructor for non-user interface applications
        /// All rasters will default to NO pyramids
        /// </summary>
        /// <remarks></remarks>
        public RasterPyramidManager()
        {
            // There's no need to actually add each raster type here because the default return
            // value from AutomaticallyBuildPyramids below should be False.
            PyramidTypes = new Dictionary<PyramidRasterTypes, bool>();
        }

        /// <summary>
        /// Use this constructor for user interface applications
        /// </summary>
        /// <param name="sPyramidString">Pass in the concatenated pyramid settings string, typically from the software products My.Settings</param>
        /// <remarks></remarks>
        public RasterPyramidManager(string sPyramidString)
        {
            PyramidTypes = new Dictionary<PyramidRasterTypes, bool>();
            // If there is no pyramid string, or something when wrong parsing it.
            // If bUseDefaults Then
            PyramidTypes[PyramidRasterTypes.DEM] = true;
            PyramidTypes[PyramidRasterTypes.Hillshade] = true;
            PyramidTypes[PyramidRasterTypes.AssociatedSurfaces] = true;
            PyramidTypes[PyramidRasterTypes.ErrorSurfaces] = true;
            PyramidTypes[PyramidRasterTypes.DoDRaw] = false;
            PyramidTypes[PyramidRasterTypes.DoDThresholded] = true;
            PyramidTypes[PyramidRasterTypes.PropagatedError] = false;
            
            if (!string.IsNullOrEmpty(sPyramidString))
            {
                // Loop over all the known raster types and try to retrieve they value from the argument string
                foreach (string sKeyValuePair in sPyramidString.Split(LISTDELIMETER))
                {
                    // Split the key and value using equal sign
                    string[] sKeyAndValue = sKeyValuePair.Split(KEYVALUEDELIMETER);

                    if (sKeyAndValue.Length == 2)
                    {
                        // Retrieve the raster type and check that its not empty and represents one of the raster types
                        string sKey = sKeyAndValue[0];
                        if (!string.IsNullOrEmpty(sKey))
                        {
                            if (Enum.IsDefined(typeof(PyramidRasterTypes), sKey))
                            {
                                PyramidRasterTypes eRasterType = (PyramidRasterTypes)Enum.Parse(typeof(PyramidRasterTypes), sKey);

                                // check that the argument string contains a value and that is not null and also a boolean
                                string sValue = sKeyAndValue[1];
                                if (!string.IsNullOrEmpty(sValue))
                                {
                                    bool bValue = false;
                                    if (bool.TryParse(sValue, out bValue))
                                    {
                                        // Update the pyramid raster setting based on the retrieved value
                                        PyramidTypes[eRasterType] = bValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool AutomaticallyBuildPyramids(PyramidRasterTypes eRasterType)
        {
            if (PyramidTypes.ContainsKey(eRasterType))
            {
                return PyramidTypes[eRasterType];
            }
            else
            {
                // The default is to not build pyramids
                return false;
            }
        }

        public void SetAutomaticPyramidsForRasterType(PyramidRasterTypes eRasterType, bool bBuildPyramids)
        {
            if (PyramidTypes.ContainsKey(eRasterType))
            {
                PyramidTypes[eRasterType] = bBuildPyramids;
            }
            else
            {
                PyramidTypes.Add(eRasterType, bBuildPyramids);
            }
        }

        public string GetPyramidSettingString()
        {
            string sResult = string.Empty;
            foreach (PyramidRasterTypes eRasterType in PyramidTypes.Keys)
            {
                sResult += string.Format("{0}{1}{2}{3}", eRasterType, KEYVALUEDELIMETER, PyramidTypes[eRasterType], LISTDELIMETER);
            }

            if (!string.IsNullOrEmpty(sResult))
            {
                sResult = sResult.TrimEnd(LISTDELIMETER);
            }

            return sResult;
        }

        public string GetRasterTypeName(PyramidRasterTypes eRasterType)
        {

            switch (eRasterType)
            {
                case PyramidRasterTypes.DEM:
                    return "DEM";
                case PyramidRasterTypes.Hillshade:
                    return "Hillshade";
                case PyramidRasterTypes.AssociatedSurfaces:
                    return "Associated Surfaces";
                case PyramidRasterTypes.ErrorSurfaces:
                    return "Error Surfaces";
                case PyramidRasterTypes.DoDRaw:
                    return "Raw DEM of Difference";
                case PyramidRasterTypes.DoDThresholded:
                    return "Thresholded DEM of Difference";
                case PyramidRasterTypes.PropagatedError:
                    return "Propagated Error Rasters";
                case PyramidRasterTypes.ProbabilityRasters:
                    return "Probability Rasters";
                default:
                    Exception ex = new Exception("Unhandled raster type within pyramid manager.");
                    ex.Data["Raster Type"] = eRasterType;
                    throw ex;
            }
        }

        public void PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes ePyramidRasterType, FileInfo rasterPath)
        {
            if (PyramidTypes[ePyramidRasterType])
            {
                if (rasterPath.Exists)
                {
                    GCDConsoleLib.RasterOperators.BuildPyramids(rasterPath);
                }
            }
        }
    }

    /// <summary>
    /// Use this class to store raster pyramid types in a checked listbox, like in the GCD options form
    /// </summary>
    /// <remarks></remarks>
    public class PyramidRasterTypeDisplay
    {
        private RasterPyramidManager.PyramidRasterTypes m_eRasterType;

        private string m_sName;
        public RasterPyramidManager.PyramidRasterTypes RasterType
        {
            get { return m_eRasterType; }
        }

        public override string ToString()
        {
            return m_sName;
        }

        public PyramidRasterTypeDisplay(RasterPyramidManager.PyramidRasterTypes eRasterType)
        {
            m_eRasterType = eRasterType;
            m_sName = Project.ProjectManager.PyramidManager.GetRasterTypeName(eRasterType);
        }
    }
}