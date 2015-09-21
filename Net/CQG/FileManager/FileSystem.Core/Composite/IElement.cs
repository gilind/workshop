namespace FileSystem.Core.Composite
{
#if UNIT
	public
#else
    internal
#endif
		interface IElement : IPrototype
	{
		string Name { get; }
		string Path { get; }

		bool IsContainer { get; }
		bool IsLocked { get; }
		bool IsRewritten { get; }
		int ChildrenCount { get; }

		IElement AddChild( IElement element );
		IElement this[ int index ] { get; }
		IElement this[ string name ] { get; }

		bool Contains( params IElement[] elements );
		void MoveTo( IElement parent );
		void CopyTo( IElement parent );
		void Remove();


		#region Service methods

		void RemoveChild( IElement element );
		void AttachLink( IElement link, bool locked );
		void DetachLink( IElement link, bool locked );
		void SetParent( IElement parent );

		#endregion Service methods
	}
}