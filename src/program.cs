using System.CommandLine;
using static Belin.Which.Finder;

// The program handler.
Func<string, bool, bool, Task<int>> handler = (command, all, silent) => {
	var resultSet = Which(command);
	var executables = new List<string>();
	if (all) executables.AddRange(resultSet.All);
	else {
		var executable = resultSet.First;
		if (executable is not null) executables.Add(executable);
	}

	if (executables.Count > 0) {
		if (!silent) Console.WriteLine(string.Join(Environment.NewLine, executables));
		return Task.FromResult(0);
	}

	if (!silent) Console.Error.WriteLine($"No \"{command}\" in ({string.Join(Path.PathSeparator, resultSet.Finder.Paths)}).");
	return Task.FromResult(404);
};

// Configure the command line.
var commandArgument = new Argument<string>("command", "The name of the executable to find.");
var allOption = new Option<bool>(["-a", "--all"], "List all executable instances found (instead of just the first one).");
var silentOption = new Option<bool>(["-s", "--silent"], "Silence the output, just return the exit code (0 if any executable is found, otherwise 404).");
var program = new RootCommand("Find the instances of an executable in the system path.") {
	commandArgument,
	allOption,
	silentOption
};

// Start the application.
program.SetHandler(handler, commandArgument, allOption, silentOption);
return program.Invoke(args);
