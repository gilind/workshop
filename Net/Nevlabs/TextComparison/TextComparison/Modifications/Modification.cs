using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TextComparison.Collections;

namespace TextComparison.Modifications
{
    public abstract class Modification : OwnedItem, ICloneable
    {
        protected static Color DefaultColor = SystemColors.Window;
        protected static Color RemovedColor = Color.FromArgb(243, 230, 216);
        protected static Color ReplacedColor = Color.FromArgb(230, 202, 172);
        protected static Color AddedColor = Color.FromArgb(209, 232, 207);
        protected static Color EmptyColor = Color.FromArgb(208, 220, 234);
        protected static Color MixedColor = Color.FromArgb(255, 208, 116);

        private class NoChangedModification : Modification
        {
            public NoChangedModification(IEnumerable<string> lines)
                : base(NoChanged, lines, lines, DefaultColor, DefaultColor)
            {
            }

            protected override Modification[] DoSplit(int primaryIndex)
            {
                string[] firstLines;
                string[] secondLines;
                SplitLines(Primary.Lines, primaryIndex - Primary.StartIndex, out firstLines, out secondLines);

                return new Modification[]
                {
                    new NoChangedModification(firstLines),
                    new NoChangedModification(secondLines)
                };
            }

            public override object Clone()
            {
                return new NoChangedModification(Primary.Lines);
            }
        }

        private class ReplacedModification : Modification
        {
            public ReplacedModification(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
                : base("Replaced", primaryLines, secondaryLines, ReplacedColor, AddedColor)
            {
            }

            protected override Modification[] DoSplit(int primaryIndex)
            {
                string[] primaryFirstLines;
                string[] primarySecondLines;
                SplitLines(Primary.Lines, primaryIndex - Primary.StartIndex, out primaryFirstLines, out primarySecondLines);

                string[] secondaryFirstLines;
                string[] secondarySecondLines;
                SplitLines(Secondary.Lines, primaryIndex - Secondary.StartIndex, out secondaryFirstLines, out secondarySecondLines);

                return new Modification[]
                {
                    new ReplacedModification(primaryFirstLines, secondaryFirstLines),
                    new ReplacedModification(primarySecondLines, secondarySecondLines)
                };
            }

            public override object Clone()
            {
                return new ReplacedModification(Primary.Lines, Secondary.Lines);
            }
        }

        private class RemovedModification : Modification
        {
            public RemovedModification(IEnumerable<string> primaryLines)
                : base("Removed", primaryLines, new List<string>(), RemovedColor, EmptyColor)
            {
            }

            protected override Modification[] DoSplit(int primaryIndex)
            {
                string[] firstLines;
                string[] secondLines;
                SplitLines(Secondary.Lines, primaryIndex - Secondary.StartIndex, out firstLines, out secondLines);

                return new Modification[]
                {
                    new RemovedModification(firstLines),
                    new RemovedModification(secondLines)
                };
            }

            public override object Clone()
            {
                return new RemovedModification(Primary.Lines);
            }
        }

        private class AddedModification : Modification
        {
            public AddedModification(IEnumerable<string> secondaryLines)
                : base("Added", new List<string>(), secondaryLines, EmptyColor, AddedColor)
            {
            }

            protected override Modification[] DoSplit(int primaryIndex)
            {
                // Added нельзя разделить
                return new Modification[] {this};
            }

            public override object Clone()
            {
                return new AddedModification(Secondary.Lines);
            }
        }

        private class MixedModification : Modification
        {
            private readonly IEnumerable<Modification> _firstModifications;
            private readonly IEnumerable<Modification> _secondModifications;

            public MixedModification(IEnumerable<Modification> firstModifications, IEnumerable<Modification> secondModifications)
                : base("Mixed", null, null, MixedColor, MixedColor)
            {
                _firstModifications = firstModifications;
                _secondModifications = secondModifications;

                Primary = new PrimaryMixedSection(this, firstModifications, MixedColor);
                Secondary = new SecondaryMixedSection(this, firstModifications, secondModifications, MixedColor);
            }

            protected override Modification[] DoSplit(int primaryIndex)
            {
                // Mixed нельзя разделить
                return new Modification[] { this };
            }

            public override object Clone()
            {
                return new MixedModification(_firstModifications, _secondModifications);
            }
        }

        public string Name { get; }

        private const string NoChanged = "NoChanged";

        public bool IsNoChanged
        {
            get { return Name == NoChanged; }
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

        public static Modification CreateNoChanged(IEnumerable<string> lines)
        {
            return new NoChangedModification(lines);
        }

        public static Modification CreateReplaced(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
        {
            return new ReplacedModification(primaryLines, secondaryLines);
        }

        public static Modification CreateRemoved(IEnumerable<string> primaryLines)
        {
            return new RemovedModification(primaryLines);
        }

        public static Modification CreateAdded(IEnumerable<string> secondaryLines)
        {
            return new AddedModification(secondaryLines);
        }

        public static Modification CreateMixed(IEnumerable<Modification> firstModifications, IEnumerable<Modification> secondModifications)
        {
            return new MixedModification(firstModifications, secondModifications);
        }

        protected abstract Modification[] DoSplit(int primaryIndex);

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
    }
}