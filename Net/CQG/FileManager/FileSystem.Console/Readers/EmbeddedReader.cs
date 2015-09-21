using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

namespace FileSystem.Console.Readers
{
    internal class EmbeddedReader : FileReader
    {
        public override StringCollection Read(string fileName)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string assemplyName = assembly.GetName().Name;

            string resourceName = assemplyName + "." + fileName;
            Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException("File not found", fileName);

            return Read(new StreamReader(stream, Encoding.GetEncoding(1251)));
        }
    }
}