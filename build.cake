using static System.IO.Path;

var release = HasArgument("r") || HasArgument("release");
var target = Argument<string>("t", null) ?? Argument("task", "default");

Task("build").Description("Builds the project.").Does(() => {
	DotNetBuild("lcov.sln", new DotNetBuildSettings { Configuration = release ? "Release" : "Debug" });
});

Task("clean").Description("Deletes all generated files.").Does(() => {
	var deleteDirectorySettings = new DeleteDirectorySettings { Recursive = true };
	foreach (var dir in new[] { "bin", "src/obj", "test/obj" }) EnsureDirectoryDoesNotExist(dir, deleteDirectorySettings);
	CleanDirectory("var", fileSystemInfo => GetRelativePath("var", fileSystemInfo.Path.FullPath) != ".gitkeep");
});

Task("format").Description("Formats the source code.").Does(() => {
	DotNetFormat("lcov.sln");
});

Task("default").Description("The default task.")
	.IsDependentOn("clean")
	.IsDependentOn("build");

RunTarget(target);
