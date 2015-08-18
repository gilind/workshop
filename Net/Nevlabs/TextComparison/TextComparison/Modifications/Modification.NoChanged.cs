using System.Collections.Generic;
using System.Linq;

namespace TextComparison.Modifications
{
    partial class Modification
    {
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

            protected override Modification[] DoMerge(Modification other)
            {
                IList<Modification> result = new List<Modification>();

                if (other.IsAdded)
                {
                    // отдельная обработка добавленных
                    result.Add(other);

                    if (other.Next != null && ((Modification) other.Next).Primary.StartIndex == Primary.StartIndex)
                    {
                        result.Add((Modification)other.Next);
                    }

                    return result.ToArray();
                }

                return new[] {other};
            }

            public override object Clone()
            {
                return new NoChangedModification(Primary.Lines);
            }
        }
    }
}