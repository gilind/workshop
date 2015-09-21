using System;
using System.Collections.Specialized;
using System.IO;

namespace FileSystem.Console.Readers
{
    internal abstract class FileReader : IFileReader
    {
        public abstract StringCollection Read(string fileName);

        protected static StringCollection Read(StreamReader streamReader)
        {
            StringCollection result = new StringCollection();

            try
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                    result.Add(line);
            }
            catch (Exception e)
            {
                throw new IOException("The file could not be read", e);
            }
            finally
            {
                streamReader.Close();
            }

            return result;
        }
    }
}