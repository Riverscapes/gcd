namespace GCDCore.UserInterface.UtilityForms
{
    public class ucVectorInput : naru.ui.ucInput
    {
        private GCDConsoleLib.GDalGeometryType.SimpleTypes m_GeometryType;
        private string m_sNoun;
        public string Noun { get { return m_sNoun; } }

        public GCDConsoleLib.Vector SelectedItem
        {
            get
            {
                if (Path is System.IO.FileInfo)
                {
                    return new GCDConsoleLib.Vector(Path);
                }
                else
                {
                    return null;
                }
            }
        }

        private GCDConsoleLib.GDalGeometryType.SimpleTypes GeometryType
        {
            get { return m_GeometryType; }
            set { m_GeometryType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sNoun"></param>
        /// <param name="eBrowseType"></param>
        /// <remarks></remarks>
        public new void Initialize(string sNoun, GCDConsoleLib.GDalGeometryType.SimpleTypes eBrowseType)
        {
            m_sNoun = sNoun;
            GeometryType = eBrowseType;
        }

        private void VectorInputUC_Load(object sender, System.EventArgs e)
        {
        }
        public ucVectorInput()
        {
            Load += VectorInputUC_Load;
        }
    }
}