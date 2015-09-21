using System;
using System.Collections.Generic;
using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Composite
{
    internal abstract class Element : IElement
    {
        private static IComparer<IElement> _comparer;
        private readonly List<IElement> _childrenList;
        private readonly List<IElement> _linkList;
        private readonly string _name;
        private int _lockedLinkCount;
        private IElement _parent;

        protected Element(string name)
        {
            _name = name;
            _parent = null;

            _childrenList = new List<IElement>();
            _linkList = new List<IElement>();

            LockedLinkCount = 0;
        }

        private int LockedLinkCount
        {
            get { return _lockedLinkCount; }
            set { _lockedLinkCount = value; }
        }

        protected List<IElement> ChildrenList
        {
            get { return _childrenList; }
        }

        private List<IElement> LinkList
        {
            get { return _linkList; }
        }

        private IElement Parent
        {
            get { return _parent; }
        }

        private void RemoveAllLinks()
        {
            while (LinkList.Count > 0)
                LinkList[LinkList.Count - 1].Remove();
        }

        private void RemoveAllChildren()
        {
            while (ChildrenCount > 0)
                ChildrenList[ChildrenCount - 1].Remove();
        }

        public override string ToString()
        {
            return Name;
        }

        #region NameComparer

        private class NameComparer : IComparer<IElement>
        {
            public int Compare(IElement x, IElement y)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    return -1;
                }
                if (y == null)
                    return 1;
                return x.Name.ToUpper().CompareTo(y.Name.ToUpper());
            }
        }


        protected static IComparer<IElement> Comparer
        {
            get
            {
                if (_comparer == null)
                    _comparer = new NameComparer();
                return _comparer;
            }
        }

        #endregion NameComparer

        #region NamedElement

        private sealed class NamedElement : Element
        {
            public NamedElement(string name)
                : base(name)
            {
            }
        }


        protected static IElement GetNamedElement(string name)
        {
            return new NamedElement(name);
        }

        #endregion NamedElement

        #region IElement Members

        public virtual string Name
        {
            get { return _name; }
        }


        public string Path
        {
            get
            {
                if (Parent != null)
                    return Parent.Path + "\\" + Name;
                return Name;
            }
        }


        public virtual bool IsContainer
        {
            get { return false; }
        }


        public virtual IElement AddChild(IElement element)
        {
            throw new ElementIsNotContainerException(Name);
        }


        public virtual IElement this[int index]
        {
            get { throw new ElementIsNotContainerException(Name); }
        }


        public virtual IElement this[string name]
        {
            get { throw new ElementIsNotContainerException(Name); }
        }


        public virtual int ChildrenCount
        {
            get { return ChildrenList.Count; }
        }


        public virtual void Remove()
        {
            if (IsLocked)
                throw new ElementIsLockedException(Name);
            RemoveAllChildren();
            RemoveAllLinks();

            Parent.RemoveChild(this);
        }


        public virtual bool Contains(params IElement[] elements)
        {
            if (elements == null)
                return false;

            for (int elementIndex = 0; elementIndex < elements.Length; elementIndex ++)
            {
                if (elements[elementIndex] == this)
                    return true;

                for (int childIndex = 0; childIndex < ChildrenCount; childIndex++)
                {
                    IElement childElement = ChildrenList[childIndex];
                    if (childElement.Contains(elements[elementIndex]))
                        return true;
                }
            }
            return false;
        }


        public virtual void RemoveChild(IElement element)
        {
            throw new ElementIsNotContainerException(Name);
        }


        public virtual bool IsLocked
        {
            get
            {
                if (LockedLinkCount > 0)
                    return true;

                foreach (IElement element in ChildrenList)
                    if (element.IsLocked)
                        return true;

                return false;
            }
        }


        public virtual bool IsRewritten
        {
            get { return false; }
        }


        public virtual void AttachLink(IElement link, bool locked)
        {
            if (locked)
                LockedLinkCount++;
            LinkList.Add(link);
        }


        public virtual void DetachLink(IElement link, bool locked)
        {
            if (locked)
                LockedLinkCount--;
            LinkList.Remove(link);
        }


        public virtual void SetParent(IElement parent)
        {
            _parent = parent;
        }


        public virtual void MoveTo(IElement parent)
        {
            if (IsLocked)
                throw new ElementIsLockedException(Name);

            IElement oldParent = Parent;
            // если parent не контейнер, будет exception, но элемент останется со старым parent'ом
            parent.AddChild(this);
            oldParent.RemoveChild(this);
        }


        public virtual void CopyTo(IElement parent)
        {
            parent.AddChild(Clone());
        }


        public virtual IElement Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}