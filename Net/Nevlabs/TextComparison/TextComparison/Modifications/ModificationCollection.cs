﻿using System.Collections.Generic;
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

        public ModificationCollection(ModificationCollection otherCollection)
        {
            _primary = otherCollection._primary;
            _secondary = otherCollection._secondary;

            foreach (Modification modification in otherCollection)
            {
                Add((Modification) modification.Clone());
            }
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

        public void Split(int targetPrimaryIndex)
        {
            Modification target = FindModificationByPrimaryIndex(targetPrimaryIndex);

            if (target == null)
            {
                return;
            }

            int targetIndex = target.Index;
            Modification[] splited = target.Split(targetPrimaryIndex);

            target.Remove();

            foreach (Modification modification in splited)
            {
                Insert(targetIndex, modification);
                targetIndex++;
            }
        }

        public void Split(IEnumerable<Modification> modifications)
        {
            foreach (Modification modification in modifications)
            {
                Split(modification.Primary.StartIndex);
            }
        }
    }
}