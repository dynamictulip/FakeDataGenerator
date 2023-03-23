﻿using System.Text.Json;
using Bogus;

namespace FakeDataGenerator;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, awesome world!");

        var fakeTrust = GenerateTrust();

        var jsonisisedTrust = JsonSerializer.Serialize(fakeTrust);
        Console.WriteLine(jsonisisedTrust);
        File.WriteAllText("output.json", jsonisisedTrust);
    }

    private static Trust GenerateTrust()
    {
        var contactFaker = CreateContactFaker();
        var trustDetailsFaker = CreateTrustDetailsFaker(contactFaker);
        var trustFaker = CreateTrustFaker(trustDetailsFaker);

        return trustFaker.Generate();
    }

    private static Faker<Trust> CreateTrustFaker(Faker<TrustDetails> trustDetailsFaker)
    {
        var trustFaker = new Faker<Trust>()
            // .RuleFor(t => t.Name, f => $"{f.Company.CompanyName()} Academies Trust" );
            // .RuleFor(t => t.Name, f => $"{f.Random.Word()} Academies Trust" );
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustDetails, f => trustDetailsFaker.Generate());
        return trustFaker;
    }

    private static Faker<TrustDetails> CreateTrustDetailsFaker(Faker<Contact> contactFaker)
    {
        var trustDetailsFaker = new Faker<TrustDetails>()
            .RuleFor(td => td.TrustRelationshipManager, f => contactFaker.Generate());
        return trustDetailsFaker;
    }

    private static Faker<Contact> CreateContactFaker()
    {
        var contactFaker = new Faker<Contact>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Telephone, f => f.Person.Phone);
        return contactFaker;
    }
}