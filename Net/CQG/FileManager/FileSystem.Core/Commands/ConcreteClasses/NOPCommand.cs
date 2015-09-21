using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
    internal class NOPCommand : Command
    {
        internal NOPCommand(IPathInfo[] operands)
            : base(null, operands, 0, CommandTypes.NOP)
        {
        }

        protected override void DoExecute()
        {
        }
    }
}