using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands
{
	public interface ICommand
	{
		void Execute();
		IPathInfo[] Operands { get; }

#if UNIT
		CommandTypes CommandType { get; }
#endif
	}
}