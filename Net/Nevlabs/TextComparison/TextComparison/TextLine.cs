using System;

namespace TextComparison
{
    public class TextLine : IComparable
    {
        public string Line { get; }
        private readonly int _hash;

        public TextLine(string line)
        {
            Line = line;
            _hash = CalculateHashCode(line);
        }

        private static int CalculateHashCode(string line)
        {
            int length;

            do
            {
                length = line.Length;
                line = line.Trim().Trim('\t');
            }
            while (length != line.Length);

            return line.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((TextLine) obj)._hash);
        }

        public override string ToString()
        {
            return Line;
        }
    }
}