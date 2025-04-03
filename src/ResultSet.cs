namespace Belin.Which;

/// <summary>
/// Provides convenient access to the stream of search results.
/// </summary>
/// <param name="command">The searched command.</param>
/// <param name="finder">The finder used to perform the search.</param>
public sealed class ResultSet(string command, Finder finder) {

	/// <summary>
	/// All instances of the searched command.
	/// </summary>
	public string[] All => [.. Stream.Distinct()];

	/// <summary>
	/// The first instance of the searched command. Returns <see langword="null"/> if not found.
	/// </summary>
	public string? First => Stream.FirstOrDefault();

	/// <summary>
	/// A stream of instances of the searched command.
	/// </summary>
	public IEnumerable<string> Stream => finder.Find(command);
}
