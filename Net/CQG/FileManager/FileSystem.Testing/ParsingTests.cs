using System.Text.RegularExpressions;
using FileSystem.Core;
using FileSystem.Core.Commands;
using FileSystem.Core.Exceptions;
using FileSystem.Core.Parsing;
using FileSystem.Core.Parsing.ConcreteClasses;
using NUnit.Framework;

namespace FileSystem.Testing
{
	[ TestFixture ]
	public class ParsingTests
	{
		// Commands, file and directory names are case insensitive. Cd, CD, Cd, cD does mean the same thing.
		public ParsingTests()
		{
			_manager = new FileManager();
		}


		private static ICommandParser Parser
		{
			get { return FileManager.DefaultCommandParser; }
		}


		private readonly FileManager _manager = null;


		private FileManager Manager
		{
			get { return _manager; }
		}


		#region PathInfo

		private static string LinkNameGenerator( Match m )
		{
			return "link" + 36;
		}


		// The output format for dynamic and hard links should be the following: hlink[ full path ] for hard links and dlink[ full path ] respectively.
		[ Test ]
		public void Test_RegexFindLinks()
		{
			const string pattern = @"[hd]link\[[^\]]+\]";

			Regex regex = new Regex( pattern );
			Assert.AreEqual( regex.Match( @"c:\Dir1\hlink[c:\Dir4\Dir5]\Dir3\file" ).Value, @"hlink[c:\Dir4\Dir5]" );
			Assert.AreEqual( regex.Match( @"c:\Dir1\dlink[c:\Dir4\Dir5]\Dir3\file" ).Value, @"dlink[c:\Dir4\Dir5]" );
			Assert.AreEqual(
				regex.Replace( @"c:\Dir1\dlink[c:\Dir4\Dir5]\hlink[c:\Dir4\Dir5]",
				               new MatchEvaluator( LinkNameGenerator ) ), @"c:\Dir1\link36\link36" );
		}


		[ Test ]
		public void Test_RegexSplit()
		{
			const string pattern = @"^c:|[hd]link\[[^\]]+\]|[0-9a-zA-Z]+\.?[0-9a-zA-Z]*";

			Regex regex = new Regex( pattern, RegexOptions.IgnoreCase );
			MatchCollection mathes = regex.Matches( @"c:\Dir1\dlink[c:\Dir4\Dir5]\Dir3\hlink[\Dir4\Dir5]\file" );
			Assert.AreEqual( mathes.Count, 6 );
			Assert.AreEqual( mathes[ 0 ].Value, @"c:" );
			Assert.AreEqual( mathes[ 1 ].Value, @"Dir1" );
			Assert.AreEqual( mathes[ 2 ].Value, @"dlink[c:\Dir4\Dir5]" );
			Assert.AreEqual( mathes[ 3 ].Value, @"Dir3" );
			Assert.AreEqual( mathes[ 4 ].Value, @"hlink[\Dir4\Dir5]" );
			Assert.AreEqual( mathes[ 5 ].Value, @"file" );

			mathes = regex.Matches( @"c:\Test" );
			Assert.AreEqual( mathes.Count, 2 );
			Assert.AreEqual( mathes[ 0 ].Value, @"c:" );
			Assert.AreEqual( mathes[ 1 ].Value, @"Test" );
		}


