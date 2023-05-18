namespace FakeDataGenerator.Model;

public class TrustDetails
{
    public Contact TrustRelationshipManager { get; set; }
    public Contact SfsoLead { get; set; }
    public Contact MainContactAtTrust { get; set; }

    public string Address { get; set; }
    public string Website { get; set; }
    public IEnumerable<string> LocalAuthorities { get; set; }
    public string RegionsGroup => "Yorkshire and the Humber";
    public string SchoolsFinancialSupportTerritory => "Yorkshire and the Humber";

    public DateTime DateIncorporated { get; set; }
    public DateTime DateOpened { get; set; }
    public string TrustReferenceNumber { get; set; }
    public string Ukprn { get; set; }

    public string CompaniesHouseNumber { get; set; }
    public string CompaniesHouseFilingHistoryUrl { get; set; }
    public string GetInformationAboutSchoolsUrl { get; set; }

    public string SponsorStatus => "Approved";
    public string SponsorRestrictions => "No restrictions";
    public DateTime SponsorApprovalDate { get; set; }
    public string SponsorName { get; set; }
}