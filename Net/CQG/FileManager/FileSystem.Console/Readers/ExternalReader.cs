using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace FileSystem.Console.Readers
{
    internal class ExternalReader : FileReader
    {
        public override StringCollection Read(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            return Read(new StreamReader(fileName, Encoding.GetEncoding(1251)));
        }
    }
}