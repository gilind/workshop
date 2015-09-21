using System;
using System.Collections.Specialized;
using FileSystem.Console.Readers;
using FileSystem.Core;

namespace FileSystem.Console
{
	internal class Program
	{
		private static readonly string[] EmbeddedFiles = new string[]
			{
				"Tests.test1.txt",
				"Tests.test2.txt",
				"Tests.test3.txt",
				"Tests.test4.txt",
				"Tests.mytest.txt"
			};


		private static void Main( string[] args )
		{
			bool existsExternalFiles = args.Length != 0;

			IFileReader fileReader;
			if ( existsExternalFiles )
				fileReader = new ExternalReader();
			else
				fileReader = new EmbeddedReader();

			ExecuteFiles( fileReader, existsExternalFiles ? args : EmbeddedFiles );
		}


		private static void ExecuteFiles( IFileReader fileReader, string[] files )
		{
			FileManager manager = new FileManager();
			manager.Printed += Manager_Printed;

			for ( int fileIndex = 0; fileIndex < files.Length; fileIndex++ )
			{
				try
				{
					string fileName = files[ fileIndex ];
					System.Console.WriteLine( "file: " + fileName + ":" );
					System.Console.WriteLine( "-------------------------------------------------------------------" );

					StringCollection comands = fileReader.Read( fileName );
					StringCollectionToConsole( comands );
					System.Console.WriteLine();

					manager.Reset();
					manager.ExecuteBatch( comands );

					System.Console.WriteLine( manager );
					System.Console.WriteLine();
				}
				catch ( Exception ex )
				{
					System.Console.WriteLine( manager );
					System.Console.WriteLine( ex.Message );
					System.Console.WriteLine();
				}
			}

			Pause();
		}


		private static void StringCollectionToConsole( StringCollection lines )
		{
			foreach ( string line in lines )
				System.Console.WriteLine( line );
		}


		private static void Manager_Printed( object sender, PrintedEventArgs args )
		{
			System.Console.WriteLine( args.Text );
		}


		private static void Pause()
		{
			System.Console.ReadLine();
		}
	}
}