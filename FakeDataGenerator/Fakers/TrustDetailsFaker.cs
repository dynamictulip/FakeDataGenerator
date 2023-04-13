using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class TrustDetailsFaker
{
    private readonly Faker<TrustDetails> _trustDetailsFaker;

    public TrustDetailsFaker(Faker generalFaker, string trustName)
    {
        var trustContactFaker = new ContactFaker(trustName);
        var internalContactFaker = new ContactFaker(trustName, true);

        var localAuthorities = generalFaker.PickRandom(Data.LocalAuthorities, generalFaker.Random.Int(1, 3));
        var dateIncorporated = generalFaker.Date.Past(10);
        var companiesHouseNumber = generalFaker.Random.Int(1100000, 09999999).ToString("D8");

        _trustDetailsFaker = new Faker<TrustDetails>("en_GB")
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
            .RuleFor(td => td.TrustReferenceNumber, f => $"TR{f.Random.Int(0, 9999)}")
            .RuleFor(td => td.CompaniesHouseNumber, companiesHouseNumber)
            .RuleFor(td => td.CompaniesHouseFilingHistoryUrl,
                $"https://find-and-update.company-information.service.gov.uk/company/{companiesHouseNumber}/filing-history")
            .RuleFor(td => td.SponsorApprovalDate, f => f.Date.Past(5))
            .RuleFor(td => td.SponsorName, f => f.PickRandom(Data.TrustNames));
    }

    public TrustDetails Generate()
    {
        return _trustDetailsFaker.Generate();
    }
}