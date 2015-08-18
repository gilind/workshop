using System.Collections.Generic;

namespace TextComparison.Modifications
{
    partial class Modification
    {
        private class MixedModification : Modification
        {
            private readonly IEnumerable<Modification> _firstModifications;
            private readonly IEnumerable<Modification> _secondModifications;

            public MixedModification(IEnumerable<Modification> firstModifications,
                IEnumerable<Modification> secondModifications)
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
                return new Modification[] {this};
            }

            protected override Modification[] DoMerge(Modification other)
            {
                // выходит за рамки задачи
                throw new System.NotImplementedException();
            }

            public override object Clone()
            {
                return new MixedModification(_firstModifications, _secondModifications);
            }
        }
    }
}