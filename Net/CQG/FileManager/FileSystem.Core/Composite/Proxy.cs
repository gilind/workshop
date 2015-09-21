using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Composite
{
    internal abstract class Proxy : Element
    {
        private readonly bool _lockReference;
        private readonly IElement _reference;

        protected Proxy(string name, IElement reference, bool lockReference)
            : base(name)
        {
            _reference = reference;
            _lockReference = lockReference;

            Reference.AttachLink(this, LockReference);
        }

        protected IElement Reference
        {
            get { return _reference; }
        }

        private bool LockReference
        {
            get { return _lockReference; }
        }

        #region IElement members

        public override string Name
        {
            get { return base.Name + "[" + Reference.Path + "]"; }
        }


        public override IElement AddChild(IElement element)
        {
            return Reference.AddChild(element);
        }


        public override IElement this[int index]
        {
            get { return Reference[index]; }
        }


        public override IElement this[string name]
        {
            get { return Reference[name]; }
        }


        public override int ChildrenCount
        {
            get { return Reference.ChildrenCount; }
        }


        public override void Remove()
        {
            Reference.DetachLink(this, LockReference);
            base.Remove();
        }


        public override void RemoveChild(IElement element)
        {
            Reference.RemoveChild(element);
        }


        public override bool IsLocked
        {
            get { return false; }
        }


        public override void AttachLink(IElement link, bool locked)
        {
            throw new UnableCreateLinkException(Name);
        }


        public override void DetachLink(IElement link, bool locked)
        {
            throw new UnableCreateLinkException(Name);
        }


        public override bool Contains(params IElement[] elements)
        {
            return Reference.Contains(elements);
        }


        public override bool IsContainer
        {
            get { return false; }
        }


        public override bool IsRewritten
        {
            get { return true; }
        }

        #endregion IElement members
    }
}