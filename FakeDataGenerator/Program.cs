using System.Text.Json;
using Bogus;
using FakeDataGenerator.Fakers;
using FakeDataGenerator.Model;

namespace FakeDataGenerator;

public static class Program
{
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
        var trustDetailsFaker = new TrustDetailsFaker(trustName, new ContactFaker(trustName));
        var academyFaker = new AcademyFaker();
        var governanceFaker = new GovernanceFaker(new GovernorFaker(trustName, false),
            new GovernorFaker(trustName, true));
        var trustFaker = new TrustFaker(trustDetailsFaker, governanceFaker, academyFaker, trustName);

        return trustFaker.Generate();
    }
}