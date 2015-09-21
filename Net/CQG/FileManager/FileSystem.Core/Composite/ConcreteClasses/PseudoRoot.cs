using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Composite.ConcreteClasses
{
	internal class PseudoRoot : ElementContainer
	{
		internal PseudoRoot()
			: base( FileManager.DriveName )
		{}


		public override void SetParent( IElement parent )
		{
			throw new RemoveImpossibleException( Name );
		}


		public override void Remove()
		{
			throw new RemoveImpossibleException( Name );
		}


		public override void CopyTo( IElement parent )
		{
			throw new CopyImpossibleException( Name );
		}
	}
}