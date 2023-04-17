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
            .RuleFor(g => g.Past, GeneratePastGovernors);
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

    private IEnumerable<Contact> GenerateCurrentGovernors(Faker f)
    {
        return new[]
        {
            _currentGovernorContactFaker.Generate("Accounting Officer"),
            _currentGovernorContactFaker.Generate("Chair of Trustees"),
            _currentGovernorContactFaker.Generate("Chief Financial Officer")
        }.Concat(_currentGovernorContactFaker.Generate("Trustee", f.Random.Int(3, 10)));
    }

    public Governance Generate()
    {
        return _governanceFaker.Generate();
    }
}