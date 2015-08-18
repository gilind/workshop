using System.Collections.Generic;

namespace TextComparison.Modifications
{
    partial class Modification
    {
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
                SplitLines(Primary.Lines, primaryIndex - Primary.StartIndex, out primaryFirstLines,
                    out primarySecondLines);

                string[] secondaryFirstLines;
                string[] secondarySecondLines;
                SplitLines(Secondary.Lines, primaryIndex - Secondary.StartIndex, out secondaryFirstLines,
                    out secondarySecondLines);

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
    }
}