		[ Test ]
		public void Test_PathInfo()
		{
			IPathInfo pathInfo = new PathInfo( @"c:\Dir1\Dir2\Dir3\file" );
			Assert.AreEqual( pathInfo.Count, 4 );
			Assert.IsTrue( pathInfo.HasRoot );
			Assert.AreEqual( pathInfo[ 0 ], "Dir1" );
			Assert.AreEqual( pathInfo[ 1 ], "Dir2" );
			Assert.AreEqual( pathInfo[ 2 ], "Dir3" );
			Assert.AreEqual( pathInfo[ 3 ], "file" );

			pathInfo = new PathInfo( @"\Dir1\Dir2\Dir3\file.txt" );
			Assert.AreEqual( pathInfo.Count, 4 );
			Assert.IsFalse( pathInfo.HasRoot );
			Assert.AreEqual( pathInfo[ 0 ], "Dir1" );
			Assert.AreEqual( pathInfo[ 1 ], "Dir2" );
			Assert.AreEqual( pathInfo[ 2 ], "Dir3" );
			Assert.AreEqual( pathInfo[ 3 ], "file.txt" );

			pathInfo = new PathInfo( @"c:\Dir1\hlink[c:\Dir4\Dir5]\Dir3\file" );
			Assert.AreEqual( pathInfo.Count, 4 );
			Assert.AreEqual( pathInfo[ 0 ], @"Dir1" );
			Assert.AreEqual( pathInfo[ 1 ], @"hlink[c:\Dir4\Dir5]" );
			Assert.AreEqual( pathInfo[ 2 ], @"Dir3" );
			Assert.AreEqual( pathInfo[ 3 ], @"file" );

			pathInfo = new PathInfo( @"c:\Dir1\dlink[c:\Dir4\Dir5]\Dir3\file" );
			Assert.AreEqual( pathInfo.Count, 4 );
			Assert.AreEqual( pathInfo[ 0 ], @"Dir1" );
			Assert.AreEqual( pathInfo[ 1 ], @"dlink[c:\Dir4\Dir5]" );
			Assert.AreEqual( pathInfo[ 2 ], @"Dir3" );
			Assert.AreEqual( pathInfo[ 3 ], @"file" );
		}

		#endregion PathInfo


		#region Unknown

		[ Test ]
		[ ExpectedException( typeof ( UnknownCommandException ) ) ]
		public void Test_UnknownCommand()
		{
			Parser.Parse( "sahdgash" );
		}


		[ Test ]
		[ ExpectedException( typeof ( UnknownCommandException ) ) ]
		public void Test_UnknownCommand2()
		{
			Parser.Parse( " 1NOP " );
		}

		#endregion Unknown


		#region NOP

		[ Test ]
		public void Test_NOPCommand()
		{
			Parser.Parse( string.Empty );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );

			Parser.Parse( null );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );

