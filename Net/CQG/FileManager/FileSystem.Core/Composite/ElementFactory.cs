using FileSystem.Core.Composite.ConcreteClasses;

namespace FileSystem.Core.Composite
{
#if UNIT
	public
#else
    internal
#endif
		class ElementFactory
	{
		private ElementFactory()
		{}


		public static IElement CreateRoot()
		{
			return new PseudoRoot();
		}


		public static IElement CreateFile( string fileName )
		{
			return new PseudoFile( fileName );
		}


		public static IElement CreateDirectory( string dirName )
		{
			return new PseudoDirectory( dirName );
		}


		public static IElement CreateDLink( IElement element )
		{
			return new PseudoDLink( element );
		}


		public static IElement CreateHLink( IElement element )
		{
			return new PseudoHLink( element );
		}
	}
}