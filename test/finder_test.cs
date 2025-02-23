namespace Belin.Which;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests the features of the <see cref="Finder"/> class.
/// </summary>
[TestClass]
public sealed class FinderTest {

	[TestMethod]
	public void Constructor() {
		// It should set the `Paths` property to the value of the `PATH` environment variable by default.
		var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
		List<string> paths = pathEnv.Length > 0 ? [.. pathEnv.Split(Path.PathSeparator).Where(item => item.Length > 0).Distinct()] : [];
		CollectionAssert.AreEqual(paths, new Finder().Paths.ToList());

		// It should set the `Extensions` property to the value of the `PATHEXT` environment variable by default.
		var pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? "";
		List<string> extensions = pathExt.Length > 0 ? [.. pathExt.Split(';').Select(item => item.ToLowerInvariant()).Distinct()] : [];
		CollectionAssert.AreEqual(extensions, new Finder().Extensions.ToList());

		// It should put in lower case the list of file extensions.
		CollectionAssert.AreEqual(new List<string> { ".exe", ".js", ".ps1" }, new Finder(extensions: [".EXE", ".JS", ".PS1"]).Extensions.ToList());
	}

	[TestMethod]
	public void Find() {
		var finder = new Finder(paths: ["../res"]);

		// It should return the path of the `executable.cmd` file on Windows.
		List<string> executables = [.. finder.Find("executable")];
		AreEqual(OperatingSystem.IsWindows() ? 1 : 0, executables.Count);
		if (OperatingSystem.IsWindows()) StringAssert.EndsWith(executables.First(), @"res\executable.cmd");

		// It should return the path of the `executable.sh` file on POSIX.
		executables = [.. finder.Find("executable.sh")];
		AreEqual(OperatingSystem.IsWindows() ? 0 : 1, executables.Count);
		if (!OperatingSystem.IsWindows()) StringAssert.EndsWith(executables.First(), "res/executable.sh");

		// It should return an empty array if the searched command is not executable or not found.
		AreEqual(0, finder.Find("not_executable.sh").Count());
		AreEqual(0, finder.Find("foo").Count());
	}

	[TestMethod]
	public void IsExecutable() {
		var finder = new Finder();

		// It should return `false` if the searched command is not executable or not found.
		IsFalse(finder.IsExecutable("foo/bar/baz.qux"));
		IsFalse(finder.IsExecutable("res/not_executable.sh"));

		// It should return `false` for a POSIX executable, when test is run on Windows.
		AreEqual(!OperatingSystem.IsWindows(), finder.IsExecutable("../res/executable.sh"));

		// It should return `false` for a Windows executable, when test is run on POSIX.
		AreEqual(OperatingSystem.IsWindows(), finder.IsExecutable("../res/executable.cmd"));
	}
}
