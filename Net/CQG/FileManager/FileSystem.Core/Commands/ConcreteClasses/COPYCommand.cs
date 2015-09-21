using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class COPYCommand : Command
    {
        internal COPYCommand(FileManager manager, IPathInfo[] operands)
            : base(manager, operands, 2, CommandTypes.COPY)
        {
        }

        protected override void DoExecute()
        {
            FindElement(Operands[0]).CopyTo(FindElement(Operands[1]));
        }
    }
}