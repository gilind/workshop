using FileSystem.Core.Composite;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class MHLCommand : MXLCommand
	{
		internal MHLCommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, CommandTypes.MHL )
		{}


		protected override void DoExecute()
		{
			FindReferenceAndDestination();
			_destination.AddChild( ElementFactory.CreateHLink( _reference ) );
		}
	}
}