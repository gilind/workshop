using System;
using FileSystem.Core;
using FileSystem.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystem.Testing
{
    [TestClass]
    public class BusinessTests
	{
		public BusinessTests()
		{
			_manager = new FileManager();
		}


		private readonly FileManager _manager = null;


		private FileManager Manager
		{
			get { return _manager; }
		}


        [TestInitialize]
        public void SetUp()
		{
			Manager.Reset();
		}


		// Initially file system contains only the root directory marked as "C:"
		[TestMethod]
		public void Test_ExistsTheRoot()
		{
			Manager.Reset();
			Assert.IsNotNull( Manager.Root );
			Assert.AreEqual( Manager.Root.Name, "C:" );
		}


		#region MD

		// MD - Creates Directory
		// a. MD C:\Test – creates a directory called Test\ in the root directory.
		[TestMethod]
		public void Test_CreateDirectoryInTheRoot()
		{
			Manager.ExecuteCommand( @"MD C:\Test" );
			Assert.IsNotNull( Manager.Root[ "Test" ] );
		}


		// b. MD Test – creates a subdirectory called Test\ in the current directory
		[TestMethod]
		public void Test_CreateDirectoryInCurrentDirectory()
		{
			Assert.AreEqual( Manager.CurrentDirectory.Name, "C:" );
			Manager.ExecuteCommand( @"MD Test" );
			Assert.IsNotNull( Manager.Root[ "Test" ] );
		}


		// c. MD C:\Dir1\Dir2\NewDir – creates a subdirectory "NewDir" if directory "C:\Dir1\Dir2" exists.
		[TestMethod]
		public void Test_CreateSubdirectory()
		{
			Manager.ExecuteCommand( @"MD c:\Dir1" );
			Manager.ExecuteCommand( @"MD c:\Dir1\Dir2" );
			Manager.ExecuteCommand( @"MD c:\Dir1\Dir2\NewDir" );
			Assert.IsNotNull( Manager.Root[ "Dir1" ][ "Dir2" ][ "NewDir" ] );
		}


		// d. MD should not create any intermediate directories in the path
		[TestMethod]
		[ ExpectedException( typeof ( InvalidPathException ) ) ]
		public void Test_NotCreateIntermediateDirectories()
		{
			Manager.ExecuteCommand( @"MD c:\Dir1\Dir2\NewDir" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidPathException ) ) ]
		public void Test_NotCreateRootDirectory()
		{
			Manager.ExecuteCommand( @"MD c:" );
		}


		[TestMethod]
		public void Test_ValidDirectoryName()
		{
			Manager.ExecuteCommand( @"MD abcd1234" );
			Assert.IsNotNull( Manager.Root[ "abcd1234" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidDirectoryName()
		{
			Manager.ExecuteCommand( @"MD abcd12345" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidDirectoryName2()
		{
			Manager.ExecuteCommand( @"MD אבסהו" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidDirectoryName3()
		{
			Manager.ExecuteCommand( @"MD Dir1" );
			Manager.ExecuteCommand( @"MD Dir1\אבסהו" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementAlreadyExistsException ) ) ]
		public void Test_CreateExistingDirectory()
		{
			Manager.ExecuteCommand( @"MD c:\Dir1" );
			Manager.ExecuteCommand( @"MD c:\Dir1" );
		}

		#endregion MD


		#region CD

		// CD – changes the current directory.
		// a. CD C: – set root as the current directory.
		[TestMethod]
		public void Test_SetRootAsTheCurrentDirectory()
		{
			Manager.ExecuteCommand( @"CD c:" );
			Assert.AreSame( Manager.Root, Manager.CurrentDirectory );
		}


		// b. CD C:\Dir1 – set "C:\Dir1" as the current directory.
		[TestMethod]
		public void Test_SetDirectoryAsTheCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Assert.IsNotNull( Manager.Root[ "Dir1" ] );

			Manager.ExecuteCommand( @"CD C:\Dir1" );
			Assert.AreSame( Manager.Root[ "Dir1" ], Manager.CurrentDirectory );
		}


		// c. CD Dir1 – set Dir1 subdirectory of the current directory as new current directory.
		[TestMethod]
		public void Test_SetSubdirectoryOfTheCurrentDirectoryAsNewCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir3" );
			Manager.ExecuteCommand( @"CD C:\Dir1\" );
			Manager.ExecuteCommand( @"CD Dir3" );
			Assert.AreSame( Manager.Root[ "Dir1" ][ "Dir3" ], Manager.CurrentDirectory );
		}


		// d. Using CD without parameters is not allowed.
		[TestMethod]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_ChangeCurrentDirectoryWithoutParameters()
		{
			Manager.ExecuteCommand( @"CD" );
		}


		public void Test_SetFileAsTheCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\file.txt" );
			Assert.IsNotNull( Manager.Root[ "file.txt" ] );

			Manager.ExecuteCommand( @"CD C:\file.txt" );
		}

		#endregion CD


		#region RD

		// RD – removes a directory if it is empty( doesn ’t contain any files or subdirectories ).
		// a. 
		[TestMethod]
		public void Test_RemoveSubdirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Assert.IsNotNull( Manager.Root[ "Dir1" ] );

			Manager.ExecuteCommand( @"RD C:\Dir1" );
			Assert.IsNull( Manager.Root[ "Dir1" ] );
		}


		// b.
		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_RemoveNotEmptySubdirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2" );
			Assert.IsNotNull( Manager.Root[ "Dir1" ][ "Dir2" ] );

			Manager.ExecuteCommand( @"RD C:\Dir1" );
		}


		// c.
		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_RemoveCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"cd C:\Dir1" );
			Assert.AreEqual( Manager.Root[ "Dir1" ], Manager.CurrentDirectory );

			Manager.ExecuteCommand( @"RD C:\Dir1" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_RemoveRoot()
		{
			Manager.ExecuteCommand( @"RD C:\" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_RemoveFile()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MF C:\Dir1\file.txt" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "FILE.TXT" ] );

			Manager.ExecuteCommand( @"RD c:\dir1\file.txt" );
		}

		#endregion RD


		#region DELTREE

		// DELTREE – removes a directory with all its subdirectories.

		[TestMethod]
		public void Test_DeltreeDirectoryWithSubdirectories()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\Dir3" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "dir2" ][ "dir3" ] );

			Manager.ExecuteCommand( @"Deltree dir1" );
			Assert.IsNull( Manager.Root[ "dir1" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_DeltreeDirectoryContainsCurrentDirectoryAsSubdirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\Dir3" );
			Manager.ExecuteCommand( @"CD C:\Dir1\Dir2\Dir3" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "dir2" ][ "dir3" ] );
			Assert.AreSame( Manager.Root[ "dir1" ][ "dir2" ][ "dir3" ], Manager.CurrentDirectory );

			Manager.ExecuteCommand( @"Deltree C:\Dir1" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsLockedException ) ) ]
		public void Test_DeltreeDirectoryWithNestedHardLinkedSubdirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\Dir3" );
			Manager.ExecuteCommand( @"MHL C:\Dir1\Dir2\Dir3 c:" );
			Manager.ExecuteCommand( @"deltree C:\Dir1" );
		}


		// One can not delete a file which has an attached hard link but can delete a dynamic link.
		[TestMethod]
		[ ExpectedException( typeof ( ElementIsLockedException ) ) ]
		public void Test_DeltreeDirectoryWithNestedHardLinkedFile()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MF dir1\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\file.txt c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\file.txt]" ] );

			Manager.ExecuteCommand( @"DELTREE dir1" );
		}


		// When a file is deleted its all dynamic links are also should be deleted. If a file has both hard and
		// dynamic links FME should keep them all unchanged.
		[TestMethod]
		public void Test_DeltreeDirectoryWithNestedDynamicLinkedSubdirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\" );
			Manager.ExecuteCommand( @"MD C:\Dir1\Dir2\Dir3" );
			Manager.ExecuteCommand( @"MDL C:\Dir1\Dir2\Dir3 c:" );
			Assert.IsNotNull( Manager.Root[ @"dlink[C:\Dir1\Dir2\Dir3]" ] );

			Manager.ExecuteCommand( @"deltree C:\Dir1" );
			Assert.IsNull( Manager.Root[ @"dlink[C:\Dir1\Dir2\Dir3]" ] );
		}

		#endregion DELTREE


		#region MF

		// MF – creates a file.

		[TestMethod]
		public void Test_CreateFileInDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MF C:\Dir1\file.txt" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "FILE.TXT" ] );
		}


		[TestMethod]
		public void Test_CreateFileInCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"CD C:\Dir1" );
			Manager.ExecuteCommand( @"MF file.txt" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "FILE.TXT" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementAlreadyExistsException ) ) ]
		public void Test_CreateExistingFile()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MF C:\Dir1\file.txt" );
			Manager.ExecuteCommand( @"MF C:\Dir1\file.txt" );
		}


		//5. Files and directories naming conventions shall be the following
		//a. File or directory names shall not exceed 8 characters length.
		//b. File extension shall not exceed 3 characters length.
		//c. Only alphabetical [a…z] and numerical [0…9] characters allowed in the file or directory
		//   names and extensions.
		[TestMethod]
		public void Test_ValidFileName()
		{
			Manager.ExecuteCommand( @"MF abcd1234.123" );
			Assert.IsNotNull( Manager.Root[ "abcd1234.123" ] );

			Manager.ExecuteCommand( @"MF abcd1234.12" );
			Assert.IsNotNull( Manager.Root[ "abcd1234.12" ] );

			Manager.ExecuteCommand( @"MF abcd1234." );
			Assert.IsNotNull( Manager.Root[ "abcd1234." ] );

			Manager.ExecuteCommand( @"MF 11" );
			Assert.IsNotNull( Manager.Root[ "11" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidFileName()
		{
			Manager.ExecuteCommand( @"MF אבגדהו" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidFileName2()
		{
			Manager.ExecuteCommand( @"MF 123456789" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidNameException ) ) ]
		public void Test_InvalidFileName3()
		{
			Manager.ExecuteCommand( @"MF 12345678.1234" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidPathException ) ) ]
		public void Test_InvalidPathOfFile()
		{
			Manager.ExecuteCommand( @"MF dir1\file.txt" );
		}

		#endregion MF


		#region MHL

		// MHL – creates a hard link to a file/directory and places it in given location.
		// creates a hard link to a file/directory
		[TestMethod]
		public void Test_CreateHardLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MHL dir1 c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1]" ] );
		}


		// If such link already exists then FME should continue to the next command in the batch file without any error rising.
		[TestMethod]
		//[ExpectedException( typeof ( ElementAlreadyExistsException ) )]
		public void Test_CreateExistingHardLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MHL dir1 c:" );
			Manager.ExecuteCommand( @"MHL DIR1 C:\" );
		}


		// The output format for hard links should be the following: hlink[ full path ]
		[TestMethod]
		public void Test_HardLinkName()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL c:\dir1\dir2\file.txt C:\dir1" );
			Assert.AreEqual( Manager.Root[ "dir1" ][ @"hlink[c:\dir1\dir2\file.txt]" ].Name,
			                 @"hlink[C:\dir1\dir2\file.txt]" );
		}


		[TestMethod]
		public void Test_ExpandHardLinkContent()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL c:\dir1\ C:\" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1]" ] );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1]" ][ @"dir2" ] );
		}


		// It is not allowed to create links to links.
		[TestMethod]
		[ ExpectedException( typeof ( UnableCreateLinkException ) ) ]
		public void Test_CreateHardLinkToLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL c:\dir1\dir2\file.txt C:\dir1" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"hlink[c:\dir1\dir2\file.txt]" ] );
			Manager.ExecuteCommand( @"MHL c:\dir1\hlink[c:\dir1\dir2\file.txt] C:\" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RecursiveLinkException ) ) ]
		public void Test_CreateRecursiveHardLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MHL c:\dir1 c:\dir1\dir2" );
		}

		#endregion MHL


		#region MDL

		// MDL– creates a dynamic link to a file/directory and places it in given location.
		// creates a dynamic link to a file/directory
		[TestMethod]
		public void Test_CreateDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MDL dir1 c:" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1]" ] );
		}


		// If such link already exists then FME should continue to the next command in the batch file without any error rising.
		[TestMethod]
		//[ExpectedException( typeof ( ElementAlreadyExistsException ) )]
		public void Test_CreateExistingDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MDL dir1 c:" );
			Manager.ExecuteCommand( @"MDL DIR1 C:\" );
		}


		// The output format for dynamic links should be the following: dlink[ full path ]
		[TestMethod]
		public void Test_DynamicLinkName()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL c:\dir1\dir2\file.txt C:\dir1" );
			Assert.AreEqual( Manager.Root[ "dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ].Name,
			                 @"dlink[C:\dir1\dir2\file.txt]" );
		}


		[TestMethod]
		public void Test_ExpandDynamicLinkContent()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL c:\dir1\ C:\" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1]" ] );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1]" ][ @"dir2" ] );
		}


		// It is not allowed to create links to links.
		[TestMethod]
		[ ExpectedException( typeof ( UnableCreateLinkException ) ) ]
		public void Test_CreateDynamicLinkToLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL c:\dir1\dir2\file.txt C:\dir1" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Manager.ExecuteCommand( @"MDL c:\dir1\dlink[c:\dir1\dir2\file.txt] C:\" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RecursiveLinkException ) ) ]
		public void Test_CreateRecursiveDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD c:\dir1\dir2" );
			Manager.ExecuteCommand( @"MDL c:\dir1 c:\dir1\dir2" );
		}

		#endregion MDL


		#region DEL

		// DEL – removes a file or link.

		[TestMethod]
		public void Test_DeleteFile()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Manager.ExecuteCommand( @"MF C:\Dir1\file.txt" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ "FILE.TXT" ] );

			Manager.ExecuteCommand( @"Del c:\dir1\file.txt" );
			Assert.IsNull( Manager.Root[ "dir1" ][ "file.txt" ] );
		}


		[TestMethod]
		public void Test_DeleteHardLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MHL dir1 c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1]" ] );

			Manager.ExecuteCommand( @"Del c:\hlink[c:\dir1]" );
			Assert.IsNull( Manager.Root[ @"hlink[c:\dir1]" ] );
		}


		[TestMethod]
		public void Test_DeleteDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MDL dir1 c:" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1]" ] );

			Manager.ExecuteCommand( @"Del c:\dlink[c:\dir1]" );
			Assert.IsNull( Manager.Root[ @"dlink[c:\dir1]" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsLockedException ) ) ]
		public void Test_DeleteHardLinkedFile()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MF dir1\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\file.txt c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\file.txt]" ] );

			Manager.ExecuteCommand( @"DEL dir1\file.txt" );
		}


		[TestMethod]
		public void Test_DeleteHardLinkedFile2()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MF dir1\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\file.txt c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\file.txt]" ] );

			Manager.ExecuteCommand( @"DEL hlink[c:\dir1\file.txt]" );
			Manager.ExecuteCommand( @"DEL dir1\file.txt" );
		}


		[TestMethod]
		public void Test_DeleteDynamicLinkedFileTogetherDynamicLinks()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt c:" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt c:\dir1" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt c:\dir1\dir2" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );

			Manager.ExecuteCommand( @"DEL dir1\dir2\file.txt" );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"file.txt" ] );
			Assert.IsNull( Manager.Root[ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_DeleteDirectory()
		{
			Manager.ExecuteCommand( @"MD C:\Dir1" );
			Assert.IsNotNull( Manager.Root[ "Dir1" ] );

			Manager.ExecuteCommand( @"Del C:\Dir1" );
		}

		#endregion DEL


		#region COPY

		// COPY – copy an existed directory/file/link to another location.

		// Program should copy directory with all its content.
		[TestMethod]
		public void Test_CopyDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dest" );
			Manager.ExecuteCommand( @"COPY dir1 dest" );

			Assert.IsNotNull( Manager.Root[ "dest" ][ "dir1" ][ "dir2" ][ "file.txt" ] );
			Assert.AreNotSame( Manager.Root[ "dest" ][ "dir1" ][ "dir2" ][ "file.txt" ],
			                   Manager.Root[ "dir1" ][ "dir2" ][ "file.txt" ] );
		}


		[TestMethod]
		public void Test_CopyFileFromCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dest" );
			Manager.ExecuteCommand( @"CD dir1\dir2" );
			Manager.ExecuteCommand( @"COPY file.txt c:" );

			Assert.IsNotNull( Manager.Root[ "file.txt" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( InvalidPathException ) ) ]
		public void Test_CopyFileToNotExistsDestination()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dest" );
			Manager.ExecuteCommand( @"COPY dir1\dir2\file.txt dir1\dir3" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementAlreadyExistsException ) ) ]
		public void Test_CopyFileToDirectoryAlreadyContainThisFileName()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dest" );
			Manager.ExecuteCommand( @"CD dir1\dir2" );
			Manager.ExecuteCommand( @"COPY file.txt c:" );
			Manager.ExecuteCommand( @"COPY file.txt c:" );
		}


		// Destination path should not contain any file name otherwise FME should raise error.
		[TestMethod]
		[ ExpectedException( typeof ( ElementIsNotContainerException ) ) ]
		public void Test_CopyToFile()
		{
			Manager.ExecuteCommand( @"MF file.txt" );
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dest" );
			Manager.ExecuteCommand( @"COPY dir1 file.txt" );
		}


		[TestMethod]
		public void Test_CopyLinks()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\dir2\file.txt c:" );
			Manager.ExecuteCommand( @"COPY c:\hlink[c:\dir1\dir2\file.txt] c:\dir1" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ @"hlink[c:\dir1\dir2\file.txt]" ] );

			Manager.Reset();
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt c:" );
			Manager.ExecuteCommand( @"COPY c:\dlink[c:\dir1\dir2\file.txt] c:\dir1" );
			Assert.IsNotNull( Manager.Root[ "dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
		}


		[TestMethod]
		public void Test_CopyToLink()
		{
			Manager.ExecuteCommand( @"MF file.txt" );
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MHL dir1\dir2\ c:" );
			Manager.ExecuteCommand( @"COPY file.txt c:\hlink[c:\dir1\dir2]" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\dir2]" ][ @"file.txt" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( CopyImpossibleException ) ) ]
		public void Test_CopyRootToDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"Copy c: dir1" );
		}

		#endregion COPY


		#region MOVE

		// MOVE – move an existing directory/file/link to another location
		[TestMethod]
		public void Test_MoveDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dir1\dir2\dir3" );
			Manager.ExecuteCommand( @"MF dir1\dir2\dir3\file.txt" );
			Manager.ExecuteCommand( @"MOVE dir1\dir2 c:" );

			Assert.IsNotNull( Manager.Root[ @"dir2" ][ @"file.txt" ] );
			Assert.IsNotNull( Manager.Root[ @"dir2" ][ @"dir3" ][ @"file.txt" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dir2" ] );
		}


		[TestMethod]
		public void Test_MoveFile()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MOVE dir1\dir2\file.txt c:" );

			Assert.IsNotNull( Manager.Root[ @"file.txt" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"file.txt" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsNotContainerException ) ) ]
		public void Test_MoveFileToFile()
		{
			Manager.ExecuteCommand( @"MF file1" );
			Manager.ExecuteCommand( @"MF file2" );
			Manager.ExecuteCommand( @"copy file1 file2" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_MoveCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dir1\dir2\dir3" );
			Manager.ExecuteCommand( @"MF dir1\dir2\dir3\file.txt" );
			Manager.ExecuteCommand( @"CD dir1\dir2\" );

			Manager.ExecuteCommand( @"MOVE c:\dir1\dir2 c:" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_MoveNestedCurrentDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MD dir1\dir2\dir3" );
			Manager.ExecuteCommand( @"MF dir1\dir2\dir3\file.txt" );
			Manager.ExecuteCommand( @"CD dir1\dir2\dir3" );

			Manager.ExecuteCommand( @"MOVE c:\dir1\dir2 c:" );
		}


		[TestMethod]
		public void Test_MoveHardLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\dir2\file.txt dir1" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"hlink[c:\dir1\dir2\file.txt]" ] );

			Manager.ExecuteCommand( @"Move dir1\hlink[c:\dir1\dir2\file.txt] c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"hlink[c:\dir1\dir2\file.txt]" ] );
		}


		[TestMethod]
		public void Test_MoveDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt dir1" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );

			Manager.ExecuteCommand( @"Move dir1\dlink[c:\dir1\dir2\file.txt] c:" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
		}


		// In case when a file or directory which is being moved has a hard link, FME should terminate the MOVE operation and batch file execution.
		[TestMethod]
		[ ExpectedException( typeof ( ElementIsLockedException ) ) ]
		public void Test_MoveHardLinkedFile()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MHL dir1\dir2\file.txt c:" );
			Assert.IsNotNull( Manager.Root[ @"hlink[c:\dir1\dir2\file.txt]" ] );

			Manager.ExecuteCommand( @"Move dir1\dir2 c:" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( ElementIsLockedException ) ) ]
		public void Test_MoveDirectoryWithHardLinkedContent()
		{
			//C:
			//|_DIR1
			//| |_DIR2
			//| | |_DIR3
			//| |   |_readme.txt
			//| |
			//| |_DIR4
			//| | |_hlink[C:\DIR1\DIR2\DIR3\readme.txt]
			//| |
			//| |_DIR5
			//| | |_dlink[C:\DIR1\DIR2\DIR3\readme.txt]
			//And FME found command MOVE C:\DIR1\DIR2\DIR3 C:\DIR1\DIR4 in a batch file.
			//Following to our requirements FME should terminate command execution because of the
			//hard link hlink[C:\DIR1\DIR2\DIR3\readme.txt] existence.
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MD dir1\dir2\dir3" );
			Manager.ExecuteCommand( @"MF dir1\dir2\dir3\readme.txt" );
			Manager.ExecuteCommand( @"MD dir1\dir4" );
			Manager.ExecuteCommand( @"MHL dir1\dir2\dir3\readme.txt dir1\dir4" );
			Manager.ExecuteCommand( @"MD dir1\dir5" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\dir3\readme.txt dir1\dir5" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir4" ][ @"hlink[c:\dir1\dir2\dir3\readme.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir5" ][ @"dlink[c:\dir1\dir2\dir3\readme.txt]" ] );

			Manager.ExecuteCommand( @"MOVE C:\DIR1\DIR2\DIR3 C:\DIR1\DIR4" );
		}


		// In case when any dynamic link(s) found and no hard link exists, then dynamic link(s) should be modified and contain new location information instead of the old one
		[TestMethod]
		public void Test_MoveDirectoryWithDynamicLinkAndModifiedDynamicLink()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MF dir1\dir2\file.txt" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt c:" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt dir1" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\file.txt dir1\dir2" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"dlink[c:\dir1\dir2\file.txt]" ] );

			Manager.ExecuteCommand( @"Move dir1\dir2\file.txt c:" );
			Assert.IsNotNull( Manager.Root[ @"dlink[c:\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dlink[c:\file.txt]" ] );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir2" ][ @"dlink[c:\file.txt]" ] );

			//C:
			//|_DIR1
			//| |_DIR2
			//| | |_DIR3
			//| |   |_readme.txt
			//| |
			//| |_DIR4
			//| |
			//| |_DIR5
			//| | |_dlink[C:\DIR1\DIR2\DIR3\readme.txt]
			//After MOVE C:\DIR1\DIR2\DIR3 C:\DIR1\DIR4 command execution the directories
			//structure and dlink will be changed by the following way.
			//C:
			//|_DIR1
			//| |_DIR2
			//| |
			//| |_DIR4
			//| | |_DIR3
			//| |   |_readme.txt
			//| |
			//| |_DIR5
			//| | |_dlink[C:\DIR1\DIR4\DIR3\readme.txt]
			Manager.Reset();
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"MD dir1\dir2\dir3" );
			Manager.ExecuteCommand( @"MF dir1\dir2\dir3\readme.txt" );
			Manager.ExecuteCommand( @"MD dir1\dir4" );
			Manager.ExecuteCommand( @"MD dir1\dir5" );
			Manager.ExecuteCommand( @"MDL dir1\dir2\dir3\readme.txt dir1\dir5" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir5" ][ @"dlink[c:\dir1\dir2\dir3\readme.txt]" ] );

			Manager.ExecuteCommand( @"MOVE C:\DIR1\DIR2\DIR3 C:\DIR1\DIR4" );
			Assert.IsNotNull( Manager.Root[ @"dir1" ][ @"dir5" ][ @"dlink[c:\dir1\dir4\dir3\readme.txt]" ] );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_MoveRoot()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"Move c: dir1" );
		}


		[TestMethod]
		[ ExpectedException( typeof ( RemoveImpossibleException ) ) ]
		public void Test_MoveDirectoryToNestedDirectory()
		{
			Manager.ExecuteCommand( @"MD dir1" );
			Manager.ExecuteCommand( @"MD dir1\dir2" );
			Manager.ExecuteCommand( @"Move dir1 dir1\dir2" );
		}

		#endregion MOVE


		#region AutoSorting

		// The output shall be organized by the manner given below. The output should be organized in
		// alphabetical ascending order.
		[TestMethod]
		public void Test_AutoSorting()
		{
			Manager.ExecuteCommand( @"MD cc" );
			Manager.ExecuteCommand( @"MD bb" );
			Manager.ExecuteCommand( @"MF aa" );
			Manager.ExecuteCommand( @"MF dd" );

			Assert.AreEqual( Manager.Root[ 0 ].Name, "aa" );
			Assert.AreEqual( Manager.Root[ 1 ].Name, "bb" );
			Assert.AreEqual( Manager.Root[ 2 ].Name, "cc" );
			Assert.AreEqual( Manager.Root[ 3 ].Name, "dd" );

			//C:
			//|_AAA
			//| |_readme.txt
			//|
			//|_BBB
			//| |_readme.txt
			//|
			//|_CCC
			//|  
			//|_dlink[C:\AAA\readme.txt]
			//|_dlink[C:\BBB\readme.txt]
			//After MOVE C:\AAA\readme.txt C:\CCC command execution the directories
			//structure and dlink will be changed by the following way.
			//C:
			//|_AAA
			//|
			//|_BBB
			//| |_readme.txt
			//|
			//|_CCC
			//| |_readme.txt
			//|
			//|_dlink[C:\BBB\readme.txt] - order changed
			//|_dlink[C:\CCC\readme.txt]

			Manager.Reset();
			Manager.ExecuteCommand( @"MD AAA" );
			Manager.ExecuteCommand( @"MF AAA\readme.txt" );
			Manager.ExecuteCommand( @"MD BBB" );
			Manager.ExecuteCommand( @"MF BBB\readme.txt" );
			Manager.ExecuteCommand( @"MD CCC" );
			Manager.ExecuteCommand( @"MDL C:\AAA\readme.txt C:" );
			Manager.ExecuteCommand( @"MDL C:\BBB\readme.txt C:" );

			Assert.AreEqual( Manager.Root[ 0 ].Name, "AAA" );
			Assert.AreEqual( Manager.Root[ 1 ].Name, "BBB" );
			Assert.AreEqual( Manager.Root[ 2 ].Name, "CCC" );
			Assert.AreEqual( Manager.Root[ 3 ].Name, @"dlink[C:\AAA\readme.txt]" );
			Assert.AreEqual( Manager.Root[ 4 ].Name, @"dlink[C:\BBB\readme.txt]" );

			Manager.ExecuteCommand( @"MOVE C:\AAA\readme.txt C:\CCC" );

			Assert.AreEqual( Manager.Root[ 0 ].Name, "AAA" );
			Assert.AreEqual( Manager.Root[ 1 ].Name, "BBB" );
			Assert.AreEqual( Manager.Root[ 2 ].Name, "CCC" );
			Assert.AreEqual( Manager.Root[ 3 ].Name, @"dlink[C:\BBB\readme.txt]" );
			Assert.AreEqual( Manager.Root[ 4 ].Name, @"dlink[C:\CCC\readme.txt]" );
		}

		#endregion AutoSorting


		#region Print

		[TestMethod]
		public void Test_Print()
		{
			Manager.ExecuteCommand( @"MD c:\Dir1" );
			Manager.ExecuteCommand( @"MF c:\Dir1\file7" );
			Manager.ExecuteCommand( @"MF c:\Dir1\file5" );
			Manager.ExecuteCommand( @"MF c:\Dir1\file6" );

			Manager.ExecuteCommand( @"MD c:\Dir1\Dir2" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\file7" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\file5" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\file6" );

			Manager.ExecuteCommand( @"MD c:\Dir1\Dir2\Dir3" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\Dir3\file1" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\Dir3\file2" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\Dir3\file3" );
			Manager.ExecuteCommand( @"MF c:\Dir1\Dir2\Dir3\file4" );

			Manager.ExecuteCommand( @"MD c:\Dir4" );
			Manager.ExecuteCommand( @"MD c:\Dir4\Dir5" );
			Manager.ExecuteCommand( @"MF c:\Dir4\Dir5\file1" );
			Manager.ExecuteCommand( @"MF c:\Dir4\Dir5\file2" );
			Manager.ExecuteCommand( @"MF c:\Dir4\Dir5\file3" );
			Manager.ExecuteCommand( @"MF c:\Dir4\Dir5\file4" );
			Manager.ExecuteCommand( @"MF c:\Dir4\file7" );
			Manager.ExecuteCommand( @"MF c:\Dir4\file5" );
			Manager.ExecuteCommand( @"MF c:\Dir4\file6" );
			Manager.ExecuteCommand( @"MHL c:\Dir1\Dir2\Dir3\file1 c:\Dir4" );
			Manager.ExecuteCommand( @"MDL c:\Dir1\ c:\Dir4" );

			//C:
			//|_Dir1
			//|  |_Dir2
			//|  |  |_Dir3
			//|  |  |  |_file1
			//|  |  |  |_file2
			//|  |  |  |_file3
			//|  |  |  |_file4
			//|  |  |  
			//|  |  |_file5
			//|  |  |_file6
			//|  |  |_file7
			//|  | 
			//|  |_file5
			//|  |_file6
			//|  |_file7
			//| 
			//|_Dir4
			//   |_Dir5
			//   |  |_file1
			//   |  |_file2
			//   |  |_file3
			//   |  |_file4
			//   | 
			//   |_dlink[c:\Dir1\Dir2\Dir3\file1]
			//   |_file5
			//   |_file6
			//   |_file7
			//   |_hlink[c:\Dir1\Dir2\Dir3\file1]
			//
			//C:\Dir1\Dir2>

			Manager.ExecuteCommand( @"CD c:\Dir1\Dir2\" );

			Console.WriteLine( Manager );
		}

		#endregion Print
	}
}