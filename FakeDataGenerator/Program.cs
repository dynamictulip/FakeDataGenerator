using System.Text.Json;
using Bogus;

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
        var trustDetailsFaker = CreateTrustDetailsFaker(trustName);
        var trustFaker = CreateTrustFaker(trustDetailsFaker, trustName);

        return trustFaker.Generate();
    }

    private static Faker<Trust> CreateTrustFaker(Faker<TrustDetails> trustDetailsFaker, string trustName)
    {
        var governanceFaker = CreateGovernanceFaker(trustName);
        var trustFaker = new Faker<Trust>("en_GB")
            .RuleFor(t => t.Name, trustName)
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustDetails, f => trustDetailsFaker.Generate())
            .RuleFor(t => t.Governance, governanceFaker.Generate());
        return trustFaker;
    }

    private static Faker<TrustDetails> CreateTrustDetailsFaker(string trustName)
    {
        var trustContactFaker = CreateContactFaker(trustName);
        var internalContactFaker = CreateContactFaker(trustName, true);

        var localAuthorities = GeneralFaker.PickRandom(Data.LocalAuthorities, GeneralFaker.Random.Int(1, 3));
        var dateIncorporated = GeneralFaker.Date.Past(10);
        var companiesHouseNumber = GeneralFaker.Random.Int(1100000, 09999999).ToString("D8");

        var trustDetailsFaker = new Faker<TrustDetails>("en_GB")
            .RuleFor(td => td.TrustRelationshipManager, f => internalContactFaker.Generate())
            .RuleFor(td => td.SfsoLead, f => internalContactFaker.Generate())
            .RuleFor(td => td.MainContactAtTrust, f => trustContactFaker.Generate())
            .RuleFor(td => td.Address,
                f => $"{f.Address.StreetName()}, {localAuthorities.First()}, {f.Address.ZipCode()}")
            .RuleFor(td => td.Website, $"https://www.{Helper.GetDomain(trustName)}")
            .RuleFor(td => td.LocalAuthorities, f => localAuthorities)
            .RuleFor(td => td.DateIncorporated, f => dateIncorporated)
            .RuleFor(td => td.DateOpened,
                f => f.Date.Between(dateIncorporated, DateTime.Now))
            .RuleFor(td => td.TrustReferenceNumber, f => $"TR{f.Random.Int(min: 0, max: 9999)}")
            .RuleFor(td => td.CompaniesHouseNumber, companiesHouseNumber)
            .RuleFor(td => td.CompaniesHouseFilingHistoryUrl,
                $"https://find-and-update.company-information.service.gov.uk/company/{companiesHouseNumber}/filing-history")
            .RuleFor(td => td.SponsorApprovalDate, f => f.Date.Past(5))
            .RuleFor(td => td.SponsorName, f => f.PickRandom(Data.TrustNames));
        return trustDetailsFaker;
    }

    private static Faker<Contact> CreateContactFaker(string trustName, bool isInternal = false)
    {
        var domain = isInternal ? "education.gov.uk" : $"{Helper.GetDomain(trustName)}";
        var contactFaker = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => $"{f.Person.FirstName}.{f.Person.LastName}@{domain}")
            .RuleFor(c => c.Telephone, f => f.Person.Phone);
        return contactFaker;
    }

    private static Faker<Governance> CreateGovernanceFaker(string trustName)
    {
        List<Contact> presentGovernors = new()
        {
            CreateGovernorFaker(trustName, "Accounting Officer", true).Generate(),
            CreateGovernorFaker(trustName, "Chair of Trustees", true).Generate(),
            CreateGovernorFaker(trustName, "Chief Financial Officer", true).Generate()
        };
        presentGovernors.AddRange(CreateGovernorFaker(trustName, "Trustee", true)
            .Generate(GeneralFaker.Random.Int(3, 10)));

        List<Contact> members = new();
        members.AddRange(CreateGovernorFaker(trustName, "Member", true).Generate(GeneralFaker.Random.Int(3, 5)));

        List<Contact> past = new()
        {
            CreateGovernorFaker(trustName, "Accounting Officer", false).Generate(),
            CreateGovernorFaker(trustName, "Chair of Trustees", false).Generate(),
            CreateGovernorFaker(trustName, "Chief Financial Officer", false).Generate()
        };
        past.AddRange(CreateGovernorFaker(trustName, "Trustee", false).Generate(GeneralFaker.Random.Int(3, 15)));

        var governanceFaker = new Faker<Governance>()
            .RuleFor(g => g.Present, presentGovernors)
            .RuleFor(g => g.Members, members)
            .RuleFor(g => g.Past, past);

        return governanceFaker;
    }

    private static Faker<Contact> CreateGovernorFaker(string trustName, string role, bool isCurrent)
    {
        var dateAppointed = isCurrent ? GeneralFaker.Date.Past(2) : GeneralFaker.Date.Past(10);
        var governorFaker = CreateContactFaker(trustName)
            .RuleFor(c => c.Role, role)
            .RuleFor(c => c.DateAppointed, dateAppointed)
            .RuleFor(c => c.TermEnd,
                f => isCurrent ? GeneralFaker.Date.Future(2) : GeneralFaker.Date.Between(dateAppointed, DateTime.Now));
        return governorFaker;
    }
}