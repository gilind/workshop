using System;

namespace FileSystem.Core.Exceptions
{
	public class BusinessException : ApplicationException
	{
		public BusinessException( string message )
			: base( message )
		{}
	}


	public class CommandFormatException : BusinessException
	{
		public CommandFormatException( string commandName )
			: base( "Invalid command format: <" + commandName + ">." )
		{}
	}


	public class CopyImpossibleException : BusinessException
	{
		public CopyImpossibleException( string elementName )
			: base( "Unable to copy item <" + elementName + ">." )
		{}
	}


	public class ElementAlreadyExistsException : BusinessException
	{
		public ElementAlreadyExistsException( string elementName )
			: base( "The item <" + elementName + "> already exists." )
		{}
	}


	public class UnableCreateLinkException : BusinessException
	{
		public UnableCreateLinkException( string elementName )
			: base( "Unable to create link for the item <" + elementName + ">." )
		{}
	}


	public class ElementIsLockedException : BusinessException
	{
		public ElementIsLockedException( string elementName )
			: base( "The item <" + elementName + "> is locked." )
		{}
	}


	public class ElementIsNotContainerException : BusinessException
	{
		public ElementIsNotContainerException( string elementName )
			: base( "The item <" + elementName + "> is not a container." )
		{}
	}


	public class InvalidNameException : BusinessException
	{
		public InvalidNameException( string name )
			: base( "Invalid name <" + name + ">." )
		{}
	}


	public class InvalidPathException : BusinessException
	{
		public InvalidPathException( string path )
			: base( "Invalid path <" + path + ">." )
		{}
	}


	public class RecursiveLinkException : BusinessException
	{
		public RecursiveLinkException( string elementName )
			: base( "Unable to create recursive link for the item <" + elementName + ">." )
		{}
	}


	public class RemoveImpossibleException : BusinessException
	{
		public RemoveImpossibleException( string elementName )
			: base( "Unable to move or delete item <" + elementName + ">." )
		{}
	}


	public class UnknownCommandException : BusinessException
	{
		public UnknownCommandException( string commandName )
			: base( "Unknown command <" + commandName + ">." )
		{}
	}
}