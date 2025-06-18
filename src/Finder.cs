namespace Belin.Which;

using Mono.Unix.Native;
using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

/// <summary>
/// Finds the instances of an executable in the system path.
/// </summary>
public partial class Finder {

	/// <summary>
	/// Gets the regular expression used to remove quotation marks from a path.
	/// </summary>
	/// <returns>The regular expression used to remove quotation marks from a path.</returns>
	[GeneratedRegex(@"^""|""$")]
	private static partial Regex QuotePattern();

	/// <summary>
	/// The list of executable file extensions.
	/// </summary>
	public IList<string> Extensions { get; }

	/// <summary>
	/// The list of system paths.
	/// </summary>
	public IList<string> Paths { get; }

	/// <summary>
	/// Creates a new finder.
	/// </summary>
	/// <param name="paths">The list of system paths.</param>
	/// <param name="extensions">The list of executable file extensions.</param>
	public Finder(IEnumerable<string>? paths = null, IEnumerable<string>? extensions = null) {
		paths ??= [];
		if (!paths.Any()) {
			var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
			paths = pathEnv.Length > 0 ? pathEnv.Split(Path.PathSeparator) : [];
		}

		extensions ??= [];
		if (!extensions.Any()) {
			var pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? string.Empty;
			extensions = pathExt.Length > 0 ? pathExt.Split(';') : [".exe", ".cmd", ".bat", ".com"];
		}

		var regex = QuotePattern();
		Extensions = [.. extensions.Select(item => item.ToLowerInvariant()).Distinct()];
		Paths = [.. paths.Select(item => regex.Replace(item, string.Empty)).Where(item => item.Length > 0).Distinct()];
	}

	/// <summary>
	/// Finds the instances of the specified command in the system path.
	/// </summary>
	/// <param name="command">The command to be resolved.</param>
	/// <param name="paths">The system path. Defaults to the <c>PATH</c> environment variable.</param>
	/// <param name="extensions">The executable file extensions. Defaults to the <c>PATHEXT</c> environment variable.</param>
	/// <returns>The search results.</returns>
	public static ResultSet Which(string command, IEnumerable<string>? paths = null, IEnumerable<string>? extensions = null) =>
		new(command, new Finder(paths, extensions));

	/// <summary>
	/// Finds the instances of an executable in the system path.
	/// </summary>
	/// <param name="command">The command to be resolved.</param>
	/// <returns>The paths of the executables found.</returns>
	public IEnumerable<string> Find(string command) => Paths.SelectMany(directory => FindExecutables(directory, command));

	/// <summary>
	/// Gets a value indicating whether the specified file is executable.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	public bool IsExecutable(string file) =>
		File.Exists(file) && (OperatingSystem.IsWindows() ? CheckFileExtension(file) : CheckFilePermissions(file));

	/// <summary>
	/// Checks that the specified file is executable according to the executable file extensions.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	private bool CheckFileExtension(string file) => Extensions.Contains(Path.GetExtension(file).ToLowerInvariant());

	/// <summary>
	/// Checks that the specified file is executable according to its permissions.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	[UnsupportedOSPlatform("windows")]
	private static bool CheckFilePermissions(string file) {
		// Others.
		_ = Syscall.stat(file, out var stat);
		if ((stat.st_mode & FilePermissions.S_IXOTH) != 0) return true;

		// Group.
		var gid = Syscall.getgid();
		if ((stat.st_mode & FilePermissions.S_IXGRP) != 0) return gid == stat.st_gid;

		// Owner.
		var uid = Syscall.getuid();
		if ((stat.st_mode & FilePermissions.S_IXUSR) != 0) return uid == stat.st_uid;

		// Root.
		return (stat.st_mode & (FilePermissions.S_IXGRP | FilePermissions.S_IXUSR)) != 0 && uid == 0;
	}

	/// <summary>
	/// Finds the instances of an executable in the specified directory.
	/// </summary>
	/// <param name="directory">The directory path.</param>
	/// <param name="command">The command to be resolved.</param>
	/// <returns>The paths of the executables found.</returns>
	private IEnumerable<string> FindExecutables(string directory, string command) => new string[] { string.Empty }
		.Concat(OperatingSystem.IsWindows() ? Extensions : [])
		.Select(extension => Path.GetFullPath(Path.Join(directory, $"{command}{extension}")))
		.Where(IsExecutable);
}
