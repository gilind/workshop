using System;
using System.Collections.Specialized;
using FileSystem.Core.Commands;
using FileSystem.Core.Composite;
using FileSystem.Core.Converting;
using FileSystem.Core.Converting.ConcreteClasses;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;
using FileSystem.Core.Parsing.ConcreteClasses;

namespace FileSystem.Core
{
    public class PrintedEventArgs : EventArgs
    {
        private readonly string _text;

        internal PrintedEventArgs(string text)
        {
            _text = text;
        }

        public string Text
        {
            get { return _text; }
        }
    }


    public delegate void PrintedEventHandler(object sender, PrintedEventArgs args);


    public class FileManager
    {
        public const string DriveName = "C:";
        private ICommandParser _commandParser;
        private IElement _currentDirectory;
        private IElement _root;
        private IStringConverter _stringConverter;

        public FileManager()
        {
            SetParser(DefaultCommandParser);
            SetConverter(DefaultStringConverter);
            Reset();
        }

        internal ICommandParser Parser
        {
            get { return _commandParser; }
        }

        private IStringConverter Converter
        {
            get { return _stringConverter; }
        }

#if UNIT
        public
#else
        internal
#endif
            static ICommandParser DefaultCommandParser
        {
            get { return new CommandParser(); }
        }

        public static IStringConverter DefaultStringConverter
        {
            get { return new StringConverter(); }
        }

#if UNIT
        public
#else
        internal
#endif
            IElement Root
        {
            get { return _root; }
        }

#if UNIT
        public
#else
        internal
#endif
            IElement CurrentDirectory
        {
            get { return _currentDirectory; }
            set { _currentDirectory = value; }
        }

        public event PrintedEventHandler Printed;

        internal void OnPrinted(string text)
        {
            if (Printed != null)
                Printed(this, new PrintedEventArgs(text));
        }

        public void Reset()
        {
            _root = ElementFactory.CreateRoot();
            CurrentDirectory = _root;
        }

#if UNIT
        public
#else
        internal
#endif
            void SetParser(ICommandParser commandParser)
        {
            _commandParser = commandParser;
        }

        public void SetConverter(IStringConverter stringConverter)
        {
            _stringConverter = stringConverter;
        }

        public void ExecuteCommand(string commandString)
        {
            ICommand command = CommandFactory.CreateCommand(this, commandString);
            command.Execute();
        }

        public void ExecuteBatch(StringCollection commandStrings)
        {
            foreach (string commandString in commandStrings)
            {
                try
                {
                    ExecuteCommand(commandString);
                }
                catch (BusinessException ex)
                {
                    OnPrinted(ex.Message + Environment.NewLine);
                }
            }
        }

        public override string ToString()
        {
            return Converter.Convert(this);
        }
    }
}