using System;
using System.Collections.Specialized;
using FileSystem.Core.Composite;

namespace FileSystem.Core.Converting.ConcreteClasses
{
    internal class StringConverter : IStringConverter
    {
        public string Convert(FileManager manager)
        {
            StringCollection lines = ElementToStringCollection(manager.Root);
            lines.Add(manager.CurrentDirectory.Path + ">");
            return ConvertStringCollectionToString(lines);
        }

        private static string ConvertStringCollectionToString(StringCollection lines)
        {
            string result = string.Empty;
            foreach (string line in lines)
                result += line + Environment.NewLine;
            return result;
        }

        private static StringCollection ElementToStringCollection(IElement root)
        {
            StringCollection lines = new StringCollection();
            AddElementToStringCollection(root, ref lines, 0, string.Empty, true);
            return lines;
        }

        private static void AddElementToStringCollection(IElement element, ref StringCollection stringCollection,
            int currentLevel, string prefix, bool isLast)
        {
            if (currentLevel == 0)
                stringCollection.Add(element.ToString());
            else
            {
                stringCollection.Add(prefix + "|_" + element);

                if (isLast && element.ChildrenCount == 0)
                    stringCollection.Add(prefix);

                if (isLast)
                    prefix += "   ";
                else
                    prefix += "|  ";
            }

            for (int childIndex = 0; childIndex < element.ChildrenCount; childIndex++)
            {
                IElement child = element[childIndex];
                AddElementToStringCollection(child, ref stringCollection, currentLevel + 1, prefix,
                    childIndex == element.ChildrenCount - 1);
            }
        }
    }
}