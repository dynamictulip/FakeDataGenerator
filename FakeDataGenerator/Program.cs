using System.Text.Json;
using Bogus;

namespace FakeDataGenerator;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, awesome world!");

        //The randomizer seed enables us to generate repeatable data sets
        Randomizer.Seed = new Random(28698);
        var fakeTrust = GenerateTrust();

        var jsonisisedTrust = JsonSerializer.Serialize(fakeTrust);
        Console.WriteLine(jsonisisedTrust);
        File.WriteAllText("output.json", jsonisisedTrust);
    }

    private static Trust GenerateTrust()
    {
        var trustName = Data.TrustNames.First();
        var contactFaker = CreateContactFaker();
        var trustDetailsFaker = CreateTrustDetailsFaker(contactFaker, trustName);
        var trustFaker = CreateTrustFaker(trustDetailsFaker, trustName);

        return trustFaker.Generate();
    }

    private static Faker<Trust> CreateTrustFaker(Faker<TrustDetails> trustDetailsFaker, string trustName)
    {
        var trustFaker = new Faker<Trust>("en_GB")
            .RuleFor(t => t.Name, trustName)
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustDetails, f => trustDetailsFaker.Generate());
        return trustFaker;
    }

    private static Faker<TrustDetails> CreateTrustDetailsFaker(Faker<Contact> contactFaker, string trustName)
    {
        var trustDetailsFaker = new Faker<TrustDetails>("en_GB")
            // TODO: Trust relationship manager and SFSO lead need DfE email addresses
            .RuleFor(td => td.TrustRelationshipManager, f => contactFaker.Generate())
            .RuleFor(td => td.SfsoLead, f => contactFaker.Generate())
            .RuleFor(td => td.MainContactAtTrust, f => contactFaker.Generate())
            //.RuleFor(td => td.Address, f => f.Address.CountryOfUnitedKingdom());
            .RuleFor(td => td.Website, $"https://www.{trustName.ToLower().Replace(" ", "").Replace("'","")}.co.uk")
            .RuleFor(td => td.LocalAuthorities, f => f.PickRandom(Data.LocalAuthorities,f.Random.Int(1,3)));
        return trustDetailsFaker;
    }

    private static Faker<Contact> CreateContactFaker()
    {
        var contactFaker = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Telephone, f => f.Person.Phone);
        return contactFaker;
    }
}