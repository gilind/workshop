using System.Collections.Generic;

namespace TextComparison
{
    /// <summary>
    /// Область, одновременно представленная в двух файлах - Primary и Secondary
    /// </summary>
    public class Area
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
    }
}