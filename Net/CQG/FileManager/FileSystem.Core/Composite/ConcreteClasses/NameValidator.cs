using System.Text.RegularExpressions;
using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Composite.ConcreteClasses
{
    internal class NameValidator
    {
        private NameValidator()
        {
        }

        public static void ValidateFileName(string fileName)
        {
            string[] parts = fileName.Split('.');
            if (parts.Length < 1 || parts.Length > 2 ||
                parts.Length > 0 && !IsValidName(parts[0], 8, false) ||
                parts.Length > 1 && !IsValidName(parts[1], 3, true))
                throw new InvalidNameException(fileName);
        }

        public static void ValidateDirectoryName(string directoryName)
        {
            if (!IsValidName(directoryName, 8, false))
                throw new InvalidNameException(directoryName);
        }

        protected static bool IsValidName(string name, int expectedLength, bool allowEmpty)
        {
            if (name == string.Empty)
            {
                if (allowEmpty)
                    return true;
                return false;
            }

            const string expectedChars = "[a-zA-Z0-9]";

            bool isValidName = true;
            if (name == null || name.Length == 0 || name.Length > expectedLength)
                isValidName = false;

            if (isValidName)
            {
                Regex regex = new Regex(expectedChars);
                isValidName = regex.Match(name).Success;
            }

            return isValidName;
        }
    }
}