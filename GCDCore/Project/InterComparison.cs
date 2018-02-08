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

        public InterComparison(string name, FileInfo summaryXML, List<DoDBase> dods)
            : base(name)
        {
            _SummaryXML = summaryXML;
            _DoDs = new naru.ui.SortableBindingList<DoDBase>(dods);
        }

        public InterComparison(XmlNode nodParent, Dictionary<string, DoDBase> dods)
            : base(nodParent.SelectSingleNode("Name").InnerText)
        {
            _SummaryXML = ProjectManager.Project.GetAbsolutePath(nodParent.SelectSingleNode("SummaryXML").InnerText);

            _DoDs = new naru.ui.SortableBindingList<DoDBase>();
            foreach (XmlNode nodDoD in nodParent.SelectNodes("DoDs/DoD"))
            {
                string dodName = nodDoD.InnerText;
                if (dods.ContainsKey(dodName))
                {
                    _DoDs.Add(dods[dodName]);
                }
                else
                {
                    // TODO: don't really know what to do here? Should not occur if deleting DoDs checks intercomparisons first!
                    throw new Exception(string.Format("Unable to find DoD \"{0}\" that's part of inter-comparison.", dodName));
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
    }
}
