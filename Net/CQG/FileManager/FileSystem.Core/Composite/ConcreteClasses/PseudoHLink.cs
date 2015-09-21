namespace FileSystem.Core.Composite.ConcreteClasses
{
    internal class PseudoHLink : Proxy
    {
        internal PseudoHLink(IElement reference)
            : base("hlink", reference, true)
        {
        }

        public override IElement Clone()
        {
            return new PseudoHLink(Reference);
        }
    }
}