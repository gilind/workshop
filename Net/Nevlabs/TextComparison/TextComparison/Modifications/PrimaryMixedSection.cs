using System.Collections.Generic;
using System.Drawing;

namespace TextComparison.Modifications
{
    public class PrimaryMixedSection : PrimarySection
    {
        public PrimaryMixedSection(Modification modification, IEnumerable<Modification> firstModifications, Color color)
            : base(modification, null, color)
        {
            List<string> lines = new List<string>();

            foreach (Modification firstModification in firstModifications)
            {
                lines.AddRange(firstModification.Primary.Lines);
            }

            Lines = lines.ToArray();
        }
    }
}