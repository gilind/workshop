using System.Collections.Generic;
using System.Drawing;

namespace TextComparison.Modifications
{
    public class SecondarySection : Section
    {
        public SecondarySection(Modification modification, IEnumerable<string> lines, Color color)
            : base(modification, lines, color)
        {
        }

        public override int StartIndex
        {
            get
            {
                if (Modification.Previous == null)
                {
                    return 0;
                }

                Modification previous = (Modification)Modification.Previous;

                return previous.Secondary.StartIndex + previous.Secondary.Length + 1;
            }
        }
    }
}
