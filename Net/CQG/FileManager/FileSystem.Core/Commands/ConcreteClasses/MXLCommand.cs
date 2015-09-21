using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class MXLCommand : Command
    {
        protected IElement _destination;
        protected IElement _reference;

        internal MXLCommand(FileManager manager, IPathInfo[] operands, CommandTypes commandType)
            : base(manager, operands, 2, commandType)
        {
        }

        protected void FindReferenceAndDestination()
        {
            _reference = FindElement(Operands[0]);
            _destination = FindElement(Operands[1]);
            if (_reference.Contains(_destination))
                throw new RecursiveLinkException(_reference.Name);
        }
    }
}