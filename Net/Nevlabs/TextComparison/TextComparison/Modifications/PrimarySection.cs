using System.Collections.Generic;
using System.Drawing;

namespace TextComparison.Modifications
{
    public class PrimarySection : Section
    {
        public PrimarySection(Modification modification, IEnumerable<string> lines, Color color)
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

                Modification previous = (Modification) Modification.Previous;

                return previous.Primary.StartIndex + previous.Primary.Length + 1;
            }
        }
    }
}