using System.Collections.Specialized;
using System.Text.RegularExpressions;
using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Parsing.ConcreteClasses
{
#if UNIT
    public
#else
    internal
#endif
        class PathInfo : IPathInfo
    {
        public PathInfo()
        {
            _elements = new StringCollection();
        }


        public PathInfo(string path)
            : this()
        {
            ParseString(path);
        }


        private readonly StringCollection _elements;


        public bool HasRoot
        {
            get
            {
                if (Elements.Count > 0 && Elements[0].ToUpper() == FileManager.DriveName)
                    return true;
                return false;
            }
        }


        public bool IsRoot
        {
            get { return HasRoot && Count == 0; }
        }


        public string this[int index]
        {
            get
            {
                if (HasRoot)
                    return Elements[index + 1];
                return Elements[index];
            }
        }


        public int Count
        {
            get
            {
                if (HasRoot)
                    return Elements.Count - 1;
                return Elements.Count;
            }
        }


        public string NameOnly
        {
            get { return Elements[Elements.Count - 1]; }
        }


        private StringCollection Elements
        {
            get { return _elements; }
        }


        private void ParseString(string pathLine)
        {
            const string invalidPattern = @"[^\040\.\[\]\\" + FileManager.DriveName + @"0-9a-zA-Z]+";
            const string splitPattern =
                @"^" + FileManager.DriveName + @"|[hd]link\[[^\]]+\]|[0-9a-zA-Z]+\.?[0-9a-zA-Z]*";

            Regex regex = new Regex(invalidPattern, RegexOptions.IgnoreCase);
            if (regex.Match(pathLine).Success)
                throw new InvalidNameException(pathLine);

            regex = new Regex(splitPattern, RegexOptions.IgnoreCase);
            MatchCollection mathes = regex.Matches(pathLine);

            if (mathes.Count == 0)
                throw new InvalidNameException(pathLine);

            foreach (Match match in mathes) Elements.Add(match.Value);
        }


        public IPathInfo CopyWithoutLast()
        {
            PathInfo destinationPathInfo = new PathInfo();
            for (int elementIndex = 0; elementIndex < Count - 1; elementIndex++)
                destinationPathInfo.Elements.Add(this[elementIndex]);

            return destinationPathInfo;
        }


        public override string ToString()
        {
            string result = string.Empty;

            for (int elementIndex = 0; elementIndex < Count; elementIndex++)
            {
                if (elementIndex > 0)
                    result += '\\';
                result += Elements[elementIndex];
            }

            return result;
        }
    }
}