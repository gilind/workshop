using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class DELCommand : Command
    {
        internal DELCommand(FileManager manager, IPathInfo[] operands)
            : base(manager, operands, 1, CommandTypes.DEL)
        {
        }

        protected override void DoExecute()
        {
            IElement element = FindElement(Operands[0]);
            if (element.IsContainer)
                throw new RemoveImpossibleException(element.Name);
            element.Remove();
        }
    }
}