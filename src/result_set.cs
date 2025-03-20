namespace Belin.Which;

/// <summary>
/// Provides convenient access to the stream of search results.
/// </summary>
/// <param name="Command">The searched command.</param>
/// <param name="Finder">The finder used to perform the search.</param>
public sealed record ResultSet(string Command, Finder Finder) {

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
