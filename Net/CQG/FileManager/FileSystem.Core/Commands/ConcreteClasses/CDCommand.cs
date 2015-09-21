using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class CDCommand : Command
    {
        internal CDCommand(FileManager manager, IPathInfo[] operands)
            : base(manager, operands, 1, CommandTypes.CD)
        {
        }

        protected override void DoExecute()
        {
            IElement element = FindElement(Operands[0]);
            if (!element.IsContainer)
                throw new ElementIsNotContainerException(element.Name);
            CurrentDirectory = element;
        }
    }
}