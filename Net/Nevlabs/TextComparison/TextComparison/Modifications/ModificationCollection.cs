using System.Collections.Generic;
using TextComparison.Collections;

namespace TextComparison.Modifications
{
    public class ModificationCollection : OwnedItemCollection<Modification>
    {
        public TextFile Primary { get; }
        public TextFile Secondary { get; }

        public ModificationCollection()
            : this(new TextFile(), new TextFile())
        {
        }

        public ModificationCollection(TextFile primary, TextFile secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }

        public ModificationCollection(ModificationCollection otherCollection)
        {
            Primary = otherCollection.Primary;
            Secondary = otherCollection.Secondary;

            Initialize(otherCollection);
        }

        public void Initialize(IEnumerable<Modification> otherCollection)
        {
            Clear();

            foreach (Modification modification in otherCollection)
            {
                Add((Modification)modification.Clone());
            }
        }

        public void AddNoChanged(int primaryIndex, int length)
        {
            string[] lines = Primary.GetRange(primaryIndex, length);
            Add(Modification.CreateNoChanged(lines));
        }

        public void AddReplaced(int primaryIndex, int secondaryIndex, int length)
        {
            string[] primaryLines = Primary.GetRange(primaryIndex, length);
            string[] secondaryLines = Secondary.GetRange(secondaryIndex, length);
            Add(Modification.CreateReplaced(primaryLines, secondaryLines));
        }

        public void AddRemoved(int primaryIndex, int length)
        {
            string[] primaryLines = Primary.GetRange(primaryIndex, length);
            Add(Modification.CreateRemoved(primaryLines));
        }

        public void AddAdded(int secondaryIndex, int length)
        {
            string[] secondaryLines = Secondary.GetRange(secondaryIndex, length);
            Add(Modification.CreateAdded(secondaryLines));
        }

        public Modification FindModificationByPrimaryIndex(int targetPrimaryIndex)
        {
            return FindModificationByPrimaryIndex(new Modification[0], targetPrimaryIndex);

            //foreach (Modification modification in Items)
            //{
            //    if (targetPrimaryIndex == modification.Primary.StartIndex ||
            //        targetPrimaryIndex > modification.Primary.StartIndex &&
            //        targetPrimaryIndex < modification.Primary.StartIndex + modification.Primary.Length)
            //    {
            //        return modification;
            //    }
            //}

            //return null;
        }

        public Modification FindModificationByPrimaryIndex(ICollection<Modification> exceptList, int targetPrimaryIndex)
        {
            foreach (Modification modification in Items)
            {
                if (exceptList.Contains(modification))
                {
                    continue;
                }

                if (targetPrimaryIndex == modification.Primary.StartIndex ||
                    targetPrimaryIndex > modification.Primary.StartIndex &&
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

        public void GenerateFiles()
        {
            Primary.Lines.Clear();
            Secondary.Lines.Clear();

            foreach (Modification modification in Items)
            {
                foreach (string line in modification.Primary.Lines)
                {
                    Primary.Lines.Add(new TextLine(line));
                }

                foreach (string line in modification.Secondary.Lines)
                {
                    Secondary.Lines.Add(new TextLine(line));
                }
            }
        }

    }
}