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
            MergedModifications = new List<Modification>();
        }

        public TextFile ServerFile { get; }

        public TextFile User1File { get; }

        public TextFile User2File { get; }

        public ModificationCollection ServerUser1Modifications { get; private set; }

        public ModificationCollection ServerUser2Modifications { get; private set; }

        public List<Modification> MergedModifications { get; }

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
            //MarkConflicts();
            MergeModification();
            Поехали();
        }

        private void Поехали()
        {
            //// на первый список изменений начинаем накладывать второй
            //foreach (var user2Modification in ServerUser2Modifications)
            //{
            //    if (user2Modification.Type == ModificationType.NoChanged)
            //    {
            //        continue;
            //    }

            //    int start = user2Modification.PrimaryIndex;
            //    int end = user2Modification.PrimaryIndex + user2Modification.Length;

            //    Modification startModification = null;
            //    Modification endModification = null;

            //    // найти среди первых изменений, с которой пересекается текущая вторая
            //    foreach (var user1Modification in ServerUser1Modifications)
            //    {
            //        if (user1Modification.Contains(start))
            //        {
            //            startModification = user1Modification;
            //            break;
            //        }
            //    }

            //    foreach (var user1Modification in ServerUser2Modifications)
            //    {
            //        if (user1Modification.Contains(start))
            //        {
            //            startModification = user1Modification;
            //            break;
            //        }
            //    }


            //}

        }

        private void MarkConflicts()
        {
            //foreach (Modification user1Modification in ServerUser1Modifications)
            //{
            //    if (user1Modification.Type == ModificationType.NoChanged ||
            //        user1Modification.Type == ModificationType.Added)
            //    {
            //        continue;    
            //    }

            //    foreach (Modification user2Modification in ServerUser2Modifications)
            //    {
            //        if (user2Modification.Type == ModificationType.NoChanged ||
            //            user2Modification.Type == ModificationType.Added)
            //        {
            //            continue;
            //        }

            //        if (user1Modification.IntersectByPrimary(user2Modification))
            //        {
            //            user1Modification.HasConflict = true;
            //            user2Modification.HasConflict = true;
            //        }
            //    }
            //}
        }

        private void MergeModification()
        {
            MergedModifications.Clear();

            foreach (Modification user1Modification in ServerUser1Modifications)
            {
                //if (user1Modification.Type == ModificationType.NoChanged)
                //{
                //    continue;
                //}

                MergedModifications.Add(user1Modification);
            }

            foreach (Modification user2Modification in ServerUser2Modifications)
            {
                //if (user2Modification.Type == ModificationType.NoChanged)
                //{
                //    continue;
                //}

                MergedModifications.Add(user2Modification);
            }

            //MergedModifications.Sort(Modification.PrimaryIndexComparer);
        }
    }
}