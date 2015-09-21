using FileSystem.Core.Composite;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class MFCommand : Command
	{
		internal MFCommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, 1, CommandTypes.MF )
		{}


		protected override void DoExecute()
		{
			FindParentElement( Operands[ 0 ] ).AddChild( ElementFactory.CreateFile( Operands[ 0 ].NameOnly ) );
		}
	}
}