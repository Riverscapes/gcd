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
            nodProject.AppendChild(xmlDoc.CreateElement("Description")).InnerText = Description;

            XmlNode nodUnits = nodProject.AppendChild(xmlDoc.CreateElement("Units"));
            nodUnits.AppendChild(xmlDoc.CreateElement("Horizontal")).InnerText = Units.HorizUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Vertical")).InnerText = Units.VertUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Area")).InnerText = Units.ArUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Volume")).InnerText = Units.VolUnit.ToString();

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

        public static GCDProject Load(FileInfo projectFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFile.FullName);

            XmlNode nodProject = xmlDoc.SelectSingleNode("Project");
            string name = nodProject.SelectSingleNode("Name").InnerText;
            string desc = nodProject.SelectSingleNode("Description").InnerText;
            string gcdv = nodProject.SelectSingleNode("GCDVersion").InnerText;

            DateTime dtCreated = DateTime.Parse(nodProject.SelectSingleNode("DateTimeCreated").InnerText);
            int precision = int.Parse(nodProject.SelectSingleNode("Precision").InnerText);

            UnitsNet.Units.LengthUnit horiz = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), nodProject.SelectSingleNode("Units/Horizontal").InnerText);
            UnitsNet.Units.LengthUnit vert = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), nodProject.SelectSingleNode("Units/Vertical").InnerText);
            UnitsNet.Units.AreaUnit area = (UnitsNet.Units.AreaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaUnit), nodProject.SelectSingleNode("Units/Area").InnerText);
            UnitsNet.Units.VolumeUnit vol = (UnitsNet.Units.VolumeUnit)Enum.Parse(typeof(UnitsNet.Units.VolumeUnit), nodProject.SelectSingleNode("Units/Volume").InnerText);
            GCDConsoleLib.GCD.UnitGroup units = new GCDConsoleLib.GCD.UnitGroup(vol, area, vert, horiz);

            GCDProject project = new GCDProject(name, desc, projectFile, dtCreated, gcdv, precision, units);

            foreach (XmlNode nodDEM in nodProject.SelectNodes("DEMSurveys/DEM"))
            {
                DEMSurvey dem = DEMSurvey.Deserialize(nodDEM);
                project.DEMSurveys[dem.Name] = dem;
            }

            foreach (XmlNode nodDoD in nodProject.SelectNodes("DoDs/DoD"))
            {
                DoD dod = DoD.Deserialize(nodDoD, project.DEMSurveys);
                project.DoDs[dod.Name] = dod;
            }

            return project;
        }
    }
}
