using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TextComparison.Collections;

namespace TextComparison.Experiments
{
    public abstract class Section
    {
        protected readonly Mody Mody;

        public Section(Mody mody, IEnumerable<string> lines, Color color)
        {
            Mody = mody;
            Color = color;
            Lines = lines.ToArray();
        }

        public abstract int StartIndex { get; }

        //public int EndIndex
        //{
        //    get { return StartIndex + Length; }
        //}

        public int Length
        {
            get { return Lines.Length; }
        }

        public string[] Lines { get; }

        public Color Color { get; }
    }

    public class PrimarySection : Section
    {
        public PrimarySection(Mody mody, IEnumerable<string> lines, Color color)
            : base(mody, lines, color)
        {
        }

        public override int StartIndex
        {
            get
            {
                if (Mody.Previous == null)
                {
                    return 0;
                }

                Mody previous = (Mody) Mody.Previous;

                return previous.Primary.StartIndex + previous.Primary.Length + 1;
            }
        }
    }

    public class SecondarySection : Section
    {
        public SecondarySection(Mody mody, IEnumerable<string> lines, Color color)
            : base(mody, lines, color)
        {
        }

        public override int StartIndex
        {
            get
            {
                if (Mody.Previous == null)
                {
                    return 0;
                }

                Mody previous = (Mody)Mody.Previous;

                return previous.Secondary.StartIndex + previous.Secondary.Length + 1;
            }
        }
    }

    public abstract class Mody : OwnedItem
    {
        private class NoChangedMody : Mody
        {
            public NoChangedMody(IEnumerable<string> lines)
                : base(lines, lines, DefaultColor, DefaultColor)
            {
            }
        }

        private class ReplacedMody : Mody
        {
            public ReplacedMody(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
                : base(primaryLines, secondaryLines, DefaultColor, DefaultColor)
            {
            }
        }

        private class RemovedMody : Mody
        {
            public RemovedMody(IEnumerable<string> primaryLines)
                : base(primaryLines, new List<string>(), DefaultColor, DefaultColor)
            {
            }
        }

        private class AddedMody : Mody
        {
            public AddedMody(IEnumerable<string> secondaryLines)
                : base(new List<string>(), secondaryLines, DefaultColor, DefaultColor)
            {
            }
        }

        protected static Color DefaultColor = Color.White;

        protected Mody(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines, Color primaryColor,
            Color secondaryColor)
        {
            Primary = new PrimarySection(this, primaryLines, primaryColor);
            Secondary = new SecondarySection(this, secondaryLines, secondaryColor);
        }

        public Section Primary { get; }

        public Section Secondary { get; }

        public static Mody CreateNoChanged(IEnumerable<string> lines)
        {
            return new NoChangedMody(lines);
        }

        public static Mody CreateReplaced(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
        {
            return new ReplacedMody(primaryLines, secondaryLines);
        }

        public static Mody CreateRemoved(IEnumerable<string> primaryLines)
        {
            return new RemovedMody(primaryLines);
        }

        public static Mody CreateAdded(IEnumerable<string> secondaryLines)
        {
            return new AddedMody(secondaryLines);
        }
    }
}