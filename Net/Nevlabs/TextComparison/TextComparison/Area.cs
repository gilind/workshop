using System.Collections.Generic;

namespace TextComparison
{
    /// <summary>
    /// �������, ������������ �������������� � ���� ������ - Primary � Secondary
    /// </summary>
    public class Area// todo: ������ �� ����������
    {
        private class SecondaryComparer : IComparer<Area>
        {
            public int Compare(Area x, Area y)
            {
                return x.SecondaryIndex.CompareTo(y.SecondaryIndex);
            }
        }

        public static IComparer<Area> SecondaryIndexComparer
        {
            get { return new SecondaryComparer(); }
        }

        public Area(int primaryIndex, int secondaryIndex, int length)
        {
            PrimaryIndex = primaryIndex;
            SecondaryIndex = secondaryIndex;
            Length = length;
        }

        public int PrimaryIndex { get; }

        public int SecondaryIndex { get; }

        public int Length { get; set; }

        private int EndIndex
        {
            get { return PrimaryIndex + Length; }
        }

        public bool IntersectByPrimary(Area other)//todo: �������
        {
            if (PrimaryIndex == other.PrimaryIndex || 
                PrimaryIndex == other.EndIndex ||
                EndIndex == other.PrimaryIndex ||
                EndIndex == other.EndIndex)
            {
                return true;
            }

            if (PrimaryIndex > other.PrimaryIndex && PrimaryIndex < other.EndIndex ||
                EndIndex > other.PrimaryIndex && EndIndex < other.EndIndex)
            {
                return true;
            }

            if (PrimaryIndex < other.PrimaryIndex && PrimaryIndex < other.EndIndex &&
                EndIndex > other.PrimaryIndex && EndIndex > other.EndIndex)
            {
                return true;
            }

            return false;
        }
    }
}