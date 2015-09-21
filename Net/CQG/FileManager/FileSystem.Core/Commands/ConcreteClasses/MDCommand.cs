using FileSystem.Core.Composite;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class MDCommand : Command
    {
        internal MDCommand(FileManager manager, IPathInfo[] operands)
            : base(manager, operands, 1, CommandTypes.MD)
        {
        }

        protected override void DoExecute()
        {
            FindParentElement(Operands[0]).AddChild(ElementFactory.CreateDirectory(Operands[0].NameOnly));
        }
    }
}