using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernanceFaker
{
    private readonly Faker<Governance> _governanceFaker;

    public GovernanceFaker(Faker generalFaker, string trustName)
    {
        List<Contact> presentGovernors = new()
        {
            new GovernorFaker(generalFaker, trustName, "Accounting Officer", true).Generate(),
            new GovernorFaker(generalFaker, trustName, "Chair of Trustees", true).Generate(),
            new GovernorFaker(generalFaker, trustName, "Chief Financial Officer", true).Generate()
        };
        presentGovernors.AddRange(new GovernorFaker(generalFaker, trustName, "Trustee", true)
            .Generate(generalFaker.Random.Int(3, 10)));

        List<Contact> members = new();
        members.AddRange(
            new GovernorFaker(generalFaker, trustName, "Member", true).Generate(generalFaker.Random.Int(3, 5)));

        List<Contact> past = new()
        {
            new GovernorFaker(generalFaker, trustName, "Accounting Officer", false).Generate(),
            new GovernorFaker(generalFaker, trustName, "Chair of Trustees", false).Generate(),
            new GovernorFaker(generalFaker, trustName, "Chief Financial Officer", false).Generate()
        };
        past.AddRange(
            new GovernorFaker(generalFaker, trustName, "Trustee", false).Generate(generalFaker.Random.Int(3, 15)));

        _governanceFaker = new Faker<Governance>()
            .RuleFor(g => g.Present, presentGovernors)
            .RuleFor(g => g.Members, members)
            .RuleFor(g => g.Past, past);
    }

    public Governance Generate()
    {
        return _governanceFaker.Generate();
    }
}