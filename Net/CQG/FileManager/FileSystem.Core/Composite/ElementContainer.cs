using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Composite
{
	internal abstract class ElementContainer : Element
	{
		protected ElementContainer( string name )
			: base( name )
		{}


		private int IndexOf( string name )
		{
			ChildrenList.Sort( Comparer );
			return ChildrenList.BinarySearch( GetNamedElement( name ), Comparer );
		}


		protected IElement Clone( IElement destinationElement )
		{
			for ( int childIndex = 0; childIndex < ChildrenCount; childIndex++ )
				destinationElement.AddChild( ChildrenList[ childIndex ].Clone() );

			return destinationElement;
		}


		#region IElement members

		public override IElement this[ int index ]
		{
			get
			{
				ChildrenList.Sort( Comparer );
				return ChildrenList[ index ];
			}
		}


		public override IElement this[ string name ]
		{
			get
			{
				int index = IndexOf( name );
				if ( index < 0 ) return null;
				return ChildrenList[ index ];
			}
		}


		public override void RemoveChild( IElement element )
		{
			if ( element.IsLocked )
				throw new ElementIsLockedException( element.Name );
			ChildrenList.Remove( element );
		}


		public override IElement AddChild( IElement element )
		{
			if ( this[ element.Name ] != null )
			{
				if ( element.IsRewritten )
					return this[ element.Name ];
				else
					throw new ElementAlreadyExistsException( element.Name );
			}

			ChildrenList.Add( element );
			element.SetParent( this );
			return element;
		}


		public override bool IsContainer
		{
			get { return true; }
		}

		#endregion IElement members
	}
}