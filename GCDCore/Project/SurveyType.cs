using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GCDCore.Project
{
    public class SurveyType
    {
        public string Name { get; set; }

        private decimal m_fError;
        public decimal ErrorValue
        {
            get { return m_fError; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The error value must be greater than or equal to zero");
                }

                m_fError = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public SurveyType(string sName, decimal fError)
        {
            Name = sName;
            m_fError = fError;
        }

        public static Dictionary<string, SurveyType> Load(FileInfo filePath)
        {
            Dictionary<string, SurveyType> dSurveyTypes = new Dictionary<string, SurveyType>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);

            foreach (XmlNode nodType in xmlDoc.SelectNodes("SurveyTypes/SurveyType"))
            {
                string sName = nodType.SelectSingleNode("Name").InnerText;
                decimal fError = decimal.Parse(nodType.SelectSingleNode("Error").InnerText);
                dSurveyTypes[sName] = new SurveyType(sName, fError);
            }

            return dSurveyTypes;
        }

        public static void Save(FileInfo filePath, Dictionary<string, SurveyType> dSurveyTypes)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodRoot = xmlDoc.CreateElement("SurveyTypes");
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", null, null), nodRoot);

            foreach (SurveyType sType in dSurveyTypes.Values)
            {
                XmlElement nodType = xmlDoc.CreateElement("SurveyType");
                nodRoot.AppendChild(nodType);

                XmlElement nodName = xmlDoc.CreateElement("Name");
                nodName.InnerText = sType.Name;
                nodType.AppendChild(nodName);

                XmlElement nodError = xmlDoc.CreateElement("Error");
                nodError.InnerText = sType.ErrorValue.ToString();
                nodType.AppendChild(nodError);
            }

            xmlDoc.Save(filePath.FullName);
        }
    }
}