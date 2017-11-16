using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.BudgetSegregation
{
    public class BSResultSet
    {
        public DirectoryInfo Folder { get; internal set; }
        public Dictionary<string, BSResult> ClassResults { get; internal set; }
        public FileInfo PolygonMask { get { return naru.os.File.GetNewSafeName(Folder.FullName, "Mask", "shp"); } }
        public string FieldName { get; internal set; }

        public BSResultSet(DirectoryInfo folder, string sFieldName)
        {
            Folder = folder;
            FieldName = sFieldName;
            ClassResults = new Dictionary<string, BSResult>();
        }
    }
}
