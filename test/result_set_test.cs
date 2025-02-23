namespace Belin.Which;

using static Belin.Which.Finder;

/// <summary>
/// Tests the features of the <see cref="ResultSet"/> class.
/// </summary>
[TestClass]
public sealed class ResultSetTest {

	[TestMethod]
	public void All() {
		var paths = new string[] { "../res/fixtures" };

		// It should return the path of the `executable.cmd` file on Windows.
		var executables = Which("executable", paths).All;
		if (!OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], @"\res\fixtures\executable.cmd");
		}

		// It should return the path of the `executable.sh` file on POSIX.
		executables = Which("executable.sh", paths).All;
		if (OperatingSystem.IsWindows()) AreEqual(0, executables.Length);
		else {
			AreEqual(1, executables.Length);
			StringAssert.EndsWith(executables[0], "/res/fixtures/executable.sh");
		}

		// It should return an empty array if the searched command is not executable or not found.
		AreEqual(0, Which("not_executable.sh", paths).All.Length);
		AreEqual(0, Which("foo", paths).All.Length);
	}

	[TestMethod]
	public void First() {
		var paths = new string[] { "../res/fixtures" };

		// It should return the path of the `executable.cmd` file on Windows.
		var executable = Which("executable", paths).First;
		if (OperatingSystem.IsWindows()) StringAssert.EndsWith(executable, @"\res\fixtures\executable.cmd");
		else IsNull(executable);

		// It should return the path of the `executable.sh` file on POSIX.
		executable = Which("executable.sh", paths).First;
		if (OperatingSystem.IsWindows()) IsNull(executable);
		else StringAssert.EndsWith(executable, "/res/fixtures/executable.sh");

		// It should return an empty string if the searched command is not executable or not found.
		IsNull(Which("not_executable.sh", paths).First);
		IsNull(Which("foo", paths).First);
	}
}
