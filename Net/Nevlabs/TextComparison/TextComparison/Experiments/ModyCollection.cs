using TextComparison.Collections;

namespace TextComparison.Experiments
{
    public class ModyCollection : OwnedItemCollection<Mody>
    {
        private readonly TextFile _primary;
        private readonly TextFile _secondary;

        public ModyCollection()
            : this(null, null)
        {
        }

        public ModyCollection(TextFile primary, TextFile secondary)
        {
            _primary = primary;
            _secondary = secondary;
        }

        public void AddNoChanged(int primaryIndex, int secondaryIndex, int length)
        {
            string[] lines = _primary.GetRange(primaryIndex, length);
            Add(Mody.CreateNoChanged(lines));
        }

        //public static Mody CreateNoChanged(Area area)
        //{
        //    return new Modification(ModificationType.NoChanged, area.PrimaryIndex, area.SecondaryIndex, area.Length/*, new List<string>()*/);
        //}

        public void AddReplaced(int primaryIndex, int secondaryIndex, int length)
        {
            string[] primaryLines = _primary.GetRange(primaryIndex, length);
            string[] secondaryLines = _secondary.GetRange(secondaryIndex, length);
            Add(Mody.CreateReplaced(primaryLines, secondaryLines));
        }

        public void AddRemoved(int primaryIndex, int length)
        {
            string[] primaryLines = _primary.GetRange(primaryIndex, length);
            Add(Mody.CreateRemoved(primaryLines));
        }

        public void AddAdded(int secondaryIndex, int length)
        {
            string[] secondaryLines = _primary.GetRange(secondaryIndex, length);
            Add(Mody.CreateAdded(secondaryLines));
        }
    }
}