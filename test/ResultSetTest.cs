namespace Belin.Which;

using static Belin.Which.Finder;

/// <summary>
/// Tests the features of the <see cref="ResultSet"/> class.
/// </summary>
[TestClass]
public sealed class ResultSetTest {

	/// <summary>
	/// The path to the test fixtures.
	/// </summary>
	private readonly string fixtures = Path.GetFullPath(Path.Join(AppContext.BaseDirectory, "../res"));

	[TestMethod]
	public void All() {
		var paths = new string[] { fixtures };

		// It should return the path of the `Executable.cmd` file on Windows.
		var executables = Which("executable", paths).All;
		if (!OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], @"\res\Executable.cmd");
		}

		// It should return the path of the `Executable.sh` file on POSIX.
		executables = Which("Executable.sh", paths).All;
		if (OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], "/res/Executable.sh");
		}

		// It should return an empty array if the searched command is not executable or not found.
		AreEqual(0, Which("NotExecutable.sh", paths).All.Length);
		AreEqual(0, Which("foo", paths).All.Length);
	}

	[TestMethod]
	public void First() {
		var paths = new string[] { fixtures };

		// It should return the path of the `Executable.cmd` file on Windows.
		var executable = Which("executable", paths).First;
		if (OperatingSystem.IsWindows()) StringAssert.EndsWith(executable, @"\res\Executable.cmd");
		else IsNull(executable);

		// It should return the path of the `Executable.sh` file on POSIX.
		executable = Which("Executable.sh", paths).First;
		if (OperatingSystem.IsWindows()) IsNull(executable);
		else StringAssert.EndsWith(executable, "/res/Executable.sh");

		// It should return an empty string if the searched command is not executable or not found.
		IsNull(Which("NotExecutable.sh", paths).First);
		IsNull(Which("foo", paths).First);
	}
}
