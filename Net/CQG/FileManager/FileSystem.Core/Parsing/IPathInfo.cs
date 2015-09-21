namespace FileSystem.Core.Parsing
{
	public interface IPathInfo
	{
		bool HasRoot { get; }
		bool IsRoot { get; }

		string this[ int index ] { get; }
		string NameOnly { get; }

		int Count { get; }

		IPathInfo CopyWithoutLast();
	}
}