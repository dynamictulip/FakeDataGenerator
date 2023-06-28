using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernanceFaker
{
    private readonly GovernorContactFaker _currentGovernorContactFaker;
    private readonly Faker<Governance> _governanceFaker;
    private readonly GovernorContactFaker _pastGovernorContactFaker;

    public GovernanceFaker(GovernorContactFaker pastGovernorContactFaker,
        GovernorContactFaker currentGovernorContactFaker)
    {
        _currentGovernorContactFaker = currentGovernorContactFaker;
        _pastGovernorContactFaker = pastGovernorContactFaker;

        _governanceFaker = new Faker<Governance>("en_GB")
            .RuleFor(g => g.Present, GenerateCurrentGovernors)
            .RuleFor(g => g.Members, f => _currentGovernorContactFaker.Generate("Member", f.Random.Int(3, 5)))
            .RuleFor(g => g.Past, GeneratePastGovernors)
            .RuleFor(g => g.TrustManagement, GenerateTrustManagement)
            .RuleFor(g => g.Trustees, GenerateTrustees);
    }

    private IEnumerable<Contact> GeneratePastGovernors(Faker f)
    {
        return new[]
        {
            _pastGovernorContactFaker.Generate("Accounting Officer"),
            _pastGovernorContactFaker.Generate("Chair of Trustees"),
            _pastGovernorContactFaker.Generate("Chief Financial Officer")
        }.Concat(_pastGovernorContactFaker.Generate("Trustee", f.Random.Int(3, 15)));
    }

    private IEnumerable<Contact> GenerateTrustManagement()
    {
        return new[]
        {
            _currentGovernorContactFaker.Generate("Accounting Officer"),
            _currentGovernorContactFaker.Generate("Chair of Trustees"),
            _currentGovernorContactFaker.Generate("Chief Financial Officer")
        };
    }

    private IEnumerable<Contact> GenerateTrustees(Faker f)
    {
        return _currentGovernorContactFaker.Generate("Trustee", f.Random.Int(3, 10));
    }

    private IEnumerable<Contact> GenerateCurrentGovernors(Faker f)
    {
        return GenerateTrustManagement().Concat(GenerateTrustees(f));
    }

    public Governance Generate()
    {
        return _governanceFaker.Generate();
    }
}