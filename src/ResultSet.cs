namespace Belin.Which;

using System.Collections;

/// <summary>
/// Provides convenient access to the stream of search results.
/// </summary>
/// <param name="command">The searched command.</param>
/// <param name="finder">The finder used to perform the search.</param>
public sealed class ResultSet(string command, Finder finder): IEnumerable<string> {

	/// <summary>
	/// All instances of the searched command.
	/// </summary>
	public string[] All => [.. this.Distinct()];

	/// <summary>
	/// The first instance of the searched command. Returns <see langword="null"/> if not found.
	/// </summary>
	public string? First => this.FirstOrDefault();

	/// <summary>
	/// Returns an enumerator that iterates through the sort properties.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the sort properties.</returns>
	public IEnumerator<string> GetEnumerator() => finder.Find(command).GetEnumerator();

	/// <summary>
	/// Returns an enumerator that iterates through the sort properties.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the sort properties.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
