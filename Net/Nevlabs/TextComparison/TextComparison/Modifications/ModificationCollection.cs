using TextComparison.Collections;

namespace TextComparison.Modifications
{
    public class ModificationCollection : OwnedItemCollection<Modification>
    {
        private readonly TextFile _primary;
        private readonly TextFile _secondary;

        public ModificationCollection()
            : this(new TextFile(), new TextFile())
        {
        }

        public ModificationCollection(TextFile primary, TextFile secondary)
        {
            _primary = primary;
            _secondary = secondary;
        }

        public void AddNoChanged(int primaryIndex, int length)
        {
            string[] lines = _primary.GetRange(primaryIndex, length);
            Add(Modification.CreateNoChanged(lines));
        }

        public void AddReplaced(int primaryIndex, int secondaryIndex, int length)
        {
            string[] primaryLines = _primary.GetRange(primaryIndex, length);
            string[] secondaryLines = _secondary.GetRange(secondaryIndex, length);
            Add(Modification.CreateReplaced(primaryLines, secondaryLines));
        }

        public void AddRemoved(int primaryIndex, int length)
        {
            string[] primaryLines = _primary.GetRange(primaryIndex, length);
            Add(Modification.CreateRemoved(primaryLines));
        }

        public void AddAdded(int secondaryIndex, int length)
        {
            string[] secondaryLines = _secondary.GetRange(secondaryIndex, length);
            Add(Modification.CreateAdded(secondaryLines));
        }

        public Modification FindModificationByPrimaryIndex(int targetPrimaryIndex)
        {
            foreach (Modification modification in Items)
            {
                if (modification.Primary.StartIndex <= targetPrimaryIndex &&
                    targetPrimaryIndex < modification.Primary.StartIndex + modification.Primary.Length)
                {
                    return modification;
                }
            }

            return null;
        }
    }
}