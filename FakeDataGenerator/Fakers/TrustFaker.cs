using Bogus;
using FakeDataGenerator.Helpers;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class TrustFaker
{
    private readonly Faker<Trust> _trustFaker;

    public TrustFaker(TrustDetailsFaker trustDetailsFaker, GovernanceFaker governanceFaker, AcademyFaker academyFaker,
        TrustToGenerate trustToGenerate)
    {
        _trustFaker = new Faker<Trust>("en_GB")
            .RuleFor(t => t.Name, trustToGenerate.Name)
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustType, () => trustToGenerate.TrustType == TrustType.MultiAcademyTrust? "Multi-academy trust": "Single-academy trust")
            .RuleFor(t => t.Governance, governanceFaker.Generate())
            .RuleFor(t => t.TrustDetails, (f, t) =>
                trustDetailsFaker.Generate(f.PickRandom(t.Governance.Present.Where(g => g.Role != "Trustee"))))
            .RuleFor(t => t.AcademiesInTrust, (f, t) => new AcademiesInTrust
            {
                Academies = academyFaker
                    .SetLocalAuthorities(t.TrustDetails.LocalAuthorities)
                    .Generate(f.Random.Int(1, 40))
            });
    }

    public Trust Generate()
    {
        return _trustFaker.Generate();
    }
}