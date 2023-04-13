using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernanceFaker
{
    private readonly GovernorFaker _currentGovernorFaker;
    private readonly Faker<Governance> _governanceFaker;
    private readonly GovernorFaker _pastGovernorFaker;

    public GovernanceFaker(GovernorFaker pastGovernorFaker, GovernorFaker currentGovernorFaker)
    {
        _currentGovernorFaker = currentGovernorFaker;
        _pastGovernorFaker = pastGovernorFaker;

        _governanceFaker = new Faker<Governance>("en_GB")
            .RuleFor(g => g.Present, GenerateCurrentGovernors)
            .RuleFor(g => g.Members, f => _currentGovernorFaker.Generate("Member", f.Random.Int(3, 5)))
            .RuleFor(g => g.Past, GeneratePastGovernors);
    }

    private IEnumerable<Contact> GeneratePastGovernors(Faker f)
    {
        return new[]
        {
            _pastGovernorFaker.Generate("Accounting Officer"),
            _pastGovernorFaker.Generate("Chair of Trustees"),
            _pastGovernorFaker.Generate("Chief Financial Officer")
        }.Concat(_pastGovernorFaker.Generate("Trustee", f.Random.Int(3, 15)));
    }

    private IEnumerable<Contact> GenerateCurrentGovernors(Faker f)
    {
        return new[]
        {
            _currentGovernorFaker.Generate("Accounting Officer"),
            _currentGovernorFaker.Generate("Chair of Trustees"),
            _currentGovernorFaker.Generate("Chief Financial Officer")
        }.Concat(_currentGovernorFaker.Generate("Trustee", f.Random.Int(3, 10)));
    }

    public Governance Generate()
    {
        return _governanceFaker.Generate();
    }
}