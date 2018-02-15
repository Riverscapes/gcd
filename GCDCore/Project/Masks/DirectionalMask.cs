using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.Masks
{
    public class DirectionalMask : Mask
    {
        public readonly string LabelField;
        public readonly string DirectionField;
        public readonly string DistanceField;

        public DirectionalMask(string name, FileInfo shapefile, string field, string labelField, string directionField, string distanceField)
            : base(name, shapefile, field)
        {
            LabelField = labelField;
            DirectionField = directionField;
            DistanceField = distanceField;
        }
    }
}
