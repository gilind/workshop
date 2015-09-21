using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class DELTREECommand : Command
    {
        internal DELTREECommand(FileManager manager, IPathInfo[] operands)
            : base(manager, operands, 1, CommandTypes.DELTREE)
        {
        }

        protected override void DoExecute()
        {
            IElement element = FindElement(Operands[0]);
            if (!element.IsContainer || element.Contains(CurrentDirectory))
                throw new RemoveImpossibleException(element.Name);
            element.Remove();
        }
    }
}