using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class RDCommand : Command
	{
		internal RDCommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, 1, CommandTypes.RD )
		{}


		protected override void DoExecute()
		{
			IElement element = FindElement( Operands[ 0 ] );
			if ( !element.IsContainer || element.ChildrenCount != 0 || element == CurrentDirectory )
				throw new RemoveImpossibleException( element.Name );
			element.Remove();
		}
	}
}