			Parser.Parse( "      " );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );

			Parser.Parse( "NOP" );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );

			Parser.Parse( "  NOP  " );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );

			Parser.Parse( "nOp" );
			Assert.AreEqual( Parser.CommandType, CommandTypes.NOP );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_NOPCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, @"NOP c:\test1\test2\test3\" );
		}

		#endregion NOP


		#region MD

		[ Test ]
		public void Test_MDCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"     MD       c:\test      " );
			Assert.AreEqual( command.CommandType, CommandTypes.MD );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"md c:\test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MD );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.AreEqual( command.Operands[ 0 ][ 1 ], "test2" );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );

			command = CommandFactory.CreateCommand( Manager, @"md test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MD );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.AreEqual( command.Operands[ 0 ][ 1 ], "test2" );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MDCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, @"MD" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MDCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"MD c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion MD


		#region CD

		[ Test ]
		public void Test_CDCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"CD c:\test" );
			Assert.AreEqual( command.CommandType, CommandTypes.CD );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"cd test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.CD );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_CDCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "CD" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_CDCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"CD c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion CD


		#region RD

		[ Test ]
		public void Test_RDCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"RD c:\test" );
			Assert.AreEqual( command.CommandType, CommandTypes.RD );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"rd test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.RD );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_RDCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "RD" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_RDCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"RD c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion RD


		#region DELTREE

		[ Test ]
		public void Test_DELTREECommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"DELTREE c:\test" );
			Assert.AreEqual( command.CommandType, CommandTypes.DELTREE );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"DELTREE test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.DELTREE );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_DELTREECommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "DELTREE" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_DELTREECommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"DELTREE c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion DELTREE


		#region MF

		[ Test ]
		public void Test_MFCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"MF c:\test" );
			Assert.AreEqual( command.CommandType, CommandTypes.MF );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"MF test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MF );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MFCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "MF" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MFCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"MF c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion MF


		#region MHL

		[ Test ]
		public void Test_MHLCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @" MHL    c:\test1  c:\test2" );
			Assert.AreEqual( command.CommandType, CommandTypes.MHL );
			Assert.AreEqual( command.Operands.Length, 2 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.IsTrue( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 0 ], "test2" );

			command = CommandFactory.CreateCommand( Manager, @"MHL test1\test2\test3\ test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MHL );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
			Assert.IsFalse( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MHLCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "MHL" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MHLCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"MHL c:\test1\test2\test3\" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MHLCommand_FormatException3()
		{
			CommandFactory.CreateCommand( Manager,
			                              @"MHL c:\test1\test2\test3\ c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion MHL


		#region MDL

		[ Test ]
		public void Test_MDLCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"   MDL   c:\test1     c:\test2     " );
			Assert.AreEqual( command.CommandType, CommandTypes.MDL );
			Assert.AreEqual( command.Operands.Length, 2 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.IsTrue( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 0 ], "test2" );

			command = CommandFactory.CreateCommand( Manager, @"MDL test1\test2\test3\ test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MDL );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
			Assert.IsFalse( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MDLCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "MDL" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MDLCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"MDL c:\test1\test2\test3\" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MDLCommand_FormatException3()
		{
			CommandFactory.CreateCommand( Manager,
			                              @"MDL c:\test1\test2\test3\ c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion MDL


		#region DEL

		[ Test ]
		public void Test_DELCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"DEL c:\test" );
			Assert.AreEqual( command.CommandType, CommandTypes.DEL );
			Assert.AreEqual( command.Operands.Length, 1 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test" );

			command = CommandFactory.CreateCommand( Manager, @"DEL test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.DEL );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_DELCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "DEL" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_DELCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"DEL c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion DEL


		#region COPY

		[ Test ]
		public void Test_COPYCommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"   COPY   c:\test1     c:\test2     " );
			Assert.AreEqual( command.CommandType, CommandTypes.COPY );
			Assert.AreEqual( command.Operands.Length, 2 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.IsTrue( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 0 ], "test2" );

			command = CommandFactory.CreateCommand( Manager, @"COPY test1\test2\test3\ test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.COPY );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
			Assert.IsFalse( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_COPYCommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "COPY" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_COPYCommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"COPY c:\test1\test2\test3\" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_COPYCommand_FormatException3()
		{
			CommandFactory.CreateCommand( Manager,
			                              @"COPY c:\test1\test2\test3\ c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion COPY


		#region MOVE

		[ Test ]
		public void Test_MOVECommand()
		{
			ICommand command = CommandFactory.CreateCommand( Manager, @"   MOVE   c:\test1     c:\test2     " );
			Assert.AreEqual( command.CommandType, CommandTypes.MOVE );
			Assert.AreEqual( command.Operands.Length, 2 );
			Assert.IsTrue( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 0 ], "test1" );
			Assert.IsTrue( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 0 ], "test2" );

			command = CommandFactory.CreateCommand( Manager, @"MOVE test1\test2\test3\ test1\test2\test3\" );
			Assert.AreEqual( command.CommandType, CommandTypes.MOVE );
			Assert.IsFalse( command.Operands[ 0 ].HasRoot );
			Assert.AreEqual( command.Operands[ 0 ][ 2 ], "test3" );
			Assert.IsFalse( command.Operands[ 1 ].HasRoot );
			Assert.AreEqual( command.Operands[ 1 ][ 2 ], "test3" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MOVECommand_FormatException()
		{
			CommandFactory.CreateCommand( Manager, "MOVE" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MOVECommand_FormatException2()
		{
			CommandFactory.CreateCommand( Manager, @"MOVE c:\test1\test2\test3\" );
		}


		[ Test ]
		[ ExpectedException( typeof ( CommandFormatException ) ) ]
		public void Test_MOVECommand_FormatException3()
		{
			CommandFactory.CreateCommand( Manager,
			                              @"MOVE c:\test1\test2\test3\ c:\test1\test2\test3\ c:\test1\test2\test3\" );
		}

		#endregion MOVE
	}
}