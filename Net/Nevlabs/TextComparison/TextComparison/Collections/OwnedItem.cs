using System;

namespace TextComparison.Collections
{
    [Serializable]
    public class OwnedItem : IOwnedItem
    {
        private IOwner _owner;

        public void SetOwner(IOwner ownerCollection)
        {
            _owner = ownerCollection;
        }

        public virtual void Remove()
        {
            if (_owner != null)
            {
                _owner.RemoveItem(this);
            }

            _owner = null;

            if (Removed != null)
            {
                Removed(this, EventArgs.Empty);
            }
        }

        public event EventHandler Removed;

        public int Index
        {
            get { return _owner == null ? -1 : _owner.GetItemIndex(this); }
        }

        public IOwnedItem Next
        {
            get { return _owner == null ? null : _owner.GetNextItem(this); }
        }

        public IOwnedItem Previous
        {
            get { return _owner == null ? null : _owner.GetPreviousItem(this); }
        }
    }
}