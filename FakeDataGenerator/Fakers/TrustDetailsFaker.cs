using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class TrustDetailsFaker
{
    private readonly Faker<TrustDetails> _trustDetailsFaker;
    private readonly IEnumerable<string> _localAuthorities;

    public TrustDetailsFaker(InternalContactFaker internalContactFaker, IEnumerable<string> sponsors,
        string trustWebDomain)
    {
        var generalFaker = new Faker();
        _localAuthorities = generalFaker.PickRandom(Data.LocalAuthorities, generalFaker.Random.Int(1, 3)).ToArray();
        
        _trustDetailsFaker = new Faker<TrustDetails>("en_GB")
            .RuleFor(td => td.TrustRelationshipManager, f => internalContactFaker.Generate())
            .RuleFor(td => td.SfsoLead, f => internalContactFaker.Generate())
            .RuleFor(td => td.LocalAuthorities, f => _localAuthorities)
            .RuleFor(td => td.Address,
                (f, td) => $"{f.Address.StreetName()}, {_localAuthorities.First()}, {f.Address.ZipCode()}")
            .RuleFor(td => td.Website, $"https://www.{trustWebDomain}")
            .RuleFor(td => td.DateIncorporated, f => f.Date.Past(10))
            .RuleFor(td => td.DateOpened,
                (f, td) => f.Date.Between(td.DateIncorporated, DateTime.Now))
            .RuleFor(td => td.TrustReferenceNumber, f => $"TR{f.Random.Int(0, 9999)}")
            .RuleFor(td => td.Ukprn, f => $"100{f.Random.Int(0,99999):D5}" )
            .RuleFor(td => td.CompaniesHouseNumber, f => f.Random.Int(1100000, 09999999).ToString("D8"))
            .RuleFor(td => td.CompaniesHouseFilingHistoryUrl, (f, td) =>
                $"https://find-and-update.company-information.service.gov.uk/company/{td.CompaniesHouseNumber}/filing-history")
            .RuleFor(td => td.SponsorApprovalDate, f => f.Date.Past(5))
            .RuleFor(td => td.SponsorName, f => f.PickRandom(sponsors));
    }

    public TrustDetails Generate(Contact mainContactAtTrust)
    {
        var trustDetails = _trustDetailsFaker.Generate();
        trustDetails.MainContactAtTrust = mainContactAtTrust;
        return trustDetails;
    }
}