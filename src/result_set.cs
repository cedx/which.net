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
	public string Command { get; private set; } = command;

	/// <summary>
	/// The finder used to perform the search.
	/// </summary>
	public Finder Finder { get; private set; } = finder;

	/// <summary>
	/// All instances of the searched command.
	/// </summary>
	public string[] All => [.. Stream.Distinct()];

	/// <summary>
	/// The first instance of the searched command.
	/// </summary>
	public string? First => Stream.FirstOrDefault();

	/// <summary>
	/// A stream of instances of the searched command.
	/// </summary>
	public IEnumerable<string> Stream => Finder.Find(Command);
}
