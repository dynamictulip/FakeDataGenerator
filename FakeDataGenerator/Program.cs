using System.Globalization;
using System.Text.Json;
using Bogus;
using CsvHelper;
using FakeDataGenerator.Fakers;
using FakeDataGenerator.Helpers;
using FakeDataGenerator.Model;

namespace FakeDataGenerator;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, awesome world!");
        //The randomizer seed enables us to generate slightly repeatable data sets
        Randomizer.Seed = new Random(28698);

        var fakeTrusts = Data.TrustsToGenerate
            .Select(GenerateTrust)
            .OrderBy(trust => trust.Name)
            .ToArray();

        JsonifyTrusts(fakeTrusts);
        CvsifyTrusts(fakeTrusts);
    }

    private static void CvsifyTrusts(Trust[] fakeTrusts)
    {
        var cvsisedTrusts = fakeTrusts.Select(trust => new { trust.Name, trust.TrustDetails.Address });

        var outputFileName = "trusts.csv";
        using var writer = new StreamWriter(outputFileName);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(cvsisedTrusts);

        Console.WriteLine($"Written CSV to '{Path.GetFullPath(outputFileName)}'");
    }

    private static void JsonifyTrusts(Trust[] fakeTrusts)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonisisedTrusts = JsonSerializer.Serialize(fakeTrusts, serializeOptions);

        var outputFileName = "output.json";
        File.WriteAllText(outputFileName, jsonisisedTrusts);

        Console.WriteLine($"Written JSON to '{Path.GetFullPath(outputFileName)}'");
    }

    private static Trust GenerateTrust(TrustToGenerate trustToGenerate)
    {
        var trustWebDomain = Helper.GetDomain(trustToGenerate.Name);
        var sponsors = Data.TrustsToGenerate.Select(t => t.Name).ToArray();

        var internalContactFaker = new InternalContactFaker();
        var academyFaker = new AcademyFaker();
        var pastGovernorContactFaker = new GovernorContactFaker(false, trustWebDomain);
        var currentGovernorContactFaker = new GovernorContactFaker(true, trustWebDomain);
        var governanceFaker = new GovernanceFaker(pastGovernorContactFaker, currentGovernorContactFaker);
        var trustDetailsFaker = new TrustDetailsFaker(internalContactFaker,
            sponsors, trustWebDomain);
        var trustFaker = new TrustFaker(trustDetailsFaker, governanceFaker, academyFaker, trustToGenerate);

        return trustFaker.Generate();
    }
}