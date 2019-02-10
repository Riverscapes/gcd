using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class InterComparison : GCDProjectItem
    {
        public readonly FileInfo _SummaryXML;
        public readonly naru.ui.SortableBindingList<DoDBase> _DoDs;

        public override string Noun { get { return "Inter Comparison"; } }

        /// <summary>
        /// Inter-comparisons are never considered in use
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return false;
            }
        }

        public InterComparison(string name, FileInfo summaryXML, List<DoDBase> dods)
            : base(name)
        {
            _SummaryXML = summaryXML;
            _DoDs = new naru.ui.SortableBindingList<DoDBase>(dods);
        }

        public InterComparison(XmlNode nodParent, List<DoDBase> dods)
            : base(nodParent.SelectSingleNode("Name").InnerText)
        {
            _SummaryXML = ProjectManager.Project.GetAbsolutePath(nodParent.SelectSingleNode("SummaryXML").InnerText);

            _DoDs = new naru.ui.SortableBindingList<DoDBase>();
            foreach (XmlNode nodDoD in nodParent.SelectNodes("DoDs/DoD"))
            {
                if (dods.Any(x => string.Compare(x.Name, nodDoD.InnerText, true) == 0))
                {
                    _DoDs.Add(dods.First(x => string.Compare(x.Name, nodDoD.InnerText, true) == 0));
                }
                else
                {
                    // TODO: don't really know what to do here? Should not occur if deleting DoDs checks intercomparisons first!
                    throw new Exception(string.Format("Unable to find DoD \"{0}\" that's part of inter-comparison.", nodDoD.InnerText));
                }
            }
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodIC = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("InterComparison"));
            nodIC.AppendChild(nodIC.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodIC.AppendChild(nodIC.OwnerDocument.CreateElement("SummaryXML")).InnerText = ProjectManager.Project.GetRelativePath(_SummaryXML);
            XmlNode nodDoDs = nodIC.AppendChild(nodIC.OwnerDocument.CreateElement("DoDs"));

            foreach (DoDBase dod in _DoDs)
                nodDoDs.AppendChild(nodParent.OwnerDocument.CreateElement("DoD")).InnerText = dod.Name;
        }

        public override void Delete()
        {
            try
            {
                List<System.Diagnostics.Process> processes = naru.os.FileUtil.WhoIsLocking(_SummaryXML.FullName);
                if (processes.Count > 0)
                {
                    IOException ex = new IOException("The file is in use by another process");
                    ex.Data["Processes"] = string.Join(",", processes.Select(x => x.ProcessName).ToArray());
                    throw ex;
                }

                _SummaryXML.Delete();
            }
            catch (IOException ex)
            {
                ex.Data["Path"] = _SummaryXML.FullName;
                throw;
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error attempting to delete inter-comparison spreadsheet", ex);
                ex2.Data["Path"] = _SummaryXML.FullName;
                throw ex2;
            }

            // Remove the intercomparison from the project
            ProjectManager.Project.InterComparisons.Remove(this);
            ProjectManager.Project.Save();
        }
    }
}
