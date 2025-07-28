namespace Belin.Which.Cli;

using System.CommandLine;

/// <summary>
/// Finds the instances of an executable in the system path.
/// </summary>
internal class RootCommand: System.CommandLine.RootCommand {

	/// <summary>
	/// The name of the executable to find.
	/// </summary>
	private readonly Argument<string> commandArgument = new Argument<string>("command") {
		Description = "The name of the executable to find."
	}.AcceptLegalFileNamesOnly();

	/// <summary>
	/// Value indicating whether to list all executable instances found.
	/// </summary>
	private readonly Option<bool> allOption = new("--all", ["-a"]) {
		Description = "List all executable instances found (instead of just the first one)."
	};

	/// <summary>
	/// Value indicating whether to silence the output.
	/// </summary>
	private readonly Option<bool> silentOption = new("--silent", ["-s"]) {
		Description = "Silence the output, just return the exit code (0 if any executable is found, otherwise 404)."
	};

	/// <summary>
	/// Creates a new root command.
	/// </summary>
	public RootCommand(): base("Find the instances of an executable in the system path.") {
		Arguments.Add(commandArgument);
		Options.Add(allOption);
		Options.Add(silentOption);
		SetAction(Invoke);
	}

	/// <summary>
	/// Invokes this command.
	/// </summary>
	/// <param name="parseResult">The results of parsing the command line input.</param>
	/// <returns>The exit code.</returns>
	public int Invoke(ParseResult parseResult) {
		var command = parseResult.GetRequiredValue(commandArgument);
		var finder = new Finder();
		var resultSet = new ResultSet(command, finder);

		var executables = new List<string>();
		if (parseResult.GetValue(allOption)) executables.AddRange(resultSet.All);
		else {
			var executable = resultSet.First;
			if (executable is not null) executables.Add(executable);
		}

		var silent = parseResult.GetValue(silentOption);
		if (executables.Count > 0) {
			if (!silent) Console.WriteLine(string.Join(Environment.NewLine, executables));
			return 0;
		}

		if (!silent) Console.Error.WriteLine($"No \"{command}\" in ({string.Join(Path.PathSeparator, finder.Paths)}).");
		return 404;
	}
}
