using Belin.Which;
using System.CommandLine;

// Configure the root command.
var commandArgument = new Argument<string>("command") { Description = "The name of the executable to find." };
var allOption = new Option<bool>("--all", ["-a"]) { Description = "List all executable instances found (instead of just the first one)." };
var silentOption = new Option<bool>("--silent", ["-s"]) { Description = "Silence the output, just return the exit code (0 if any executable is found, otherwise 404)." };
var rootCommand = new RootCommand("Find the instances of an executable in the system path.") {
	commandArgument,
	allOption,
	silentOption
};

// Start the application.
rootCommand.SetAction(parseResult => {
	var command = parseResult.GetValue(commandArgument)!;
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
});

return rootCommand.Parse(args).Invoke();
