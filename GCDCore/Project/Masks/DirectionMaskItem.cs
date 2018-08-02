using System;

namespace GCDCore.Project.Masks
{
    public class DirectionMaskItem : MaskItem, IComparable<DirectionMaskItem>
    {
        public readonly int Direction;

        public DirectionMaskItem(bool include, string fieldValue, string label, int direction)
            : base(include, fieldValue, label)
        {
            Direction = direction;
        }

        /// <summary>
        /// Sort direction mask items by their direction
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DirectionMaskItem other)
        {
            return Direction - other.Direction;
        }
    }
}
