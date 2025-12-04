using System;
using System.Collections.Generic;
using System.IO;

namespace GCDViewer.ProjectTree
{
    public abstract class GISDataset : FileSystemDataset, IGISLayer, IMetadata
    {
        public const string ProgramKey = "_rs_wh_program";
        public const string ProjectKey = "_rs_wh_id";

        public string SymbologyKey { get; private set; }
        public short Transparency { get; private set; }

        public string GISPath { get { return Path.FullName; } }

        public Dictionary<string, string> Metadata { get; internal set; }

        public abstract Uri GISUri { get; }

        public bool HasWarehouseRefernce
        {
            get
            {
                return Metadata != null && Metadata.Count >= 2 &&
                    Metadata.ContainsKey(ProgramKey) && Metadata.ContainsKey(ProjectKey);

            }
        }

        //public Uri WarehouseReference
        //{
        //    get
        //    {
        //        if (!HasWarehouseRefernce)
        //            return null;

        //        Uri baseUri = new Uri(Properties.Resources.DataWarehouseURL);
        //        Uri projectUri = new Uri(baseUri, string.Format("#/{0}/{1}", Metadata[ProgramKey], Metadata[ProjectKey]));
        //        return projectUri;
        //    }
        //}

        public GISDataset(GCDProject project, string name, FileSystemInfo fsInfo, string symbologyKey, short transparency, string image_Exists, string image_Missing, string id, Dictionary<string, string> metadata)
            : base(project, name, fsInfo, image_Exists, image_Missing, id)
        {
            SymbologyKey = symbologyKey;
            Transparency = transparency;

            Metadata = metadata;
        }
    }
}
