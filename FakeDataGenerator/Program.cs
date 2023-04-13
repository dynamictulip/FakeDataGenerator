using System.Text.Json;
using Bogus;
using FakeDataGenerator.Fakers;
using FakeDataGenerator.Model;

namespace FakeDataGenerator;

public static class Program
{
    private static readonly Faker GeneralFaker = new("en_GB");

    public static void Main()
    {
        Console.WriteLine("Hello, awesome world!");
        //The randomizer seed enables us to generate repeatable data sets
        Randomizer.Seed = new Random(28698);

        var fakeTrusts = Data.TrustNames.Select(GenerateTrust);

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonisisedTrusts = JsonSerializer.Serialize(fakeTrusts, serializeOptions);

        Console.WriteLine(jsonisisedTrusts);
        File.WriteAllText("output.json", jsonisisedTrusts);
    }

    private static Trust GenerateTrust(string trustName)
    {
        var trustDetailsFaker = new TrustDetailsFaker(GeneralFaker, trustName);
        var academyFaker = new AcademyFaker();
        var trustFaker = new TrustFaker(trustDetailsFaker, new GovernanceFaker(GeneralFaker, trustName), academyFaker, trustName);
        
        return trustFaker.Generate();
    }
}