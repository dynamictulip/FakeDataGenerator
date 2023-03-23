using System.Text.Json;
using Bogus;

Console.WriteLine("Hello, World!");

var thing = "";

File.WriteAllText("output.json", JsonSerializer.Serialize(thing));
