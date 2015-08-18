using System.Collections.Generic;

namespace TextComparison.Modifications
{
    partial class Modification
    {
        public static Modification CreateNoChanged(IEnumerable<string> lines)
        {
            return new NoChangedModification(lines);
        }

        public static Modification CreateReplaced(IEnumerable<string> primaryLines, IEnumerable<string> secondaryLines)
        {
            return new ReplacedModification(primaryLines, secondaryLines);
        }

        public static Modification CreateRemoved(IEnumerable<string> primaryLines)
        {
            return new RemovedModification(primaryLines);
        }

        public static Modification CreateAdded(IEnumerable<string> secondaryLines)
        {
            return new AddedModification(secondaryLines);
        }

        //public static Modification CreateMixed(IEnumerable<Modification> firstModifications,
        //    IEnumerable<Modification> secondModifications)
        //{
        //    return new MixedModification(firstModifications, secondModifications);
        //}
    }
}