using FileSystem.Core.Commands;

namespace FileSystem.Core.Parsing
{
	public interface ICommandParser
	{
		void Parse( string line );

		CommandTypes CommandType { get; }
		IPathInfo[] Operands { get; }
	}
}