namespace FileSystem.Core.Composite.ConcreteClasses
{
    internal class PseudoFile : Element
    {
        internal PseudoFile(string name)
            : base(name)
        {
            NameValidator.ValidateFileName(name);
        }

        public override IElement Clone()
        {
            return new PseudoFile(Name);
        }
    }
}