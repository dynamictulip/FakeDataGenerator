using System.Globalization;
using System.Text.Json;
using Bogus;
using CsvHelper;
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

        var fakeTrusts = Data.TrustNames.Select(GenerateTrust).OrderBy(trust => trust.Name);

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonisisedTrusts = JsonSerializer.Serialize(fakeTrusts, serializeOptions);

        Console.WriteLine(jsonisisedTrusts);
        File.WriteAllText("output.json", jsonisisedTrusts);

        var cvsisedTrusts = from trust in fakeTrusts
                                                    select new { trust.Name, trust.TrustDetails.Address };
        
        using var writer = new StreamWriter("trusts.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(cvsisedTrusts);
    }

    private static Trust GenerateTrust(string trustName)
    {
        var trustWebDomain = Helper.GetDomain(trustName);
        var internalContactFaker = new InternalContactFaker();
        var academyFaker = new AcademyFaker();
        var pastGovernorContactFaker = new GovernorContactFaker(false, trustWebDomain);
        var currentGovernorContactFaker = new GovernorContactFaker(true, trustWebDomain);
        var governanceFaker = new GovernanceFaker(pastGovernorContactFaker, currentGovernorContactFaker);
        var trustDetailsFaker = new TrustDetailsFaker(internalContactFaker, trustWebDomain);
        var trustFaker = new TrustFaker(trustDetailsFaker, governanceFaker, academyFaker, trustName);

        return trustFaker.Generate();
    }
}