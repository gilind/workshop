using FileSystem.Core.Composite;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class MDLCommand : MXLCommand
	{
		internal MDLCommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, CommandTypes.MDL )
		{}


		protected override void DoExecute()
		{
			FindReferenceAndDestination();
			_destination.AddChild( ElementFactory.CreateDLink( _reference ) );
		}
	}
}