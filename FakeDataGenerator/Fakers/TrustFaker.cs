using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class TrustFaker
{
    private readonly Faker<Trust> _trustFaker;

    public TrustFaker(TrustDetailsFaker trustDetailsFaker, GovernanceFaker governanceFaker, AcademyFaker academyFaker,
        string trustName)
    {

        _trustFaker = new Faker<Trust>("en_GB")
            .RuleFor(t => t.Name, trustName)
            .RuleFor(t => t.Uid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(t => t.TrustDetails, f => trustDetailsFaker.Generate())
            .RuleFor(t => t.Governance, governanceFaker.Generate())
            .RuleFor(t => t.AcademiesInTrust, (f, t)=>  new AcademiesInTrust
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