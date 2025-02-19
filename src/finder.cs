namespace Belin.Which;

using Mono.Unix.Native;
using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

/// <summary>
/// Finds the instances of an executable in the system path.
/// </summary>
public class Finder {

	/// <summary>
	/// The list of executable file extensions.
	/// </summary>
	public IList<string> Extensions { get; private set; }

	/// <summary>
	/// The list of system paths.
	/// </summary>
	public IList<string> Paths { get; private set; }

	/// <summary>
	/// The list of operating system types indicating the Windows platform.
	/// </summary>
	private static readonly string[] windowsPlatforms = ["cygwin", "msys"];

	/// <summary>
	/// Creates a new finder.
	/// </summary>
	/// <param name="paths">The list of system paths.</param>
	/// <param name="extensions">The list of executable file extensions.</param>
	public Finder(IList<string>? paths = null, IList<string>? extensions = null) {
		extensions ??= [];
		if (extensions.Count == 0) {
			var pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? "";
			extensions = pathExt.Length > 0 ? pathExt.Split(';') : [".exe", ".cmd", ".bat", ".com"];
		}

		paths ??= [];
		if (paths.Count == 0) {
			var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
			paths = pathEnv.Length > 0 ? pathEnv.Split(Path.PathSeparator) : [];
		}

		var regex = new Regex(@"^""|""$");
		Extensions = [.. extensions.Select(item => item.ToLowerInvariant()).Distinct()];
		Paths = [.. paths.Select(item => regex.Replace(item, "")).Where(item => item.Length > 0).Distinct()];
	}

	/// <summary>
	/// Gets a value indicating whether the specified file is executable.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	public bool IsExecutable(string file) {
		return File.Exists(file) && (OperatingSystem.IsWindows() ? CheckFileExtension(file) : CheckFilePermissions(file));
	}

	/// <summary>
	/// Checks that the specified file is executable according to the executable file extensions.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	private bool CheckFileExtension(string file) {
		return Extensions.Contains(Path.GetExtension(file).ToLowerInvariant());
	}

	/// <summary>
	/// Checks that the specified file is executable according to its permissions.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns><see langword="true"/> if the specified file is executable, otherwise <see langword="false"/>.</returns>
	[UnsupportedOSPlatform("windows")]
	private static bool CheckFilePermissions(string file) {
		// TODO check "result" code

		// Others.
		var result = Syscall.stat(file, out var stat);
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
}
