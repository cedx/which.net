using Belin.Which.Cli;
using System.Reflection;

// Set the text of the console title bar.
var assembly = typeof(Program).Assembly;
Console.Title = assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product;

// Start the application.
return new RootCommand().Parse(args).Invoke();
