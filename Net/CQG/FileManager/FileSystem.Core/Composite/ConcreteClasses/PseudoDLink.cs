namespace FileSystem.Core.Composite.ConcreteClasses
{
	internal class PseudoDLink : Proxy
	{
		internal PseudoDLink( IElement reference )
			: base( "dlink", reference, false )
		{}


		public override IElement Clone()
		{
			return new PseudoDLink( Reference );
		}
	}
}