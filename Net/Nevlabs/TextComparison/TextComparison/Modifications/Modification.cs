using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TextComparison.Collections;

namespace TextComparison.Modifications
{
    public abstract partial class Modification : OwnedItem, ICloneable
    {
        protected static Color DefaultColor = SystemColors.Window;
        protected static Color RemovedColor = Color.FromArgb(243, 230, 216);
        protected static Color ReplacedColor = Color.FromArgb(230, 202, 172);
        protected static Color AddedColor = Color.FromArgb(209, 232, 207);
        protected static Color EmptyColor = Color.FromArgb(208, 220, 234);
        protected static Color MixedColor = Color.FromArgb(255, 208, 116);

        public string Name { get; }

        private const string NoChanged = "NoChanged";
        private const string Added = "Added";

        public bool IsNoChanged
        {
            get { return Name == NoChanged; }
        }

        public bool IsAdded
        {
            get { return Name == Added; }
        }

        protected Modification(
            string name,
            IEnumerable<string> primaryLines,
            IEnumerable<string> secondaryLines,
            Color primaryColor,
            Color secondaryColor)
        {
            Name = name;
            Primary = new PrimarySection(this, primaryLines, primaryColor);
            Secondary = new SecondarySection(this, secondaryLines, secondaryColor);
        }

        public Section Primary { get; protected set; }

        public Section Secondary { get; protected set; }

        public int Length
        {
            get { return Math.Max(Primary.Length, Secondary.Length); }
        }

        protected abstract Modification[] DoSplit(int primaryIndex);

        protected virtual Modification[] DoMerge(Modification other)
        {
            if (other.IsNoChanged)
            {
                return new[] {this};
            }

            return new[] {new MixedModification(new[] {this}, new[] {other})};
        }

        public override string ToString()
        {
            return $"{Name}, ({Primary.StartIndex}, {Primary.Length}, {Secondary.StartIndex}, {Secondary.Length})";
        }

        public abstract object Clone();

        protected void SplitLines(IList<string> lines, int index, out string[] first, out string[] second)
        {
            IList<string> firstResult = new List<string>();

            for (int lineIndex = 0; lineIndex < index; lineIndex++)
            {
                firstResult.Add(lines[lineIndex]);
            }

            first = firstResult.ToArray();

            IList<string> secondResult = new List<string>();

            for (int lineIndex = index; lineIndex < lines.Count; lineIndex++)
            {
                secondResult.Add(lines[lineIndex]);
            }

            second = secondResult.ToArray();
        }

        public Modification[] Split(int primaryIndex)
        {
            IList<Modification> result = new List<Modification>();

            if (primaryIndex <= Primary.StartIndex || primaryIndex >= Primary.StartIndex + Primary.Length)
            {
                result.Add(this);

                return result.ToArray();
            }

            return DoSplit(primaryIndex);
        }

        public Modification[] Merge(Modification other)
        {
            IList<Modification> result = new List<Modification>();

            if (other.Primary.StartIndex != Primary.StartIndex)
            {
                result.Add(this);

                return result.ToArray();
            }

            return DoMerge(other);
        }
    }
}