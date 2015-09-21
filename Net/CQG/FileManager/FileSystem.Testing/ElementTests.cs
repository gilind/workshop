using FileSystem.Core.Composite;
using FileSystem.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystem.Testing
{
    [TestClass]
    public class ElementTests
	{
		public ElementTests()
		{}


		[TestMethod]
		public void Test_Element_Creating()
		{
			IElement file = ElementFactory.CreateFile( "file" );

			Assert.AreEqual( file.Name, "file" );
			Assert.AreEqual( file.ChildrenCount, 0 );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsNotContainerException ) ) ]
		public void Test_Element_Indexer_Exception()
		{
			Assert.IsNotNull( ElementFactory.CreateFile( "file1" )[ 0 ].Name );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsNotContainerException ) ) ]
		public void Test_Element_AddChild_Exception()
		{
			ElementFactory.CreateFile( "file1" ).AddChild( ElementFactory.CreateFile( "file2" ) );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsNotContainerException ) ) ]
		public void Test_Element_RemoveChild_Exception()
		{
			ElementFactory.CreateFile( "file1" ).RemoveChild( ElementFactory.CreateFile( "file2" ) );
		}


		[TestMethod]
		public void Test_Container_Creating()
		{
			IElement dir = ElementFactory.CreateDirectory( "dir" );

			Assert.AreEqual( dir.Name, "dir" );
			Assert.AreEqual( dir.ChildrenCount, 0 );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementAlreadyExistsException ) ) ]
		public void Test_Container_AddTwoEqualNames_Exception()
		{
			IElement dir = ElementFactory.CreateDirectory( "dir" );
			dir.AddChild( ElementFactory.CreateFile( "file" ) );
			dir.AddChild( ElementFactory.CreateFile( "file" ) );
		}


		[TestMethod]
		public void Test_Container_RemoveChild()
		{
			IElement dir = ElementFactory.CreateDirectory( "dir" );
			IElement file = dir.AddChild( ElementFactory.CreateFile( "file" ) );
			Assert.AreEqual( dir.ChildrenCount, 1 );

			dir.RemoveChild( file );
			Assert.AreEqual( dir.ChildrenCount, 0 );
		}


		[TestMethod]
		public void Test_Container_AddElement_ChildrenCount_Indexers()
		{
			IElement root = ElementFactory.CreateRoot();

			root.AddChild( ElementFactory.CreateFile( "file1" ) );
			Assert.AreEqual( root.ChildrenCount, 1 );

			root.AddChild( ElementFactory.CreateFile( "file2" ) );
			root.AddChild( ElementFactory.CreateFile( "file3" ) );
			Assert.AreEqual( root.ChildrenCount, 3 );

			Assert.AreEqual( root[ "file2" ].Name, "file2" );
			Assert.AreEqual( root[ 0 ].Name, "file1" );
		}


		[TestMethod]
		public void Test_Container_AutoSorting()
		{
			IElement root = ElementFactory.CreateRoot();

			root.AddChild( ElementFactory.CreateFile( "bbb" ) );
			root.AddChild( ElementFactory.CreateFile( "ddd" ) );
			root.AddChild( ElementFactory.CreateFile( "aaa" ) );
			root.AddChild( ElementFactory.CreateFile( "ccc" ) );

			Assert.AreEqual( root[ 0 ].Name, "aaa" );
			Assert.AreEqual( root[ 1 ].Name, "bbb" );
			Assert.AreEqual( root[ 2 ].Name, "ccc" );
			Assert.AreEqual( root[ 3 ].Name, "ddd" );
		}


		[TestMethod]
		public void Test_Container_AddNestedElement()
		{
			IElement root = ElementFactory.CreateRoot();
			IElement directory = root.AddChild( ElementFactory.CreateDirectory( "Dir" ) );

			directory.AddChild( ElementFactory.CreateFile( "file1" ) );
			directory.AddChild( ElementFactory.CreateFile( "file2" ) );
			IElement nestedDirectory = directory.AddChild( ElementFactory.CreateDirectory( "nested" ) );

			nestedDirectory.AddChild( ElementFactory.CreateFile( "file11" ) );
			nestedDirectory.AddChild( ElementFactory.CreateFile( "file12" ) );
			nestedDirectory.AddChild( ElementFactory.CreateFile( "file13" ) );
			nestedDirectory.AddChild( ElementFactory.CreateFile( "file14" ) );

			Assert.AreEqual( root.ChildrenCount, 1 );
			Assert.AreEqual( root[ "dir" ].ChildrenCount, 3 );

			Assert.AreEqual( root[ "dir" ][ 0 ].Name, "file1" );
			Assert.AreEqual( root[ "dir" ][ 1 ].Name, "file2" );
			Assert.AreEqual( root[ "dir" ][ 2 ].Name, "nested" );

			Assert.AreEqual( root[ "dir" ][ 2 ].ChildrenCount, 4 );

			Assert.AreEqual( root[ "dir" ][ "nested" ][ 0 ].Name, "file11" );
			Assert.AreEqual( root[ "dir" ][ "nested" ][ 1 ].Name, "file12" );
			Assert.AreEqual( root[ "dir" ][ "nested" ][ 2 ].Name, "file13" );
			Assert.AreEqual( root[ "dir" ][ "nested" ][ 3 ].Name, "file14" );
		}


		[TestMethod]
		public void Test_Container_CaseInsensitive()
		{
			IElement root = ElementFactory.CreateRoot();

			root.AddChild( ElementFactory.CreateFile( "aAa" ) );
			root.AddChild( ElementFactory.CreateDirectory( "BbB" ) );
			root.AddChild( ElementFactory.CreateFile( "cCc" ) );

			Assert.AreSame( root[ "aaa" ], root[ "AaA" ] );
			Assert.AreEqual( root[ "aaa" ].Name, "aAa" );

			Assert.AreSame( root[ "bbb" ], root[ "bBb" ] );
			Assert.AreEqual( root[ "BBB" ].Name, "BbB" );
		}


		[TestMethod]
		public void Test_Container_Path()
		{
			IElement file =
				ElementFactory.CreateRoot().AddChild( ElementFactory.CreateDirectory( "dir1" ) ).AddChild(
					ElementFactory.CreateDirectory( "dir2" ) ).AddChild( ElementFactory.CreateFile( "file" ) );

			Assert.AreEqual( file.Path, @"C:\dir1\dir2\file" );
		}
	}
}