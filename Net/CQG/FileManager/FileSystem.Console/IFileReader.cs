using System.Collections.Specialized;

namespace FileSystem.Console
{
    internal interface IFileReader
    {
        StringCollection Read(string fileName);
    }
}