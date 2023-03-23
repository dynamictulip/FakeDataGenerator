using System.Text.Json;
using Bogus;

namespace FakeDataGenerator;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, awesome world!");

        var thing = GenerateTrust();

        var jsonisisedTrust = JsonSerializer.Serialize(thing);
        Console.WriteLine(jsonisisedTrust);
        File.WriteAllText("output.json", jsonisisedTrust);
    }

    private static Trust GenerateTrust()
    {
        var contactFaker = new Faker<Contact>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Telephone, f => f.Person.Phone);

        var trustDetailsFaker = new Faker<TrustDetails>()
            .RuleFor(td => td.TrustRelationshipManager, f => contactFaker.Generate());

        var trustFaker = new Faker<Trust>()
            // .RuleFor(t => t.Name, f => $"{f.Company.CompanyName()} Academies Trust" );
            // .RuleFor(t => t.Name, f => $"{f.Random.Word()} Academies Trust" );
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustDetails, f => trustDetailsFaker.Generate());

        return trustFaker.Generate();
    }
}