namespace Belin.Which;

using System;
using System.IO;
using System.Runtime.InteropServices;
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
		// Others.
		var perms = File.GetUnixFileMode(file);
		if ((perms & UnixFileMode.OtherExecute) != 0) return true;

		// Group.
		var gid = LibC.getgid();
		// TODO if ((perms & UnixFileMode.GroupExecute) != 0) return gid == stats.gid;

		// Owner.
		var uid = LibC.getuid();
		// TODO if ((perms & UnixFileMode.UserExecute) != 0) return uid == stats.uid;

		// Root.
		return (perms & (UnixFileMode.GroupExecute | UnixFileMode.UserExecute)) != 0 && uid == 0;
	}
}

/// <summary>
/// Provides access to the <c>libc</c> native library.
/// </summary>
internal static partial class LibC {

	/// <summary>
	/// The library name.
	/// </summary>
	private const string LibraryName = "libc";

	/// <summary>
	/// Gets the real group identifier of the calling process.
	/// </summary>
	/// <returns>The real group identifier of the calling process.</returns>
	[LibraryImport(LibraryName, SetLastError = true)]
	public static partial int getgid();

	/// <summary>
	/// Gets the real user identifier of the calling process.
	/// </summary>
	/// <returns>The real user identifier of the calling process.</returns>
	[LibraryImport(LibraryName, SetLastError = true)]
	public static partial int getuid();
}
