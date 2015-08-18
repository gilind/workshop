using System;
using System.Collections.Generic;
using System.Linq;
using TextComparison.Modifications;

namespace TextComparison
{
    public class Merger
    {
        public Merger()
        {
            ServerFile = new TextFile();
            User1File = new TextFile();
            User2File = new TextFile();

            ServerUser1Modifications = new ModificationCollection(ServerFile, User1File);
            ServerUser2Modifications = new ModificationCollection(ServerFile, User2File);
            MergedModifications = new ModificationCollection();
        }

        public TextFile ServerFile { get; }

        public TextFile User1File { get; }

        public TextFile User2File { get; }

        public ModificationCollection ServerUser1Modifications { get; private set; }

        public ModificationCollection ServerUser2Modifications { get; private set; }

        public ModificationCollection MergedModifications { get; private set; }

        public event EventHandler StateChanged;

        private void RaiseStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        public void ExecuteComapare()
        {
            FileComparer fileComparer = new FileComparer();

            ServerUser1Modifications = fileComparer.Compare(ServerFile, User1File);
            ServerUser2Modifications = fileComparer.Compare(ServerFile, User2File);
            MergedModifications = new ModificationCollection();

            RaiseStateChanged();
        }

        public void ExecuteMerge()
        {
            MergedModifications = new ModificationCollection(new TextFile(ServerFile.Name), new TextFile("Merged"));
            MergedModifications.Initialize(ServerUser1Modifications);
            MergedModifications.Split(ServerUser2Modifications);

            ModificationCollection temporary = new ModificationCollection(ServerUser2Modifications);
            temporary.Split(MergedModifications);

            ICollection<Modification> processed = new List<Modification>();

            for (int mergedIndex = 0; mergedIndex < MergedModifications.Count; mergedIndex++)
            {
                Modification currentMerged = MergedModifications[mergedIndex];

                Modification currentUser2 = temporary.FindModificationByPrimaryIndex(processed, currentMerged.Primary.StartIndex);

                if (currentUser2 == null)
                {
                    continue;
                }

                processed.Add(currentUser2);

                Modification[] merged = currentMerged.Merge(currentUser2);

                currentMerged.Remove();

                foreach (Modification mergedModification in merged)
                {
                    MergedModifications.Insert(mergedIndex, mergedModification);
                    mergedIndex++;
                }

                mergedIndex--;
            }

            MergedModifications.GenerateFiles();

            RaiseStateChanged();
        }

        private bool IsFirstMoreSecond(ModificationCollection first, ModificationCollection second)
        {
            if (first.Count == 0)
            {
                return second.Count == 0;
            }

            if (second.Count == 0)
            {
                return true;
            }

            Modification firstLast = first.Last();

            Modification secondLast = second.Last();

            return firstLast.Secondary.StartIndex + firstLast.Secondary.Length >
                   secondLast.Secondary.StartIndex + secondLast.Secondary.Length;
        }
    }
}