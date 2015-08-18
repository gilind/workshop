using System.Collections.Generic;

namespace TextComparison.Modifications
{
    partial class Modification
    {
        private class AddedModification : Modification
        {
            public AddedModification(IEnumerable<string> secondaryLines)
                : base(Added, new List<string>(), secondaryLines, EmptyColor, AddedColor)
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
    }
}