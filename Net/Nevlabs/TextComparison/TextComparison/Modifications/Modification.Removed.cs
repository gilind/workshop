using System.Collections.Generic;

namespace TextComparison.Modifications
{
    partial class Modification
    {
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
    }
}