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
            
        }

       
    }
}