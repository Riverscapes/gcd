namespace GCDCore.UserInterface.Project
{
    public class ProjectTreeNode
    {
        public enum GCDNodeTypes
        {
            Project,
            InputsGroup,
            SurveysGroup,
            DEMSurvey,
            AssocGroup,
            AssociatedSurface,
            ErrorSurfaceGroup,
            ErrorSurface,
            ReferenceSurfaceGroup,
            ReferenceSurface,
            MasksGroup,
            Mask,
            AOIGroup,
            AOI,
            AnalysesGroup,
            ChangeDetectionGroup,
            ChangeDetectionDEMPair,
            DoD,
            BudgetSegregationGroup,
            BudgetSegregationMask,
            BudgetSegregation,
            MorphologicalAnalysisGroup,
            MorphologicalAnalysis,
            InterComparisonGroup,
            InterComparison,
            ReservoirGroup,
            Reservoir
        }

        private GCDNodeTypes m_Type;

        private object m_Item;
        public object Item
        {
            get { return m_Item; }
        }

        public GCDNodeTypes NodeType
        {
            get { return m_Type; }
        }

        public override string ToString()
        {
            return Item.ToString();
        }

        public string Name
        {
            get
            {
                string name = NodeType.ToString();
                if (Item != null)
                {
                    name = string.Format("{0}_{1}", name.ToString());
                }

                return name;
            }
        }

        public ProjectTreeNode(GCDNodeTypes type, object theItem)
        {
            m_Type = type;
            m_Item = theItem;
        }

        public bool Equals(ProjectTreeNode obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (NodeType == obj.NodeType)
                {
                    return object.ReferenceEquals(Item, obj.Item);
                }
                else
                {
                    return false;
                }
            }
        }        
    }
}