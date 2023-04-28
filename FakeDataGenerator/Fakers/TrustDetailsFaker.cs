using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class TrustDetailsFaker
{
    private readonly Faker<TrustDetails> _trustDetailsFaker;

    public TrustDetailsFaker(InternalContactFaker internalContactFaker, IEnumerable<string> sponsors,
        string trustWebDomain)
    {
        _trustDetailsFaker = new Faker<TrustDetails>("en_GB")
            .RuleFor(td => td.TrustRelationshipManager, f => internalContactFaker.Generate())
            .RuleFor(td => td.SfsoLead, f => internalContactFaker.Generate())
            .RuleFor(td => td.LocalAuthorities, f => f.PickRandom(Data.LocalAuthorities, f.Random.Int(1, 3)))
            .RuleFor(td => td.Address,
                (f, td) => $"{f.Address.StreetName()}, {td.LocalAuthorities.First()}, {f.Address.ZipCode()}")
            .RuleFor(td => td.Website, $"https://www.{trustWebDomain}")
            .RuleFor(td => td.DateIncorporated, f => f.Date.Past(10))
            .RuleFor(td => td.DateOpened,
                (f, td) => f.Date.Between(td.DateIncorporated, DateTime.Now))
            .RuleFor(td => td.TrustReferenceNumber, f => $"TR{f.Random.Int(0, 9999)}")
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