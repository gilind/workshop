using FileSystem.Core.Commands;

namespace FileSystem.Core.Parsing
{
#if UNIT
	public
#else
    internal
#endif
    interface ICommandParser
	{
		void Parse( string line );

		CommandTypes CommandType { get; }
		IPathInfo[] Operands { get; }
	}
}