using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.ErrorCalculation.FIS
{
    /// <summary>
    /// Used for FIS library item publications and example datasets
    /// </summary>
    public class FISMetaItem
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Title;
        }

        // Default constructor needed for binding
        public FISMetaItem()
        {

        }

        public FISMetaItem(string title, string value)
        {
            Title = title;
            Value = value;
        }
    }
}
