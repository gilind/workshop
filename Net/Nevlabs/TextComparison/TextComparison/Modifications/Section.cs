using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TextComparison.Modifications
{
    public abstract class Section
    {
        protected readonly Modification Modification;

        protected Section(Modification modification, IEnumerable<string> lines, Color color)
        {
            Modification = modification;
            Color = color;
            Lines = lines == null ? new string[0] : lines.ToArray();
        }

        public abstract int StartIndex { get; }

        public int Length
        {
            get { return Lines.Length; }
        }

        public string[] Lines { get; protected set; }

        public Color Color { get; }
    }
}