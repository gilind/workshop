namespace FileSystem.Core.Composite.ConcreteClasses
{
	internal class PseudoDirectory : ElementContainer
	{
		internal PseudoDirectory( string name )
			: base( name )
		{
			NameValidator.ValidateDirectoryName( name );
		}


		public override IElement Clone()
		{
			return Clone( new PseudoDirectory( Name ) );
		}
	}
}