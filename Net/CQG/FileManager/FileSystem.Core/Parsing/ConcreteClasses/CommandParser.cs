using System;
using System.Text.RegularExpressions;
using FileSystem.Core.Commands;
using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Parsing.ConcreteClasses
{
	internal class CommandParser : ICommandParser
	{
		public CommandParser()
		{
			Reset();
		}


		private CommandTypes _commandType;
		private IPathInfo[] _operands;


		public CommandTypes CommandType
		{
			get { return _commandType; }
		}


		public IPathInfo[] Operands
		{
			get { return _operands; }
		}


		private void Reset()
		{
			_commandType = CommandTypes.NOP;
			_operands = new PathInfo[0];
		}


		public void Parse( string commandLine )
		{
			Reset();

			if ( commandLine == null || ( commandLine = commandLine.Trim() ) == string.Empty )
				return;

			string[] substrings = Split( commandLine );
			if ( substrings.Length == 0 )
				return;

			ParseCommandType( substrings );
			ParseOperands( substrings );
		}


		private static string[] Split( string commandLine )
		{
			Regex regex = new Regex( @" +" );
			return regex.Split( commandLine );
		}


		private void ParseCommandType( string[] substrings )
		{
			string commandName = substrings[ 0 ];

			try
			{
				_commandType = (CommandTypes) Enum.Parse( typeof ( CommandTypes ), commandName, true );
			}
			catch ( Exception )
			{
				throw new UnknownCommandException( commandName );
			}
		}


		private void ParseOperands( string[] strings )
		{
			if ( strings == null || strings.Length < 2 )
				return;

			IPathInfo[] operands = new PathInfo[strings.Length - 1];
			for ( int operandIndex = 0; operandIndex < operands.Length; operandIndex++ )
				operands[ operandIndex ] = new PathInfo( strings[ operandIndex + 1 ] );

			_operands = operands;
		}
	}
}