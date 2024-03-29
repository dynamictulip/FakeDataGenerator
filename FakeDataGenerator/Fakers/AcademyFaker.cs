using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class AcademyFaker
{
    private readonly Faker<Academy> _academyFaker;
    private IEnumerable<string> _localAuthorities;

    public AcademyFaker()
    {
        _academyFaker = new Faker<Academy>("en_GB")
            .RuleFor(a => a.LocalAuthority, f => f.PickRandom(_localAuthorities))
            .RuleSet("randomSchoolName", set =>
            {
                set.RuleFor(a => a.Phase, f => f.PickRandom("Primary", "Secondary"))
                    .RuleFor(a => a.Name, GenerateSchoolName)
                    .RuleFor(a => a.MinPupilAge, (f, a) => a.Phase == "Primary" ? f.PickRandom(4, 5) : 11)
                    .RuleFor(a => a.MaxPupilAge, (f, a) => a.Phase == "Primary" ? 11 : f.PickRandom(16, 18));
            })
            .RuleSet("definedSchoolName",
                set =>
                {
                    set.RuleFor(a => a.Phase,
                            (f, a) => a.Name.Contains("Primary", StringComparison.CurrentCultureIgnoreCase)
                                ? "Primary"
                                : a.Name.Contains("Secondary", StringComparison.CurrentCultureIgnoreCase)
                                    ? "Secondary"
                                    : f.PickRandom("Primary", "Secondary"))
                        .RuleFor(a => a.MinPupilAge, (f, a) => a.Phase == "Primary" ? f.PickRandom(4, 5) : 11)
                        .RuleFor(a => a.MaxPupilAge, (f, a) => a.Phase == "Primary" ? 11 : f.PickRandom(16, 18));
                })
            .RuleFor(a => a.Urn, f => $"{f.Random.Int(100000, 150000)}")
            .RuleFor(a => a.Type, f => f.PickRandom("Academy sponsor led", "Academy converter", "Free school"))
            .RuleFor(a => a.RuralOrUrban, f => f.PickRandom("Urban city and town", "Rural town and fringe", "Rural village in a sparse setting", "Urban major conurbation", "Urban minor conurbation"))
            .RuleFor(a => a.Capacity, f => f.Random.Int(100, 3000))
            .RuleFor(a => a.PupilNumbers, (f, a) => (int)Math.Round(a.Capacity * f.Random.Double(0.4, 1.3)))
            .RuleFor(a => a.DateJoined, f => f.Date.Past(8))
            .RuleFor(a => a.CurrentOfstedRating, f => f.PickRandom(Data.OfstedRatings))
            .RuleFor(a => a.CurrentOfstedRatingDate, f => f.Date.Past(4))
            .RuleFor(a => a.PreviousOfstedRating, f => f.PickRandom(Data.OfstedRatings))
            .RuleFor(a => a.PreviousOfstedRatingDate, (f, a) => f.Date.Past(9, a.CurrentOfstedRatingDate))
            .RuleFor(a => a.SenPupilsPercentage, (f => f.Random.Int(1, 10)))
            .RuleFor(a => a.EnglishAsAnAdditionalLangPercentage, (f => f.Random.Int(1,10)))
            .RuleFor(a => a.FreeSchoolMealsPercentage, f => f.Random.Int(1, 30));
    }

    private static string GenerateSchoolName(Faker f, Academy a)
    {
        var name = f.PickRandom(Data.Schools.Concat(new[] { a.LocalAuthority, f.Address.StreetName() }));
        if (name.StartsWith("st", StringComparison.InvariantCultureIgnoreCase) || f.Random.Bool())
            name = $"{name} {f.PickRandom("Church of England", "Cofe", "CE", "Catholic", "C of E", "R.C.")}";

        if (f.Random.Bool())
            name = $"{name} {a.Phase}";
        name = $"{name} {f.PickRandom("School", "Academy")}";

        return name;
    }

    public AcademyFaker SetLocalAuthorities(IEnumerable<string> localAuthorities)
    {
        _localAuthorities = localAuthorities;
        return this;
    }

    public IEnumerable<Academy> Generate(int num)
    {
        return _academyFaker.Generate(num, "default,randomSchoolName");
    }

    public IEnumerable<Academy> Generate(IEnumerable<string> schools)
    {
        return schools.Select(n =>
        {
            _academyFaker.RuleFor(a => a.Name, n);
            return _academyFaker.Generate("default,definedSchoolName");
        });
    }
}