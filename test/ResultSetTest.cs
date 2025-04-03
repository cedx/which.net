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

		// It should return the path of the `executable.cmd` file on Windows.
		var executables = Which("executable", paths).All;
		if (!OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], @"\res\executable.cmd");
		}

		// It should return the path of the `executable.sh` file on POSIX.
		executables = Which("executable.sh", paths).All;
		if (OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], "/res/executable.sh");
		}

		// It should return an empty array if the searched command is not executable or not found.
		AreEqual(0, Which("not_executable.sh", paths).All.Length);
		AreEqual(0, Which("foo", paths).All.Length);
	}

	[TestMethod]
	public void First() {
		var paths = new string[] { fixtures };

		// It should return the path of the `executable.cmd` file on Windows.
		var executable = Which("executable", paths).First;
		if (OperatingSystem.IsWindows()) StringAssert.EndsWith(executable, @"\res\executable.cmd");
		else IsNull(executable);

		// It should return the path of the `executable.sh` file on POSIX.
		executable = Which("executable.sh", paths).First;
		if (OperatingSystem.IsWindows()) IsNull(executable);
		else StringAssert.EndsWith(executable, "/res/executable.sh");

		// It should return an empty string if the searched command is not executable or not found.
		IsNull(Which("not_executable.sh", paths).First);
		IsNull(Which("foo", paths).First);
	}
}
