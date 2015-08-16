using System;
using System.Collections.ObjectModel;

namespace TextComparison.Collections
{
    public interface IOwnedItem
    {
        void SetOwner(IOwner ownerCollection);

        void Remove();

        event EventHandler Removed;

        int Index { get; }

        IOwnedItem Previous { get; }

        IOwnedItem Next { get; }
    }
}