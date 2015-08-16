using System.Collections.Generic;
using System.IO;

namespace TextComparison
{
    public class TextFile
    {
        public IList<TextLine> Lines { get; }

        public TextFile() : this(string.Empty)
        {
        }

        public TextFile(string name)
        {
            Name = name;
            Lines = new List<TextLine>();
        }

        public string Name { get; set; }

        public void Load(string fileName)
        {
            Lines.Clear();
            Name = fileName;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Lines.Add(new TextLine(line));
                }
            }
        }

        public void AddLine(string line)
        {
            Lines.Add(new TextLine(line));
        }

        public void AddLine()
        {
            AddLine(string.Empty);
        }

        public int LineCount
        {
            get { return Lines.Count; }
        }

        public TextLine this[int index]
        {
            get { return Lines[index]; }
        }

        public IList<string> GetRange(int start, int length)
        {
            IList<string> result = new List<string>();

            for (int row = start; row < start + length; row++)
            {
                result.Add(Lines[row].Line);
            }

            return result;
        }
    }
}