using System;
using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands
{
    internal abstract class Command : ICommand
    {
        private readonly CommandTypes _commandType;
        protected readonly FileManager _manager;
        private readonly IPathInfo[] _operands;

        public Command(FileManager manager, IPathInfo[] operands, int expectedOperandCount, CommandTypes commandType)
        {
            if (operands.Length != expectedOperandCount)
                throw new CommandFormatException(commandType.ToString());

            _manager = manager;
            _operands = operands;
            _commandType = commandType;
        }

        protected IElement Root
        {
            get { return _manager.Root; }
        }

        protected IElement CurrentDirectory
        {
            get { return _manager.CurrentDirectory; }
            set { _manager.CurrentDirectory = value; }
        }

        public IPathInfo[] Operands
        {
            get { return _operands; }
        }

        public void Execute()
        {
            DoExecute();
        }

#if UNIT
        public CommandTypes CommandType
        {
            get { return _commandType; }
        }
#endif

        protected virtual void DoExecute()
        {
            throw new NotImplementedException();
        }

        #region Service methods

        protected IElement FindElement(IPathInfo pathInfo)
        {
            IElement currentElement = pathInfo.HasRoot ? Root : CurrentDirectory;
            for (int elementIndex = 0; elementIndex < pathInfo.Count; elementIndex++)
            {
                string childElementName = pathInfo[elementIndex];
                currentElement = currentElement[childElementName];

                if (currentElement == null)
                    throw new InvalidPathException(pathInfo.ToString());
            }

            return currentElement;
        }


        protected IElement FindParentElement(IPathInfo pathInfo)
        {
            if (pathInfo.IsRoot)
                throw new InvalidPathException(pathInfo.ToString());
            return FindElement(pathInfo.CopyWithoutLast());
        }

        #endregion Service methods
    }
}