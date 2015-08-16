namespace TextComparison.Collections
{
    public interface IOwner
    {
        void RemoveItem(IOwnedItem item);

        int GetItemIndex(IOwnedItem item);

        IOwnedItem GetNextItem(IOwnedItem item);

        IOwnedItem GetPreviousItem(IOwnedItem item);
    }
}