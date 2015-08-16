using System;
using System.Collections.ObjectModel;

namespace TextComparison.Collections
{
    [Serializable]
    public class OwnedItemCollection<TItem> : Collection<TItem>, IOwner
        where TItem : class, IOwnedItem
    {
        private bool _stopEvents;

        void IOwner.RemoveItem(IOwnedItem item)
        {
            _stopEvents = true;

            Remove((TItem) item);

            _stopEvents = false;
        }

        public int GetItemIndex(IOwnedItem item)
        {
            return IndexOf((TItem)item);
        }

        IOwnedItem IOwner.GetNextItem(IOwnedItem item)
        {
            int index = IndexOf((TItem)item);

            return index + 1 >= Count ? null : this[index + 1];
        }

        IOwnedItem IOwner.GetPreviousItem(IOwnedItem item)
        {
            int index = IndexOf((TItem)item);

            return index - 1 < 0 ? null : this[index - 1];
        }

        protected override void InsertItem(int index, TItem item)
        {
            if (item == null)
            {
                return;
            }

            item.SetOwner(this);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);

            if (_stopEvents)
            {
                return;
            }

            item.Remove();
        }

        protected override void ClearItems()
        {
            while (Items.Count > 0)
            {
                Items[0].Remove();
            }

            base.ClearItems();
        }


    }
}