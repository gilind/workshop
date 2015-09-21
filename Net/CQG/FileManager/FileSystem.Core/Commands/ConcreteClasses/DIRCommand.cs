using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class DIRCommand : Command
	{
		internal DIRCommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, 0, CommandTypes.DIR )
		{}


		protected override void DoExecute()
		{
			_manager.OnPrinted( _manager.ToString() );
		}
	}
}