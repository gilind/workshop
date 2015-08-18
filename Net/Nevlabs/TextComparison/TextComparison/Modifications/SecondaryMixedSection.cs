using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TextComparison.Modifications
{
    public class SecondaryMixedSection : SecondarySection
    {
        public SecondaryMixedSection(Modification modification, IEnumerable<Modification> firstModifications, IEnumerable<Modification> secondModifications, Color color)
            : base(modification, null, color)
        {
            IList<string> lines = new List<string>();

            foreach (Modification firstModification in firstModifications)
            {
                foreach (string line in firstModification.Secondary.Lines)
                {
                    lines.Add("User1:" + line);
                }
            }

            foreach (Modification secondModification in secondModifications)
            {
                foreach (string line in secondModification.Secondary.Lines)
                {
                    lines.Add("User2:" + line);
                }
            }

            Lines = lines.ToArray();
        }
    }
}