using System;
using System.Collections.Generic;
using System.Drawing;
using TextComparison.Collections;

namespace TextComparison.Modifications
{
    public abstract class Modification : OwnedItem
    {
        protected static Color DefaultColor = SystemColors.Window;
        protected static Color RemovedColor = Color.FromArgb(243, 230, 216);
        protected static Color ReplacedColor = Color.FromArgb(230, 202, 172);
        protected static Color AddedColor = Color.FromArgb(209, 232, 207);
        protected static Color EmptyColor = Color.FromArgb(208, 220, 234);

        private class NoChangedModification : Modification
        {
            public NoChangedModification(IEnumerable<string> lines)
                : base("NoChanged", lines, lines, DefaultColor, DefaultColor)
            {
            }
        }

        private class ReplacedModification : Modification
        {
            public ReplacedModification(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
                : base("Replaced", primaryLines, secondaryLines, ReplacedColor, AddedColor)
            {
            }
        }

        private class RemovedModification : Modification
        {
            public RemovedModification(IEnumerable<string> primaryLines)
                : base("Removed", primaryLines, new List<string>(), RemovedColor, EmptyColor)
            {
            }
        }

        private class AddedModification : Modification
        {
            public AddedModification(IEnumerable<string> secondaryLines)
                : base("Added", new List<string>(), secondaryLines, EmptyColor, AddedColor)
            {
            }
        }

        public string Name { get; }

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

        public Section Primary { get; }

        public Section Secondary { get; }

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

        public override string ToString()
        {
            return $"{Name}, ({Primary.StartIndex}, {Primary.Length}, {Secondary.StartIndex}, {Secondary.Length})";
        }
    }
}