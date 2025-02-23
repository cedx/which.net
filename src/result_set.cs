namespace Belin.Which;

/// <summary>
/// Provides convenient access to the stream of search results.
/// </summary>
/// <param name="command">The searched command.</param>
/// <param name="finder">The finder used to perform the search.</param>
public sealed class ResultSet(string command, Finder finder) {
	
	/// <summary>
	/// The searched command.
	/// </summary>
	private readonly string command = command;

	/// <summary>
	/// The finder used to perform the search.
	/// </summary>
	private readonly Finder finder = finder;

	/// <summary>
	/// All instances of the searched command.
	/// </summary>
	public string[] All {
		get => [.. Stream.Distinct()];
	}

	/// <summary>
	/// The first instance of the searched command.
	/// </summary>
	public string? First {
		get => Stream.FirstOrDefault();
	}

	/// <summary>
	/// A stream of instances of the searched command.
	/// </summary>
	public IEnumerable<string> Stream {
		get => finder.Find(command);
	}
}
