namespace FileSystem.Core.Composite
{
#if UNIT
    public
#else
    internal
#endif
        interface IPrototype
    {
        IElement Clone();
    }
}