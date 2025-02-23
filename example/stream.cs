using static Belin.Which.Finder;
using System;

// Finds all instances of an executable and returns them one at a time.
Console.WriteLine("The 'foobar' command is available at these locations:");
foreach (var path in Which("foo").Stream) Console.WriteLine($"- {path}");
