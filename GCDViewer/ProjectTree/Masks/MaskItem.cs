using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDViewer.ProjectTree.Masks
{
    public class MaskItem
    {
        public bool Include { get; set; }
        public string FieldValue { get; set; }
        public string Label { get; set; }

        /// <summary>
        /// Default constructor for UI binding
        /// </summary>
        public MaskItem()
        {

        }

        public MaskItem(bool include, string fieldValue, string label)
        {
            Include = include;
            FieldValue = fieldValue;
            Label = label;
        }
    }
}
