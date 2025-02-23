using Belin.Which;
using System.CommandLine;

// Configure the command line arguments.
var commandArgument = new Argument<string>("command", "The name of the executable to find.");
var allOption = new Option<bool>(["-a", "--all"], "List all executable instances found (instead of just the first one).");
var silentOption = new Option<bool>(["-s", "--silent"], "Silence the output, just return the exit code (0 if any executable is found, otherwise 404).");

var program = new RootCommand("Find the instances of an executable in the system path.") {
	commandArgument,
	allOption,
	silentOption
};

// Configure the command line handler.
program.SetHandler((command, all, silent) => {
	var finder = new Finder();
	var resultSet = new ResultSet(command, finder);

	var executables = new List<string>();
	if (all) executables.AddRange(resultSet.All);
	else {
		var executable = resultSet.First;
		if (executable is not null) executables.Add(executable);
	}

	if (!silent) {
		if (executables.Count > 0) Console.WriteLine(string.Join(Environment.NewLine, executables));
		else {
			var paths = string.Join(Path.PathSeparator, finder.Paths);
			Console.Error.WriteLine($"No \"{command}\" in ({paths}).");
		}
	}

	return Task.FromResult(executables.Count > 0 ? 0 : 404);
}, commandArgument, allOption, silentOption);

// Start the application.
return program.Invoke(args);
