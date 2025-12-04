using System;
using System.Collections.Generic;
using System.IO;

namespace GCDViewer.ProjectTree
{
    class Vector : GISDataset, IGISLayer
    {
        public readonly string DefinitionQuery;

        public Vector(GCDProject project, string name, string path, string symbology, short transparency, string id, Dictionary<string, string> metadata, string def_query)
            : base(project, name, new FileInfo(path), symbology, transparency, "polygon16.png", "polygon16.png", id, metadata)
        {
            DefinitionQuery = def_query;
        }

        public override Uri GISUri
        {
            get
            {
                switch (this.WorkspaceType)
                {
                    case GISDataStorageTypes.GeoPackage:
                        string[] parts = this.GISPath.Split("\\");
                        string path = this.GISPath.Substring(0, this.GISPath.IndexOf(".gpkg") + 5);
                        //return new Uri( string.Format("geopackage:///{0}?layer={1}",path , parts[parts.Length-1]));
                        
                        // From Chat GPT:
                        //Uri uri = new Uri("file:///C:/GISData/example.gpkg|buildings");

                        return new Uri(string.Format("{0}\\main.{1}", path, parts[parts.Length-1]));

                    default:
                        return new Uri(GISPath);
                }
            }
        }
    }
}
