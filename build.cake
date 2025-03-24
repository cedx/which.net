using System.Text.RegularExpressions;
using static System.IO.File;

var release = HasArgument("r") || HasArgument("release");
var target = Argument<string>("t", null) ?? Argument("target", "default");
var version = Context.Configuration.GetValue("package_version");

Task("build")
	.Description("Builds the project.")
	.Does(() => DotNetBuild("src", new() { Configuration = release ? "Release" : "Debug" }));

Task("clean")
	.Description("Deletes all generated files.")
	.Does(() => EnsureDirectoryDoesNotExist("bin"))
	.DoesForEach(GetDirectories("*/obj"), EnsureDirectoryDoesNotExist)
	.Does(() => CleanDirectory("var", fileSystemInfo => fileSystemInfo.Path.Segments[^1] != ".gitkeep"));

Task("format")
	.Description("Formats the source code.")
	.DoesForEach(["src", "test"], project => DotNetFormat(project));

Task("publish")
	.Description("Publishes the package.")
	.WithCriteria(release, @"the ""Release"" configuration must be enabled")
	.IsDependentOn("default")
	.DoesForEach(["tag", "push origin"], action => StartProcess("git", $"{action} v{version}"));

Task("test")
	.Description("Runs the test suite.")
	.Does(() => DotNetTest("test", new() { Settings = ".runsettings" }));

Task("version")
	.Description("Updates the version number in the sources.")
	.DoesForEach(GetFiles("*/*.csproj"), file => {
		var pattern = new Regex(@"<Version>\d+(\.\d+){2}</Version>");
		WriteAllText(file.FullPath, pattern.Replace(ReadAllText(file.FullPath), $"<Version>{version}</Version>"));
	});

Task("default")
	.Description("The default task.")
	.IsDependentOn("clean")
	.IsDependentOn("version")
	.IsDependentOn("build");

RunTarget(target);
