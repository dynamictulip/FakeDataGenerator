using FakeDataGenerator.Model;

namespace FakeDataGenerator.Helpers;

public class CsvRowOutput
{
    public CsvRowOutput(Trust trust)
    {
        Name = trust.Name;
        Address = trust.TrustDetails.Address;
        Uid = trust.Uid;
        TrustReferenceNumber = trust.TrustDetails.TrustReferenceNumber;
        Ukprn = trust.TrustDetails.Ukprn;
        CompaniesHouseNumber = trust.TrustDetails.CompaniesHouseNumber;
    }
    public string Name { get; }
    public string Address { get; set; }
    public string Uid { get; }
    public string TrustReferenceNumber { get; set; }
    public string Ukprn { get; set; }
    public string CompaniesHouseNumber { get; set; }
}