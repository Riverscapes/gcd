using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.Project
{
    class GCDProject : GCDProjectItem
    {
        public string Description { get; set; }
        public readonly FileInfo ProjectFile;
        public readonly DateTime DateTimeCreated;
        public readonly string GCDVersion;
        public readonly int Precision;
        public GCDConsoleLib.GCD.UnitGroup Units { get; set; }

        public Dictionary<string, DEMSurvey> DEMSurveys { get; internal set; }
        public Dictionary<string, DoD> DoDs { get; internal set; }

        public GCDProject(string name, string description, FileInfo projectFile,
            DateTime dtCreated, string gcdVersion, int nPrecision, GCDConsoleLib.GCD.UnitGroup units)
            : base(name)
        {
            Description = description;
            ProjectFile = projectFile;
            DateTimeCreated = dtCreated;
            GCDVersion = gcdVersion;
            Precision = nPrecision;
            Units = units;

            DEMSurveys = new Dictionary<string, DEMSurvey>();
            DoDs = new Dictionary<string, DoD>();
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodProject = xmlDoc.CreateElement("Project");
            xmlDoc.AppendChild(nodProject);

            nodProject.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodProject.AppendChild(xmlDoc.CreateElement("DateTimeCreated")).InnerText = DateTimeCreated.ToString("o");
            nodProject.AppendChild(xmlDoc.CreateElement("GCDVersion")).InnerText = GCDVersion;
            nodProject.AppendChild(xmlDoc.CreateElement("Precision")).InnerText = Precision.ToString();

            XmlNode nodUnits = nodProject.AppendChild(xmlDoc.CreateElement("Units"));
            nodUnits.AppendChild(xmlDoc.CreateElement("HorizontalUnits")).InnerText = Units.HorizUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("VerticalUnits")).InnerText = Units.VertUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("AreaUnits")).InnerText = Units.ArUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("VolumeUnits")).InnerText = Units.VolUnit.ToString();

            if (DEMSurveys.Count > 0)
            {
                XmlNode nodDEMs = nodProject.AppendChild(xmlDoc.CreateElement("DEMSurveyrs"));
                foreach (DEMSurvey dem in DEMSurveys.Values)
                    dem.Serialize(xmlDoc, nodDEMs);
            }

            if (DoDs.Count > 0)
            {
                XmlNode nodDoDs = nodProject.AppendChild(xmlDoc.CreateElement("DoDs"));
                foreach (DoD dod in DoDs.Values)
                    dod.Serialize(xmlDoc, nodDoDs);
            }

            xmlDoc.Save(ProjectFile.FullName);
        }
    }
}
