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
            AssociatedSurfaceGroup,
            AssociatedSurface,
            ErrorSurfaceGroup,
            ErrorSurface,
            AOIGroup,
            AOI,
            AnalysesGroup,
            ChangeDetectionGroup,
            ChangeDetectionDEMPair,
            DoD,
            BudgetSegregationGroup,
            BudgetSegregationMask,
            BudgetSegregation,
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

        public ProjectTreeNode(GCDNodeTypes type, object theItem)
        {
            m_Type = type;
            m_Item = theItem;
        }

        public static bool operator ==(ProjectTreeNode node1, ProjectTreeNode node2)
        {

            if (node1 == null && node2 == null)
            {
                return true;
            }
            else if (node1 == null)
            {
                return false;
            }
            else if (node2 == null)
            {
                return false;
            }
            else
            {
                if ((node1.NodeType == node2.NodeType))
                {
                    return object.ReferenceEquals(node1.Item, node2.Item);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool operator !=(ProjectTreeNode node1, ProjectTreeNode node2)
        {
            return !(node1 == node2);
        }
    }
}