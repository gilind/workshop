using System;
using System.Collections.Generic;
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
            MergedModifications = new ModificationCollection(new TextFile(ServerFile.Name), new TextFile("Merged"));
        }

        public TextFile ServerFile { get; }

        public TextFile User1File { get; }

        public TextFile User2File { get; }

        public ModificationCollection ServerUser1Modifications { get; private set; }

        public ModificationCollection ServerUser2Modifications { get; private set; }

        public ModificationCollection MergedModifications { get; }

        public event EventHandler StateChanged;

        public void ExecuteComapare()
        {
            FileComparer fileComparer = new FileComparer();

            ServerUser1Modifications = fileComparer.Compare(ServerFile, User1File);
            ServerUser2Modifications = fileComparer.Compare(ServerFile, User2File);

            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        public void ExecuteMerge()
        {
            MergedModifications.Initialize(ServerUser1Modifications);
            MergedModifications.Split(ServerUser2Modifications);
            MergeLargeWithSmall();
            MergeSmallWithLarge();
            MergedModifications.GenerateFiles();
        }

        private void MergeLargeWithSmall()
        {
            bool startMixed = false;
            bool endMixed = false;

            for (int mergedIndex = 0; mergedIndex < MergedModifications.Count; mergedIndex++)
            {
                Modification currentMerged = MergedModifications[mergedIndex];

                if (currentMerged.IsNoChanged)
                {
                    continue;
                }

                IList<Modification> mixed = new List<Modification>();

                for (int user2Index = 0; user2Index < ServerUser2Modifications.Count; user2Index++)
                {
                    Modification currentUser2 = ServerUser2Modifications[user2Index];

                    if (currentUser2.IsNoChanged)
                    {
                        continue;
                    }

                    if (currentMerged.Primary.StartIndex == currentUser2.Primary.StartIndex)
                    {
                        startMixed = true;
                    }

                    if (startMixed)
                    {
                        mixed.Add(currentUser2);
                    }

                    if (startMixed &&
                        currentMerged.Primary.StartIndex + currentMerged.Primary.Length ==
                        currentUser2.Primary.StartIndex + currentUser2.Primary.Length)
                    {
                        endMixed = true;
                        currentUser2.Remove();
                        break;
                    }
                }

                if (endMixed)
                {
                    currentMerged.Remove();

                    MergedModifications.AddMixed(new List<Modification> {currentMerged}, mixed);
                }

                startMixed = false;
                endMixed = false;
            }
        }

        private void MergeSmallWithLarge()
        {
            bool startMixed = false;
            bool endMixed = false;

            for (int user2Index = 0; user2Index < ServerUser2Modifications.Count; user2Index++)
            {
                Modification currentUser2 = ServerUser2Modifications[user2Index];

                if (currentUser2.IsNoChanged)
                {
                    continue;
                }

                IList<Modification> mixed = new List<Modification>();

                for (int mergedIndex = 0; mergedIndex < MergedModifications.Count; mergedIndex++)
                {
                    Modification currentMerged = MergedModifications[mergedIndex];

                    if (currentMerged.IsNoChanged)
                    {
                        continue;
                    }

                    if (currentMerged.Primary.StartIndex == currentUser2.Primary.StartIndex)
                    {
                        startMixed = true;
                    }

                    if (startMixed)
                    {
                        mixed.Add(currentMerged);
                    }

                    if (startMixed &&
                        currentMerged.Primary.StartIndex + currentMerged.Primary.Length ==
                        currentUser2.Primary.StartIndex + currentUser2.Primary.Length)
                    {
                        endMixed = true;
                        currentMerged.Remove();
                        break;
                    }
                }

                if (endMixed)
                {
                    currentUser2.Remove();

                    MergedModifications.AddMixed(mixed, new List<Modification> {currentUser2});
                }

                startMixed = false;
                endMixed = false;
            }
        }
    }
}