using GCDCore.Project;
using System;

namespace GCDCore.UserInterface
{
    /// <summary>
    /// Class to manage the items put in a DataGridView that simply need a property and value pair.
    /// </summary>
    /// <remarks>
    /// Header items have no value string and these are displayed in bold. Items with a value string
    /// are indented by the software left indent setting</remarks>
    public class GridViewPropertyValueItem
    {
        public string Property { get; protected set; }
        public string Value { get; protected set; }
        public readonly bool Header;

        // Non-header cells are indented
        public int LeftPadding { get { return Header ? 0 : GCDCore.Properties.Settings.Default.PropertyGridLeftIndent; } }

        /// <summary>
        /// Default constructor required for grid binding
        /// </summary>
        public GridViewPropertyValueItem()
        {

        }

        /// <summary>
        /// Constructor for regular NON-HEADER items
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public GridViewPropertyValueItem(string prop, string value)
        {
            Property = prop;
            Value = value;
            Header = false;
        }

        /// <summary>
        /// Constructor for headers only
        /// </summary>
        /// <param name="header"></param>
        public GridViewPropertyValueItem(string header)
        {
            Property = header;
            Value = string.Empty;
            Header = true;
        }
    }

    public class GridViewGCDProjectItem : GridViewPropertyValueItem
    {
        public readonly GCDProjectItem ProjectItem;

        public GridViewGCDProjectItem(string prop, string value, GCDProjectItem item)
            : base(prop, value)
        {
            ProjectItem = item;

            if (item is GCDProjectRasterItem)
                Value = ProjectManager.Project.GetRelativePath(((GCDProjectRasterItem)item).Raster.GISFileInfo);
            else if (item is GCDProjectVectorItem)
                Value = ProjectManager.Project.GetRelativePath(((GCDProjectVectorItem)item).Vector.GISFileInfo);
        }

        public GridViewGCDProjectItem(GCDProjectItem item)
            : base(item.Noun, string.Empty)
        {
            ProjectItem = item;

            if (item is GCDProjectRasterItem)
                Value = ProjectManager.Project.GetRelativePath(((GCDProjectRasterItem)item).Raster.GISFileInfo);
            else if (item is GCDProjectVectorItem)
                Value = ProjectManager.Project.GetRelativePath(((GCDProjectVectorItem)item).Vector.GISFileInfo);
        }
    }
}
