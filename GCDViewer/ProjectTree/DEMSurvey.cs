using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class DEMSurvey : Surface
    {
        public readonly List<AssocSurface> AssocSurfaces;
        public SurveyDateTime SurveyDate { get; set; }

        public int? ChronologicalOrder { get; set; } // Optional zero-based index of chronological order maintain by the Multi-Epoch GCD Analysis

        public override string Noun { get { return "DEM Survey"; } }

        public override string ContextMenu => "DEMSurvey";

        public DEMSurvey(GCDProject project, XmlNode nodDEM) :
            base(project, nodDEM, "DEMSurvey.png", "DEMSurvey.png")
        {
            //SurveyDateTime surveyDT = null;
            XmlNode nodSurveyDate = nodDEM.SelectSingleNode("SurveyDate");
            if (nodSurveyDate is XmlNode)
            {
                SurveyDate = new SurveyDateTime();
                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
                    SurveyDate.Year = ushort.Parse(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText))
                    SurveyDate.Month = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText))
                    SurveyDate.Day = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText))
                    SurveyDate.Hour = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText))
                    SurveyDate.Minute = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText);
            }

            //read Chronological Order, if set
            XmlNode nodChronologicalOrder = nodDEM.SelectSingleNode("ChronologicalOrder");
            if (nodChronologicalOrder is XmlNode)
            {
                string sChronologicalOrder = nodChronologicalOrder.InnerText;
                int iChronologicalOrder;
                Boolean bParseSuccessful = int.TryParse(sChronologicalOrder, out iChronologicalOrder);
                if (bParseSuccessful)
                {
                    ChronologicalOrder = iChronologicalOrder;
                }
            }

            AssocSurfaces = new List<AssocSurface>();
            foreach (XmlNode nodAssoc in nodDEM.SelectNodes("AssociatedSurfaces/AssociatedSurface"))
            {
                AssocSurface assoc = new AssocSurface(project, nodAssoc, this);
                AssocSurfaces.Add(assoc);
            }

            // Load the error surfaces now that the associated surfaces have been loaded
            LoadErrorSurfaces(project, nodDEM);
            //LoadLinearExtractions(nodDEM);
        }
    }
}
