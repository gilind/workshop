using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands
{
    public interface ICommand
    {
        IPathInfo[] Operands { get; }
#if UNIT
        CommandTypes CommandType { get; }
#endif
        void Execute();
    }
}