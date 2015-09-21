using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;

namespace FileSystem.Core.Commands.ConcreteClasses
{
	internal class MOVECommand : Command
	{
		internal MOVECommand( FileManager manager, IPathInfo[] operands )
			: base( manager, operands, 2, CommandTypes.MOVE )
		{}


		protected override void DoExecute()
		{
			IElement element = FindElement( Operands[ 0 ] );
			IElement destination = FindElement( Operands[ 1 ] );
			if ( element.Contains( CurrentDirectory, destination ) )
				throw new RemoveImpossibleException( element.Name );

			element.MoveTo( destination );
		}
	}
}