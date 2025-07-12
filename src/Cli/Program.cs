using Belin.Which.Cli;

// Start the application.
return await new RootCommand().Parse(args).InvokeAsync();
